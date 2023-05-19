using System;
using System.Collections.Generic;
using TheSTAR.Utility.Pointer;
using UnityEngine;
using TheSTAR.GUI;
using TMPro;

namespace TheSTAR.GUI.Screens
{
    public class GameScreen : GuiScreen, ITransactionReactable, IUpgradeReactable, IHpReactable, IWaveReactable
    {
        [SerializeField] private PointerButton exitButton;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI coinsCounter;
        [SerializeField] private PointerButton showHideUpgradesButton;

        [Header("Panels")]
        [SerializeField] private TowerStatsPanel towerStatsPanel;
        [SerializeField] private WaveStatsPanel waveStatsPanel;
        [SerializeField] private UpgradePanel upgradePanel;

        private Sound.SoundController _sound;

        public void Init(UpgradeController upgrades, CurrencyController currency, Sound.SoundController sound, Action exitAction)
        {
            _sound = sound;

            exitButton.Init(exitAction);
            upgradePanel.Init(upgrades, currency);

            showHideUpgradesButton.Init(OnShowHideUpgradesButtonClick);
        }

        #region Anim

        protected override void AnimateShow(out int time)
        {
            canvasGroup.alpha = 0;
            LeanTween.alphaCanvas(canvasGroup, 1, AnimateTime);
            time = (int)(AnimateTime * 1000);
        }

        protected override void AnimateHide(out int time)
        {
            canvasGroup.alpha = 1;
            LeanTween.alphaCanvas(canvasGroup, 0, AnimateTime);
            time = (int)(AnimateTime * 1000);
        }

        #endregion

        protected override void OnShow() => _sound.PlayMusic(Sound.MusicType.BattleTheme);

        private void OnShowHideUpgradesButtonClick()
        {
            upgradePanel.gameObject.SetActive(!upgradePanel.gameObject.activeSelf);
        }

        #region Reacts

        public void OnTransactionReact(CurrencyType currencyType, int finalValue)
        {
            coinsCounter.text = $"$ {finalValue}";
            upgradePanel.OnTransactionReact(currencyType, finalValue);
        }

        public void OnUpgradeReact(UpgradeType upgradeType, float value)
        {
            towerStatsPanel.OnUpgradeReact(upgradeType, value);
            upgradePanel.OnUpgradeReact(upgradeType, value);
        }

        public void OnChangeHpReact(HpOwner hpOwner) => towerStatsPanel.OnChangeHpReact(hpOwner);

        public void OnSetWaveProgress(float progress) => waveStatsPanel.OnSetWaveProgress(progress);
        public void OnStartWave(int waveIndex, BattlePhaseType battlePhaseType, BattleWaveData waveData) => waveStatsPanel.OnStartWave(waveIndex, battlePhaseType, waveData);

        #endregion
    }
}