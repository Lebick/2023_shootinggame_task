using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : Enemy
{
    private const string player = "Player";
    int Now_Pattern;

    bool Patterning;

    public GameObject Pattern2_Axis;
    Transform[] Pattern2_Pos;



    void Update()
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
        }

        if (HP <= 0)
        {
            GameManager.Score += MyScore;
            Destroy(gameObject);
        }
    }

    IEnumerator Pattern0()
    {
        //유도탄 몇개 (빠름, 소환될때만 유도)
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                foreach (Transform pos in BulletSpawnPos)
                {
                    Unit.Instance.SummonBullet(bullet, pos.transform.position, Vector3.zero, 20, 1, 80, player, 3, true);
                }
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(0.3f);
        }
        Patterning = false;
    }


    IEnumerator Pattern1()
    {
        //난사

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                foreach (Transform pos in BulletSpawnPos)
                {
                    if (i % 2 == 0)
                        pos.Rotate((5 - j) * 4 * Random.Range(1, 1.5f), 0, 0);
                    else
                        pos.Rotate(-(5 - j) * 4 * Random.Range(1f, 1.5f), 0, 0);

                    Unit.Instance.SummonBullet(bullet, pos.transform.position, pos.eulerAngles, 20, 1, 20, player, 3);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        foreach (Transform pos in BulletSpawnPos)
        {
            pos.eulerAngles = new Vector3(0, 0, -90);
        }

        yield return new WaitForSeconds(1f);
        Patterning = false;
    }

    IEnumerator Pattern2()
    {
        //원형 파장
        if (Pattern2_Pos == null) //자식오브젝트 위치 안받아온상태면
        {
            Pattern2_Pos = Pattern2_Axis.GetComponentsInChildren<Transform>(true);
        }

        for (int i = 0; i < 15; i++)
        {
            foreach (Transform pos in Pattern2_Pos)
            {
                Unit.Instance.SummonBullet(bullet, pos.transform.position, pos.eulerAngles, 20, 1, 30, player, 3);
            }
            yield return new WaitForSeconds(0.33f);
            Pattern2_Axis.transform.Rotate(0, Random.Range(0, 180), 0);
        }


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                foreach (Transform pos in Pattern2_Pos)
                {
                    Unit.Instance.SummonBullet(bullet, pos.transform.position, pos.eulerAngles, 20, 1, 30, player, 3);
                }
                yield return new WaitForSeconds(0.1f);
            }
            Pattern2_Axis.transform.Rotate(0, 11.25f, 0);
        }

        yield return new WaitForSeconds(1f);
        Patterning = false;
    }


    void Pattern3()
    {
        //플레이어 가둘만한 탄막 몇개
    }

    void Pattern4()
    {
        //공격반사 (짧은시간)
    }

    void Pattern5()
    {
        //홀로그램 생성(분신), 가짜에게 공격 적중시 공격반사
    }
}