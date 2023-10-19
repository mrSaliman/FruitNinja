using System.Collections.Generic;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeReference] private List<BaseController> controllers;
        
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            foreach (var manager in controllers)
            {
                manager.RegisterInLocator();
            }
            
            foreach (var manager in controllers)
            {
                manager.Init();
            }
        }
    }
}