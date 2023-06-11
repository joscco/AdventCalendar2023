using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class Eye : MonoBehaviour
{
    [SerializeField] private Sprite openEye;
    [SerializeField] private Sprite mediumEye;
    [SerializeField] private Sprite closedEye;

    private bool blinking;
    private bool startingToBlink;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startingToBlink)
        {
            return;
        }

        if (blinking && Random.value < 0.015f)
        {
            blinking = false;
            startingToBlink = true;
            DOVirtual.DelayedCall(0.1f, () =>
            {
                startingToBlink = false;
                UpdateSprite();
            });
            UpdateSprite();
        }
        else if (!blinking && Random.value < 0.001f)
        {
            blinking = true;
            startingToBlink = true;
            DOVirtual.DelayedCall(0.1f, () =>
            {
                startingToBlink = false;
                UpdateSprite();
            });
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        if (startingToBlink)
        {
            _spriteRenderer.sprite = mediumEye;
        }
        else
        {
            _spriteRenderer.sprite = blinking ? closedEye : openEye;
        }
    }
}