using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Colors", menuName = "Space Invaders/Colors")]
    public class Colors : ScriptableObject
    {
        public Color red;
        public Color blue;
        public Color green;
        public Color yellow;
    }
}