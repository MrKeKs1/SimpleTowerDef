using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheSTAR.Utility;

public class BulletsContainer : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    private List<Bullet> _activeBullets = new ();
    private List<Bullet> _bulletsPool = new ();

    private bool _isSimulate = false;

    public void Shoot(Shooter shooter, HpOwner goal, float force)
    {
        Bullet bullet = PoolUtility.GetPoolObject(_bulletsPool, info => !info.gameObject.activeSelf, shooter.Transform.position, CreateNewBullet);

        bullet.Init(force, goal, OnBulletReachedGoal);
        _activeBullets.Add(bullet);

        Bullet CreateNewBullet(Vector2 pos)
        {
            var bullet = Instantiate(bulletPrefab, shooter.Transform.position, Quaternion.identity, transform);
            _bulletsPool.Add(bullet);
            return bullet;
        }
    }

    public void StartSimulate()
    {
        if (_isSimulate) return;

        _isSimulate = true;
    }

    public void StopSimulate()
    {
        if (!_isSimulate) return;

        _isSimulate = false;
        HideBullets();

        _activeBullets.Clear();
    }

    private void Update()
    {
        if (!_isSimulate) return;

        for (int i = 0; i < _activeBullets.Count; i++)
        {
            Bullet b = _activeBullets[i];
            b.MoveToGoal();
        }
    }

    private void OnBulletReachedGoal(Bullet b)
    {
        _activeBullets.Remove(b);
    }

    private void HideBullets()
    {
        foreach (var b in _bulletsPool) b.gameObject.SetActive(false);
    }
}