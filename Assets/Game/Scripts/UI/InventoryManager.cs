using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private Transform _inventoryPanel;
    [SerializeField] private CurrentEquipment _currentEquipment;
    [SerializeField] private TextMeshProUGUI _coinText;

    private List<CurrentEquipment> _currentEquipments = new List<CurrentEquipment>();

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.ToggleInventoryCanvas();
        }
    }

    public void PopulateInventory()
    {
        int equipmentCount = Enum.GetValues(typeof(EquipmentType)).Length;

        for (int i = 0; i < equipmentCount; i++)
        {
            CurrentEquipment currentEquipment = Instantiate(_currentEquipment, _inventoryPanel);

            _currentEquipments.Add(currentEquipment);
        }
    }

    public void SetCoinText(int amount)
    {
        _coinText.text = amount.ToString();
    }

    public void UpdateImage(EquipmentType equipmentType, RarityData rarityData)
    {
        _currentEquipments[(int)equipmentType].SetImage(equipmentType, rarityData);
    }
}
