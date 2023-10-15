using App.GameScene.Gameplay_Management.Block_Management;
using App.GameScene.Visualization;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Input_Management
{
    public class TouchManager : Manager
    {
        [SerializeReference] private CameraManager cameraManager;

        [SerializeField] private TouchManagerSettings settings;
        private float _minSliceSpeed;
        private float _deathLineThickness;
        private float _deathLineLifetime;

        [SerializeReference] private BlockInteractionManager blockInteractionManager;

        [SerializeReference] private TrailHandler trailHandler;

        private DeathLine _currentDeathLine;

        private bool _isMoving;
        private Vector2 _previousPosition;
        
        public override void Init()
        {
            _minSliceSpeed = settings.MinSliceSpeed;
            _deathLineThickness = settings.DeathLineThickness;
            trailHandler.cameraManager = cameraManager;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && _isMoving)
            {
                Vector2 currentPosition = Input.mousePosition;
                var speed = (currentPosition - _previousPosition).magnitude / Time.deltaTime;

                if (speed >= _minSliceSpeed)
                {
                    var deathLineFrom = cameraManager.mainCamera.ScreenToWorldPoint(_previousPosition);
                    var deathLineTo = cameraManager.mainCamera.ScreenToWorldPoint(currentPosition);

                    blockInteractionManager.HandleDeathLine(
                        new DeathLine(deathLineFrom, deathLineTo, _deathLineThickness));
                }

                _previousPosition = currentPosition;
                trailHandler.MoveTo(currentPosition);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                _isMoving = true;
                _previousPosition = Input.mousePosition;
                trailHandler.TeleportTo(_previousPosition);
            }
            else
            {
                _isMoving = false;
            }
        }
    }
}