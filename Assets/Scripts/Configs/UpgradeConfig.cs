using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Upgrade", fileName = "UpgradeConfig")]
public class UpgradeConfig : ScriptableObject
{
    [SerializeField] private UpgradeData[] upgrades = new UpgradeData[0];

    public UpgradeData[] Upgrades => upgrades;
}

[Serializable]
public class UpgradeData
{
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private string name;
    [SerializeField] private UpgradeLevelData[] levels = new UpgradeLevelData[0];

    public UpgradeType UpgradeType => upgradeType;
    public string Name => name;
    public UpgradeLevelData[] Levels => levels;
}

[Serializable]
public class UpgradeLevelData
{
    [SerializeField] private int cost;
    [SerializeField] private float value;

    public int Cost => cost;
    public float Value => value;
}