using System;
using GameScene.Dialog.Data;
using Unity.VisualScripting;

namespace GameScene.Dialog.Background
{
    [Serializable]
    public struct DialogFact
    {
        public DialogFact(DialogFactId id, int value)
        {
            this.id = id;
            this.value = value;
        }
        
        public DialogFactId id;
        public int value;
    }
}