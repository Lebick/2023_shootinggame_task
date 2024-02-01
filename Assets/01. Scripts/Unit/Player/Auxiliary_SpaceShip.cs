using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auxiliary_SpaceShip : MonoBehaviour
{
    GameObject Target_Enemy;
    GameObject Axis;

    [SerializeField] private GameObject bullet;

    float Atk_CD_Timer;

    [SerializeField] private float Movement_Speed;

    Vector3 Default_Pos;

    void Start()
    {
        Axis = transform.GetChild(0).gameObject;

        Default_Pos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, Default_Pos, Time.deltaTime * 2);
        //본래 위치쪽으로 서서히(플레이어로부터 일정거리 이상 벗어나지 못하게 하기 위함)sd


        if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            float enemy_distance = Mathf.Infinity;

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                float distance = Vector3.Distance(enemy.transform.position, transform.position);

                if (enemy.GetComponent<Enemy>().Type == EnemyType.Monster4) //몹 4면
                    continue; //저리가
                else if (distance < enemy_distance)
                {
                    enemy_distance = distance;
                    Target_Enemy = enemy;
                }
            }
        }

        if (Target_Enemy != null) //목표 적이 있다면?
        {
            //적을 보게하는 회전값
            Quaternion targetRotation = Quaternion.LookRotation(Target_Enemy.transform.position - transform.position);
            targetRotation *= Quaternion.Euler(0, 180, 0); //180도 돌림

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
            transform.Translate(0, 0, -Movement_Speed * Time.deltaTime);

            if (Atk_CD_Timer > 0)
            {
                Atk_CD_Timer -= Time.deltaTime;
            }
            else
            {
                Atk_CD_Timer += 0.2f;
                Unit.Instance.SummonBullet(bullet, transform.position, Axis.transform.eulerAngles, 5, 0.5f, 300, "Enemy");
            }
        }
    }
}
