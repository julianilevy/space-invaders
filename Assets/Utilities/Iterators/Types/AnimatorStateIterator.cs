using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorStateIterator : GenericIterator<string>
    {
        private Animator _animator = null;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.Play(orderedItems[0]);
        }

        protected override void OnGoToNextItem(int index)
        {
            _animator.Play(orderedItems[index]);
        }

        protected override void OnGoToLastItem()
        {
            _animator.Play(lastItem);
        }

        protected override void OnRestart()
        {
            _animator.Play(orderedItems[0]);
        }
    }
}