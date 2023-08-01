using System;
using UnityEngine;
using UnityEngine.Localization;

namespace GameScene.Dialog.Background
{
    [Serializable]
    public class DialogNode
    {
        public DialogSpeaker speaker;
        public LocalizedString text;
    }
}