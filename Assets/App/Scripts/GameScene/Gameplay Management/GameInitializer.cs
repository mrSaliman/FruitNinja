using System.Collections.Generic;
using App.GameScene.Gameplay_Management.State;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public class GameInitializer : BaseController
    {
        [SerializeReference] private List<BaseController> controllers;
        
        private void Awake()
        {
            Init();
        }
        
        public override void Init()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            RegisterControllers();
            ControllerLocator.Instance.PushGameState(GameState.Paused);
            InitControllers();
            ControllerLocator.Instance.PushGameState(GameState.InGame);
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
            ControllerLocator.Instance.PushGameState(GameState.InGame);
        }
    }
}