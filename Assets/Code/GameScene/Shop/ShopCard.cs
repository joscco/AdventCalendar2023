using System;
using Code.GameScene.Items.Item;
using Code.GameScene.UI;
using UnityEngine;

namespace Code.GameScene.Shop
{
    public class ShopCard : MonoBehaviour
    {
        public ShopCardData shopCardData;
        public SpriteRenderer spriteRenderer;
        public BuyButton buyButton;
        public SellButton sellButton;

        private void Start()
        {
            spriteRenderer.sprite = shopCardData.cardSprite;

            buyButton.SetPrice(shopCardData.buyPrice);
            sellButton.SetPrice(shopCardData.sellPrice);
        }
    }
}