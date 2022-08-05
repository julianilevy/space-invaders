using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceInvaders.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void OnPlayButton()
        {
            SceneManager.LoadScene("SpaceInvaders");
        }

        public void OnQuitButton()
        {
            Application.Quit();
        }
    }
}