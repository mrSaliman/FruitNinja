using System.Collections.Generic;
using App.GameScene.Visualization;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeReference] private List<Manager> managers;
        [SerializeReference] private Camera mainCamera;

        private CameraManager _cameraManager;

        private void Reset()
        {
            _cameraManager = new CameraManager(mainCamera);

            foreach (var manager in managers)
            {
                manager.Init(_cameraManager);
            }
        }

        private void Awake()
        {
            _cameraManager = new CameraManager(mainCamera);

            foreach (var manager in managers)
            {
                manager.Init(_cameraManager);
            }
        }
    }
}