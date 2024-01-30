using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private float speed;

    [Header    ("0 : Atk_Upgrade\n" +
                "1 : Invincility\n" +
                "2 : HP_Recovery\n" +
                "3 : Fuel_Recovery")]

    public int Item_Type;

    private void Start()
    {
        transform.eulerAngles = new Vector3(0, Random.Range(-180, 180), 0);
    }

    private void Update()
    {
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (Item_Type)
            {
                case 0: //���� ���׷��̵� ������
                    if (PlayerState.Atk_Level < 3)
                        PlayerState.Atk_Level++;
                    else
                        GameManager.Score += 2000;
                    break;

                case 1: //���� ������
                    PlayerState.Item_Invincibility = true;
                    PlayerState.Invincibility = true;
                    Player.Instance.Item_Invincibility_Timer = 0;
                    break;

                case 2: //ü�� ȸ�� ������
                    PlayerState.HP += 20;
                    break;

                case 3: //���� ȸ�� ������
                    PlayerState.Fuel += 20;
                    break;
            }

            Destroy(gameObject); //������Ʈ ����
        }
    }
}
