using System;
using App.GameScene.Blocks;
using App.GameScene.Gameplay_Management;
using App.GameScene.Gameplay_Management.State;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.GameScene.Physics
{
    public class TimeController : BaseController
    {
        private float _timeScale;
        private float _currentTimeScale;

        [SerializeField] private Image freezeBg;
        [SerializeField] [Range(0, 1)] private float frozenTimeScale;
        [SerializeField] private float timeToFreeze;
        private float _freezeTimer;

        public float CurrentTimeScale
        {
            set
            {
                if (Math.Abs(_timeScale - _currentTimeScale) < 0.01f)
                {
                    _currentTimeScale = value;
                    _timeScale = _currentTimeScale;
                }
                else
                {
                    _currentTimeScale = value;
                }
            }
        }

        public float DeltaTime => Time.deltaTime * _timeScale;

        public override void Init()
        {
            CurrentTimeScale = 1;
        }

        public override void SetState(GameState newGameState)
        {
            base.SetState(newGameState);
            _timeScale = newGameState switch
            {
                GameState.Paused => 0,
                GameState.InGame => _currentTimeScale,
                GameState.GameOver => _currentTimeScale,
                _ => throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null)
            };
        }

        public void HandleBlockHit(Block block)
        {
            if (block is FreezeBlock)
            {
                _freezeTimer = timeToFreeze;
                CurrentTimeScale = frozenTimeScale;
                freezeBg.DOFade(0.5f, 0.4f);
            }
        }

        private void Update()
        {
            if (!(_freezeTimer > 0)) return;
            _freezeTimer -= Time.deltaTime * (CurrentGameState is GameState.Paused ? 0 : 1);
            if (_freezeTimer <= 0)
            {
                CurrentTimeScale = 1;
                freezeBg.DOFade(0f, 0.4f);
            }
        }
    }
}