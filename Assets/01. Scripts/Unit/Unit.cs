using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static Unit Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    public void SummonBullet(GameObject bullet, Vector3 position, Vector3 rotation, float damage, float size, float speed, string tag, float destroy_time = 10)
    {
        GameObject bul = Instantiate(bullet, position, Quaternion.Euler(rotation));
        Bullet bul_info = bul.GetComponent<Bullet>();

        bul_info.Damage = damage;
        bul_info.Atk_Obj_Tag = tag;
        bul_info.Speed = speed;
        bul_info.Destroy_Time = destroy_time;
        bul.transform.localScale = Vector3.one * size;

    }
}
