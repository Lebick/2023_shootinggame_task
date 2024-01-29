using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stage2_Boss : Enemy
{
    private const string player = "Player";

    private int Now_Pattern;

    private float   Original_HP;
    
    private bool Patterning;
    private bool isSummon;
    private bool isDeath;

    private GameObject Obj_Axis;
    private GameObject Eye;
    private GameObject Guard;
    private GameObject player_obj;

    private Material[] Eye_Met;

    [SerializeField] private GameObject Pattern0_Axis;
                     private Transform[] Pattern0_Pos;

    [SerializeField] private GameObject Pattern1_Warning_Effect;
    [SerializeField] private GameObject Pattern1_Range;

    [SerializeField] private GameObject Pattern2_Enemy;

    [SerializeField] private GameObject[] bullets;
    [SerializeField] private GameObject Bomb;

    private void Start()
    {
        player_obj = GameObject.FindGameObjectWithTag(player);
        Obj_Axis = transform.Find("Axis").gameObject;
        Eye = Obj_Axis.transform.Find("Eye_Axis").gameObject;
        Guard = Obj_Axis.transform.Find("Guard_Axis").gameObject;

        Eye_Met = Eye.transform.Find("Eye").GetComponent<MeshRenderer>().materials;

        Pattern0_Pos = Pattern0_Axis.GetComponentsInChildren<Transform>(true);

        Original_HP = HP;

        Boss_HPBar.Instance.Show();
    }

    void Update()
    {
        if (!isDeath)
        {
            if (isSummon)
            {
                if (!Patterning)
                {
                    Patterning = true;
                    int Pattern = Random.Range(0, 3);
                    while (Now_Pattern == Pattern)
                    {
                        Pattern = Random.Range(0, 3);
                    }

                    Now_Pattern = Pattern;
                    StartCoroutine($"Pattern{Now_Pattern}");
                    //StartCoroutine($"Pattern2");
                }

                Boss_HPBar.Instance.HP_Fill.fillAmount = HP / Original_HP;

                if (HP <= 0)
                {
                    isDeath = true;
                    GameManager.Score += MyScore;
                    Cam_Effect.Instance.StartCoroutine(Cam_Effect.Instance.Cam_Shake(10, 3));
                    Invoke(nameof(Death), 3);
                    StopCoroutine($"Pattern{Now_Pattern}");
                }
            }
            else
                Summon();
        }
    }

    void Death()
    {
        GameObject Effect = Instantiate(Death_Effect, transform.position, Death_Effect.transform.rotation); //이펙트 소환
        Destroy(Effect, Effect.GetComponent<ParticleSystem>().main.startLifetime.constant); //이펙트 시간 다되면 삭제
        Result.Instance.StartCoroutine(Result.Instance.Result_Show(2));
        Destroy(gameObject);
    }

    void Summon()
    {
        transform.position += Time.deltaTime * Vector3.back * 10;
        if(transform.position.z <= 30)
        {
            transform.position = new Vector3(0, 0, 30);
            isSummon = true;
        }
    }

    //자신의 갑옷을 굴리며 파편을 튀게함.
    IEnumerator Pattern0()
    {
        StartCoroutine(Pattern0_1_Atk(bullet));
        float timer = 0;
        while (timer <= 5f)
        {
            timer += Time.deltaTime;

            Guard.transform.Rotate(new Vector3(1000, 0, 0) * Time.deltaTime, Space.Self);

            yield return null;
        }

        timer = 0;
        while (timer <= 2f)
        {
            timer += Time.deltaTime;

            Guard.transform.rotation = Quaternion.Lerp(Guard.transform.rotation, Quaternion.identity, Time.deltaTime * 20);

            yield return null;
        }

        Patterning = false;
    }


    //일정시간동안 플레이어에게 유도되다가 레이저를 쏨
    IEnumerator Pattern1()
    {
        float wait_time = 2.0f;
        ParticleSystem.MainModule Effect = Pattern1_Warning_Effect.GetComponent<ParticleSystem>().main;

        float timer = 0;
        while (timer <= 3)
        {
            transform.LookAt(player_obj.transform.position);
            transform.rotation *= Quaternion.Euler(0, 180, 0);
            timer += Time.deltaTime;
            Eye_Met[0].color = new Color(timer / 3, 0, 0);

            yield return null;
        }

        for(int i=0; i<4; i++)
        {
            wait_time -= 0.35f;

            Effect.duration = wait_time;
            Effect.startColor = new Color(1, 0, 0, 0.2f);
            timer = 0;
            Pattern1_Warning_Effect.SetActive(true);
            while (timer <= wait_time + 0.2f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(player_obj.transform.position - transform.position);
                targetRotation *= Quaternion.Euler(0, 180, 0);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
                timer += Time.deltaTime;

                yield return null;
            }
            Pattern1_Warning_Effect.SetActive(false);
            yield return new WaitForSecondsRealtime(0.3f);

            Effect.duration = wait_time * 0.25f;
            Effect.startColor = new Color(1, 0, 0, 1f);
            Pattern1_Warning_Effect.SetActive(true);

            Pattern1_Range.SetActive(true);

            Cam_Effect.Instance.StartCoroutine(Cam_Effect.Instance.Cam_Shake(3, wait_time * 0.25f));

            StartCoroutine(Pattern0_1_Atk(bullets[0], wait_time * 0.25f, 4));

            yield return new WaitForSecondsRealtime(wait_time * 0.25f);

            Pattern1_Warning_Effect.SetActive(false);
            Pattern1_Range.SetActive(false);

        }

        while (timer <= 1)
        {
            timer += Time.deltaTime;
            Eye_Met[0].color = new Color(1 - (1 / timer), 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 20);

            yield return null;
        }

        Patterning = false;

        yield return null;
    }


    IEnumerator Pattern0_1_Atk(GameObject bul, float time = 5, float amount = 15)
    {
        float timer = 0;
        while (timer <= time)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < amount; i++)
            {
                foreach (Transform pos in Pattern0_Pos)
                {
                    Unit.Instance.SummonBullet(bul, pos.transform.position, pos.eulerAngles, 20, 1, 20, player);
                }
                yield return new WaitForSecondsRealtime(0.05f);
                timer += 0.05f;
                Pattern0_Axis.transform.Rotate(0, Random.Range(0, 180), 0);
            }
        }

        yield return null;
    }


    //플레이어의 움직임을 제한할 적 소환
    IEnumerator Pattern2()
    {
        Instantiate(Pattern2_Enemy, new Vector3(Random.Range(25, 50), 0, 60), Quaternion.identity);
        Instantiate(Pattern2_Enemy, new Vector3(Random.Range(-50, -25), 0, 60), Quaternion.identity);
        Cam_Effect.Instance.StartCoroutine(Cam_Effect.Instance.Cam_Shake(5, 2));

        yield return new WaitForSecondsRealtime(3f);

        Patterning = false;
        yield return null;
    }


    IEnumerator Pattern3()
    {
        yield return null;
    }

    IEnumerator Pattern4()
    {
        yield return null;
    }

    IEnumerator Pattern5()
    {
        yield return null;
    }
}