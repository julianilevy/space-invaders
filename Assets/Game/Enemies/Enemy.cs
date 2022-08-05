using UnityEngine;
using System;
using System.Collections;

namespace SpaceInvaders.Gameplay
{
    [RequireComponent(typeof(Destroyable))]
    [RequireComponent(typeof(Weapon))]
    [RequireComponent(typeof(ScoreGiver))]
    public class Enemy : MonoBehaviour
    {
        public Colors colors = null;

        [HideInInspector]
        public Type type = 0;
        [HideInInspector]
        public int row = 0;
        [HideInInspector]
        public int column = 0;
        [HideInInspector]
        public bool enemyEnabled = true;
        [HideInInspector]
        public bool canShoot = false;

        public Action<Enemy> EnemyDeath;

        private AnimatorStateIterator _animatorStateIterator = null;
        private AnimatorOverrideControllerIterator _animatorOverrideControllerIterator = null;
        private SpriteRenderer _spriteRenderer = null;
        private Destroyable _destroyable = null;
        private Weapon _weapon = null;
        private float _timeToDisappearAfterDeath = 0.4f;

        public enum Type
        {
            Red,
            Yellow,
            Blue,
            Green
        }

        private void Awake()
        {
            _animatorStateIterator = GetComponentInChildren<AnimatorStateIterator>();
            _animatorOverrideControllerIterator = GetComponentInChildren<AnimatorOverrideControllerIterator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _destroyable = GetComponent<Destroyable>();
            _destroyable.HealthSetted += OnHealthSetted;
            _destroyable.Damaged += OnDamaged;
            _destroyable.HealthDrained += OnHealthDrained;

            _weapon = GetComponent<Weapon>();
        }

        public void Move()
        {
            _animatorStateIterator.GoToNextItem();
        }

        public void Shoot()
        {
            _weapon.Shoot(Vector2.down, _spriteRenderer.color);
        }

        #region Destroyable

        private void OnHealthSetted(int health)
        {
            if (health == 1)
                SetTypeAndColor(Type.Blue, Type.Green, colors.blue, colors.green);
            if (health == 2)
                SetTypeAndColor(Type.Red, Type.Yellow, colors.red, colors.yellow);
        }

        private void SetTypeAndColor(Type typeA, Type typeB, Color colorA, Color colorB)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                type = typeA;
                _spriteRenderer.color = colorA;
            }
            else
            {
                type = typeB;
                _spriteRenderer.color = colorB;
            }
        }

        private void OnDamaged()
        {
            _animatorOverrideControllerIterator.GoToNextItem();
        }

        private void OnHealthDrained()
        {
            StartCoroutine(_OnHealthDrained());
        }

        private IEnumerator _OnHealthDrained()
        {
            enemyEnabled = false;
            _animatorStateIterator.GoToLastItem();
            GetComponent<ScoreGiver>().ClaimScore();
            Destroy(GetComponent<Collider2D>());
            EnemyDeath?.Invoke(this);

            yield return new WaitForSeconds(_timeToDisappearAfterDeath);

            _spriteRenderer.gameObject.SetActive(false);
        }

        #endregion
    }
}