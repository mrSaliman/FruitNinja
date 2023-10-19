using System;
using App.GameScene.Blocks;
using App.GameScene.Visualization;
using App.GameScene.Visualization.UI;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.UI_Management
{
    public class ScoreController : BaseController
    {
        [SerializeField] private NumberLabel scoreLabel;
        [SerializeField] private PopLabel popLabelPrefab;
        [SerializeField] private Transform popLabelsContainer;
        [SerializeField] private float comboTimerDelay;

        private CameraInfoProvider _cameraInfoProvider;

        private int _comboCount;
        private float _comboTimer;
        
        public override void Init()
        {
            _cameraInfoProvider = ControllerLocator.Instance.GetController<CameraInfoProvider>();
            scoreLabel.ResetValue();
            _comboCount = 0;
            _comboTimer = 0;
        }
        
        private void Update()
        {
            if (_comboTimer > 0) _comboTimer -= Time.deltaTime;

            if (_comboTimer <= 0)
            {
                _comboTimer = 0;
                if (_comboCount > 1)
                {
                    scoreLabel.AddValueAnimated(50 * _comboCount * (_comboCount - 1));
                }

                _comboCount = 0;
            }
        }

        public void HandleBlockHit(Block block)
        {
            if (block is ScoreBlock)
            {
                scoreLabel.AddValueAnimated(50);
                SpawnPopLabel(block, 50.ToString());
                _comboCount++;
                _comboTimer = comboTimerDelay;
            }
        }

        private void SpawnPopLabel(Component block, string text)
        {
            var popLabel = Instantiate(popLabelPrefab,
                block.transform.position,
                Quaternion.identity,
                popLabelsContainer);
            
            popLabel.Setup(_cameraInfoProvider.mainCamera, text, 1);
        }
    }
}