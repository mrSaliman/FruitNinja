using System.Collections.Generic;
using App.GameScene.Visualization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.GameScene.Gameplay_Management
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeReference] private List<Manager> managers;

        [SerializeReference] public CameraManager cameraManager;
        
        private void Awake()
        {
            foreach (var manager in managers)
            {
                manager.Init();
            }
        }
    }
}