using UnityEngine;
using System;
using System.Collections;

namespace SpaceInvaders.Gameplay
{
    [RequireComponent(typeof(Destroyable))]
    public class Bullet : MonoBehaviour
    {
        [HideInInspector]
        public GameObject shooter = null;

        public Action<Bullet> BulletDestroyed;

        private AnimatorStateIterator _animatorStateIterator = null;
        private Destroyable _destroyable = null;
        private bool _enabled = false;
        private int _damage = 0;
        private float _speed = 0f;
        private Vector3 _direction = Vector3.zero;
        private float _timeToDisappearAfterDestroy = 0.2f;

        private void Awake()
        {
            _animatorStateIterator = GetComponentInChildren<AnimatorStateIterator>();

            _destroyable = GetComponent<Destroyable>();
            _destroyable.HealthDrained += OnHealthDrained;
        }

        private void Update()
        {
            if (_enabled)
                transform.position += _speed * _direction * Time.deltaTime;
        }

        public void SetColor(Color color)
        {
            GetComponentInChildren<SpriteRenderer>().color = color;
        }

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void Shoot(int damage, float speed, Vector3 direction)
        {
            _enabled = true;
            _damage = damage;
            _speed = speed;
            _direction = direction;
        }

        #region Destroyable

        private void OnHealthDrained()
        {
            StartCoroutine(_OnHealthDrained());
        }

        private IEnumerator _OnHealthDrained()
        {
            _enabled = false;
            _animatorStateIterator.GoToLastItem();
            Destroy(GetComponent<Collider2D>());
            BulletDestroyed?.Invoke(this);

            yield return new WaitForSeconds(_timeToDisappearAfterDestroy);

            Destroy(gameObject);
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D collider)
        {
            ColliderAction(collider);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            ColliderAction(collision.collider);
        }

        private void ColliderAction(Collider2D collider)
        {
            if (_enabled)
            {
                if (collider.gameObject.layer == Layer.Destroyable)
                {
                    if (collider.gameObject != shooter)
                    {
                        if (collider.GetComponent<Bullet>() && collider.GetComponent<Bullet>().shooter == shooter)
                            return;

                        collider.GetComponent<Destroyable>().Damage(_damage);
                        GetComponent<Destroyable>().DrainFullHealth();
                    }
                }
                else if (collider.gameObject.layer == Layer.Border)
                    GetComponent<Destroyable>().DrainFullHealth();
            }
        }
    }
}