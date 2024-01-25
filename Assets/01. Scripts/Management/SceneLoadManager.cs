using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneNames
{
    MainMenu,
    Ranking,
    Stage1,
    Stage2
}

public class SceneLoadManager : MonoBehaviour
{
    public  static  SceneLoadManager    Instance;

    public  GameObject[]     FadeImages;

    bool    Fading;

    AsyncOperation sc;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

        if (Fading)
        {
            bool all_move = false;
            foreach (GameObject fade in FadeImages)
            {
                RectTransform tr = fade.GetComponent<RectTransform>();
                if (tr.anchoredPosition.x >= 100)
                {
                    tr.anchoredPosition = new Vector2(Mathf.Lerp(tr.anchoredPosition.x, 0, Time.deltaTime * Random.Range(0.1f, 5f)), tr.anchoredPosition.y);
                }
                else
                    all_move = true;

            }

            if (all_move)
            {
                sc.allowSceneActivation = true;
                foreach (GameObject fade in FadeImages)
                {
                    RectTransform tr = fade.GetComponent<RectTransform>();
                    if (tr.anchoredPosition.x >= -2400)
                    {
                        tr.anchoredPosition = new Vector2(Mathf.Lerp(tr.anchoredPosition.x, -2500, Time.deltaTime * Random.Range(0.1f, 5f)), tr.anchoredPosition.y);
                    }
                    else
                    {
                        Fading = false;
                        GameManager.Pause = false;
                    }
                }
            }
        }
    }


    public void SceneLoad (SceneNames Scene)
    {
        foreach (GameObject fade in FadeImages)
        {
            RectTransform tr = fade.GetComponent<RectTransform>();
            tr.anchoredPosition = new Vector2(2500, tr.anchoredPosition.y);

        }
        sc = SceneManager.LoadSceneAsync(Scene.ToString());
        sc.allowSceneActivation = false;

        GameManager.Pause = true;
        GameManager.time = 0;
        Fading = true;
    }

}
