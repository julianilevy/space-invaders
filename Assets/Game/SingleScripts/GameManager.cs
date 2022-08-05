using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using SpaceInvaders.UI;

namespace SpaceInvaders.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public float initialDelay = 0f;
        public float timeToQuitAfterLosing = 2f;

        private int _lives = 3;
        private bool _paused = false;

        public struct Events
        {
            public const string StartGame = "GameManager.StartGame";
            public const string PauseGame = "GameManager.PauseGame";
        }

        private void Awake()
        {
            EventsHub.Connect<int>(Player.Events.LivesLost, UpdateLives);
            EventsHub.Connect(PauseUI.Events.ResumeGame, Pause);
            EventsHub.Connect(PauseUI.Events.QuitGame, Quit);
        }

        private void Start()
        {
            StartCoroutine(_StartGame());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                Pause();
        }

        private void Pause()
        {
            _paused = !_paused;

            if (_paused)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

            EventsHub.Post<bool>(Events.PauseGame, _paused);
        }

        private void Quit()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private IEnumerator _StartGame()
        {
            yield return new WaitForSeconds(initialDelay);

            EventsHub.Post(Events.StartGame);
        }

        private void UpdateLives(int livesLost)
        {
            _lives -= livesLost;
            if (_lives <= 0)
                StartCoroutine(_Reset());
        }

        private IEnumerator _Reset()
        {
            yield return new WaitForSeconds(timeToQuitAfterLosing);

            Quit();
        }

        private void OnDestroy()
        {
            EventsHub.Disconnect<int>(Player.Events.LivesLost, UpdateLives);
            EventsHub.Disconnect(PauseUI.Events.ResumeGame, Pause);
            EventsHub.Disconnect(PauseUI.Events.QuitGame, Quit);
        }
    }
}