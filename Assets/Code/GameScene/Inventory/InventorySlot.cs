using DG.Tweening;
using TMPro;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public TextMeshPro counterText;
    public GameObject counterSprite;
    public SpriteRenderer itemSpriteRenderer;
    public InventoryItemWiki wiki;
    public InventoryItem item;
    private bool _shown;

    public void OnEnable()
    {
        item = InventoryItem.None;
        transform.localScale = new Vector3(0, 0, 0);
        _shown = false;
    }

    public void UpdateNumber(int newAmount)
    {
        string amountText = newAmount.ToString();
        if (!amountText.Equals(counterText.text))
        {
            WiggleAndChangeNumber(amountText);
        }
    }

    private void WiggleAndChangeNumber(string amountText)
    {
        var scaleSequence = DOTween.Sequence();
        scaleSequence.Append(transform.DOScale(0.75f, 0.2f)).SetEase(Ease.OutBack);
        scaleSequence.Append(transform.DOScale(1f, 0.2f)).SetEase(Ease.OutBack);
        scaleSequence.AppendCallback(() =>
        {
            counterText.text = amountText;
        });
    }

    public void BlendOut()
    {
        _shown = false;
        var scaleSequence = DOTween.Sequence();
        scaleSequence.Append(transform.DOScale(0, 0.5f).SetEase(Ease.InBack));
        scaleSequence.AppendCallback(() => ChangeItem(InventoryItem.None));
    }

    public void ChangeItem(InventoryItem newItem)
    {
        item = newItem;
        itemSpriteRenderer.sprite = wiki.GetInventoryIconSpriteForItem(newItem);
    }

    public void BlendIn()
    {
        _shown = true;
        transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
    }

    public bool IsShown()
    {
        return _shown;
    }
}