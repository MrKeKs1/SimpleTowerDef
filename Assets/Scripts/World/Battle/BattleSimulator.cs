using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TheSTAR.Data;
using TheSTAR.Utility;

public class BattleSimulator : MonoBehaviour
{
    [SerializeField] private Tower tower;
    [SerializeField] private EnemySimulator enemySimulator;
    [SerializeField] private BulletsContainer bulletsContainer;

    [Inject] private readonly DataController dataController;
    [Inject] private readonly GameController gameController;
    [Inject] private readonly CurrencyController currencyController;
    [Inject] private readonly UpgradeController upgradeController;

    private BattleWaves waves;

    public Tower Tower => tower;

    public void Init(List<IHpReactable> hrs, List<IWaveReactable> wrs)
    {
        wrs.Add(enemySimulator);
        waves = new(dataController, wrs);
        enemySimulator.Init(tower.Damage);
        tower.Init(hrs, bulletsContainer.Shoot, gameController.BattleLost);
    }

    public void StartBattle()
    {
        waves.StartSimulate();

        tower.Reset();
        currencyController.ClearCurrency(CurrencyType.Coin);
        upgradeController.ResetUpgrades();

        enemySimulator.StartSimulate(tower.transform);
        bulletsContainer.StartSimulate();
    }

    public void StopBattle()
    {
        waves.StopSimulate();
        enemySimulator.StopSimulate();
        bulletsContainer.StopSimulate();
    }
}