using System.Collections.Generic;
using UnityEngine;

namespace App.GameScene.Gameplay_Management
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeReference] private List<Manager> managers;
        
        private void Awake()
        {
            foreach (var manager in managers)
            {
                manager.Init();
            }
        }
    }
}