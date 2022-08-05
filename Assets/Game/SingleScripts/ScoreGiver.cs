using UnityEngine;

namespace SpaceInvaders.Gameplay
{
    public class ScoreGiver : MonoBehaviour
    {
        public int score = 0;

        public struct Events
        {
            public const string Score = "ScoreGiver.Score";
        }

        public void SetScore(int score)
        {
            this.score = score;
        }

        public void ClaimScore()
        {
            EventsHub.Post(Events.Score, score);
        }
    }
}