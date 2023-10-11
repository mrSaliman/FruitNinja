using System.Collections.Generic;
using App.GameScene.Visualization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.GameScene.Gameplay_Management
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeReference] private List<Manager> managers;

        [SerializeReference] public CameraManager cameraManager;
        
        private void Awake()
        {
            foreach (var manager in managers)
            {
                manager.Init();
            }
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(EntryPoint))]
    internal class EntryPointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var entryPoint = (EntryPoint)target;

            DrawDefaultInspector();

            entryPoint.cameraManager.camera = (Camera)EditorGUILayout.ObjectField("Camera Reference", entryPoint.cameraManager.camera, typeof(Camera), true);
        }
    }
    #endif
    
}