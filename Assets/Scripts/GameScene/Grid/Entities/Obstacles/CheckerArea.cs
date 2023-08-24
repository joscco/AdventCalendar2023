using System;
using System.Collections.Generic;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using UnityEngine;

public class CheckerArea : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public InteractableItemTag demandedTag;
    public TileColorScheme color;
    public int pattern;

    private void Start()
    {
        _spriteRenderer.color = new Color(0, 0, 0, 0);
    }

    public Bounds GetBounds()
    {
        return _spriteRenderer.bounds;
    }
}