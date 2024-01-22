using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public  Image   Skill_1_CD;

    public  Image   Skill_2_CD;

    public  Image   HP_Gauge,
                    Fuel_Gauge;

    public  Image[] Atk_Level_Gauge;

    void Start()
    {
        
    }

    void Update()
    {
        HP_UI();

        Atk_Level_UI();

        SkillUI();
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
        Skill_1_CD.fillAmount = 1 - PlayerState.Skill_1_CD_time;

        Skill_2_CD.fillAmount = 1 - PlayerState.Skill_2_CD_time;
    }
}
