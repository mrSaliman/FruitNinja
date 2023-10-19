using System;
using App.GameScene.Blocks;
using App.GameScene.Visualization.UI;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.UI_Management
{
    public class ScoreController : BaseController
    {
        [SerializeField] private NumberLabel scoreLabel;
        [SerializeField] private float comboTimerDelay;

        private int _comboCount;
        private float _comboTimer;
        
        public override void Init()
        {
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
                _comboCount++;
                _comboTimer = comboTimerDelay;
            }
        }
    }
}