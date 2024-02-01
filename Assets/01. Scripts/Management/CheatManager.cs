using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private Dictionary<KeyCode, Action> cheatActions = new();

    void Start()
    {
        cheatActions.Add(KeyCode.F1, CheatAction1); //F1Ű�� Key��, CheatAction1�� Value�� �����Ѵ�.
        cheatActions.Add(KeyCode.F2, CheatAction2); //F2Ű�� Key��, CheatAction2�� Value�� �����Ѵ�.
        cheatActions.Add(KeyCode.F3, CheatAction3); //�ݺ�...
        cheatActions.Add(KeyCode.F4, CheatAction4);
        cheatActions.Add(KeyCode.F5, CheatAction5);
        cheatActions.Add(KeyCode.F6, CheatAction6);
    }

    private void Update()
    {
        if(!GameManager.Pause)
            CheatKey(); // ġƮŰ ����


    }

    private void CheatKey()
    {
        foreach (var kvp in cheatActions)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                kvp.Value.Invoke(); //�ش� Ű���� �ش��ϴ� Value�� (�Լ�)�� ȣ��
            }
        }
    }


    //��� �� ���� ����
    private void CheatAction1()
    {
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in Enemys)
        {
            if (enemy.GetComponent<Enemy>())
                enemy.GetComponent<Enemy>().HP = 0;
        }
    }

    //���� ���׷��带 �ְ�ܰ��.
    private void CheatAction2()
    {
        PlayerState.Atk_Level = 3;
    }

    //��ų�� ��Ÿ��, Ƚ�� �ʱ�ȭ
    private void CheatAction3()
    {
        PlayerState.Skill_1_CD_timer = 0;
        PlayerState.Skill_1_Count = 0;
        PlayerState.Skill_2_CD_timer = 0;
        PlayerState.Skill_2_Count = 0;
    }

    //������ �ʱ�ȭ
    private void CheatAction4()
    {
        PlayerState.HP = PlayerState.Max_HP;
    }

    //���� �ʱ�ȭ
    private void CheatAction5()
    {
        PlayerState.Fuel = PlayerState.Max_Fuel;
    }

    private void CheatAction6()
    {
        GameManager.time = 0;

        GameManager.Spawning = false;
        foreach (GameObject enemys in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemys);

        foreach (GameObject bullets in GameObject.FindGameObjectsWithTag("Bullet"))
            Destroy(bullets);

        Result.Instance.StartCoroutine(Result.Instance.Result_Show());
    }
}
