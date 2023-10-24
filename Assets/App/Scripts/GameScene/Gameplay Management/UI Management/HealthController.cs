using App.GameScene.Blocks;
using App.GameScene.Gameplay_Management.State;
using App.GameScene.Settings;
using App.GameScene.Visualization.UI;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.UI_Management
{
    public class HealthController : BaseController
    {
        [SerializeField] private HealthControllerSettings settings;
        private int _maxHp;

        private int _currentHp;

        [SerializeField] private HealthPointsContainer hpContainer;

        private GameStateController _gameStateController;
        
        public override void Init()
        {
            _gameStateController = ControllerLocator.Instance.GetController<GameStateController>();
            _maxHp = settings.MaxHp;
            _currentHp = settings.StartHp;
            ValidHp();
            hpContainer.Setup(settings.HpSprite, settings.Spacing, settings.MaxHpInRow, _currentHp);
        }

        public void HandleBlockMiss(Block block)
        {
            if (block is ScoreBlock)
            {
                AddHp(-1);
            }
        }

        private void SetHp(int hp)
        {
            _currentHp = hp;
            ValidHp();
            UpdateContainer();
            if (_currentHp == 0 && CurrentGameState != GameState.GameOver) _gameStateController.SwitchGameState(GameState.GameOver);
        }

        private void AddHp(int hp)
        {
            SetHp(_currentHp + hp);
        }

        private void ValidHp()
        {
            _currentHp = Mathf.Max(Mathf.Min(_currentHp, _maxHp), 0);
        }

        private void UpdateContainer()
        {
            hpContainer.UpdateContent(_currentHp);
        }
    }
}