using System.Collections.Generic;
using System.Linq;
using GameScene.Dialog.Bubble;
using GameScene.Dialog.Data;
using UnityEngine;

namespace GameScene.Dialog.Background
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private List<DialogBubble> speakerList;
        [SerializeField] private List<Data.Dialog> dialogList;

        private Data.Dialog dialogWaitingInLine;

        private Data.Dialog currentDialog;
        private int currentNodeIndex;

        private Dictionary<string, Data.Dialog> dialogs = new();
        private Dictionary<DialogFactId, int> facts = new();
        private Dictionary<DialogSpeaker, DialogBubble> speakerBubbles = new();

        private void Start()
        {
            foreach (var speakerBubble in speakerList)
            {
                speakerBubbles[speakerBubble.speaker] = speakerBubble;
            }

            foreach (var dialog in dialogList)
            {
                dialogs[dialog.id] = dialog;
            }

            UpdateDialogs();
        }

        public void PublishFacts(List<DialogFact> newFacts)
        {
            if (null != newFacts || newFacts.Count > 0)
            {
                foreach (var fact in newFacts)
                {
                    facts[fact.id] = fact.value;
                }

                UpdateDialogs();
            }
        }

        public void PublishFact(DialogFact newFact)
        {
            PublishFacts(new List<DialogFact> { newFact });
        }

        public bool HasCurrentDialog()
        {
            return null != currentDialog;
        }

        public void ContinueDialog()
        {
            if (null != currentDialog)
            {
                HideCurrentSpeaker();
                currentNodeIndex++;

                if (currentDialog.nodes.Count < currentNodeIndex + 1)
                {
                    // Reset Index to hide last speech bubble
                    currentNodeIndex--;
                    EndCurrentDialog();
                }
                else
                {
                    ShowSpeaker(currentDialog.nodes[currentNodeIndex]);
                }
            }
        }

        private void UpdateDialogs()
        {
            CancelCurrentDialogIfNecessary();
            StartDialogIfNecessary();
            ShowHintsForDialogsIfNecessary();
        }

        private void CancelCurrentDialogIfNecessary()
        {
            if (null != currentDialog && ConditionsAreMet(currentDialog.cancelConditions))
            {
                CancelCurrentDialog();
            }
        }

        private void CancelCurrentDialog()
        {
            HideCurrentSpeaker();

            var factsToPublish = currentDialog.factPublishedOnCancel;

            // Set current Dialog to null before publishing new facts.
            // Otherwise we have an infinite loop of canceling the current dialog
            currentDialog = null;

            PublishFacts(factsToPublish);
        }

        private void EndCurrentDialog()
        {
            HideLastSpeaker();

            var factsToPublish = currentDialog.factPublishedOnEnd;

            currentDialog = null;

            PublishFacts(factsToPublish);
        }
        
        private void StartDialog(Data.Dialog dialog)
        {
            if (null == currentDialog || currentDialog.id != dialog.id)
            {
                if (currentDialog)
                {
                    CancelCurrentDialog();
                }

                dialog.GetInvolvedSpeakers().ForEach(HideSpeaker);

                currentDialog = dialog;
                currentNodeIndex = 0;

                if (null != currentDialog)
                {
                    // Dialog Exists so take first speaker and show it
                    ShowSpeaker(currentDialog.nodes[currentNodeIndex]);
                    PublishFacts(dialog.factPublishedOnStart);
                }
            }
        }

        private void HideCurrentSpeaker()
        {
            HideSpeaker(currentDialog.nodes[currentNodeIndex].speaker);
        }
        
        private void HideLastSpeaker()
        {
            HideSpeaker(currentDialog.nodes[^1].speaker);
        }
        
        private void HideSpeaker(DialogSpeaker speaker)
        {
            speakerBubbles[speaker].Hide();
        }

        private void ShowHintsForDialogsIfNecessary()
        {
            foreach (var dialog in dialogs.Values)
            {
                if (ConditionsAreMet(dialog.hintConditions))
                {
                    ShowHint(dialog.hintSpeaker);
                }
            }
        }

        private void StartDialogIfNecessary()
        {
            foreach (var dialog in dialogs.Values)
            {
                if (ConditionsAreMet(dialog.startConditions))
                {
                    StartDialog(dialog);
                    break;
                }
            }
        }

        private void ShowSpeaker(DialogNode node)
        {
            speakerBubbles[node.speaker].Show(node.text);
        }

        private void ShowHint(DialogSpeaker speaker)
        {
            if (null == currentDialog || !currentDialog.ContainsSpeaker(speaker))
            {
                speakerBubbles[speaker].ShowDotHint();
            }
        }

        private bool ConditionsAreMet(List<DialogFactCondition> demandedFacts)
        {
            return null == demandedFacts
                   || demandedFacts.Count == 0
                   || demandedFacts.All(condition => ConditionIsMet(condition));
        }

        private bool ConditionIsMet(DialogFactCondition condition)
        {
            int currentValue = facts.ContainsKey(condition.id) ? facts[condition.id] : 0;

            switch (condition.op)
            {
                case DialogFactOperator.Equal:
                    return currentValue == condition.value;
                case DialogFactOperator.Less:
                    return currentValue < condition.value;
                case DialogFactOperator.Greater:
                    return currentValue > condition.value;
                case DialogFactOperator.NotEqual:
                    return currentValue != condition.value;
            }

            // Unknown operator or key
            return false;
        }
    }
}