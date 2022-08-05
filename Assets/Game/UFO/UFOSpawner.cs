using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public class UFOSpawner : MonoBehaviour
    {
        public UFO ufoPrefab = null;
        public Transform rightSpawnPoint = null;
        public Transform leftSpawnPoint = null;
        public float minSpawnCooldown = 15f;
        public float maxSpawnCooldown = 30f;
        public float ufoSpeed = 0.5f;

        private bool _enabled = false;
        private float _spawnCooldown = 0f;
        private float _lastSpawnTime = 0f;

        private void Awake()
        {
            EventsHub.Connect(GameManager.Events.StartGame, StartSpawningUFOs);
        }

        private void Update()
        {
            if (_enabled)
                SpawnUFO();
        }

        private void StartSpawningUFOs()
        {
            _enabled = true;
            SetSpawnTime();
        }

        public void SpawnUFO()
        {
            if ((Time.time - _lastSpawnTime) > _spawnCooldown)
            {
                var newUFO = Instantiate(ufoPrefab);

                var randomSpawnSide = Random.Range(0, 2);
                if (Random.Range(0, 2) == 0)
                    newUFO.StartMoving(rightSpawnPoint.position, ufoSpeed, Vector3.left, 30f);
                else
                    newUFO.StartMoving(leftSpawnPoint.position, ufoSpeed, Vector3.right, 30f);

                SetSpawnTime();
            }
        }

        private void SetSpawnTime()
        {
            _spawnCooldown = Random.Range(minSpawnCooldown, maxSpawnCooldown);
            _lastSpawnTime = Time.time;
        }

        private void OnDestroy()
        {
            EventsHub.Disconnect(GameManager.Events.StartGame, StartSpawningUFOs);
        }
    }
}