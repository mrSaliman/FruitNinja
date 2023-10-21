using UnityEngine;

namespace App.GameScene.Settings
{
    [CreateAssetMenu(menuName = "Settings/HealthControllerSettings", fileName = "New HealthControllerSettings")]
    public class HealthControllerSettings : ScriptableObject
    {
        [SerializeField] private Sprite hpSprite;
        [SerializeField] private float spacing;
        [SerializeField] private int maxHpInRow;
        [SerializeField] private int startHp;
        [SerializeField] private int maxHp;

        public int StartHp => startHp;
        public int MaxHp => maxHp;
        public Sprite HpSprite => hpSprite;
        public float Spacing => spacing;
        public int MaxHpInRow => maxHpInRow;
    }
}