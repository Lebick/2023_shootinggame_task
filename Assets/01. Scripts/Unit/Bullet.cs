using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public  float   Speed;
    public  float   Damage;

    public  string  Atk_Obj_Tag;

    public  float   Destroy_Wait_Time;
    float   Destroy_Timer = 1;

    public  bool    isFollow;
    GameObject      Follow_Obj;

    void Start()
    {
        if (isFollow)
        {
            transform.LookAt(GameObject.FindGameObjectWithTag(Atk_Obj_Tag).transform.position);
            transform.Rotate(0, 180, 0);
            isFollow = false;
        }
    }

    void Update()
    {

        Invoke("Bul_Destroy", Destroy_Wait_Time);

        transform.Translate(0, 0, -Speed * Time.deltaTime);


        if (!GetComponent<MeshRenderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    void Bul_Destroy()
    {
        Destroy_Timer -= Time.deltaTime * 0.5f;
        transform.localScale *= Destroy_Timer; //총알이 점점 작아지게 함

        if (Destroy_Timer <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Atk_Obj_Tag))
        {
            AttackManager.Instance.Attack(other.gameObject, Damage);
            Destroy(gameObject);
        }
    }
}
