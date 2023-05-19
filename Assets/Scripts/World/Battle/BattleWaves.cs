using System;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Data;

[Serializable]
public class BattleWaves
{
    private BattlePhaseType battlePhaseType;

    private const string BattleConfigPath = "Configs/BattleConfig";

    private BattleConfig _battleConfig;
    public BattleConfig BattleConfig
    {
        get
        {
            if (_battleConfig == null) _battleConfig = Resources.Load<BattleConfig>(BattleConfigPath);
            return _battleConfig;
        }
    }

    private int _waveIndex = -1;
    private int _counterLTID = -1;
    private bool _isSimulate = false;

    private DataController _data;

    public BattleWaves(DataController data, List<IWaveReactable> waveReactables)
    {
        _data = data;
        _waveReactables = waveReactables;
    }

    private List<IWaveReactable> _waveReactables;

    public void StartSimulate()
    {
        if (_isSimulate) return;

        _isSimulate = true;

        StartPhase(BattlePhaseType.Attack);
    }

    public void StopSimulate()
    {
        if (!_isSimulate) return;

        _isSimulate = false;

        if (_counterLTID != -1)
        {
            LeanTween.cancel(_counterLTID);
            _counterLTID = -1;
        }

        _waveIndex = -1;
    }

    private void StartPhase(BattlePhaseType phaseType)
    {
        battlePhaseType = phaseType;

        float phaseTime;
        if (battlePhaseType == BattlePhaseType.Attack)
        {
            _waveIndex++;
            phaseTime = BattleConfig.WavesData.AttackPhaseTime;

            _data.gameData.battleData.currentWaveIndex = _waveIndex;
        }
        else phaseTime = BattleConfig.WavesData.RestPhaseTime;

        var currentWaveData = GetCurrentWaveData();

        foreach (var wr in _waveReactables) wr.OnStartWave(_waveIndex, battlePhaseType, currentWaveData);

        _counterLTID =
        LeanTween.value(0, 1, phaseTime)
        .setOnUpdate(value =>
        {
            foreach (var wr in _waveReactables) wr.OnSetWaveProgress(value);
        })
        .setOnComplete(() =>
        {
            StartPhase(phaseType == BattlePhaseType.Attack ? BattlePhaseType.Rest : BattlePhaseType.Attack);
        }).id;
    }

    public BattleWaveData GetCurrentWaveData() => BattleConfig.WavesData.WaveDatas[_waveIndex];
}

public enum BattlePhaseType
{
    Attack,
    Rest
}

public interface IWaveReactable
{
    void OnSetWaveProgress(float progress);
    void OnStartWave(int waveIndex, BattlePhaseType battlePhaseType, BattleWaveData waveData);
}