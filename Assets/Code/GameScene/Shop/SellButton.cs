using Code.GameScene.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.GameScene.Shop
{
    public class SellButton: ScalingButton
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