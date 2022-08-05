using UnityEditor;

namespace SpaceInvaders.Tools
{
    [CustomEditor(typeof(AnimatorOverrideControllerIterator))]
    public class AnimatorOverrideControllerIteratorEditor : GenericIteratorEditor
    {
        private AnimatorOverrideControllerIterator _target;

        protected override void OnEnable()
        {
            base.OnEnable();

            _target = (AnimatorOverrideControllerIterator)target;
        }

        public override void OnInspectorGUI()
        {
            OnInspectorGUI(_target);
        }
    }
}