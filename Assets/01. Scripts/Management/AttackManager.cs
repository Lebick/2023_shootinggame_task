using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public static AttackManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Attack(GameObject Get_Damage_Obj, float Damage)
    {
        if (Get_Damage_Obj.CompareTag("Player")) //공격을 받는 주체가 플레이어라면.
        {
            if (PlayerState.Invincibility) //무적상태였다면
                return; //리턴

            PlayerState.HP -= Damage; //플레이어의 HP를 차감함.
            PlayerState.Invincibility = true; //무적 상태로 변환.
        }
        else //공격을 받는 주체가 적이라면.
        {
            Get_Damage_Obj.GetComponent<Enemy>().HP -= Damage;
        }
    }
}
