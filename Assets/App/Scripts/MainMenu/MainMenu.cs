using App.Mixed;
using App.Mixed.Visualization.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NumberLabel bestScoreLabel;
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private SceneTransition sceneTransition;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            Time.timeScale = 1;
            bestScoreLabel.ResetValue();
            bestScoreLabel.SetValueAnimated(DataRepository.BestScore);
            playButton.interactable = true;
            exitButton.interactable = true;
        }

        private void DeactivateButtons()
        {
            playButton.interactable = false;
            exitButton.interactable = false;
        }

        public void OnPlayButtonClicked()
        {
            sceneTransition.SwitchToScene("GameScene");
            DeactivateButtons();
        }

        public void OnExitButtonClocked()
        {
            Application.Quit();
            DeactivateButtons();
        }
    }
}