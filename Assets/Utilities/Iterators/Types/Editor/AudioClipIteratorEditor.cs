using UnityEditor;

namespace SpaceInvaders.Tools
{
    [CustomEditor(typeof(AudioClipIterator))]
    public class AudioClipIteratorEditor : GenericIteratorEditor
    {
        private AudioClipIterator _target;

        protected override void OnEnable()
        {
            base.OnEnable();

            _target = (AudioClipIterator)target;
        }

        public override void OnInspectorGUI()
        {
            OnInspectorGUI(_target);
        }
    }
}