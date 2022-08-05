using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    [RequireComponent(typeof(EnemyManager))]
    public class EnemySpawner : MonoBehaviour
    {
        public List<Enemy> enemyPrefabs = new List<Enemy>();
        public int rows = 4;
        public int columns = 10;
        public Vector2 padding = new Vector2(0.03f, 0.08f);
        public Vector2 enemyBlockOffset = new Vector2(0f, 0.4f);
        public float respawnCooldown = 2f;

        private EnemyManager _enemyManager = null;
        private int _baseScorePerEnemy = 10;

        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();

            EventsHub.Connect(GameManager.Events.StartGame, SpawnEnemies);
        }

        public void SpawnEnemies()
        {
            _enemyManager.enemiesMatrix = new Enemy[rows, columns];
            _enemyManager.enemies = new HashSet<Enemy>();

            var currentNumber = 1;
            var previousPositionY = 0f;

            for (int i = 0; i < rows; i++)
            {
                Enemy newEnemy = null;

                var rowCurrentEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                var previousPositionX = 0f;

                for (int j = 0; j < columns; j++)
                {
                    newEnemy = Instantiate(rowCurrentEnemyPrefab);
                    newEnemy.transform.SetParent(transform);
                    newEnemy.transform.position = new Vector3(previousPositionX, previousPositionY, 0f);
                    newEnemy.GetComponent<Destroyable>().SetHealth(Random.Range(1, 3));
                    newEnemy.GetComponent<ScoreGiver>().SetScore(_baseScorePerEnemy * (rows - i));
                    newEnemy.row = i;
                    newEnemy.column = j;
                    newEnemy.EnemyDeath += _enemyManager.OnEnemyDeath;
                    if (i == rows - 1)
                        newEnemy.canShoot = true;

                    _enemyManager.enemiesMatrix[i, j] = newEnemy;
                    _enemyManager.enemies.Add(newEnemy);

                    previousPositionX += newEnemy.GetBounds().size.x + padding.x;
                    currentNumber++;
                }

                previousPositionY -= newEnemy.GetBounds().size.y + padding.y;
            }

            CenterToWorld();

            _enemyManager.OnEnemiesSpawned();
        }

        private void CenterToWorld()
        {
            var enemyBlockBounds = gameObject.GetBounds();
            transform.position = new Vector3(-enemyBlockBounds.center.x, -enemyBlockBounds.center.y, 0f);
            transform.position += new Vector3(enemyBlockOffset.x, enemyBlockOffset.y, 0f);
        }

        public void RespawnEnemies()
        {
            StartCoroutine(_RespawnEnemies());
        }

        private IEnumerator _RespawnEnemies()
        {
            yield return new WaitForSeconds(respawnCooldown);

            SpawnEnemies();
        }

        private void OnDestroy()
        {
            EventsHub.Disconnect(GameManager.Events.StartGame, SpawnEnemies);
        }
    }
}