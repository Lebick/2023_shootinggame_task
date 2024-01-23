using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public  static  int     Stage   = 0;

    public  static  bool    Pause;

    public  static  bool    GameStart;

    public  static  KeyCode AttackKey   = KeyCode.Space;

    public  static  float   time;

    public  static  int     Kill_Enemy;

    public  static  int     Score;

    void Start()
    {
        time = 0;
        Kill_Enemy = 0;
    }

    void Update()
    {
        switch (int.Parse(SceneManager.GetActiveScene().name[^1].ToString()))
        {
            case 1:
                Stage = 1;
                break;

            case 2:
                Stage = 2;
                break;

            default:
                Stage = 0;
                break;
        }

        time += Time.deltaTime;
    }
}
