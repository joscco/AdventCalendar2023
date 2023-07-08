using GameScene.Dialog.Data;
using UnityEngine;

namespace GameScene.Dialog.Background
{
    public class DialogArea : MonoBehaviour
    {
        [SerializeField] private DialogFactId areaFactId;
        private DialogManager dialogManager;

        private void Start()
        {
            dialogManager = FindObjectOfType<DialogManager>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                dialogManager.PublishFact(new DialogFact(areaFactId, 1));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                dialogManager.PublishFact(new DialogFact(areaFactId, 0));
            }
        }
    }
}