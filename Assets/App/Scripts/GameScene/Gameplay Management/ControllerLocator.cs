using System.Collections.Generic;
using App.GameScene.Gameplay_Management.State;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public class ControllerLocator : MonoBehaviour
    {
        private static ControllerLocator _instance;
        private readonly Dictionary<System.Type, BaseController> _controllers = new();

        public static ControllerLocator Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<ControllerLocator>();
                if (_instance != null) return _instance;
                
                var singletonObject = new GameObject("ControllerLocator");
                _instance = singletonObject.AddComponent<ControllerLocator>();
                return _instance;
            }
        }

        public T GetController<T>() where T : BaseController
        {
            if (_controllers.ContainsKey(typeof(T)))
            {
                return (T)_controllers[typeof(T)];
            }

            Debug.LogError($"Controller of type {typeof(T)} not found.");
            return null;
        }

        public void RegisterController(BaseController baseController)
        {
            var type = baseController.GetType();
            _controllers.TryAdd(type, baseController);
        }

        public void PushGameState(GameState newGameState)
        {
            foreach (var controller in _controllers.Values)
            {
                controller.SetState(newGameState);
            }
        }
    }

}