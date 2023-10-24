using System;

namespace App.GameScene.Gameplay_Management.State
{
    [Serializable]
    public enum GameState
    {
        InGame,
        Paused, 
        GameOver
    }
}