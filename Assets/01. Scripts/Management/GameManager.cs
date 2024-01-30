using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public  static  int     Stage   = 0;

    public  static  bool    Pause;

    public  static  bool    GameStart;

    public  static  bool    Spawning;

    public  static  bool    GameEnd;

    public  static  KeyCode AttackKey   = KeyCode.Space;

    public  static  float   time;

    public  static  int     Kill_Enemy;

    public  static  int     Score;

    public  static  GameManager Instance;

    [SerializeField] private GameObject GameplayManager;
    [SerializeField] private GameObject GameplayUI;
    [SerializeField] private GameObject StageUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GameStarting()
    {
        time = 0;
        Kill_Enemy = 0;
        GameplayManager.SetActive(true);
        GameplayUI.SetActive(true);

        StageUI.SetActive(false);
        StageUI.SetActive(true);
        StageUI.transform.GetChild(0).GetComponent<Text>().text = $"Stage {Stage}";
        Spawning = true;
    }

    private void Update()
    {
        if (GameStart)
            time += Time.deltaTime;

        if(SceneManager.GetActiveScene().name == "Game")
        {
            if (GameplayUI == null)
                GameplayUI = GameObject.Find("Canvases").transform.Find("GameCanvas").gameObject;

            if (StageUI == null)
                StageUI = GameplayUI.transform.Find("Stage_Num").gameObject;
        }
    }
}
