using System;
using DG.Tweening;
using UnityEngine;

public class SceneOverlay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer chessBoardFieldDarkPrefab;
    [SerializeField] private SpriteRenderer chessBoardFieldLightPrefab;
    [SerializeField] private int screenWidth = 1920;
    [SerializeField] private int screenHeight = 1080;

    private SpriteRenderer[,] _fields;
    private Sequence _fieldScaleTween;
    private int _numberOfRows;
    private int _numberOfColumns;
    private int _chessBoardFieldHeight;
    private int _chessBoardFieldWidth;

    private void Start()
    {
        var chessBoardTexture = chessBoardFieldDarkPrefab.sprite.texture;
        _chessBoardFieldHeight = chessBoardTexture.height;
        _chessBoardFieldWidth = chessBoardTexture.width;
        _numberOfColumns = screenWidth / _chessBoardFieldWidth + 1;
        _numberOfRows = screenHeight / _chessBoardFieldHeight + 1;
        _fields = new SpriteRenderer[_numberOfRows, _numberOfColumns];

        for (int row = 0; row < _numberOfRows; row++)
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                var isEven = (row + col) % 2 == 0;
                var newField = Instantiate(
                    isEven ? chessBoardFieldDarkPrefab : chessBoardFieldLightPrefab,
                    transform
                );
                var newFieldTransform = newField.transform;
                newFieldTransform.position = GetPositionForIndex(row, col) + 30 * Vector2.up;
                newFieldTransform.rotation = Quaternion.Euler(0, 90, 0);
                _fields[row, col] = newField;
            }
        }
    }

    public Tween CoverScreen()
    {
        return TurnAll(0f, 0);
    }

    public Tween UncoverScreen()
    {
        return TurnAll(90f, 30);
    }

    private Vector2 GetPositionForIndex(int row, int col)
    {
        return new Vector2(
            -(_numberOfColumns * _chessBoardFieldWidth) / 2 + _chessBoardFieldWidth / 2 + col * _chessBoardFieldWidth,
            -(_numberOfRows * _chessBoardFieldHeight) / 2 + _chessBoardFieldHeight / 2 + row * _chessBoardFieldHeight);
    }

    private Tween TurnAll(float rotation, float vertOffset)
    {
        _fieldScaleTween.Kill();
        _fieldScaleTween = DOTween.Sequence();
        for (int row = 0; row < _numberOfRows; row++)
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                _fieldScaleTween.Insert(
                    (row + col + ((row + col) % 2 == 0 ? 4 : 0)) * 0.05f,
                    _fields[row, col]
                        .transform
                        .DORotate(new Vector3(0, rotation, 0), 0.5f, RotateMode.FastBeyond360)
                        .SetEase(Ease.OutSine)
                );
                _fieldScaleTween.Insert(
                    (row + col + ((row + col) % 2 == 0 ? 4 : 0)) * 0.05f,
                    _fields[row, col]
                        .transform
                        .DOMoveY(GetPositionForIndex(row, col).y + vertOffset, 0.5f)
                        .SetEase(Ease.OutSine)
                );
            }
        }

        _fieldScaleTween.Play();
        return _fieldScaleTween;
    }
}