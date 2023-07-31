using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Dialog.Background;
using GameScene.Dialog.Bubble;
using GameScene.Dialog.Data;
using UnityEngine;

namespace GameScene.Dialog
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private List<DialogBubble> speakerList;
        [SerializeField] private List<Data.Dialog> dialogList;

        private Data.Dialog _currentDialog;
        private int _currentNodeIndex;

        private readonly Dictionary<string, Data.Dialog> _dialogs = new();
        private readonly Dictionary<DialogFactId, int> _facts = new();
        private readonly Dictionary<DialogSpeaker, DialogBubble> _speakerBubbles = new();

        private void Start()
        {
            foreach (var speakerBubble in speakerList)
            {
                _speakerBubbles[speakerBubble.speaker] = speakerBubble;
            }

            foreach (var dialog in dialogList)
            {
                _dialogs[dialog.id] = dialog;
            }

            UpdateDialogs();
        }

        private void PublishFacts(List<DialogFact> newFacts)
        {
            if (null != newFacts && newFacts.Count > 0)
            {
                foreach (var fact in newFacts)
                {
                    _facts[fact.id] = fact.value;
                }
            }
        }

        public void PublishFactAndUpdate(DialogFact newFact)
        {
            PublishFacts(new List<DialogFact> { newFact });
            UpdateDialogs();
        }

        public bool HasCurrentDialog()
        {
            return null != _currentDialog;
        }

        public void ContinueDialog()
        {
            if (null != _currentDialog)
            {
                if (_currentNodeIndex + 1 < _currentDialog.nodes.Count)
                {
                    // We can continue
                    var currentSpeaker = _currentDialog.nodes[_currentNodeIndex].speaker;
                    _currentNodeIndex++;
                    var nextSpeaker = _currentDialog.nodes[_currentNodeIndex].speaker;

                    var seq = DOTween.Sequence();
                    if (currentSpeaker != nextSpeaker)
                    {
                        seq.Append(HideSpeaker(currentSpeaker));
                    }

                    seq.Append(ShowSpeaker(_currentDialog.nodes[_currentNodeIndex]));
                }
                else
                {
                    // Dialog has Reached its end
                    EndCurrentDialog();
                }
            }
        }

        private void UpdateDialogs()
        {
            StartNewDialogIfNecessary();
            CancelCurrentDialogIfNecessary();
            ShowHintsForDialogsIfNecessary();
        }

        private void CancelCurrentDialogIfNecessary()
        {
            if (null != _currentDialog && ConditionsAreMet(_currentDialog.cancelConditions))
            {
                CancelCurrentDialog();
            }
        }

        private void CancelCurrentDialog()
        {
            HideDialogSpeakers();

            var factsToPublish = _currentDialog.factPublishedOnCancel;

            // Set current Dialog to null before publishing new facts.
            // Otherwise we have an infinite loop of canceling the current dialog
            _currentDialog = null;

            PublishFacts(factsToPublish);
        }

        private void ShowHintsForDialogsIfNecessary()
        {
            foreach (var dialog in _dialogs.Values)
            {
                if (ConditionsAreMet(dialog.hintConditions))
                {
                    ShowHint(dialog.hintSpeaker);
                }
            }
        }

        private void StartNewDialogIfNecessary()
        {
            foreach (var dialog in _dialogs.Values)
            {
                if (ConditionsAreMet(dialog.startConditions) &&
                    (null == _currentDialog || _currentDialog.id != dialog.id))
                {
                    StartDialog(dialog);
                    break;
                }
            }
        }

        private void EndCurrentDialog()
        {
            HideDialogSpeakers();

            var factsToPublish = _currentDialog.factPublishedOnEnd;

            _currentDialog = null;

            PublishFacts(factsToPublish);
        }

        private void StartDialog(Data.Dialog dialog)
        {
            var seq = DOTween.Sequence();

            if (_currentDialog)
            {
                CancelCurrentDialog();
            }

            dialog.GetInvolvedSpeakers().ForEach(speaker => HideSpeaker(speaker));

            _currentDialog = dialog;
            _currentNodeIndex = 0;

            if (null != _currentDialog)
            {
                // Dialog Exists so take first speaker and show it
                seq.Append(ShowSpeaker(_currentDialog.nodes[_currentNodeIndex]));
                PublishFacts(dialog.factPublishedOnStart);
            }
        }

        private void HideDialogSpeakers()
        {
            var speakers = _currentDialog.nodes.Select(node => node.speaker).ToHashSet();
            foreach (var speaker in speakers)
            {
                HideSpeaker(speaker);
            }
        }

        private Sequence HideSpeaker(DialogSpeaker speaker)
        {
            return _speakerBubbles[speaker].Hide();
        }

        private Sequence ShowSpeaker(DialogNode node)
        {
            return _speakerBubbles[node.speaker].ShowText(node.text);
        }

        private void ShowHint(DialogSpeaker speaker)
        {
            if (null == _currentDialog || !_currentDialog.ContainsSpeaker(speaker))
            {
                _speakerBubbles[speaker].ShowDotHint();
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
            int currentValue = _facts.ContainsKey(condition.id) ? _facts[condition.id] : 0;

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