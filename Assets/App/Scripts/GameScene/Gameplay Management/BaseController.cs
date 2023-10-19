using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public abstract class BaseController : MonoBehaviour
    {
        public abstract void Init();

        public void RegisterInLocator()
        {
            ControllerLocator.Instance.RegisterController(this);
        }
    }
}