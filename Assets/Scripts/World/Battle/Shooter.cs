using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Shooter
{
    private float _force = 1;
    private Transform _transform;

    public event Action<Shooter, HpOwner, float> ShootEvent;

    public Transform Transform => _transform;

    public Shooter(Transform transform)
    {
        _transform = transform;
    }

    public void SetForce(float value) => _force = value;

    public virtual void Shoot(HpOwner goal)
    {
        ShootEvent?.Invoke(this, goal, _force);
    }
}