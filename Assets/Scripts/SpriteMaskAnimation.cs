using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskAnimation : MonoBehaviour
{
    public List<Sprite> sprites;
    public float frameTime;
    public SpriteMask mask;

    private void Start()
    {
        StartCoroutine(StartChangingSprites());
    }

    IEnumerator StartChangingSprites()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(frameTime);
            mask.sprite = sprites[i];
            i = (i + 1) % sprites.Count;
        }
    }
}
