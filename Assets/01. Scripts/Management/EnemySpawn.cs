using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public  Vector3     SpawnPoint_Center;
    public  Vector3     SpawnPoint_Size;

    [SerializeField]    GameObject[]    Enemy;
    [SerializeField]    GameObject[]    Boss;

    public  float       Spawn_CD;

    float               Spawn_timer;

    void Update()
    {
        if (GameManager.GameStart)
        {
            if(GameManager.time >= 90f)
            {
                Instantiate(Boss[GameManager.Stage - 1], new Vector3(0, 0, 60), Quaternion.identity);
                Destroy(this);
            }

            Spawn_timer += Time.deltaTime;
            if (Spawn_timer >= Spawn_CD)
            {
                Spawn_timer -= Spawn_CD;

                for(int i=0; i<Random.Range(1,4); i++) // 1~3È¸ ¼ÒÈ¯
                {
                    int enemy = Random.Range(0, Enemy.Length);
                    Instantiate(Enemy[enemy], SpawnPoint(), Quaternion.identity);
                }
            }
        }
    }

    Vector3 SpawnPoint()
    {
        float radius = 60f;
        float yOffset = 30f;

        while (true)
        {
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);

            float x = radius * Mathf.Cos(randomAngle);
            float y = radius * Mathf.Sin(randomAngle) + yOffset;

            if (y > 20)
            {
                return new Vector3(x, 0, y);
            }
        }
    }
}
