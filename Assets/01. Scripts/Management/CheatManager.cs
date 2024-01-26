using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private Dictionary<KeyCode, Action> cheatActions = new();

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        cheatActions.Add(KeyCode.F1, CheatAction1); //F1키를 Key로, CheatAction1를 Value로 설정한다.
        cheatActions.Add(KeyCode.F2, CheatAction2); //F2키를 Key로, CheatAction2를 Value로 설정한다.
        cheatActions.Add(KeyCode.F3, CheatAction3); //반복...
        cheatActions.Add(KeyCode.F4, CheatAction4);
        cheatActions.Add(KeyCode.F5, CheatAction5);
        cheatActions.Add(KeyCode.F6, CheatAction6);
    }

    private void Update()
    {
        if(!GameManager.Pause)
            CheatKey(); // 치트키 구현


    }

    private void CheatKey()
    {
        foreach (var kvp in cheatActions)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                kvp.Value.Invoke(); //해당 키값에 해당하는 Value값 (함수)를 호출
            }
        }
    }


    //모든 적 유닛 제거
    private void CheatAction1()
    {
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject enemy in Enemys)
        {
            Destroy(enemy);
        }
    }

    //공격 업그레드를 최고단계로.
    private void CheatAction2()
    {
        PlayerState.Atk_Level = 3;
    }

    //스킬의 쿨타임, 횟수 초기화
    private void CheatAction3()
    {
        PlayerState.Skill_1_CD_timer = 0;
        PlayerState.Skill_1_Count = 0;
        PlayerState.Skill_2_CD_timer = 0;
        PlayerState.Skill_2_Count = 0;
    }

    //내구도 초기화
    private void CheatAction4()
    {
        PlayerState.HP = PlayerState.Max_HP;
    }

    //연료 초기화
    private void CheatAction5()
    {
        PlayerState.Fuel = PlayerState.Max_Fuel;
    }

    private void CheatAction6()
    {
        if (GameManager.Stage == 1)
            SceneLoadManager.Instance.SceneLoad(SceneNames.Stage2);
        else
            SceneLoadManager.Instance.SceneLoad(SceneNames.Stage1);
    }
}
