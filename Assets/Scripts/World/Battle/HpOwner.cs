using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HpOwner
{
    private float _currentHp;
    public float CurrentHp => _currentHp;

    private float _maxHp;
    public float MaxHp => _maxHp;

    private HpOwnerStatus _status;

    private Action onChangeHpAction;
    private Action onDeadAction;

    private Transform transform;
    public Transform Transform => transform;

    public HpOwner(Transform transform, Action onChangeHpAction, Action onDeadAction)
    {
        this.transform = transform;
        _status = HpOwnerStatus.Alive;

        this.onChangeHpAction = onChangeHpAction;
        this.onDeadAction = onDeadAction;
    }

    public HpOwner(Transform transform, float maxHp, Action onChangeHpAction, Action onDeadAction) : this(transform, onChangeHpAction, onDeadAction)
    {
        this._maxHp = maxHp;
        _currentHp = maxHp;
    }

    public void SetMaxHp(float value)
    {
        _maxHp = value;
    }

    public void Reset()
    {
        _currentHp = _maxHp;
        onChangeHpAction?.Invoke();
    }

    public void Damage(float force)
    {
        _currentHp -= force;

        bool die = false;

        if (_currentHp <= 0)
        {
            _currentHp = 0;
            die = true;
        }

        onChangeHpAction?.Invoke();

        if (die) Die();
    }

    private void Die() => onDeadAction?.Invoke();
}

public enum HpOwnerStatus
{
    Alive,
    Dead
}

public interface IHpReactable
{
    void OnChangeHpReact(HpOwner hpOwner);
}