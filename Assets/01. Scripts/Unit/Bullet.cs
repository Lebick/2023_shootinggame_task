using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public  float   Speed;
    public  float   Damage;

    public  string  Atk_Obj_Tag;

    public  float   Destroy_Time;


    void Update()
    {
        transform.Translate(Vector3.back * Speed * Time.deltaTime);

        if (!IsVisible())
        {
            Destroy(gameObject);
        }

        Invoke(nameof(Bullet_Destroy), Destroy_Time);
    }

    public bool IsVisible()
    {
        var viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 1.1f) return false;
        if (viewPos.x < -0.1f) return false;
        if (viewPos.y > 1.1f) return false;
        if (viewPos.y < -0.1f) return false;

        return true;
    }

    void Bullet_Destroy()
    {
        transform.localScale *= 0.9f;

        if(transform.localScale.x <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Guard"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag(Atk_Obj_Tag))
        {
            AttackManager.Instance.Attack(other.gameObject, Damage);
            Destroy(gameObject);
        }
    }
}
