using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;

class RankingData
{
    public  List<string>  Name  = new();
    public  List<string>  Score = new();
}

public class Ranking : MonoBehaviour
{

    private RankingData data = new();

    [SerializeField] private GameObject Ranking_Add;

    [SerializeField] private Text Ranking_List; //랭커 리스트

    [SerializeField] private InputField PlayerName; //플레이어 이름

    [SerializeField] private Button AddBtn;

    [SerializeField] private Text Message;

    private bool AnyKeyDown;

    private void Start()
    {
        LoadData();
        if (GameManager.GameEnd)
        {
            if (data.Score.Count > 0 && int.Parse(data.Score[data.Score.Count - 1]) >= GameManager.Score)
            {
                Ranking_Add.SetActive(false);
                AnyKeyDown = true;
                Message.color = Color.white;
                Message.text = "아무 키나 입력시 메인화면으로 돌아갑니다.";
            }
        }
        else
        {
            Ranking_Add.SetActive(false);
            AnyKeyDown = true;
            Message.color = Color.white;
            Message.text = "아무 키나 입력시 메인화면으로 돌아갑니다.";
        }
    }

    private void Update()
    {
        if(AnyKeyDown && Input.anyKeyDown)
        {
            SceneLoadManager.Instance.SceneLoad(SceneNames.MainMenu);
            Destroy(this);
        }
    }

    public void OnClickRankingAddBtn()
    {
        if (!Name_Check(PlayerName.text) || PlayerName.text.Length > 3) //만약 이름에 대문자, 영어가 아닌 텍스트가 있다면
        {
            Message.color = Color.red;
            Message.text = "이름에 다른 텍스트가 있거나 글자수를 넘김";
            return;
        }
        else
        {
            AddBtn.enabled = false;
            PlayerName.enabled = false;
            SaveData();
        }
    }

    bool Name_Check(string Text)
    {
        return Regex.IsMatch(Text, "^[A-Z]+$");
    }

    void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Ranking.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<RankingData>(json);
        }
        else
        {
            data = new RankingData();
        }
        UIUpdate();
    }

    void SaveData()
    {
        data.Name.Add(PlayerName.text);
        data.Score.Add(GameManager.Score.ToString("D6"));

        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(Application.persistentDataPath, "Ranking.json");
        print(json);
        File.WriteAllText(filePath, json);

        UIUpdate();
        Message.color = Color.white;
        Message.text = "등록 성공! 아무 키나 입력시 메인화면으로 돌아갑니다.";
        AnyKeyDown = true;
    }


    void UIUpdate()
    {
        Data_Sort();
        Ranking_List.text = string.Empty;

        for (int i=0; i<5; i++)
        {
            if (data.Name.Count > i)
                Ranking_List.text += $"{i+1} .... {data.Name[i]} : {data.Score[i]}\n";
            else
                Ranking_List.text += $"{i+1} .... 등록된 정보 없음\n";
        }
    }

    void Data_Sort()
    {
        for (int i = 0; i < data.Score.Count; i++)
        {
            for (int j = 0; j < data.Score.Count; j++)
            {
                if (int.Parse(data.Score[i].ToString()) > int.Parse(data.Score[j].ToString()))
                {
                    string copy = data.Score[i];
                    data.Score[i] = data.Score[j];
                    data.Score[j] = copy;

                    string copy2 = data.Name[i];
                    data.Name[i] = data.Name[j];
                    data.Name[j] = copy2;
                }
            }
        }
    }
}
