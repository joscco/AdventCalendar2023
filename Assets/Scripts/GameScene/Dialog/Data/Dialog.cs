using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.Dialog.Background;
using GameScene.Facts;
using UnityEngine;

namespace GameScene.Dialog.Data
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Dialog/Dialog")]
    public class Dialog : ScriptableObject
    {
        public string id;
        public List<DialogNode> nodes;
        
        public DialogSpeaker hintSpeaker;
        
        public List<FactCondition> cancelConditions;
        public List<FactCondition> startConditions;
        public List<FactCondition> hintConditions;

        public List<Fact> factPublishedOnCancel;
        public List<Fact> factPublishedOnEnd;
        public List<Fact> factPublishedOnStart;

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