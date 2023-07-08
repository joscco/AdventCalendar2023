using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.Dialog.Background;
using UnityEngine;

namespace GameScene.Dialog.Data
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Dialog/Dialog")]
    public class Dialog : ScriptableObject
    {
        public string id;
        public List<DialogNode> nodes;
        
        public DialogSpeaker hintSpeaker;
        
        public List<DialogFactCondition> cancelConditions;
        public List<DialogFactCondition> startConditions;
        public List<DialogFactCondition> hintConditions;

        public List<DialogFact> factPublishedOnCancel;
        public List<DialogFact> factPublishedOnEnd;
        public List<DialogFact> factPublishedOnStart;

        public bool ContainsSpeaker(DialogSpeaker speaker)
        {
            return nodes.Any(node => node.speaker == speaker);
        }

        public List<DialogSpeaker> GetInvolvedSpeakers()
        {
            return new HashSet<DialogSpeaker>(nodes.Select(node => node.speaker)).ToList();
        }
    }
}