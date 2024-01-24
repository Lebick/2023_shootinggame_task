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

    public  int     MyScore;

    float           Atk_Timer;

    public  EnemyType   Type;

    public  Transform[]     BulletSpawnPos;
    public  GameObject      bullet;

    GameObject Player;
    GameObject Axis;

    bool    PlayerJoin;

    bool    wasVisible;

    int Count;

    void Start()
    {
        Player = GameObject.Find("Player");
        Axis = transform.GetChild(0).gameObject;

        switch (Type)
        {
            case EnemyType.Meteor: //운석이라면
                transform.LookAt(Player.transform.position);
                break;

            case EnemyType.Monster1:
            case EnemyType.Monster2:
                Vector3 lookDirection = Vector3.zero - transform.position;
                transform.rotation = Quaternion.LookRotation(lookDirection);
                transform.eulerAngles *= Random.Range(0.9f, 1.1f);
                break;
        }
    }


    void Update()
    {
        if (!GameManager.Pause)
        {
            Move();

            if(IsVisible())
                Attack();
        }

        if (HP <= 0)
        {
            GameManager.Score += MyScore;
            GameManager.Kill_Enemy++;
            Destroy(gameObject);
        }

        if (PlayerJoin)
            AttackManager.Instance.Attack(Player, Damage);

        if (!wasVisible)
            wasVisible = IsVisible();

        if (wasVisible != IsVisible())
            Destroy(gameObject);
    }


    void Move()
    {
        switch (Type)
        {
            case EnemyType.Meteor: //운석이라면
                transform.Translate(0, 0, Movement_Speed * Time.deltaTime); //이동
                Axis.transform.Rotate(Movement_Speed * 10 * Time.deltaTime, 0, 0); //빙글빙글 돌음
                break;

            case EnemyType.Monster1: //적1 (눈쪽에서 총나가는 적)이라면

                //축이 플레이어를 보게 함.
                Quaternion targetRotation = Quaternion.LookRotation(Player.transform.position - Axis.transform.position);
                targetRotation *= Quaternion.Euler(0, 180, 0);
                Axis.transform.rotation = Quaternion.Lerp(Axis.transform.rotation, targetRotation, 10 * Time.deltaTime);

                //원본이 보는방향으로 이동함.
                transform.Translate(0, 0, Movement_Speed * Time.deltaTime);
                break;

            case EnemyType.Monster2:
                transform.Translate(0, 0, Movement_Speed * Time.deltaTime); //이동
                Axis.transform.Rotate(0, Movement_Speed * 50 * Time.deltaTime, 0); //빙글빙글 돌음
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
                        Unit.Instance.SummonBullet(bullet, pos.position, pos.eulerAngles, 10, 1, 30, "Player");
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
                        Unit.Instance.SummonBullet(bullet, pos.position, Axis.transform.eulerAngles, 10, 0.5f, 20, "Player");
                    }
                }
                break;
        }
    }


    public bool IsVisible()
    {
        var viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 1.1f) return false;
        if (viewPos.x < -0.1f) return false;
        if (viewPos.y > 1.1f) return false;
        if (viewPos.y < -0.1f) return false;

        return true;
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
