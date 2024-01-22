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
                        Bullet;

    public  Material[]  Player_Met;

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

        Player_Met = PlayerObj.GetComponent<MeshRenderer>().materials;
    }

    
    void Update()
    {
        if (!GameManager.Pause)
        {
            Horizontal = Input.GetAxisRaw("Horizontal");
            Vertical = Input.GetAxisRaw("Vertical");

            Fuel -= Time.deltaTime * Fuel_Speed;

            Move();

            Attack();

            Skill();

            AlphaChange();
        }
    }


    void Move()
    {
        transform.Translate(new Vector3(-Horizontal, 0, -Vertical) * Time.deltaTime * Movement_Speed);


        var viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 0.95f) viewPos.x = 0.95f;
        if (viewPos.x < 0.05f) viewPos.x = 0.05f;
        if (viewPos.y > 0.9f) viewPos.y = 0.9f;
        if (viewPos.y < 0.1f) viewPos.y = 0.1f;

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
                Atk_CD_Timer += 0.2f;

                if(Atk_Level >= 0)
                {
                    Unit.Instance.SummonBullet(Bullet, transform.position, transform.eulerAngles, 10, 1, 110, "Enemy", 2);
                }

                if (Atk_Level >= 1)
                {
                    Unit.Instance.SummonBullet(Bullet, transform.position + new Vector3(1, 0, 0), transform.eulerAngles, 5, 0.5f, 90, "Enemy", 2);
                    Unit.Instance.SummonBullet(Bullet, transform.position - new Vector3(1, 0, 0), transform.eulerAngles, 5, 0.5f, 90, "Enemy", 2);
                    Atk_CD_Timer -= 0.0033f;
                }

                if (Atk_Level >= 2)
                {
                    int fire = Random.Range(0, 3);
                    if(fire == 0)
                    {
                        Unit.Instance.SummonBullet(Bullet, transform.position + new Vector3(1, 0, 0), transform.eulerAngles + new Vector3(0, 45, 0), 4, 0.6f, 90, "Enemy", 2);
                        Unit.Instance.SummonBullet(Bullet, transform.position - new Vector3(1, 0, 0), transform.eulerAngles - new Vector3(0, 45, 0), 4, 0.6f, 90, "Enemy", 2);
                    }
                    Atk_CD_Timer -= 0.0033f;
                }
            }
        }
    }


    void Skill()
    {

        if(Skill_1_CD_time >= 1)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Skill_1_CD_timer = Skill_1_CD;
                HP += 10;
            }
        }
        else
        {
            Skill_1_CD_timer -= Time.deltaTime;
        }


        if (Skill_2_CD_time >= 1)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Skill_2_CD_timer = Skill_2_CD;
            }
        }
        else
        {
            Skill_2_CD_timer -= Time.deltaTime;
        }


        Skill_1_CD_time = (Skill_1_CD - Skill_1_CD_timer) / Skill_1_CD;
        Skill_2_CD_time = (Skill_2_CD - Skill_2_CD_timer) / Skill_2_CD;
    }

    void AlphaChange()
    {
        if (Invincibility)
        {
            Invincibility_Timer += Time.deltaTime;
            foreach (Material met in Player_Met)
            {
                met.color = new Color(met.color.r, met.color.g, met.color.b, Mathf.Lerp(met.color.a, Alpha, Time.deltaTime * 10));
                if (met.color.a <= 0.2f)
                    Alpha = 1;
                if (met.color.a >= 0.8f)
                    Alpha = 0;
            }

            if (Invincibility_Timer >= Invincibility_Time)
            {
                Invincibility = false;
                foreach (Material met in Player_Met)
                {
                    met.color = new Color(met.color.r, met.color.g, met.color.b, 1);
                }
                Invincibility_Timer = 0;
            }
        }
    }
}
