using TMPro;
using UnityEngine;

public class MoneyLabel : MonoBehaviour
{
    [SerializeField] private TextMeshPro moneyText;
    
    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();
    }
}
