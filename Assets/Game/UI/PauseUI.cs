using UnityEngine;
using SpaceInvaders.Gameplay;

namespace SpaceInvaders.UI
{
    public class PauseUI : MonoBehaviour
    {
        public Canvas canvas;

        public struct Events
        {
            public const string ResumeGame = "PauseUI.ResumeGame";
            public const string QuitGame = "PauseUI.QuitGame";
        }

        private void Awake()
        {
            EventsHub.Connect<bool>(GameManager.Events.PauseGame, OnGamePaused);
        }
        
        private void OnGamePaused(bool paused)
        {
            canvas.gameObject.SetActive(paused);
        }

        public void OnResumeGame()
        {
            EventsHub.Post(Events.ResumeGame);
        }

        public void OnQuitGame()
        {
            EventsHub.Post(Events.QuitGame);
        }

        private void OnDestroy()
        {
            EventsHub.Disconnect<bool>(GameManager.Events.PauseGame, OnGamePaused);
        }
    }
}