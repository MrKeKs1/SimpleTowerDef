using System;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility.Pointer;
using TMPro;

namespace TheSTAR.GUI.Screens
{
    public class ResultScreen : GuiScreen
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private PointerButton restartButton;
        [SerializeField] private PointerButton exitButton;
        [SerializeField] private TextMeshProUGUI currentWaveTitle;
        [SerializeField] private TextMeshProUGUI maxWaveTitle;

        public void Init(Action restartAction, Action exitAction)
        {
            restartButton.Init(restartAction);
            exitButton.Init(exitAction);
        }

        public void SetWavesData(int currentWaveIndex, int maxWaveIndex)
        {
            currentWaveTitle.text = $"Wave: {currentWaveIndex + 1}";
            maxWaveTitle.text = $"Highest Wave: {maxWaveIndex + 1}";
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
    }
}