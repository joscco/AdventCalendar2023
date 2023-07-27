using System.Collections;
using GameScene.Grid.Entities.ItemInteraction.Stackable;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction.Stacks
{
    // A simple stack can be stacked with anything
    public class GiftStation : CategoryStack
    {
        [SerializeField] private int numberOfGiftsNeeded = 3;
        [SerializeField] private GameObject state0;
        [SerializeField] private GameObject state1;
        [SerializeField] private GameObject state2;
        [SerializeField] private GameObject state3;
        [SerializeField] private GameObject state4;
        [SerializeField] private GameObject state5;

        protected override void OnFirstComplete()
        {
            SetInteractable(false);
            StartCoroutine(StartBagClosingAnimation());
        }

        private IEnumerator StartBagClosingAnimation()
        {
            state0.SetActive(false);
            state1.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            state1.SetActive(false);
            state2.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            state2.SetActive(false);
            state3.SetActive(true);
            GetItemHolder().gameObject.SetActive(false);
            yield return new WaitForSeconds(0.15f);
            state3.SetActive(false);
            state4.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            state4.SetActive(false);
            state5.SetActive(true);
        }

        public override bool IsComplete()
        {
            return stack.Count >= numberOfGiftsNeeded;
        }
    }
}