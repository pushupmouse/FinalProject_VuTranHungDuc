using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentOffer : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private List<EquipmentData> _equipmentData;
    [SerializeField] private TextMeshProUGUI _price;

    public void SetImage(EquipmentType equipmentType, RarityType rarityType)
    { 
        _image.sprite = _equipmentData[(int)equipmentType].RarityDataList[(int)rarityType].image;
    }

    public void SetPriceTag(int amount)
    {
        _price.text = amount.ToString();
    }
}
