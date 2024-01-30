using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public  static  Result  Instance;

    Animator anim;

    public  Text[]  Result_Text;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1080);
    }


    public IEnumerator Result_Show(float WaitTime = 0)
    {
        for (int i = 0; i < Result_Text.Length; i++)
        {
            Result_Text[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(WaitTime);
        Result_Text[0].text     = $"Stage {GameManager.Stage} Result";
        Result_Text[1].text     = $"HP : {(1 - (PlayerState.Max_HP - PlayerState.HP) / PlayerState.Max_HP) * 100}%";
        Result_Text[2].text     = $"Time :  {(int)(GameManager.time / 60)}:{(int)(GameManager.time % 60)}";
        Result_Text[3].text     = $"Kill Enemy : {GameManager.Kill_Enemy}";
        Result_Text[4].text     = $"Score: {GameManager.Score}";

        anim.SetTrigger("Show");
        yield return new WaitForSeconds(1f);
        for(int i=0; i<Result_Text.Length; i++)
        {
            Result_Text[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2f);

        if (GameManager.Stage == 2)
        {
            SceneLoadManager.Instance.SceneLoad(SceneNames.Ranking);
            GameManager.GameEnd = true;
            Destroy(GameObject.FindGameObjectWithTag("Player"), 1);
        }


        if (GameManager.Stage == 1)
        {
            anim.SetTrigger("Hide");
            yield return new WaitForSeconds(1f);
            GameManager.Stage = 2;
            GameManager.Instance.Invoke(nameof(GameManager.Instance.GameStarting), 0.7f);
            PlayerState.SkillReset();
        }
    }
}
