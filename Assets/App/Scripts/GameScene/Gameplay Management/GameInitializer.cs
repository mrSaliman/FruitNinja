using System.Collections.Generic;
using App.GameScene.Gameplay_Management.State;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public class GameInitializer : BaseController
    {
        [SerializeReference] private List<BaseController> controllers;

        private GameStateController _gameStateController;
        
        private void Awake()
        {
            Init();
        }
        
        public override void Init()
        {
            Time.timeScale = 0f;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            RegisterControllers();
            InitControllers();
            _gameStateController = ControllerLocator.Instance.GetController<GameStateController>();
            _gameStateController.SwitchGameState(GameState.InGame);
        }

        private void RegisterControllers()
        {
            foreach (var controller in controllers)
            {
                controller.RegisterInLocator();
            }
            RegisterInLocator();
        }

        private void InitControllers()
        {
            foreach (var controller in controllers)
            {
                controller.Init();
            }
        }

        public void Restart()
        {
            InitControllers();
            _gameStateController.SwitchGameState(GameState.InGame);
        }
    }
}