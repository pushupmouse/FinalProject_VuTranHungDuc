using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private Transform _shopkeeperPanel;
    [SerializeField] private EquipmentOffer _equipmentOffer;
    [SerializeField] private List<EquipmentData> _equipmentDataList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PopulateShop()
    {
        int equipmentCount = Enum.GetValues(typeof(EquipmentType)).Length;

        for (int i = 0; i < equipmentCount; i++)
        {
            EquipmentOffer equipmentOffer = Instantiate(_equipmentOffer, _shopkeeperPanel);
            equipmentOffer.SetImage((EquipmentType)i, _equipmentDataList[i].RarityDataList[3]);
        }
    }
}
