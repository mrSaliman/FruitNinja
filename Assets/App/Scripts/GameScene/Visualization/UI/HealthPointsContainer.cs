using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.GameScene.Visualization.UI
{
    public class HealthPointsContainer : MonoBehaviour
    {
        [SerializeField] private RectTransform containerRectTransform;
        [SerializeField] private HorizontalLayoutGroup layoutGroup;
        [SerializeField] private Image imagePrefab;
        [SerializeField] private TextMeshProUGUI additionalHp;
        
        private Sprite _hpSprite;
        private Vector2 _spriteSize;
        private int _maxHpInRow;

        private List<Image> _healthPointsRow;
        
        
        public void Setup(Sprite hpSprite, float spacing, int maxHpInRow, int startHp)
        {
            _hpSprite = hpSprite;
            _spriteSize = hpSprite.rect.size;
            _maxHpInRow = maxHpInRow;

            containerRectTransform.sizeDelta =
                new Vector2(maxHpInRow * _spriteSize.x + (maxHpInRow - 1) * spacing, _spriteSize.y);

            layoutGroup.spacing = spacing;

            InitHpList(_maxHpInRow);
            UpdateContent(startHp);
        }

        private void InitHpList(int length)
        {
            _healthPointsRow = new List<Image>(_maxHpInRow);
            for (var i = 0; i < length; i++)
            {
                _healthPointsRow.Add(BuildNewImage());
            }
        }

        private Image BuildNewImage()
        {
            var img = Instantiate(imagePrefab, transform);
            img.sprite = _hpSprite;
            img.rectTransform.sizeDelta = _spriteSize;
            return img;
        }

        public void UpdateContent(int hp)
        {
            for (var i = 0; i < _healthPointsRow.Count; i++)
            {
                AnimateUIElement(_healthPointsRow[i], i < hp ? 1 : 0, 1);
            }

            if (hp > _maxHpInRow)
            {
                SetAdditionalHp(hp - _maxHpInRow);
            }
            else
            {
                ClearAdditionalHp();
            }
        }

        private void SetAdditionalHp(int hp)
        {
            additionalHp.text = $"+{hp.ToString()}";
            AnimateUIElement(additionalHp, 1, 1);
        }

        private void ClearAdditionalHp()
        {
            additionalHp.text = "";
            AnimateUIElement(additionalHp, 0, 0);
        }
        
        private static void AnimateUIElement<T>(T element, float targetAlpha, float duration) where T : Graphic
        {
            element.DOKill();
            if (Math.Abs(element.color.a - targetAlpha) > 0.01)
            {
                element.DOFade(targetAlpha, duration);
            }
        }
    }
}