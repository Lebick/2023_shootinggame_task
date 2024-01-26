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

    [SerializeField] private Text Ranking_List; //��Ŀ ����Ʈ

    [SerializeField] private InputField PlayerName; //�÷��̾� �̸�

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
                Message.text = "�ƹ� Ű�� �Է½� ����ȭ������ ���ư��ϴ�.";
            }
        }
        else
        {
            Ranking_Add.SetActive(false);
            AnyKeyDown = true;
            Message.color = Color.white;
            Message.text = "�ƹ� Ű�� �Է½� ����ȭ������ ���ư��ϴ�.";
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
        if (!Name_Check(PlayerName.text) || PlayerName.text.Length > 3) //���� �̸��� �빮��, ��� �ƴ� �ؽ�Ʈ�� �ִٸ�
        {
            Message.color = Color.red;
            Message.text = "�̸��� �ٸ� �ؽ�Ʈ�� �ְų� ���ڼ��� �ѱ�";
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
        Message.text = "��� ����! �ƹ� Ű�� �Է½� ����ȭ������ ���ư��ϴ�.";
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
                Ranking_List.text += $"{i+1} .... ��ϵ� ���� ����\n";
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
