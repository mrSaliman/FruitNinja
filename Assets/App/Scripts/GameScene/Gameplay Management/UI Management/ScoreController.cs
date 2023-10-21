using App.GameScene.Blocks;
using App.GameScene.Settings;
using App.GameScene.Visualization.UI;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.UI_Management
{
    public class ScoreController : BaseController
    {
        [SerializeField] private NumberLabel scoreLabel;
        [SerializeField] private PopLabel popLabelPrefab;
        [SerializeField] private Transform popLabelsContainer;
        [SerializeField] private ScoreControllerSettings settings;

        private int _maxCombo;
        private float _comboTimerDelay;
        private int _comboCount;
        private float _comboTimer;
        
        public override void Init()
        {
            scoreLabel.ResetValue();
            _comboCount = 0;
            _comboTimer = 0;
            _maxCombo = settings.MaxCombo;
            _comboTimerDelay = settings.ComboTimerDelay;
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

        private void EnrollScoreForCombo(int combo)
        {
            scoreLabel.AddValueAnimated(50 * combo * (combo - 1));
        }

        public void HandleBlockHit(Block block)
        {
            if (block is ScoreBlock)
            {
                scoreLabel.AddValueAnimated(50);
                SpawnPopLabel(block, 50.ToString());
                _comboCount++;
                _comboTimer = _comboTimerDelay;
                if (_comboCount > _maxCombo)
                {
                    EnrollScoreForCombo(_maxCombo);
                    _comboTimer = 0;
                    _comboCount = 0;
                }
            }
        }

        private void SpawnPopLabel(Component block, string text)
        {
            var popLabel = Instantiate(popLabelPrefab,
                block.transform.position,
                Quaternion.identity,
                popLabelsContainer);
            
            popLabel.Setup(text, 1);
        }
    }
}