using System;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private MovableToGoal _movableToGoal;
    private HpOwner _goal;
    private float _force;
    private Action<Bullet> endAction;

    private const float Speed = 4;

    public void Init(float force, HpOwner goal, Action<Bullet> endAction)
    {
        _force = force;
        _goal = goal;
        _movableToGoal = new(transform, Speed, 0.1f, OnReachedGoal);
        this.endAction = endAction;
    }

    public void MoveToGoal() => _movableToGoal.MoveTo(_goal.Transform);

    private void OnReachedGoal()
    {
        _goal.Damage(_force);
        gameObject.SetActive(false);
        endAction?.Invoke(this);
    }
}