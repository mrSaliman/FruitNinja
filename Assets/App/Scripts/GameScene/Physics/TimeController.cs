using System;
using App.GameScene.Gameplay_Management;
using App.GameScene.Gameplay_Management.State;
using UnityEngine;

namespace App.GameScene.Physics
{
    public class TimeController : BaseController
    {
        private float _timeScale;

        private float _currentTimeScale;

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
    }
}