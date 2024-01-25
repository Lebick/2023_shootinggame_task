using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerState
{
    public static Player Instance;

    float   Horizontal,
            Vertical;

    public  float   Movement_Speed,
                    Fuel_Speed = 2,
                    Invincibility_Time = 1;

    public  GameObject  PlayerObj,
                        Axis,
                        Bullet,
                        Bomb,
                        Bomb_Effect;

    GameObject bomb_copy;

    float   Ho_Rotation_Value,
            Ver_Rotation_Value;

    float   Atk_CD_Timer,
            Invincibility_Timer;

    int Alpha;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StateReset();
    }

    
    void Update()
    {
        if (!GameManager.Pause)
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");

            Fuel -= Time.deltaTime * Fuel_Speed;

            Move();

            Attack();

            Skill();

            if(Invincibility && Invincibility_Timer == 0)
                StartCoroutine(AlphaChange());
        }
    }


    void Move()
    {
        transform.Translate(new Vector3(Horizontal, 0, Vertical) * Time.deltaTime * Movement_Speed, Space.World);

        Ho_Rotation_Value = (Horizontal == 0) ? 0 : (Horizontal > 0) ? 20 : -20;
        Ver_Rotation_Value = (Vertical == 0) ? 0 : (Vertical > 0) ? -20 : 20;

        Quaternion rotate = Quaternion.Euler(Ver_Rotation_Value, 180, Ho_Rotation_Value);
        Axis.transform.rotation = Quaternion.Lerp(Axis.transform.rotation, rotate, 2 * Time.deltaTime);

        var viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 1) viewPos.x = 1;
        if (viewPos.x < 0) viewPos.x = 0;
        if (viewPos.y > 1) viewPos.y = 1;
        if (viewPos.y < 0) viewPos.y = 0;

        var translatePos = Camera.main.ViewportToWorldPoint(viewPos);

        transform.position = new Vector3(translatePos.x, transform.position.y, translatePos.z);
    }


    void Attack()
    {
        if(Atk_CD_Timer > 0)
        {
            Atk_CD_Timer -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(GameManager.AttackKey))
            {
                Atk_CD_Timer += 0.1f;

                if(Atk_Level >= 0)
                {
                    Unit.Instance.SummonBullet(Bullet, transform.position, transform.eulerAngles, 10, 1, 150, "Enemy");
                }

                if (Atk_Level >= 1)
                {
                    Unit.Instance.SummonBullet(Bullet, transform.position + new Vector3(1, 0, 0), transform.eulerAngles, 5, 0.5f, 120, "Enemy");
                    Unit.Instance.SummonBullet(Bullet, transform.position - new Vector3(1, 0, 0), transform.eulerAngles, 5, 0.5f, 120, "Enemy");
                    Atk_CD_Timer -= 0.0033f;
                }

                if (Atk_Level >= 2)
                {
                    int fire = Random.Range(0, 3);
                    if(fire == 0)
                    {
                        Unit.Instance.SummonBullet(Bullet, transform.position + new Vector3(1, 0, 0), transform.eulerAngles + new Vector3(0, 45, 0), 4, 0.6f, 90, "Enemy");
                        Unit.Instance.SummonBullet(Bullet, transform.position - new Vector3(1, 0, 0), transform.eulerAngles - new Vector3(0, 45, 0), 4, 0.6f, 90, "Enemy");
                    }
                    Atk_CD_Timer -= 0.0033f;
                }
            }
        }
    }


    void Skill()
    {
        if (Input.GetKeyDown(KeyCode.R) && Skill_1_CD_time >= 1)
        {
            if (Skill_1_Count < Skill_1_Max_Use)
            {
                Skill_1_CD_timer = Skill_1_CD;
                HP += 10;

                Skill_1_Count++;
            }
            else
                GameUI.Alpha = 1;
        }
        else
        {
            Skill_1_CD_timer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.T) && Skill_2_CD_time >= 1)
        {
            if (Skill_2_Count < Skill_2_Max_Use)
            {
                Skill_2_CD_timer = Skill_2_CD;
                bomb_copy = Instantiate(Bomb, transform.position, Quaternion.identity);
                bomb_copy.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10 + Vector3.up * 10, ForceMode.Impulse);
                

                Skill_2_Count++;
            }
            else
                GameUI.Alpha = 1;
        }
        else
        {
            Skill_2_CD_timer -= Time.deltaTime;
        }

        Bomb_Skill();

        Skill_1_CD_time = (Skill_1_CD - Skill_1_CD_timer) / Skill_1_CD;
        Skill_2_CD_time = (Skill_2_CD - Skill_2_CD_timer) / Skill_2_CD;
    }

    void Bomb_Skill()
    {
        if (bomb_copy != null && bomb_copy.transform.position.y <= -1) //폭탄 폭발
        {
            GameObject Effect = Instantiate(Bomb_Effect, bomb_copy.transform.position, Quaternion.identity); //이펙트 소환
            Destroy(Effect, Effect.GetComponent<ParticleSystem>().main.startLifetime.constant); //이펙트 시간 다되면 삭제
            Destroy(bomb_copy); //폭탄 삭제

            GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] Bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject enemy in Enemys)
            {
                if (enemy.GetComponent<Enemy>())
                    enemy.GetComponent<Enemy>().HP -= 100; //일반 적 HP 감소

                if (enemy.GetComponent<Stage1_Boss>())
                    enemy.GetComponent<Stage1_Boss>().HP -= 100; //보스 HP 감소

                if (enemy.GetComponent<Stage2_Boss>())
                    enemy.GetComponent<Stage2_Boss>().HP -= 100; //보스 HP 감소
            }

            foreach (GameObject bul in Bullets)
            {
                if (bul.GetComponent<Bullet>().Atk_Obj_Tag == "Player") //플레이어를 목표로 하는 탄환
                    Destroy(bul); //제거
            }

            Cam_Effect.Instance.StartCoroutine(Cam_Effect.Instance.Cam_Shake(5, 0.5f));
        }
    }

    IEnumerator AlphaChange()
    {
        while (Invincibility_Timer <= Invincibility_Time)
        {
            Invincibility_Timer += Time.deltaTime;

            PlayerObj.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
            Invincibility_Timer += 0.1f;

            PlayerObj.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
            Invincibility_Timer += 0.1f;
        }


        Invincibility = false;
        Invincibility_Timer = 0;
    }
}
