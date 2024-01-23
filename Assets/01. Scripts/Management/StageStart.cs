using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStart : MonoBehaviour
{
    Image Bar_Amout;

    bool Add = true;

    float timer;

    void Start()
    {
        Bar_Amout = GetComponent<Image>();
    }

    void Update()
    {
        if (!GameManager.Pause)
        {

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
                        GameManager.GameStart = true;
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
