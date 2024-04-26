using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentOffer : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private List<EquipmentTuple> _equipmentList;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Button _button;

    private EquipmentData _equipmentData;
    private RarityData _rarityData;
    private CoinManager _coinManager;
    private int _priceAmount = 30;

    public void Start()
    {
        _coinManager = CoinManager.Instance;
        _button.onClick.AddListener(BuyEquipment);
        SetPriceTag();
    }

    public void SetImage(EquipmentType equipmentType, RarityData rarityData)
    { 
        EquipmentTuple equipmentTuple = _equipmentList.Find(x => x.equipmentType == equipmentType);
        _equipmentData = equipmentTuple.equipmentData; //contains types
        _rarityData = rarityData;

        _image.sprite = _equipmentData.RarityDataList[(int)rarityData.rarityType].image;
    }

    public void SetPriceTag()
    {
        _priceAmount = LevelManager.Instance.GetLevelData().ShopPrice;
        _price.text = _priceAmount.ToString();
    }

    private void BuyEquipment()
    {
        if(_coinManager.CurrentCoins >= _priceAmount)
        {
            PlayerEquipmentManager.Instance.EquipEquipment(_equipmentData, _rarityData);
            _coinManager.SubtractCoins(_priceAmount);
        }
    }
}
