using App.GameScene.Blocks;
using App.GameScene.Settings;
using App.GameScene.Visualization.UI;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.UI_Management
{
    public class ScoreController : BaseController
    {
        [SerializeField] private NumberLabel scoreLabel;
        [SerializeField] private NumberLabel bestScoreLabel;
        [SerializeField] private PopLabel popLabelPrefab;
        [SerializeField] private Transform popLabelsContainer;
        [SerializeField] private ScoreControllerSettings settings;

        private int _maxCombo;
        private int _comboCount;
        private float _comboTimerDelay;
        private float _comboTimer;

        private float _timeToSave;
        private float _saveTimer;
        
        public override void Init()
        {
            bestScoreLabel.ResetValue();
            bestScoreLabel.SetValueAnimated(PlayerPrefs.GetFloat("BestScore", 0));
            scoreLabel.ResetValue();
            _comboCount = 0;
            _comboTimer = 0;
            _saveTimer = 0;
            _maxCombo = settings.MaxCombo;
            _comboTimerDelay = settings.ComboTimerDelay;
            _timeToSave = settings.TimeToSave;
        }
        
        private void Update()
        {
            if (_comboTimer > 0) _comboTimer -= Time.deltaTime;
            if (_saveTimer > 0) _saveTimer -= Time.deltaTime;

            if (_comboTimer < 0)
            {
                _comboTimer = 0;
                if (_comboCount > 1)
                {
                    EnrollScoreForCombo(_comboCount);
                }

                _comboCount = 0;
            }

            if (_saveTimer < 0)
            {
                PlayerPrefs.SetFloat("BestScore", bestScoreLabel.GetTargetData());
            }
        }

        private void EnrollScoreForCombo(int combo)
        {
            scoreLabel.AddValueAnimated(50 * combo * (combo - 1));
            UpdateBestScore();
        }

        private void UpdateBestScore()
        {
            var currentScore = scoreLabel.GetTargetData();
            var bestScore = bestScoreLabel.GetTargetData();

            if (currentScore > bestScore)
            {
                bestScoreLabel.SetValueAnimated(currentScore);
                _saveTimer = _timeToSave;
            }
        }

        public void HandleBlockHit(Block block)
        {
            if (block is ScoreBlock)
            {
                scoreLabel.AddValueAnimated(50);
                UpdateBestScore();
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