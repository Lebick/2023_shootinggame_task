using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public  static  int     Atk_Level       = 0,
                            Skill_1_Count,
                            Skill_2_Count,
                            Skill_1_Max_Use = 5,
                            Skill_2_Max_Use = 5;


    //
    public  static  float   Max_HP      = 100,
                            HP          = 100,
                            Max_Fuel    = 100,
                            Fuel        = 100,

                            Skill_1_CD_time,
                            Skill_2_CD_time,

                            Skill_1_CD_timer,
                            Skill_2_CD_timer;

    public          float   Skill_1_CD,
                            Skill_2_CD;


    //
    public  static  bool    Invincibility = false;




    public static void SkillReset()
    {
        Skill_1_Count = 0;
        Skill_2_Count = 0;
        Skill_1_CD_timer = 0;
        Skill_2_CD_timer = 0;
    }

    public static void StateReset()
    {
        HP = Max_HP;
        Fuel = Max_Fuel;
        Atk_Level = 0;
        Skill_1_Count = 0;
        Skill_2_Count = 0;
        Skill_1_CD_timer = 0;
        Skill_2_CD_timer = 0;
    }
}
