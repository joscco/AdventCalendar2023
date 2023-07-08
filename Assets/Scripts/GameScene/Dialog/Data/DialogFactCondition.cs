using System;
using GameScene.Dialog.Data;

namespace GameScene.Dialog.Background
{
    [Serializable]
    public struct DialogFactCondition
    {
        public DialogFactCondition(DialogFactId id, DialogFactOperator op, int value)
        {
            this.id = id;
            this.op = op;
            this.value = value;
        }
        
        public DialogFactId id;
        public DialogFactOperator op;
        public int value;
    }

    public enum DialogFactOperator
    {
        Equal, Greater, Less, NotEqual
    }
}