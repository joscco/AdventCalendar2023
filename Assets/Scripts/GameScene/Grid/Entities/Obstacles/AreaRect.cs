using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaRect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer.color = new Color(0, 0, 0, 0);
    }

    public Bounds GetBounds()
    {
        return _spriteRenderer.bounds;
    }
}