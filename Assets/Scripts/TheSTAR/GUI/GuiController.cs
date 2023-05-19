using System;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;
using UnityEngine.Serialization;
using TheSTAR.GUI.Screens;
using Zenject;

namespace TheSTAR.GUI
{
    public sealed class GuiController : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private GuiScreen[] screens = new GuiScreen[0];
        [SerializeField] private GuiScreen mainScreen;
        [SerializeField] private bool deactivateOtherScreensByStart = true;
        [SerializeField] private bool showMainScreenByStart = true;

        #endregion // Inspector

        [Inject] private readonly Data.DataController data;
        [Inject] private readonly Sound.SoundController sound;
        [Inject] private readonly GameController gameController;
        [Inject] private readonly GameWorld world;
        [Inject] private readonly CurrencyController currency;
        [Inject] private readonly UpgradeController upgrades;

        private GuiScreen currentScreen;

        public void Init(
            out List<ITransactionReactable> trs,
            out List<IUpgradeReactable> urs,
            out List<IHpReactable> hrs,
            out List<IWaveReactable> wrs)
        {
            trs = new();
            urs = new();
            hrs = new();
            wrs = new();

            for (int i = 0; i < screens.Length; i++)
            {
                var screen = screens[i];

                if (screen == null) continue;

                if (deactivateOtherScreensByStart && screen.gameObject.activeSelf) screen.gameObject.SetActive(false);

                if (screen is ITransactionReactable tr) trs.Add(tr);
                if (screen is IUpgradeReactable ur) urs.Add(ur);
                if (screen is IHpReactable hr) hrs.Add(hr);
                if (screen is IWaveReactable wr) wrs.Add(wr);
            }

            var menu = FindScreen<MenuScreen>();
            menu.Init(sound, () =>
            {
                sound.StopMusic(Sound.MusicChangeType.Volume);
                HideCurrentScreen(gameController.StartBattle);
            });
            
            var game = FindScreen<GameScreen>();
            game.Init(upgrades, currency, sound, () =>
            {
                world.AnimateHideContent();
                sound.StopMusic(Sound.MusicChangeType.Volume);
                HideCurrentScreen(gameController.ExitBattle);
            });

            var result = FindScreen<ResultScreen>();
            result.Init(gameController.StartBattle, gameController.ExitBattle);

            if (showMainScreenByStart && mainScreen != null) Show(mainScreen, false);

            // game events

            gameController.StartBattleEvent += () => Show<GameScreen>(true);
            gameController.ExitBattleEvent += () => Show<MenuScreen>(true);
            gameController.BattleLostEvent += () =>
            {
                data.gameData.waveRecordIndex = Math.Max(data.gameData.waveRecordIndex, data.gameData.battleData.currentWaveIndex);
                sound.StopMusic(Sound.MusicChangeType.Volume);

                HideCurrentScreen(() =>
                {
                    var resultScreen = FindScreen<ResultScreen>();
                    resultScreen.SetWavesData(data.gameData.battleData.currentWaveIndex, data.gameData.waveRecordIndex);
                    Show(resultScreen);
                });
            };
        }

        public void Show<TScreen>(bool closeCurrentScreen = false, Action endAction = null) where TScreen : GuiScreen
        {
            GuiScreen screen = FindScreen<TScreen>();
            Show(screen, closeCurrentScreen, endAction);
        }

        public void Show<TScreen>(TScreen screen, bool closeCurrentScreen = false, Action endAction = null, bool skipShowAnim = false) where TScreen : GuiScreen
        {
            if (!screen) return;
            if (closeCurrentScreen && currentScreen) currentScreen.Hide();
            
            screen.Show(endAction, skipShowAnim);
            currentScreen = screen;
        }

        public void HideCurrentScreen(Action endAction = null)
        {
            if (!currentScreen) return;
            currentScreen.Hide(endAction);
        }

        public T FindScreen<T>() where T : GuiScreen
        {
            int index = ArrayUtility.FastFindElement<GuiScreen, T>(screens);

            if (index == -1)
            {
                Debug.LogError($"Not found screen {typeof(T)}");
                return null;
            }
            else return (T)(screens[index]);
        }

        [ContextMenu("SortScreens")]
        private void SortScreens() => Array.Sort(screens);
    }
}