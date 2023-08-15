using System;
using GameScene.Dialog.Data;

namespace GameScene.Facts
{
    [Serializable]
    public class Fact
    {
        public Fact(FactId id, int value)
        {
            this.id = id;
            this.value = value;
        }
        
        public FactId id;
        public int value;
    }
}