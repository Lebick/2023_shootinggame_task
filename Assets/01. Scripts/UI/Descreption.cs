using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Descreption : MonoBehaviour
{

    public   Image  Descreption_Image_UI; //설명을 돕기위한 이미지 UI
    public   Text   Descreption_Text_UI;  //설명을 돕기위한 텍스트 UI

    [SerializeField]    private List<Sprite>    Descreption_Image_List; //설명을 돕기위한 이미지를 리스트로 저장함.
    [SerializeField]    private List<string>    Descreption_Text_List;  //설명을 돕기위한 텍슽트를 리스트로 저장함.

    int Descreption_Num     = 0;


    void Update()
    {
        //Descreption_Image_UI.sprite     = Descreption_Image_List[Descreption_Num]; //Descreption_Num번째 이미지를 가져옴.
        Descreption_Text_UI.text        = Descreption_Text_List[Descreption_Num];  //Descreption_Num번째 텍스트를 가져옴.

        if (Input.GetButtonDown("Cancel"))
            OnClickExitBtn();
    }

    public void OnClickExitBtn()
    {
        gameObject.SetActive(false);
        Main_Menu_UI.Instance.UIButtonEnabled(true);
    }

    public void OnClickPreviousBtn()
    {
        if (Descreption_Num > 0)
            Descreption_Num--;
    }

    public void OnClickNextBtn()
    {
        if (Descreption_Num < Descreption_Text_List.Count - 1)
            Descreption_Num++;
    }
}
