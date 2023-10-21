using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.GameScene.Visualization.UI
{
    public class PopLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        public void Setup(string text, float time)
        {
            label.text = text;
            label.DOFade(0, time).OnComplete(() => Destroy(gameObject));
        }
    }
}