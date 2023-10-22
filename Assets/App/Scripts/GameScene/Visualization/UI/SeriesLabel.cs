using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.GameScene.Visualization.UI
{
    public class SeriesLabel : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TextMeshProUGUI label, fruitsLabel;

        public void Setup(int combo, float time, RectTransform canvasRect)
        {
            Vector3 clampedPosition = rectTransform.anchoredPosition;

            var rect = canvasRect.rect.size - rectTransform.rect.size;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -rect.x / 2, rect.x / 2);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -rect.y / 2, rect.y / 2);

            rectTransform.anchoredPosition = clampedPosition;

            var strCombo = combo.ToString();
            
            var last = combo % 10;
            var ending = "";
            if (last is > 1 and < 5) ending = "а";
            else if (last != 1) ending = "ов";
            if (combo % 100 / 10 == 1) ending = "ов";

            
            fruitsLabel.text = $"{strCombo} фрукт{ending}";
            label.text = $"x{strCombo}";
            group.DOFade(0, time).OnComplete(() => Destroy(gameObject));
        }
    }
}