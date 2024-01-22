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
            case EnemyType.Meteor: //��̶��
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
            case EnemyType.Meteor: //��̶��
                transform.Translate(0, 0, Movement_Speed * Time.deltaTime); //�̵�
                transform.GetChild(0).Rotate(Movement_Speed * 10 * Time.deltaTime, 0, 0); //���ۺ��� ����
                break;

            case EnemyType.Monster1: //��1 (���ʿ��� �ѳ����� ��)�̶��
                transform.LookAt(Player.transform.position);
                transform.Rotate(0, 180, 0);
                break;

            case EnemyType.Monster2:
                if (Camera.main.WorldToViewportPoint(transform.position).x >= 0.95f ||
                    Camera.main.WorldToViewportPoint(transform.position).x <= 0.05f)
                    transform.eulerAngles *= -1;
                transform.Translate(0, 0, Movement_Speed * Time.deltaTime); //�̵�
                transform.GetChild(0).Rotate(0, Movement_Speed * 50 * Time.deltaTime, 0); //���ۺ��� ����
                break;
        }
    }

    void Attack()
    {
        switch (Type)
        {
            case EnemyType.Monster1: //��1 �̶��
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


            case EnemyType.Monster2: //��2 �̶��
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
