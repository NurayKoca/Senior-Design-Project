using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Workspace.Scripts.MainMenu
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            startButton.onClick.AddListener(StartGame);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(StartGame);
            exitButton.onClick.RemoveListener(ExitGame);
        }

        private void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
}