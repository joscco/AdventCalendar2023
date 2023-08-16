using System;
using System.Collections;
using System.Collections.Generic;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

public class ColoredTile : GridEntity
{
    [SerializeField] protected PaletteColor frameColor;
    [SerializeField] protected PaletteColor backColor;

    [SerializeField] protected SpriteRenderer frame;
    [SerializeField] protected SpriteRenderer back;
    
}
