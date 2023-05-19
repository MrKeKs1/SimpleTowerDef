using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;

public class UpgradePanel : MonoBehaviour, IUpgradeReactable, ITransactionReactable
{
    [SerializeField] private UpgradeUI[] upgrades = new UpgradeUI[0];

    private UpgradeController _upgradesController;
    private CurrencyController _currencyController;

    public void Init(UpgradeController upgradesController, CurrencyController currencyController)
    {
        _upgradesController = upgradesController;
        _currencyController = currencyController;

        var upgradeTypes = EnumUtility.GetValues<UpgradeType>();

        UpgradeType upgradeType;
        UpgradeUI ui;
        UpgradeData data;
        for (int i = 0; i < upgradeTypes.Length; i++)
        {
            upgradeType = upgradeTypes[i];
            ui = upgrades[i];
            data = _upgradesController.GetUpgradeData(upgradeType);
            ui.Init(upgradeType, data.Name, OnUpgradeButtonClick);
            ui.gameObject.SetActive(true);
        }
    }

    public void OnTransactionReact(CurrencyType currencyType, int finalValue)
    {
        var upgradeTypes = EnumUtility.GetValues<UpgradeType>();
        int cost;

        for (int i = 0; i < upgradeTypes.Length; i++)
        {
            UpgradeType upgradeType = upgradeTypes[i];
            cost = _upgradesController.GetNextUpgradeLevelData(upgradeType).Cost;
            upgrades[i].SetAvailable(finalValue >= cost);
        }
    }

    public void OnUpgradeReact(UpgradeType upgradeType, float value)
    {
        var ui = upgrades[(int)upgradeType];
        int cost = _upgradesController.GetNextUpgradeLevelData(upgradeType).Cost;

        ui.SetCost(cost);
    }

    private void OnUpgradeButtonClick(UpgradeType upgradeType)
    {
        int cost = _upgradesController.GetNextUpgradeLevelData(upgradeType).Cost;
        _currencyController.ReduceCurrency(CurrencyType.Coin, cost, false, () => _upgradesController.Upgrade(upgradeType));
    }
}