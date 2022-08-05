using System.Collections;
using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    [RequireComponent(typeof(Destroyable))]
    [RequireComponent(typeof(ScoreGiver))]
    public class UFO : MonoBehaviour
    {
        private AnimatorStateIterator _animatorStateIterator = null;
        private Destroyable _destroyable = null;
        private bool _enabled = false;
        private float _speed = 0f;
        private Vector3 _direction = Vector3.zero;
        private float _lifeTime = 0f;
        private float _lifeTimeElapsed = 0f;
        private float _timeToDisappearAfterDeath = 0.4f;

        private void Awake()
        {
            _animatorStateIterator = GetComponentInChildren<AnimatorStateIterator>();

            _destroyable = GetComponent<Destroyable>();
            _destroyable.HealthDrained += OnHealthDrained;
        }

        private void Update()
        {
            if (_enabled)
            {
                _lifeTimeElapsed += Time.deltaTime;
                if (_lifeTimeElapsed >= _lifeTime)
                    Destroy(gameObject);

                transform.position += _speed * _direction * Time.deltaTime;
            }
        }


        public void StartMoving(Vector3 startPosition, float speed, Vector3 direction, float lifeTime)
        {
            transform.position = startPosition;
            _speed = speed;
            _direction = direction;
            _lifeTime = lifeTime;
            _enabled = true;
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
            GetComponent<ScoreGiver>().ClaimScore();
            Destroy(GetComponent<Collider2D>());

            yield return new WaitForSeconds(_timeToDisappearAfterDeath);

            Destroy(gameObject);
        }

        #endregion
    }
}