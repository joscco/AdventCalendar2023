using System;
using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GameScene.Grid.Entities.Obstacles
{
    public class GrassFloor : GridEntity
    {

        [FormerlySerializedAs("daisyPrefab")] [SerializeField] private Flower flowerPrefab;
        private void Start()
        {
            InstantiateDaisies();
            InstantHide();
        }

        private void InstantiateDaisies()
        {
            var numberOfDaisies = Mathf.Floor(Random.Range(1, 6));
            for (int i = 0; i < numberOfDaisies; i++)
            {
                var flower = Instantiate(flowerPrefab, transform);
                flower.Shuffle();
                flower.transform.localPosition = Random.insideUnitCircle * 90;
                flower.transform.localScale = Random.Range(0.7f, 1.5f) * Vector3.one;
            }
        }

        public void Show(float delay)
        {
            transform.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay);
        }

        public void InstantHide()
        {
            transform.localScale = Vector3.zero;
        }
    }
}