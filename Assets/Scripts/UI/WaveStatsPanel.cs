using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveStatsPanel : MonoBehaviour, IWaveReactable
{
    [SerializeField] private Bar waveProgressBar;
    [SerializeField] private TextMeshProUGUI waveIndexText;
    [SerializeField] private Color attackColor;
    [SerializeField] private Color restColor;

    [Space]
    [SerializeField] private TextMeshProUGUI enemyForceText;
    [SerializeField] private TextMeshProUGUI enemyHpText;

    public void OnSetWaveProgress(float progress)
    {
        waveProgressBar.SetValue(progress);
    }

    public void OnStartWave(int waveIndex, BattlePhaseType battlePhaseType, BattleWaveData waveData)
    {
        waveIndexText.text = $"Wave {waveIndex + 1}";
        waveProgressBar.Img.color =
            battlePhaseType == BattlePhaseType.Attack ? attackColor : restColor;

        enemyForceText.text = waveData.EnemyForce.ToString();
        enemyHpText.text = waveData.EnemyHp.ToString();
    }
}