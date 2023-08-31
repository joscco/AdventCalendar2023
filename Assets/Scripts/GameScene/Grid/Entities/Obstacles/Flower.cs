using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class Flower: MonoBehaviour
    {
        public List<Sprite> flowerSprites;
        [SerializeField] private SpriteRenderer sprity;

        public void Shuffle()
        {
            sprity.sprite = flowerSprites[Random.Range(0, flowerSprites.Count)];
        }
    }
}