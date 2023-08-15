using System;
using GameScene.Dialog.Background;
using UnityEngine.Localization;

namespace GameScene.Dialog.Data
{
    [Serializable]
    public class DialogNode
    {
        public DialogSpeaker speaker;
        public LocalizedString text;
    }
}