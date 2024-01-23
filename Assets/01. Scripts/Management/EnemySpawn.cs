using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("0 : Monster 1\n" +
            "1 : Monster 2\n" +
            "2 : Monster 3")]
    public  Vector3[]   SpawnPoints_Center;
    public  Vector3[]   SpawnPoints_Size;

    [SerializeField]    GameObject[]    Enemy;

    public  float       Spawn_CD;

    float               Spawn_timer;

    void Update()
    {
        if (GameManager.GameStart)
        {
            Spawn_timer += Time.deltaTime;
            if(Spawn_timer >= Spawn_CD)
            {
                Spawn_timer -= Spawn_CD;
                
                int enemy = Random.Range(0, SpawnPoints_Center.Length);
                print(enemy);

                Vector3 Range = new Vector3(Random.Range(SpawnPoints_Center[enemy].x - SpawnPoints_Size[enemy].x / 2, SpawnPoints_Center[enemy].x + SpawnPoints_Size[enemy].x / 2),
                                            Random.Range(SpawnPoints_Center[enemy].y - SpawnPoints_Size[enemy].y / 2, SpawnPoints_Center[enemy].y + SpawnPoints_Size[enemy].y / 2),
                                            Random.Range(SpawnPoints_Center[enemy].z - SpawnPoints_Size[enemy].z / 2, SpawnPoints_Center[enemy].z + SpawnPoints_Size[enemy].z / 2));
                Instantiate(Enemy[enemy], Range, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        for(int i=0; i<SpawnPoints_Center.Length; i++)
        {
            Gizmos.color = i == 0 ? Color.red : 
                           i == 1 ? Color.green : 
                           i == 2 ? Color.blue :
                           Color.white;

            Gizmos.DrawCube(SpawnPoints_Center[i], SpawnPoints_Size[i]);
        }
    }
}
