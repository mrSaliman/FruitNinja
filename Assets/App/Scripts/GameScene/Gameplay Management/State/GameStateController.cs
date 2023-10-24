using System;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.State
{
    public class GameStateController : BaseController
    {
        
        public override void Init()
        {
            SwitchGameState(GameState.Paused);
        }


        public void SwitchGameState(GameState newState)
        {
            ControllerLocator.Instance.PushGameState(newState);

            Time.timeScale = newState switch
            {
                GameState.InGame => 1,
                GameState.Paused => 0,
                GameState.GameOver => 1,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}