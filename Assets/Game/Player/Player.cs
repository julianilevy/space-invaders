using System.Collections;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    [RequireComponent(typeof(Destroyable))]
    [RequireComponent(typeof(Weapon))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public float movingSpeed = 0.8f;

        private AnimatorStateIterator _animatorStateIterator = null;
        private SpriteRenderer _spriteRenderer = null;
        private Destroyable _destroyable = null;
        private Weapon _weapon = null;
        private Collider2D _collider = null;
        private Rigidbody2D _rb = null;
        private float _timeToRespawn = 1f;
        private float _horizontalMovementValue = 0f;
        private int _lives = 3;
        private bool _enabled = false;
        private bool _directlyTouchedByEnemy = false;

        public struct Events
        {
            public const string LivesLost = "Player.LifeLost";
        }

        private void Awake()
        {
            _animatorStateIterator = GetComponentInChildren<AnimatorStateIterator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _destroyable = GetComponent<Destroyable>();
            _destroyable.HealthDrained += OnHealthDrained;

            _weapon = GetComponent<Weapon>();

            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();

            EventsHub.Connect(GameManager.Events.StartGame, EnablePlayer);
            EventsHub.Connect<bool>(GameManager.Events.PauseGame, OnGamePaused);
        }

        private void Update()
        {
            if (_enabled)
            {
                UpdateHorizontalMovementValue();
                Shoot();
            }
        }

        private void FixedUpdate()
        {
            if (_enabled)
                Move();
        }

        private void OnGamePaused(bool paused)
        {
            _enabled = !paused;
        }

        private void UpdateHorizontalMovementValue()
        {
            _horizontalMovementValue = Input.GetAxisRaw("Horizontal");
        }

        private void Shoot()
        {
            if (Input.GetKey(KeyCode.Space))
                _weapon.Shoot(Vector2.up, _spriteRenderer.color);
        }

        private void Move()
        {
            var movement = _horizontalMovementValue * movingSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(new Vector2(transform.position.x + movement, transform.position.y));
        }

        private void EnablePlayer()
        {
            _enabled = true;
        }

        #region Destroyable

        private void OnHealthDrained()
        {
            StartCoroutine(_OnHealthDrained());
        }

        private IEnumerator _OnHealthDrained()
        {
            var livesLost = _directlyTouchedByEnemy ? 3 : 1;

            _enabled = false;
            _animatorStateIterator.GoToLastItem();
            _lives -= livesLost;
            _collider.enabled = false;
            EventsHub.Post<int>(Events.LivesLost, livesLost);

            yield return new WaitForSeconds(_timeToRespawn);

            if (_lives > 0)
            {
                _animatorStateIterator.Restart();
                transform.position = new Vector3(0f, transform.position.y, 0f);
                _collider.enabled = true;
                _enabled = true;
            }
        }

        #endregion

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_enabled)
            {
                if (collider.gameObject.layer == Layer.Destroyable)
                {
                    if (collider.gameObject.GetComponent<Enemy>())
                    {
                        _directlyTouchedByEnemy = true;
                        GetComponent<Destroyable>().DrainFullHealth();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            EventsHub.Disconnect(GameManager.Events.StartGame, EnablePlayer);
            EventsHub.Disconnect<bool>(GameManager.Events.PauseGame, OnGamePaused);
        }
    }
}