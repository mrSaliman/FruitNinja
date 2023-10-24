using App.GameScene.Gameplay_Management.Block_Management;
using App.GameScene.Gameplay_Management.Block_Management.Block_Interaction;
using App.GameScene.Gameplay_Management.State;
using App.GameScene.Settings;
using App.GameScene.Visualization;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.GameScene.Gameplay_Management.Input_Management
{
    public class InputController : BaseController
    {
        private CameraInfoProvider _cameraInfoProvider;

        [SerializeField] private InputControllerSettings settings;
        private float _minSliceSpeed;
        private float _minTravelDistance;
        private float _deathLineThickness;

        private BlockInteractionController _blockInteractionController;

        [SerializeReference] private TrailHandler trailHandler;

        private DeathLine _currentDeathLine;

        private bool _isMoving;
        private float _travelDistance;
        private Vector2 _previousPosition;

        public override void Init()
        {
            _cameraInfoProvider = ControllerLocator.Instance.GetController<CameraInfoProvider>();
            _blockInteractionController = ControllerLocator.Instance.GetController<BlockInteractionController>();
            _minSliceSpeed = settings.MinSliceSpeed;
            _minTravelDistance = settings.MinTravelDistance;
            _deathLineThickness = settings.DeathLineThickness;
            trailHandler.cameraInfoProvider = _cameraInfoProvider;
            _isMoving = false;
        }

        private void Update()
        {
            if (CurrentGameState is GameState.Paused or GameState.GameOver) return;
            if (Input.GetMouseButton(0) && _isMoving)
            {
                Vector2 currentPosition = Input.mousePosition;
                var distance = (currentPosition - _previousPosition).magnitude;
                _travelDistance += distance;
                var speed = distance / Time.deltaTime;

                if (speed >= _minSliceSpeed && _travelDistance >= _minTravelDistance)
                {
                    var deathLineFrom = _cameraInfoProvider.mainCamera.ScreenToWorldPoint(_previousPosition);
                    var deathLineTo = _cameraInfoProvider.mainCamera.ScreenToWorldPoint(currentPosition);

                    _blockInteractionController.HandleDeathLine(
                        new DeathLine(deathLineFrom, deathLineTo, _deathLineThickness, speed / 100f));
                }

                _previousPosition = currentPosition;
                trailHandler.MoveTo(currentPosition);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                _travelDistance = 0;
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