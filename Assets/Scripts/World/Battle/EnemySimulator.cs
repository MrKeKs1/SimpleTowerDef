using System;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;
using Random = UnityEngine.Random;
using Zenject;
using TheSTAR.Data;

public class EnemySimulator : MonoBehaviour, IWaveReactable
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform enemyParent;

    [Inject] private CurrencyController currencyController;

    private bool _isSimulate = false;
    private bool _isPausedForRest = false;
    private float _spawnPeriodMin = 0.25f;
    private float _spawnPeriodMax = 3;
    private float _spawnDistance = 5f;
    private float _enemyForceForWave;
    private float _enemyHpForWave;

    private List<Enemy> enemiesPool = new ();
    private List<Enemy> activeEnemies = new ();
    private TimeCycleControl _spawnControl;
    private Transform _enemyGoal;
    private Action<float> _attackAction;

    public void Init(Action<float> attackAction)
    {
        _attackAction = attackAction;
    }

    public void StartSimulate(Transform goal)
    {
        if (_isSimulate) return;

        _enemyGoal = goal;

        _isSimulate = true;

        _spawnControl = TimeUtility.While(() => _isSimulate, _spawnPeriodMin, _spawnPeriodMax, SpawnEnemy);
    }

    public void StopSimulate()
    {
        if (!_isSimulate) return;

        _isSimulate = false;
        _spawnControl.Stop();

        activeEnemies.Clear();

        HideEnemies();
    }

    private void SpawnEnemy()
    {
        if (!_isSimulate || _isPausedForRest) return;

        Vector2 spawnPos =
            transform.position +
            (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * _spawnDistance);

        var enemy = PoolUtility.GetPoolObject(enemiesPool, info => !info.gameObject.activeSelf, spawnPos, CreateNewEnemy);
        enemy.SetStats(_enemyForceForWave, _enemyHpForWave);
        activeEnemies.Add(enemy);

        Enemy CreateNewEnemy(Vector2 pos)
        {
            var e = Instantiate(enemyPrefab, pos, Quaternion.identity, enemyParent);
            e.Init(OnEnemyReachedGoal, OnEnemyDead);
            enemiesPool.Add(e);
            return e;
        }
    }

    public void Update()
    {
        if (!_isSimulate) return;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Enemy enemy = activeEnemies[i];
            enemy.MoveTo(_enemyGoal);
        }
    }

    private void OnEnemyReachedGoal(Enemy e)
    {
        _attackAction?.Invoke(e.Force);
        e.Die();
    }

    private void OnEnemyDead(Enemy e)
    {
        activeEnemies.Remove(e);

        currencyController.AddCurrency(CurrencyType.Coin, e.Cost);
    }

    private void HideEnemies()
    {
        foreach (var e in enemiesPool) e.gameObject.SetActive(false);
    }

    public void OnSetWaveProgress(float progress)
    {
    }

    public void OnStartWave(int waveIndex, BattlePhaseType battlePhaseType, BattleWaveData waveData)
    {
        switch (battlePhaseType)
        {
            case BattlePhaseType.Attack:

                _enemyForceForWave = waveData.EnemyForce;
                _enemyHpForWave = waveData.EnemyHp;
                _spawnPeriodMin = waveData.SpawnMinPeriod;
                _spawnPeriodMax = waveData.SpawnMaxPeriod;

                if (!_isPausedForRest) return;
                _isPausedForRest = false;
                break;

            case BattlePhaseType.Rest:
                if (_isPausedForRest) return;
                _isPausedForRest = true;
                break;
        }
    }
}