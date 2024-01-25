using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stage2_Boss : Enemy
{
    private const string player = "Player";

    private int Now_Pattern;
    
    private bool Patterning;
    private bool isSummon;
    private bool isDeath;

    private GameObject Obj_Axis;
    private GameObject Eye;
    private GameObject Guard;
    private GameObject player_obj;

    [SerializeField] private GameObject Pattern0_Axis;
                     private Transform[] Pattern0_Pos;

    private void Start()
    {
        player_obj = GameObject.FindGameObjectWithTag(player);
        Obj_Axis = transform.Find("Axis").gameObject;
        Eye = Obj_Axis.transform.Find("Eye_Axis").gameObject;
        Guard = Obj_Axis.transform.Find("Guard_Axis").gameObject;
    }

    void Update()
    {
        if (!isDeath)
        {
            if (isSummon)
            {
                Eye.transform.LookAt(player_obj.transform.position);

                if (!Patterning)
                {
                    Patterning = true;
                    int Pattern = Random.Range(0, 3);
                    while (Now_Pattern == Pattern)
                    {
                        Pattern = Random.Range(0, 3);
                    }

                    Now_Pattern = Pattern;
                    //StartCoroutine($"Pattern{Now_Pattern}");
                    StartCoroutine($"Pattern0");
                }

                if (HP <= 0)
                {
                    GameManager.Score += MyScore;
                    Cam_Effect.Instance.StartCoroutine(Cam_Effect.Instance.Cam_Shake(10, 3));
                    Invoke("Death", 3);
                    isDeath = true;
                }
            }
            else
                Summon();
        }
        else
        {
            StopCoroutine($"Pattern{Now_Pattern}");
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

    IEnumerator Pattern0()
    {
        if (Pattern0_Pos == null) //자식오브젝트 위치 안받아온상태면
        {
            Pattern0_Pos = Pattern0_Axis.GetComponentsInChildren<Transform>(true);
        }

        float timer = 0;
        while (timer <= 5)
        {
            timer += Time.deltaTime;

            #region 회전
            Vector3 targetDirection = player_obj.transform.position - Guard.transform.position;
            targetDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            targetRotation *= Quaternion.Euler(0, 180, 0);

            transform.rotation = Quaternion.Lerp(Obj_Axis.transform.rotation, targetRotation, 2 * Time.deltaTime);

            Guard.transform.Rotate(new Vector3(1000, 0, 0) * Time.deltaTime);
            #endregion

            for (int i = 0; i < 15; i++)
            {
                foreach (Transform pos in Pattern0_Pos)
                {
                    Unit.Instance.SummonBullet(bullet, pos.transform.position, pos.eulerAngles, 20, 1, 20, player);
                }
                yield return new WaitForSecondsRealtime(0.05f);
                Pattern0_Axis.transform.Rotate(0, Random.Range(0, 180), 0);
            }

            yield return null;
        }
        
        timer = 0;
        //while (timer <= 3)
        //{
        //    //transform.
        //}

        yield return null;
    }


    IEnumerator Pattern1()
    {
        yield return null;
    }

    IEnumerator Pattern2()
    {
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