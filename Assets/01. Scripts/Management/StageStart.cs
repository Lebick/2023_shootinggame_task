using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStart : MonoBehaviour
{
    public  GameObject  GameStartBar;

    bool Add = true;

    float timer;

    void Start()
    {
        
    }

    void Update()
    {
        if (!GameManager.Pause)
        {
            Image Bar_Amout = GameStartBar.GetComponent<Image>();

            if (Add)
            {
                Bar_Amout.fillAmount = Mathf.Lerp(Bar_Amout.fillAmount, 1, Time.deltaTime * 5);
                if (Bar_Amout.fillAmount >= 0.99f)
                    Add = false;
            }
            else
            {
                timer += Time.deltaTime;
                if(timer >= 2)
                {
                    if (Bar_Amout.fillAmount > 0.01f)
                    {
                        Bar_Amout.fillOrigin = 1;
                        Bar_Amout.fillAmount = Mathf.Lerp(Bar_Amout.fillAmount, 0, Time.deltaTime * 5);
                    }
                    else
                    {
                        Destroy(GameStartBar);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
