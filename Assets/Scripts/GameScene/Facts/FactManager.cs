using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Facts
{
    public class FactManager : MonoBehaviour
    {
        public Action<List<Fact>> onNewFacts;
        private readonly Dictionary<FactId, int> _facts = new();


        public void PublishFactAndUpdate(Fact newFact)
        {
            PublishFactsAndUpdate(new List<Fact> { newFact });
        }

        public void PublishFactsAndUpdate(List<Fact> newFacts)
        {
            if (null != newFacts && newFacts.Count > 0)
            {
                foreach (var fact in newFacts)
                {
                    _facts[fact.id] = fact.value;
                }
                
                onNewFacts?.Invoke(newFacts);
            }
        }

        public bool ConditionsAreMet(List<FactCondition> demandedFacts)
        {
            return null == demandedFacts
                   || demandedFacts.Count == 0
                   || demandedFacts.All(condition => ConditionIsMet(condition));
        }

        private bool ConditionIsMet(FactCondition condition)
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