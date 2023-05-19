using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TheSTAR.Sound;
using TheSTAR.GUI;
using TheSTAR.GUI.Screens;

public class GameController : MonoBehaviour
{
    [Inject] private readonly SoundController sounds;
    [Inject] private readonly GuiController gui;
    [Inject] private readonly GameWorld world;
    [Inject] private readonly CurrencyController currency;
    [Inject] private readonly UpgradeController upgrades;

    public event Action StartBattleEvent;
    public event Action ExitBattleEvent;
    public event Action BattleLostEvent;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        sounds.Init();
        gui.Init(
            out var trs,
            out var urs,
            out var hrs,
            out var wrs);

        world.Init(hrs, wrs);
        urs.Add(world.Tower);

        currency.Init(trs);
        upgrades.Init(urs);
    }

    public void StartBattle() => StartBattleEvent?.Invoke();
    public void ExitBattle() => ExitBattleEvent.Invoke();
    public void BattleLost() => BattleLostEvent.Invoke();
}