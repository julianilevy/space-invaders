using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SpaceInvaders.Tools
{
    public abstract class GenericIteratorEditor : Editor
    {
        private SerializedProperty _orderedItems;
        private SerializedProperty _lastItem;

        protected virtual void OnEnable()
        {
            _orderedItems = serializedObject.FindProperty("orderedItems");
            _lastItem = serializedObject.FindProperty("lastItem");
        }

        protected void OnInspectorGUI<T>(GenericIterator<T> genericIterator) where T : class
        {
            GUILayout.Space(5);

            serializedObject.Update();

            EditorGUILayout.PropertyField(_orderedItems, true);

            genericIterator.cyclic = GUILayout.Toggle(genericIterator.cyclic, "Cyclic");

            genericIterator.lastItemEnabled = GUILayout.Toggle(genericIterator.lastItemEnabled, "Last Item Enabled");

            if (genericIterator.lastItemEnabled)
                EditorGUILayout.PropertyField(_lastItem);

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(genericIterator);
                EditorSceneManager.MarkSceneDirty(genericIterator.gameObject.scene);
            }

            GUILayout.Space(5);
        }
    }
}