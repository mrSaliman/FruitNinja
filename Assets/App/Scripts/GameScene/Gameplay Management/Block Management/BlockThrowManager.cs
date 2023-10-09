using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.GameScene.Blocks;
using UnityEditor;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Block_Management
{
    public class BlockThrowManager : MonoBehaviour
    {
        [SerializeReference] private Camera mainCamera;
        private float _cameraHeight;
        private float _cameraWidth;

        [SerializeReference] private Thrower thrower;

        [SerializeField] private BlockThrowManagerSettings settings;

        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private List<Block> prefabs;
        
        [HideInInspector] public List<ThrowZone> throwZones = new List<ThrowZone>();

        private readonly List<Block> _currentPack = new();

        private bool _stop;
        
        private void Awake()
        {
            _cameraHeight = 2f * mainCamera.orthographicSize;
            _cameraWidth = _cameraHeight * mainCamera.aspect;

            thrower.cameraHeight = _cameraHeight;
            thrower.cameraWidth = _cameraWidth;

            StartCoroutine(StartThrowingLoop());
        }
        
        public void AddNewThrowZone()
        {
            var newThrowZoneObject = new GameObject("ThrowZone")
            {
                transform =
                {
                    parent = transform
                }
            };
            var newThrowZone = newThrowZoneObject.AddComponent<ThrowZone>();
            newThrowZone.mainCamera = mainCamera;

            throwZones.Add(newThrowZone);
        }

        public IEnumerator StartThrowingLoop()
        {
            var throwPackDelay = settings.BaseThrowPackDelay;
            var throwBlockDelay = settings.BaseThrowBlockDelay;
            var difficultyFactor = settings.DifficultyFactor;
            var maxDifficulty = settings.MaxDifficulty;
            var difficulty = 1f;
            
            do
            {
                GeneratePack(settings.PackSizeRange);
                StartCoroutine(ThrowPack(_currentPack, throwBlockDelay * difficulty));
                if (difficulty > maxDifficulty) difficulty *= difficultyFactor;
                yield return new WaitForSeconds(throwPackDelay);
            } while (!_stop);
        }

        private void GeneratePack(Vector2Int packSizeRange)
        {
            _currentPack.Clear();
            System.Random random = new();
            var size = random.Next(packSizeRange.x, packSizeRange.y + 1);
            
            for (var i = 0; i < size; i++)
            {
                var block = Instantiate(prefabs[random.Next(prefabs.Count)], new Vector3(_cameraWidth, _cameraHeight), Quaternion.identity );
                block.SetSprite(sprites[random.Next(sprites.Count)]);
                block.cameraHeight = _cameraHeight;
                block.cameraWidth = _cameraWidth;
                _currentPack.Add(block);
            }
        }

        private IEnumerator ThrowPack(List<Block> blocks, float delay)
        {
            var throwZone = GetRandomThrowZone();
            
            foreach (var block in blocks)
            {
                thrower.Throw(block, throwZone);
                yield return new WaitForSeconds(delay);
            }
            
        }
        
        private ThrowZone GetRandomThrowZone()
        {
            if (throwZones.Count == 0) return null;
            var totalProbability = throwZones.Sum(throwZone => throwZone.probability);
            var randomValue = Random.value * totalProbability;
            foreach (var throwZone in throwZones)
            {
                if (randomValue < throwZone.probability)
                {
                    return throwZone;
                }
                randomValue -= throwZone.probability;
            }
            
            return throwZones[^1];
        }
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(BlockThrowManager))]
    internal class BlockThrowManagerEditor : Editor
    {
        private bool _showThrowZonesFoldout = true;
        
        public override void OnInspectorGUI()
        {
            var thrower = (BlockThrowManager)target;

            DrawDefaultInspector();
            
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
            throwZone.startThrowVelocity = EditorGUILayout.Slider("StartThrowVelocity", throwZone.startThrowVelocity, 0, 30);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.endThrowVelocity < throwZone.startThrowVelocity)
                    throwZone.endThrowVelocity = throwZone.startThrowVelocity;
            }

            EditorGUI.BeginChangeCheck();
            throwZone.endThrowVelocity = EditorGUILayout.Slider("EndThrowVelocity", throwZone.endThrowVelocity, 0, 30);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.endThrowVelocity < throwZone.startThrowVelocity)
                    throwZone.startThrowVelocity = throwZone.endThrowVelocity;
            }

            EditorGUI.BeginChangeCheck();
            throwZone.radius = EditorGUILayout.FloatField("Radius", throwZone.radius);
            if (EditorGUI.EndChangeCheck())
            {
                if (throwZone.radius < 0) throwZone.radius = 0;
            }

            throwZone.probability = EditorGUILayout.FloatField("Probability", throwZone.probability);

            throwZone.showTrajectory = EditorGUILayout.Toggle("Show Trajectories", throwZone.showTrajectory);

            EditorApplication.QueuePlayerLoopUpdate();
        }
    }
    #endif
}