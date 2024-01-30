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

        if (Target_Enemy == null) //목표 적이 없다면?
        {
            if (GameObject.FindWithTag("Enemy")) //적이 존재한다면?
                Target_Enemy = GameObject.FindWithTag("Enemy"); //적을 목표로 지정

        }
        else //목표 적이 있다면?
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
