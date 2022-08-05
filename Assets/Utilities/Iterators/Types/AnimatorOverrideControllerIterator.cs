using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorOverrideControllerIterator : GenericIterator<AnimatorOverrideController>
    {
        private Animator _animator = null;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = orderedItems[0];
        }

        protected override void OnGoToNextItem(int index)
        {
            _animator.runtimeAnimatorController = orderedItems[index];
        }

        protected override void OnGoToLastItem()
        {
            _animator.runtimeAnimatorController = lastItem;
        }

        protected override void OnRestart()
        {
            _animator.runtimeAnimatorController = orderedItems[0];
        }
    }
}