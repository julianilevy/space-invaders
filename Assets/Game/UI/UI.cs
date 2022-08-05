using UnityEngine;
using TMPro;
using System.Collections.Generic;
using SpaceInvaders.Gameplay;

namespace SpaceInvaders.UI
{
    public class UI : MonoBehaviour
    {
        public List<GameObject> lives =  new List<GameObject>();
        public TextMeshProUGUI scoreText = null;

        private int _lives = 3;
        private int _score = 0;

        private void Awake()
        {
            Time.timeScale = 1;

            EventsHub.Connect<int>(ScoreGiver.Events.Score, UpdateScore);
            EventsHub.Connect<int>(Player.Events.LivesLost, UpdateLives);
        }

        private void UpdateScore(int scoreAdded)
        {
            _score += scoreAdded;
            scoreText.text = _score.ToString();
        }

        private void UpdateLives(int livesLost)
        {
            _lives -= livesLost;
            if (livesLost == 1)
                lives[_lives].SetActive(false);
            else if(livesLost >= 3)
            {
                foreach (var life in lives)
                    life.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            EventsHub.Disconnect<int>(ScoreGiver.Events.Score, UpdateScore);
            EventsHub.Disconnect<int>(Player.Events.LivesLost, UpdateLives);
        }
    }
}