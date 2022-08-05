using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    [RequireComponent(typeof(Destroyable))]
    public class BarrierPart : MonoBehaviour
    {
        private AnimatorStateIterator _animatorStateIterator = null;
        private Destroyable _destroyable = null;

        private void Awake()
        {
            _animatorStateIterator = GetComponentInChildren<AnimatorStateIterator>();

            _destroyable = GetComponent<Destroyable>();
            _destroyable.Damaged += OnDamaged;
            _destroyable.HealthDrained += OnHealthDrained;
        }

        #region Destroyable

        public void OnDamaged()
        {
            _animatorStateIterator.GoToNextItem();
        }

        public void OnHealthDrained()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}