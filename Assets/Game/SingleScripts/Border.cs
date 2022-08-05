using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public class Border : MonoBehaviour
    {
        private float _lastTouchedTime = 0f;

        public struct Events
        {
            public const string Touched = "Border.Touched";
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == Layer.Destroyable)
            {
                if ((Time.time - _lastTouchedTime) > 1f)
                {
                    if (collision.gameObject.GetComponent<Enemy>())
                    {
                        _lastTouchedTime = Time.time;
                        EventsHub.Post(Events.Touched);
                    }
                }
            }
        }
    }
}