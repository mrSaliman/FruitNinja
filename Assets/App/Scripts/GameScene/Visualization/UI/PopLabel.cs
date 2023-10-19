using System;
using TMPro;
using UnityEngine;

namespace App.GameScene.Visualization.UI
{
    public class PopLabel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TextMeshProUGUI label;
        private float _timer;
        private float _timeToDie;

        private void Update()
        {
            if (_timeToDie == 0) return;
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                var div = _timer / _timeToDie;
                canvasGroup.alpha = div;
                label.rectTransform.localScale = Vector3.one * div;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Setup(Camera mainCamera, string text, float time)
        {
            label.rectTransform.anchoredPosition = mainCamera.WorldToScreenPoint(transform.position);
            canvas.worldCamera = mainCamera;
            label.text = text;
            _timeToDie = time;
            _timer = time;
        }
    }
}