using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _force = 1;
    private int _cost = 1;
    private HpOwner _hpOwner;
    private MovableToGoal _movableToGoal;

    public float Force => _force;
    public HpOwner HpOwner => _hpOwner;
    public int Cost => _cost;

    private const float Speed = 0.5f;

    private Action<Enemy> _onDeadAction;

    public void Init(Action<Enemy> goalReachedAction, Action<Enemy> deadAction)
    {
        _hpOwner = new(transform, null, Die);
        _movableToGoal = new(transform, Speed, 0.25f, () => goalReachedAction?.Invoke(this));
        _onDeadAction = deadAction;
    }

    public void SetStats(float force, float maxHp)
    {
        _force = force;

        _hpOwner.SetMaxHp(maxHp);
        _hpOwner.Reset();
    }

    public void MoveTo(Transform goal) => _movableToGoal.MoveTo(goal);

    public void Die()
    {
        gameObject.SetActive(false);
        _onDeadAction(this);
    }
}