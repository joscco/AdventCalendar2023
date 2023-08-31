using System;

namespace GameScene.Facts
{
    [Serializable]
    public class FactCondition
    {
        public FactCondition(FactId id, DialogFactOperator op, int value)
        {
            this.id = id;
            this.op = op;
            this.value = value;
        }
        
        public FactId id;
        public DialogFactOperator op;
        public int value;
    }

    public enum DialogFactOperator
    {
        Equal, Greater, Less, NotEqual
    }
}