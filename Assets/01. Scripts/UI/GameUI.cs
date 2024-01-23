using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public  Image   Skill_1_CD;
    public  Text    Skill_1_Count;

    public  Image   Skill_2_CD;
    public  Text    Skill_2_Count;

    public  Text    Skill_Cant_Use;
    public  static  float   Alpha;

    public  Image   HP_Gauge,
                    Fuel_Gauge;

    public  Image[] Atk_Level_Gauge;

    public  Text    Score;

    void Start()
    {
        
    }

    void Update()
    {
        HP_UI();

        Atk_Level_UI();

        SkillUI();

        Score.text = $"Score : {GameManager.Score:D6}";
    }

    void HP_UI()
    {
        HP_Gauge.fillAmount = 1 - (PlayerState.Max_HP - PlayerState.HP) / PlayerState.Max_HP;

        Fuel_Gauge.fillAmount = 1 - (PlayerState.Max_Fuel - PlayerState.Fuel) / PlayerState.Max_Fuel;
    }

    void Atk_Level_UI()
    {
        for(int i=0; i<4; i++)
        {
            if (PlayerState.Atk_Level >= i)
                Atk_Level_Gauge[i].color = new Color(1, 0.60f, 0, 1);
            else
                Atk_Level_Gauge[i].color = new Color(1, 0.88f, 0.7f, 1);
        }
    }

    void SkillUI()
    {
        int count_1 = PlayerState.Skill_1_Max_Use - PlayerState.Skill_1_Count;
        int count_2 = PlayerState.Skill_2_Max_Use - PlayerState.Skill_2_Count;

        if(count_1 > 0)
            Skill_1_CD.fillAmount = 1 - PlayerState.Skill_1_CD_time;
        else
            Skill_1_CD.fillAmount = 1;
        Skill_1_Count.text = $"{count_1}";


        if (count_2 > 0)
            Skill_2_CD.fillAmount = 1 - PlayerState.Skill_2_CD_time;
        else
            Skill_2_CD.fillAmount = 1;
        Skill_2_Count.text = $"{count_2}";


        Skill_Cant_Use.color = new Color(255, 255, 255, Mathf.Lerp(Skill_Cant_Use.color.a, Alpha, Time.deltaTime * 4));
        if (Skill_Cant_Use.color.a >= 0.95f)
            Alpha = 0;
    }
}
