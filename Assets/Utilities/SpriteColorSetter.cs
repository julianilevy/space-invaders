using UnityEngine;
using System.Collections.Generic;

namespace SpaceInvaders
{
    [ExecuteInEditMode]
    public class SpriteColorSetter : MonoBehaviour
    {
        public Color color = Color.black;

        private HashSet<SpriteRenderer> _allSprites;

        private void Update()
        {
            if (!Application.isPlaying)
                SetColor();
        }

        private void SetColor()
        {
            if (_allSprites == null)
                _allSprites = new HashSet<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());

            foreach (var sprite in _allSprites)
                sprite.color = color;
        }
    }
}