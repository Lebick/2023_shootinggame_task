using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public  static  int     Stage   = 0;

    public  static  bool    Pause;

    public  static  bool    GameStart;

    public  static  bool    GameEnd;

    public  static  KeyCode AttackKey   = KeyCode.Space;

    public  static  float   time;

    public  static  int     Kill_Enemy;

    public  static  int     Score;

    public  static  GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        switch (SceneManager.GetActiveScene().name[^1].ToString())
        {
            case "1":
                Stage = 1;
                time += Time.deltaTime;
                break;

            case "2":
                Stage = 2;
                time += Time.deltaTime;
                break;

            default:
                Stage = 0;
                break;
        }
    }
}
