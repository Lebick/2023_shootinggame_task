using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public  float   Speed;
    public  float   Damage;

    public  string  Atk_Obj_Tag;

    public  bool    isFollow;
    public  Color   BulletColor;

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

        transform.Translate(Vector3.back * Speed * Time.deltaTime);


        if (!IsVisible())
        {
            Destroy(gameObject);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Atk_Obj_Tag))
        {
            AttackManager.Instance.Attack(other.gameObject, Damage);
            Destroy(gameObject);
        }
    }
}
