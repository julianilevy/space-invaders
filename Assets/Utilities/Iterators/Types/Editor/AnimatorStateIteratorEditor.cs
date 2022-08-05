using UnityEditor;

namespace SpaceInvaders.Tools
{
    [CustomEditor(typeof(AnimatorStateIterator))]
    public class AnimatorStateIteratorEditor : GenericIteratorEditor
    {
        private AnimatorStateIterator _target;

        protected override void OnEnable()
        {
            base.OnEnable();

            _target = (AnimatorStateIterator)target;
        }

        public override void OnInspectorGUI()
        {
            OnInspectorGUI(_target);
        }
    }
}