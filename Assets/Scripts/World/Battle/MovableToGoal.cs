using System;
using System.Collections.Generic;
using UnityEngine;

public class MovableToGoal
{
    private float _neededGoalDistance = 0.25f;
    private Transform _transform;
    private Action _onGoalReached;
    private float _speed = 1;

    public MovableToGoal(Transform transform, float speed, float neededGoalDistance, Action onGoalReached)
    {
        _transform = transform;
        _speed = speed;
        _neededGoalDistance = neededGoalDistance;
        _onGoalReached = onGoalReached;
    }

    public void MoveTo(Transform goal)
    {
        var distance = Vector2.Distance(_transform.position, goal.position);
        if (distance <= _neededGoalDistance)
        {
            _onGoalReached?.Invoke();
            return;
        }

        var offset = goal.transform.position - _transform.position;
        var direction = offset.normalized;
        var step = _speed * Time.deltaTime * direction;

        _transform.Translate(step);
    }
}