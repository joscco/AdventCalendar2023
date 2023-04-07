using TMPro;
using UnityEngine;

namespace GameScene.UI
{
    public class ActionsLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshPro actionsLeftText;
    
        public void SetActionsLeft(int actionsLeft)
        {
            actionsLeftText.text = actionsLeft.ToString();
        }
    }
}
