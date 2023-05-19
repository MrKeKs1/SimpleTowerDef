using System;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility.Pointer;
using TheSTAR.Utility;

namespace TheSTAR.GUI.Screens
{
    public class MenuScreen : GuiScreen
    {
        [SerializeField] private PointerButton playButton;
        [SerializeField] private CanvasGroup canvasGroup;

        private Sound.SoundController _sound;
        public event Action StartGameEvent;

        public void Init(Sound.SoundController sound, Action startGameAction)
        {
            _sound = sound;

            playButton.Init(startGameAction);
        }

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

        protected override void OnShow() => _sound.PlayMusic(Sound.MusicType.MainMenuTheme);
    }
}