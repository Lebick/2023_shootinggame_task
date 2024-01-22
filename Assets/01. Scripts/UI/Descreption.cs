using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Descreption : MonoBehaviour
{

    public   Image  Descreption_Image_UI; //������ �������� �̹��� UI
    public   Text   Descreption_Text_UI;  //������ �������� �ؽ�Ʈ UI

    [SerializeField]    private List<Sprite>    Descreption_Image_List; //������ �������� �̹����� ����Ʈ�� ������.
    [SerializeField]    private List<string>    Descreption_Text_List;  //������ �������� �ؚ�Ʈ�� ����Ʈ�� ������.

    int Descreption_Num     = 0;


    void Update()
    {
        //Descreption_Image_UI.sprite     = Descreption_Image_List[Descreption_Num]; //Descreption_Num��° �̹����� ������.
        Descreption_Text_UI.text        = Descreption_Text_List[Descreption_Num];  //Descreption_Num��° �ؽ�Ʈ�� ������.

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
