using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Menu_UI : MonoBehaviour
{
    [SerializeField]    private GameObject[]    Buttons;

    public  GameObject      How_To_Play_Obj,
                            Player_Obj;

    public  static  Main_Menu_UI    Instance;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnClickGamePlayBtn()
    {
        UIButtonEnabled(false);
        GetComponent<Animator>().SetTrigger("Hide");
        GameManager.GameStart = true;
        GameManager.Score = 0;
        GameManager.Stage = 1;
        GameManager.Instance.Invoke(nameof(GameManager.Instance.GameStarting), 0.7f);
        PlayerState.StateReset();
    }

    public void OnClickRankingBtn()
    {
        UIButtonEnabled(false);
        SceneLoadManager.Instance.SceneLoad(SceneNames.Ranking);
    }

    public void OnClickHowtoPlayBtn()
    {
        UIButtonEnabled(false);
        How_To_Play_Obj.SetActive(true);
    }



    public void UIButtonEnabled(bool t_or_f)
    {
        foreach (var button in Buttons)
        {
            Button btn = button.GetComponent<Button>();
            btn.enabled = t_or_f;
        }
    }
}
