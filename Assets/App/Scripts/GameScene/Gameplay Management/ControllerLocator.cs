using System.Collections.Generic;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public class ControllerLocator : MonoBehaviour
    {
        private static ControllerLocator _instance;
        private readonly Dictionary<System.Type, BaseController> _managers = new();

        public static ControllerLocator Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<ControllerLocator>();
                if (_instance != null) return _instance;
                
                var singletonObject = new GameObject("ManagerLocator");
                _instance = singletonObject.AddComponent<ControllerLocator>();
                return _instance;
            }
        }

        public T GetController<T>() where T : BaseController
        {
            if (_managers.ContainsKey(typeof(T)))
            {
                return (T)_managers[typeof(T)];
            }

            Debug.LogError($"Manager of type {typeof(T)} not found.");
            return null;
        }

        public void RegisterController(BaseController baseController)
        {
            var type = baseController.GetType();
            _managers.TryAdd(type, baseController);
        }
    }

}