using App.GameScene.Blocks;
using App.GameScene.Settings;
using App.GameScene.Visualization.UI;
using App.Mixed;
using App.Mixed.Visualization.UI;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.UI_Management
{
    public class ScoreController : BaseController
    {
        [SerializeField] private NumberLabel scoreLabel;
        [SerializeField] private NumberLabel bestScoreLabel;
        
        public NumberLabel ScoreLabel => scoreLabel;
        public NumberLabel BestScoreLabel => bestScoreLabel;
        
        [SerializeField] private PopLabel popLabelPrefab;
        [SerializeField] private SeriesLabel seriesLabelPrefab;
        [SerializeField] private RectTransform popLabelsContainer;
        
        [SerializeField] private ScoreControllerSettings settings;

        private int _maxCombo;
        private int _comboCount;
        private float _comboTimerDelay;
        private float _comboTimer;
        private Vector3 _labelPosition;

        private float _timeToSave;
        private float _saveTimer;
        
        public override void Init()
        {
            bestScoreLabel.ResetValue();
            bestScoreLabel.SetValueAnimated(DataRepository.BestScore);
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
                DataRepository.BestScore = bestScoreLabel.GetTargetData();
                _saveTimer = 0;
            }
        }

        private void EnrollScoreForCombo(int combo)
        {
            SpawnSeriesLabel(combo);
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
                _labelPosition = block.transform.position;
                scoreLabel.AddValueAnimated(50);
                UpdateBestScore();
                SpawnPopLabel(50.ToString());
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

        private void SpawnPopLabel(string text)
        {
            var popLabel = Instantiate(popLabelPrefab,
                _labelPosition,
                Quaternion.identity,
                popLabelsContainer);
            
            popLabel.Setup(text, 1);
        }
        
        private void SpawnSeriesLabel(int combo)
        {
            var popLabel = Instantiate(seriesLabelPrefab,
                _labelPosition,
                Quaternion.identity,
                popLabelsContainer);
            
            popLabel.Setup(combo, 2, popLabelsContainer);
        }
    }
}