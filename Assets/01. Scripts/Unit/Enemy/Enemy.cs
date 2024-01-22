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

    public  float   Movement_Speed;

    public  float   HP,
                    Damage,
                    Atk_CD;

    float           Atk_Timer;

    public  EnemyType   Type;

    public  Transform[]     BulletSpawnPos;
    public  GameObject      bullet;

    GameObject Player;

    bool    PlayerJoin;

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
        if (!GameManager.Pause)
        {
            Move();

            Attack();
        }

        if (HP <= 0)
            Destroy(gameObject);

        if(PlayerJoin)
            AttackManager.Instance.Attack(Player, Damage);
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
                transform.LookAt(Player.transform.position);
                transform.Rotate(0, 180, 0);
                break;

            case EnemyType.Monster2:
                if (Camera.main.WorldToViewportPoint(transform.position).x >= 0.95f ||
                    Camera.main.WorldToViewportPoint(transform.position).x <= 0.05f)
                    transform.eulerAngles *= -1;
                transform.Translate(0, 0, Movement_Speed * Time.deltaTime); //이동
                transform.GetChild(0).Rotate(0, Movement_Speed * 50 * Time.deltaTime, 0); //빙글빙글 돌음
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
                        Unit.Instance.SummonBullet(bullet, pos.position, transform.GetChild(0).eulerAngles, 10, 1, 40, "Player");
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
