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
        if (Get_Damage_Obj.CompareTag("Player")) //������ �޴� ��ü�� �÷��̾���.
        {
            if (PlayerState.Invincibility) //�������¿��ٸ�
                return; //����

            PlayerState.HP -= Damage; //�÷��̾��� HP�� ������.
            PlayerState.Invincibility = true; //���� ���·� ��ȯ.
        }
        else //������ �޴� ��ü�� ���̶��.
        {
            Get_Damage_Obj.GetComponent<Enemy>().HP -= Damage;
        }
    }
}
