using Code.GameScene.UI;
using TMPro;
using UnityEngine;

namespace Code.GameScene.Shop
{
    public class BuyButton: ScalingButton
    {
        [SerializeField] private TextMeshPro priceText;
        
        public override void OnClick()
        {
            throw new System.NotImplementedException();
        }

        public void SetPrice(int price)
        {
            priceText.text = price.ToString();
        }
    }
}