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
        private int _targetHp;

        [SerializeField] private HealthPointsContainer hpContainer;

        private GameStateController _gameStateController;
        
        public override void Init()
        {
            _gameStateController = ControllerLocator.Instance.GetController<GameStateController>();
            _maxHp = settings.MaxHp;
            _currentHp = settings.StartHp;
            _targetHp = _currentHp;
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

        public void HandleBlockHit(Block block)
        {
            switch (block)
            {
                case Bomb:
                    AddHp(-1);
                    break;
                case HealthBlock:
                    if (_currentHp + 1 <= _maxHp)
                    {
                        var tween = hpContainer.AnimateHpFlight(block.transform.position, _targetHp);
                        tween.onComplete += () =>
                        {
                            if (_currentHp < _targetHp) _currentHp++;
                            hpContainer.UpdateContent(_currentHp, 0);
                        };
                        _targetHp++;
                    }
                    break;
            }
        }

        private void SetHp(int hp)
        {
            _targetHp = hp;
            _currentHp = hp;
            ValidHp();
            UpdateContainer();
            if (_targetHp == 0 && CurrentGameState != GameState.GameOver) _gameStateController.SwitchGameState(GameState.GameOver);
        }

        private void AddHp(int hp)
        {
            SetHp(_targetHp + hp);
        }

        private void ValidHp()
        {
            _targetHp = Mathf.Max(Mathf.Min(_targetHp, _maxHp), 0);
        }

        private void UpdateContainer()
        {
            hpContainer.UpdateContent(_currentHp, 1);
        }
    }
}