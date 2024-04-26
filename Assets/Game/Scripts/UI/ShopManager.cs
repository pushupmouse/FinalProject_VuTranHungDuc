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

    private List<EquipmentOffer> _equipmentOfferList = new List<EquipmentOffer>();

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

    public void OnInit()
    {
        RemoveShop();
        PopulateShop();
    }

    public void PopulateShop()
    {
        int equipmentCount = Enum.GetValues(typeof(EquipmentType)).Length;

        RarityType rarityType = LevelManager.Instance.GetLevelData().TreasureRarityDrop;

        for (int i = 0; i < equipmentCount; i++)
        {
            EquipmentOffer equipmentOffer = Instantiate(_equipmentOffer, _shopkeeperPanel);
            equipmentOffer.SetImage((EquipmentType)i, _equipmentDataList[i].RarityDataList[(int)rarityType]);
            _equipmentOfferList.Add(equipmentOffer);
        }
    }

    private void RemoveShop()
    {
        for(int i = _equipmentOfferList.Count - 1; i > 0; i--)
        {
            _equipmentOfferList.RemoveAt(i);
        }
    }
}
