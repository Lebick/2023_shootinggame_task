using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Meteor,
    Monster1,
    Monster2,
    Boss
}

public class Enemy : MonoBehaviour
{
    bool            summon;

    public  float   Movement_Speed;

    public  float   HP,
                    Damage,
                    Atk_CD;

    public  int     MyScore;

    float           Atk_Timer;

    public  EnemyType   Type;

    public  Transform[]     BulletSpawnPos;
    public  GameObject      bullet;

    GameObject Player;

    bool    PlayerJoin;

    int Count;

    void Start()
    {
        Player = GameObject.Find("Player");

        switch (Type)
        {
            case EnemyType.Meteor: //운석이라면
                transform.LookAt(Player.transform.position);
                break;
        }
    }


    void Update()
    {
        if (summon)
        {
            if (!GameManager.Pause)
            {
                Move();

                Attack();
            }

            if (HP <= 0)
            {
                GameManager.Score += MyScore;
                Destroy(gameObject);
            }

            if (PlayerJoin)
                AttackManager.Instance.Attack(Player, Damage);
        }
        else
        {
            Summon();
        }
    }

    void Summon()
    {
        switch (Type)
        {
            case EnemyType.Meteor: //운석이라면
                summon = true;
                break;

            case EnemyType.Monster1: //적1 (눈쪽에서 총나가는 적)이라면
                if (transform.position.y >= 0)
                {
                    summon = true;
                    transform.position = new Vector3(transform.position.x,
                                                     0,
                                                     transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x,
                                                     Mathf.Lerp(transform.position.y, 20, Time.deltaTime),
                                                     transform.position.z);
                }
                break;

            case EnemyType.Monster2:
                if (Camera.main.WorldToViewportPoint(transform.position).x <= 1 &&
                Camera.main.WorldToViewportPoint(transform.position).x >= 0)
                    summon = true;
                else
                {
                    if (Camera.main.WorldToViewportPoint(transform.position).x >= 1)
                        transform.rotation = Quaternion.Euler(0, -90, 0);
                    
                    if (Camera.main.WorldToViewportPoint(transform.position).x <= 0)
                        transform.rotation = Quaternion.Euler(0, 90, 0);

                    transform.Translate(0, 0, Movement_Speed * Time.deltaTime);
                }
                break;
        }
    }

    void Move()
    {
        switch (Type)
        {
            case EnemyType.Meteor: //운석이라면
                transform.Translate(0, 0, Movement_Speed * Time.deltaTime); //이동
                transform.GetChild(0).Rotate(Movement_Speed * 10 * Time.deltaTime, 0, 0); //빙글빙글 돌음
                break;

            case EnemyType.Monster1: //적1 (눈쪽에서 총나가는 적)이라면
                Quaternion targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
                targetRotation *= Quaternion.Euler(0, 180, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
                break;

            case EnemyType.Monster2:
                if ((Camera.main.WorldToViewportPoint(transform.position).x >= 1f ||
                    Camera.main.WorldToViewportPoint(transform.position).x <= 0f) && Count < 2)
                {
                    transform.eulerAngles *= -1;
                    Count++;
                }
                transform.Translate(0, 0, Movement_Speed * Time.deltaTime); //이동
                transform.GetChild(0).Rotate(0, Movement_Speed * 50 * Time.deltaTime, 0); //빙글빙글 돌음

                if (Camera.main.WorldToViewportPoint(transform.position).x >= 1.2f ||
                    Camera.main.WorldToViewportPoint(transform.position).x <= -0.2f)
                    Destroy(gameObject);
                break;
        }
    }

    void Attack()
    {
        switch (Type)
        {
            case EnemyType.Monster1: //적1 이라면
                Atk_Timer += Time.deltaTime;
                if(Atk_Timer >= Atk_CD)
                {
                    Atk_Timer -= Atk_CD;
                    foreach (Transform pos in BulletSpawnPos)
                    {
                        Unit.Instance.SummonBullet(bullet, pos.position, transform.eulerAngles, 10, 1, 30, "Player");
                    }
                }
                break;


            case EnemyType.Monster2: //적2 이라면
                Atk_Timer += Time.deltaTime;
                if (Atk_Timer >= Atk_CD)
                {
                    Atk_Timer -= Atk_CD;
                    foreach (Transform pos in BulletSpawnPos)
                    {
                        Unit.Instance.SummonBullet(bullet, pos.position, transform.GetChild(0).eulerAngles, 10, 0.5f, 20, "Player");
                    }
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerJoin = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerJoin = false;
        }
    }
}
