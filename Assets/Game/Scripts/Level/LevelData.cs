using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public float SpawnRate;
    public RarityType MinRarityDrop;
    public RarityType MeanRarityDrop;
    public RarityType TreasureRarityDrop;
    public RarityType BossRarityDrop;
    public int ShopPrice;
    public int NumRooms;
}
