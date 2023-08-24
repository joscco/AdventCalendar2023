using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelButtonBase : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private SortingGroup sortingGroup;
    [SerializeField] private SpriteRenderer boxRenderer;
    [SerializeField] private SpriteRenderer numberRenderer;
    
    public void TurnOn()
    {
        
        numberRenderer.color = new Color(1, 1, 1, 1);
        boxRenderer.color = new Color(1, 1, 1, 1);
        boxRenderer.sprite = activeSprite;
    }

    public void Sort(int sortingLayer)
    {
        sortingGroup.sortingOrder = sortingLayer;
    }

    public void TurnOff()
    {
        sortingGroup.sortingOrder = 0;
        numberRenderer.color = new Color(1, 1, 1, 0.4f);
        boxRenderer.color = new Color(1, 1, 1, 0.5f);
        boxRenderer.sprite = inactiveSprite;
    }

    public void SetSprite(Sprite image)
    {
        numberRenderer.sprite = image;
    }
}
