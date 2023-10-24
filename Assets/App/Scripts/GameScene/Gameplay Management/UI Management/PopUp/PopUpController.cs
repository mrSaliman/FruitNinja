using System;
using App.GameScene.Gameplay_Management.Block_Management.Block_Interaction;
using App.GameScene.Gameplay_Management.State;
using App.GameScene.Visualization.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.GameScene.Gameplay_Management.UI_Management.PopUp
{
    public class PopUpController : BaseController
    {
        [SerializeField] private CanvasGroup popUpBg;
        [SerializeField] private CanvasGroup pausePopUp;
        [SerializeField] private Button pauseButton;

        [SerializeField] private CanvasGroup gameOverPopUp;
        [SerializeField] private NumberLabel scoreLabel;
        [SerializeField] private NumberLabel bestScoreLabel;
        
        private GameStateController _gameStateController;
        private BlockInteractionController _blockInteractionController;
        private ScoreController _scoreController;
        private GameInitializer _gameInitializer;

        private bool _waitForBlocksDown;

        public override void Init()
        {
            _waitForBlocksDown = false;
            scoreLabel.ResetValue();
            bestScoreLabel.ResetValue();
            _gameInitializer = ControllerLocator.Instance.GetController<GameInitializer>();
            _scoreController = ControllerLocator.Instance.GetController<ScoreController>();
            _gameStateController = ControllerLocator.Instance.GetController<GameStateController>();
            _blockInteractionController = ControllerLocator.Instance.GetController<BlockInteractionController>();
        }

        public override void SetState(GameState newGameState)
        {
            base.SetState(newGameState);
            switch (CurrentGameState)
            {
                case GameState.InGame:
                    UnscaledFadeCanvasGroup(popUpBg, 0, 0.3f);
                    UnscaledFadeCanvasGroup(pausePopUp, 0, 0.3f);
                    UnscaledFadeCanvasGroup(gameOverPopUp, 0, 0.3f);
                    pauseButton.interactable = true;
                    popUpBg.interactable = false;
                    OffCanvasGroup(pausePopUp);
                    OffCanvasGroup(gameOverPopUp);
                    break;
                case GameState.Paused:
                    pauseButton.interactable = false;
                    OffCanvasGroup(gameOverPopUp);
                    UnscaledFadeCanvasGroup(popUpBg, 1, 0.3f);
                    UnscaledFadeCanvasGroup(pausePopUp, 1, 0.3f).OnComplete(() =>
                    {
                        popUpBg.interactable = true;
                        OnCanvasGroup(pausePopUp);
                    });
                    break;
                case GameState.GameOver:
                    pauseButton.interactable = false;
                    OffCanvasGroup(pausePopUp);
                    UnscaledFadeCanvasGroup(pausePopUp, 0, 0.3f);
                    UnscaledFadeCanvasGroup(popUpBg, 0, 0.3f);
                    _waitForBlocksDown = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Tween UnscaledFadeCanvasGroup(CanvasGroup group, float targetAlpha, float duration)
        {
            var tween = group.DOFade(targetAlpha, duration).SetUpdate(true);
            tween.timeScale = 1f;
            return tween;
        }

        private static void OffCanvasGroup(CanvasGroup group)
        {
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        
        private static void OnCanvasGroup(CanvasGroup group)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        private void Update()
        {
            if (!_waitForBlocksDown || _blockInteractionController.BlocksCount != 0) return;
            _waitForBlocksDown = false;
            ActivateGameOverScreen();
        }

        private void ActivateGameOverScreen()
        {
            UnscaledFadeCanvasGroup(popUpBg, 1, 0.3f);
            UnscaledFadeCanvasGroup(gameOverPopUp, 1, 0.3f).OnComplete(() =>
            {
                popUpBg.interactable = true;
                scoreLabel.SetValueAnimated(_scoreController.ScoreLabel.GetTargetData());
                bestScoreLabel.SetValueAnimated(_scoreController.BestScoreLabel.GetTargetData());
                OnCanvasGroup(gameOverPopUp);
            });
        }

        public void OnPauseButtonClicked()
        {
            _gameStateController.SwitchGameState(GameState.Paused);
        }
        
        public void OnRestartButtonClicked()
        {
            _gameInitializer.Restart();
        }

        public void OnMenuButtonClicked()
        {
            // Обработка нажатия кнопки меню
        }

        public void OnContinueButtonClicked()
        {
            popUpBg.interactable = false;
            _gameStateController.SwitchGameState(GameState.InGame);
        }

    }
}