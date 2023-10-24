using UnityEngine;
using UnityEngine.UI;

namespace App.GameScene.Gameplay_Management.UI_Management.PopUp
{
    public class BasePopUp : MonoBehaviour
    {
        [SerializeField] protected Button button1;
        [SerializeField] protected Button button2;

        public void ActivateButtons()
        {
            button1.interactable = true;
            button2.interactable = true;
        }

        public void DeactivateButtons()
        {
            button1.interactable = false;
            button2.interactable = false;
        }
    }
}