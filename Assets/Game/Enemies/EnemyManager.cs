using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    [RequireComponent(typeof(EnemySpawner))]
    [RequireComponent(typeof(AudioClipIterator))]
    public class EnemyManager : MonoBehaviour
    {
        public float movingForceHorizontal = 3f;
        public float movingForceVertical = 8.5f;
        public float initialMovingInterval = 0.8f;
        public float movingIntervalScaler = 0.4f;
        public float maxShootingCooldown = 3f;
        public float startShootingDelay = 2.5f;

        [HideInInspector]
        public Enemy[,] enemiesMatrix = null;
        [HideInInspector]
        public HashSet<Enemy> enemies = null;

        private EnemySpawner _enemySpawner = null;
        private AudioClipIterator _audioClipIterator = null;
        private float _lastMovementTime = 0f;
        private float _movingIntervalReducer = 1f;
        private bool _enabled = false;
        private Vector3 _movingDirection = new Vector3(1f, 0f, 0f);
        private bool _nextMovementIsDown = false;
        private float _shootingCooldown = 0f;
        private float _lastShootingTime = 0f;
        private float _timeSinceSpawn = 0f;

        private void Awake()
        {
            _enemySpawner = GetComponent<EnemySpawner>();
            _audioClipIterator = GetComponent<AudioClipIterator>();

            EventsHub.Connect(Border.Events.Touched, OnBorderTouched);
        }

        private void Update()
        {
            if (_enabled)
            {
                Move();
                Shoot();
            }
        }

        public void OnEnemiesSpawned()
        {
            _shootingCooldown = Random.Range(0, maxShootingCooldown);
            _timeSinceSpawn = Time.time;
            _enabled = true;
        }

        private void Move()
        {
            if ((Time.time - _lastMovementTime) > (initialMovingInterval / _movingIntervalReducer))
            {
                _lastMovementTime = Time.time;

                if (!_nextMovementIsDown)
                    transform.position += movingForceHorizontal * _movingDirection * Time.deltaTime;
                else
                {
                    transform.position += movingForceVertical * Vector3.down * Time.deltaTime;
                    _nextMovementIsDown = false;
                }

                _audioClipIterator.GoToNextItem();

                foreach (var enemy in enemies)
                {
                    if (enemy.enemyEnabled)
                        enemy.Move();
                }
            }
        }

        private void Shoot()
        {
            if ((Time.time - _timeSinceSpawn) > startShootingDelay)
            {
                if ((Time.time - _lastShootingTime) > _shootingCooldown)
                {
                    _lastShootingTime = Time.time;
                    _shootingCooldown = Random.Range(0, maxShootingCooldown);

                    var shooters = enemies.Where(enemy => enemy.enemyEnabled && enemy.canShoot).ToList();
                    if (shooters.Count > 0)
                        shooters[Random.Range(0, shooters.Count)].Shoot();
                }
            }
        }

        public void OnEnemyDeath(Enemy enemy)
        {
            if (enemies.Where(e => e.enemyEnabled).Count() > 0)
            {
                if (enemy != null)
                {
                    SetNewShooter(enemy);
                    KillSameColorNeighbours(enemy);
                }
            }
            else
            {
                Restart();
                _enemySpawner.RespawnEnemies();
            }
        }

        private void SetNewShooter(Enemy enemy)
        {
            for (int i = _enemySpawner.rows - 1; i >= 0; i--)
            {
                if (enemiesMatrix[i, enemy.column].enemyEnabled)
                {
                    enemiesMatrix[i, enemy.column].canShoot = true;
                    break;
                }
            }
        }

        private void KillSameColorNeighbours(Enemy enemy)
        {
            if (enemy != null)
            {
                if (enemy.row + 1 < _enemySpawner.rows && enemiesMatrix[enemy.row + 1, enemy.column] != null && enemiesMatrix[enemy.row + 1, enemy.column].enemyEnabled && enemiesMatrix[enemy.row + 1, enemy.column].type == enemy.type)
                    enemiesMatrix[enemy.row + 1, enemy.column].GetComponent<Destroyable>().DrainFullHealth();
                if (enemy.row - 1 >= 0 && enemiesMatrix[enemy.row - 1, enemy.column] != null && enemiesMatrix[enemy.row - 1, enemy.column].enemyEnabled && enemiesMatrix[enemy.row - 1, enemy.column].type == enemy.type)
                    enemiesMatrix[enemy.row - 1, enemy.column].GetComponent<Destroyable>().DrainFullHealth();
                if (enemy.column + 1 < _enemySpawner.columns && enemiesMatrix[enemy.row, enemy.column + 1] != null && enemiesMatrix[enemy.row, enemy.column + 1].enemyEnabled && enemiesMatrix[enemy.row, enemy.column + 1].type == enemy.type)
                    enemiesMatrix[enemy.row, enemy.column + 1].GetComponent<Destroyable>().DrainFullHealth();
                if (enemy.column - 1 >= 0 && enemiesMatrix[enemy.row, enemy.column - 1] != null && enemiesMatrix[enemy.row, enemy.column - 1].enemyEnabled && enemiesMatrix[enemy.row, enemy.column - 1].type == enemy.type)
                    enemiesMatrix[enemy.row, enemy.column - 1].GetComponent<Destroyable>().DrainFullHealth();
            }
        }

        private void OnBorderTouched()
        {
            _movingDirection *= -1;
            _movingIntervalReducer += movingIntervalScaler;
            _nextMovementIsDown = true;
        }

        private void Restart()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            enemiesMatrix = null;
            enemies.Clear();
            transform.position = Vector3.zero;
            _lastMovementTime = 0f;
            _movingIntervalReducer = 1f;
            _enabled = false;
            _movingDirection = new Vector3(1f, 0f, 0f);
            _nextMovementIsDown = false;
            _shootingCooldown = 0f;
            _lastShootingTime = 0f;
        }

        private void OnDestroy()
        {
            EventsHub.Disconnect(Border.Events.Touched, OnBorderTouched);
        }
    }
}