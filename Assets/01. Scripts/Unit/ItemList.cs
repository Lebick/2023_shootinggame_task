using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public static GameObject AtkItem,
                             HPItem,
                             FuelItem,
                             InvincibilityItem;

    [SerializeField] private GameObject[] Items;

    private void Awake()
    {
        AtkItem = Items[0];
        HPItem = Items[1];
        FuelItem = Items[2];
        InvincibilityItem = Items[3];
    }
}
