using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blocks
{
    public class Thrower : MonoBehaviour
    {
        [SerializeReference]
        private Camera mainCamera;
        private float _cameraHeight;
        private float _cameraWidth;
        private const float Gravity = -9.81f;
        public List<ThrowZone> throwZones = new List<ThrowZone>();

        private void Awake()
        {
            _cameraHeight = 2f * mainCamera.orthographicSize;
            _cameraWidth = _cameraHeight * mainCamera.aspect;
        }

        public void AddNewThrowZone()
        {
            var newThrowZoneObject = new GameObject("ThrowZone");
            newThrowZoneObject.transform.parent = transform;
            var newThrowZone = newThrowZoneObject.AddComponent<ThrowZone>();
            newThrowZone.mainCamera = mainCamera;

            throwZones.Add(newThrowZone);
        }

        public void Throw(Block block, ThrowZone throwZone)
        {
            var randomPoint = GenerateRandomPoint(throwZone);
            var launchAngle = GenerateRandomLaunchAngle(throwZone);
            
        }

        private Vector2 GenerateRandomPoint(ThrowZone throwZone)
        {   
            var center = new Vector2(
                throwZone.xIndentation * _cameraWidth - _cameraWidth / 2f, 
                throwZone.yIndentation * _cameraHeight - _cameraHeight / 2f);
            
            var randomT = Random.Range(-1f, 1f);
            var radians = throwZone.platformAngle * Mathf.Deg2Rad;
            var offset = new Vector2(Mathf.Cos(radians) * randomT * throwZone.radius, Mathf.Sin(radians) * randomT * throwZone.radius);
            
            return center + offset;
        }

        private float GenerateRandomLaunchAngle(ThrowZone throwZone)
        {
            var minLaunchAngle = throwZone.startThrowAngle + throwZone.platformAngle;
            var maxLaunchAngle = throwZone.endThrowAngle + throwZone.platformAngle;
            return Random.Range(minLaunchAngle, maxLaunchAngle);
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(Thrower))]
    internal class ThrowerEditor : Editor
    {
        private bool _showThrowZonesFoldout = true;
        
        public override void OnInspectorGUI()
        {
            var thrower = (Thrower)target;

            if (GUILayout.Button("Add New ThrowZone"))
            {
                thrower.AddNewThrowZone();
            }
            
            if (thrower.throwZones.Count > 0 && GUILayout.Button("Remove Last ThrowZone"))
            {
                var lastIndex = thrower.throwZones.Count - 1;
                DestroyImmediate(thrower.throwZones[lastIndex].gameObject);
                thrower.throwZones.RemoveAt(lastIndex);
            }
            
            _showThrowZonesFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_showThrowZonesFoldout, "ThrowZones");
            if (_showThrowZonesFoldout)
            {
                EditorGUI.indentLevel++;
                foreach (var throwZone in thrower.throwZones)
                {
                    EditorGUILayout.LabelField("ThrowZone Settings");
                    EditorGUI.indentLevel++;

                    DrawThrowZoneEditor(throwZone);

                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void DrawThrowZoneEditor(ThrowZone throwZone)
        {
            throwZone.xIndentation = EditorGUILayout.Slider("XIndentation", throwZone.xIndentation, 0, 1);
            throwZone.yIndentation = EditorGUILayout.Slider("YIndentation", throwZone.yIndentation, 0, 1);

            EditorGUI.BeginChangeCheck();
            throwZone.startThrowAngle = EditorGUILayout.Slider("StartThrowAngle", throwZone.startThrowAngle, 0, 180);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.endThrowAngle < throwZone.startThrowAngle)
                    throwZone.endThrowAngle = throwZone.startThrowAngle;
            }
            
            EditorGUI.BeginChangeCheck();
            throwZone.endThrowAngle = EditorGUILayout.Slider("EndThrowAngle", throwZone.endThrowAngle, 0, 180);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.endThrowAngle < throwZone.startThrowAngle)
                    throwZone.startThrowAngle = throwZone.endThrowAngle;
            }
            
            throwZone.platformAngle = EditorGUILayout.Slider("PlatformAngle", throwZone.platformAngle, -180, 180);

            EditorGUI.BeginChangeCheck();
            throwZone.radius = EditorGUILayout.FloatField("Radius", throwZone.radius);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.radius < 0) throwZone.radius = 0;
            }
            
            EditorApplication.QueuePlayerLoopUpdate();
        }
    }
    #endif
}