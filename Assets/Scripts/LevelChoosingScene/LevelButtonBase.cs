using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class LevelButtonBase : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private SpriteRenderer boxRenderer;
    [SerializeField] private SpriteRenderer numberRenderer;
    [SerializeField] private TextMeshPro textObject;
    
    public void TurnOn()
    {
        
        numberRenderer.color = new Color(1, 1, 1, 1);
        boxRenderer.color = new Color(1, 1, 1, 1);
        boxRenderer.sprite = activeSprite;
    }

    public void TurnOff()
    {
        numberRenderer.color = new Color(1, 1, 1, 0.4f);
        boxRenderer.color = new Color(1, 1, 1, 0.5f);
        boxRenderer.sprite = inactiveSprite;
    }

    public void SetSprite(Sprite image)
    {
        numberRenderer.sprite = image;
    }

    public void SetText(LocalizedString text)
    {
        textObject.text = null != text ? text.GetLocalizedString() : "DEFAULT";
    }
}
