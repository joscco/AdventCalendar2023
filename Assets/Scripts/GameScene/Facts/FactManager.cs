using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Facts
{
    public class FactManager : MonoBehaviour
    {
        private static FactManager _instance;
        public static Action<List<Fact>> onNewFacts;
        private readonly Dictionary<FactId, int> _facts = new();

        public static void PublishFactAndUpdate(Fact newFact)
        {
            PublishFactsAndUpdate(new List<Fact> { newFact });
        }

        public static void PublishFactsAndUpdate(List<Fact> newFacts)
        {
            InitInstanceIfNecessary();
            
            if (null != newFacts && newFacts.Count > 0)
            {
                if (!AllFactsAlreadyKnown(newFacts))
                {
                    foreach (var fact in newFacts)
                    {
                        _instance._facts[fact.id] = fact.value;
                    }

                    onNewFacts?.Invoke(newFacts);
                }
            }
        }

        private static bool AllFactsAlreadyKnown(List<Fact> newFacts)
        {
            InitInstanceIfNecessary();
            return newFacts.All(
                fact => _instance._facts.ContainsKey(fact.id) && _instance._facts[fact.id] == fact.value);
        }

        public static bool ConditionsAreMet(List<FactCondition> demandedFacts)
        {
            InitInstanceIfNecessary();
            return null == demandedFacts
                   || demandedFacts.Count == 0
                   || demandedFacts.All(condition => ConditionIsMet(condition));
        }

        private static void InitInstanceIfNecessary()
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<FactManager>();
            }
        }

        private static bool ConditionIsMet(FactCondition condition)
        {
            InitInstanceIfNecessary();
            int currentValue = _instance._facts.ContainsKey(condition.id) ? _instance._facts[condition.id] : 0;

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