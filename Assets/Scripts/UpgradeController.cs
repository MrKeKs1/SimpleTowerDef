using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;
using TheSTAR.Data;
using Zenject;

public class UpgradeController : MonoBehaviour
{
    [Inject] private readonly DataController data;

    private const string UpgradesConfigPath = "Configs/UpgradeConfig";

    private UpgradeConfig _upgradeConfig;
    public UpgradeConfig UpgradeConfig
    {
        get
        {
            if (_upgradeConfig == null) _upgradeConfig = Resources.Load<UpgradeConfig>(UpgradesConfigPath);
            return _upgradeConfig;
        }
    }

    private List<IUpgradeReactable> _upgradeReactables;

    public void Init(List<IUpgradeReactable> urs)
    {
        _upgradeReactables = urs;

        InitReaction();
    }

    private void InitReaction()
    {
        var upgradeTypes = EnumUtility.GetValues<UpgradeType>();

        float value;
        foreach (var upgradeType in upgradeTypes)
        {
            value = GetUpgradeLevelData(upgradeType).Value;
            foreach (var ur in _upgradeReactables) ur.OnUpgradeReact(upgradeType, value);
        }
    }

    private void Reaction(UpgradeType upgradeType, float value)
    {
        foreach (var tr in _upgradeReactables) tr.OnUpgradeReact(upgradeType, value);
    }

    public UpgradeData GetUpgradeData(UpgradeType upgradeType) => UpgradeConfig.Upgrades[(int)upgradeType];

    public UpgradeLevelData GetUpgradeLevelData(UpgradeType upgradeType)
    {
        var upgradeData = GetUpgradeData(upgradeType);
        return upgradeData.Levels[data.gameData.battleData.towerUpgradeData.GetLevel(upgradeType)];
    }

    public UpgradeLevelData GetNextUpgradeLevelData(UpgradeType upgradeType)
    {
        var upgradeData = GetUpgradeData(upgradeType);
        return upgradeData.Levels[data.gameData.battleData.towerUpgradeData.GetLevel(upgradeType) + 1];
    }

    public void Upgrade(UpgradeType upgradeType)
    {
        data.gameData.battleData.towerUpgradeData.LevelUp(upgradeType, out _);

        var upgradeData = GetUpgradeLevelData(upgradeType);
        Reaction(upgradeType, upgradeData.Value);
    }

    public void ResetUpgrades()
    {
        data.gameData.ResetBattleData();

        InitReaction();
    }
}

public enum UpgradeType
{
    Damage,
    AttackSpeed,
    AttackDistance
}

public interface IUpgradeReactable
{
    void OnUpgradeReact(UpgradeType upgradeType, float value);
}