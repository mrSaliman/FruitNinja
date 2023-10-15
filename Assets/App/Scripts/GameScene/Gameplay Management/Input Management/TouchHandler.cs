using System.Collections.Generic;
using UnityEngine;

namespace App.GameScene.Gameplay_Management.Input_Management
{
    public class TouchHandler : MonoBehaviour
    {
        [SerializeReference] private Camera mainCamera;
        
        private const float MinSpeed = 700f;
        public static readonly List<DeathLine> DeathLines = new();
        
        private void Update()
        {
            if (Input.touchCount <= 0) return;

            var i = 0;
            foreach (var touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Moved) continue;
                var speed = touch.deltaPosition.magnitude / touch.deltaTime;
                
                
                if (speed > MinSpeed)
                {
                    var deathLineFrom = mainCamera.ScreenToWorldPoint(touch.position - touch.deltaPosition);
                    var deathLineTo = mainCamera.ScreenToWorldPoint(touch.position);

                    if (DeathLines.Count <= i)
                    {
                        DeathLines.Add(new DeathLine(deathLineFrom, deathLineTo, 0.4f));
                    }
                    else
                    {
                        DeathLines[i].From = deathLineFrom;
                        DeathLines[i].To = deathLineTo;
                    }
                }

                i++;
            }
        }
    }
}