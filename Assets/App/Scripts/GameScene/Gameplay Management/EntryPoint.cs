using System.Collections.Generic;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeReference] private List<Manager> managers;
        
        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            foreach (var manager in managers)
            {
                manager.Init();
            }
        }
    }
}