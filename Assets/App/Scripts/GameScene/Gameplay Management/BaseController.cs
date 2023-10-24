using App.GameScene.Gameplay_Management.State;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public abstract class BaseController : MonoBehaviour
    {
        protected GameState CurrentGameState;
        public abstract void Init();

        public void RegisterInLocator()
        {
            ControllerLocator.Instance.RegisterController(this);
        }

        public virtual void SetState(GameState newGameState)
        {
            CurrentGameState = newGameState;
        }
    }
}