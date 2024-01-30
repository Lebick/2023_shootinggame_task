using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_HPBar : MonoBehaviour
{
    public  static  Boss_HPBar  Instance;

    [SerializeField] private GameObject contant;
    public Image HP_Fill;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Show()
    {
        contant.SetActive(true);
    }

    public void Hide()
    {
        contant.SetActive(false);
    }
}
