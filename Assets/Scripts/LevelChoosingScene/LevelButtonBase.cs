using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class LevelButtonBase : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private SpriteRenderer boxRenderer;
    [SerializeField] private TextMeshPro numberObject;
    [SerializeField] private TextMeshPro textObject;
    
    public void TurnOn()
    {
        boxRenderer.color = new Color(1, 1, 1, 1);
        boxRenderer.sprite = activeSprite;
    }

    public void TurnOff()
    {
        boxRenderer.color = new Color(1, 1, 1, 0.5f);
        boxRenderer.sprite = inactiveSprite;
    }

    public void SetText(LocalizedString text)
    {
        textObject.text = null != text ? text.GetLocalizedString() : "DEFAULT";
    }

    public void SetNumber(int level)
    {
        numberObject.text = level.ToString();
    }
}
