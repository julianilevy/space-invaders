using UnityEngine;
using System.Collections.Generic;

namespace SpaceInvaders.Gameplay
{
    public class Weapon : MonoBehaviour
    {
        public int damage = 1;
        public float speed = 1f;
        public bool oneBulletAtTime = false;
        public Transform bulletInitialPosition = null;
        public Bullet bulletPrefab = null;
        public AudioSource audioSource = null;

        private HashSet<Bullet> _bullets = new HashSet<Bullet>();
        private Color _bulletsColor = Color.white;

        public void Shoot(Vector3 direction, Color color)
        {
            _bulletsColor = color;
            Shoot(direction);
        }

        public void Shoot(Vector3 direction)
        {
            if (oneBulletAtTime)
            {
                if (_bullets.Count >= 1)
                    return;
            }

            var newBullet = Instantiate(bulletPrefab);
            newBullet.transform.position = bulletInitialPosition.position;
            newBullet.SetShooter(gameObject);
            newBullet.SetColor(_bulletsColor);
            newBullet.BulletDestroyed += OnBulletDestroyed;
            newBullet.Shoot(damage, speed, direction);
            if (audioSource != null)
                audioSource.Play();

            _bullets.Add(newBullet);
        }

        private void OnBulletDestroyed(Bullet bullet)
        {
            _bullets.Remove(bullet);
        }
    }
}