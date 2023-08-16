using System;
using System.Collections;
using System.Collections.Generic;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

public class MultiColoredTile : ColoredTile
{
    [SerializeField] private PaletteColor topColor;
    [SerializeField] private PaletteColor rightColor;
    [SerializeField] private PaletteColor bottomColor;
    [SerializeField] private PaletteColor leftColor;

    [SerializeField] private SpriteRenderer topTri;
    [SerializeField] private SpriteRenderer rightTri;
    [SerializeField] private SpriteRenderer bottomTri;
    [SerializeField] private SpriteRenderer leftTri;

    private void OnValidate()
    {
        frame.color = frameColor.value;
        back.color = backColor.value;
        topTri.color = topColor.value;
        rightTri.color = rightColor.value;
        bottomTri.color = bottomColor.value;
        leftTri.color = leftColor.value;
    }
}
