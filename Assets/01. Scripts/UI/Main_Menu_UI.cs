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

    bool Start;

    float Player_Obj_Speed = 0.05f;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnClickGamePlayBtn()
    {
        UIButtonEnabled(false);
        GetComponent<Animator>().SetTrigger("Hide");
        Start = true;
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

    private void Update()
    {
        if (Start)
        {
            Player_Obj_Speed += Player_Obj_Speed * Time.deltaTime;
            Player_Obj.transform.Translate(0, 0, -Player_Obj_Speed);
            if(Player_Obj.transform.position.z >= 1000)
            {
                SceneLoadManager.Instance.SceneLoad(SceneNames.Stage1);
                Destroy(gameObject);
            }
        }
    }
}
