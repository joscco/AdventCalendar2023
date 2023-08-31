using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using GameScene.Grid.Entities.ItemInteraction.Logic.Properties;
using SceneManagement;
using UnityEngine;

public class SceneOverlay : MonoBehaviour
{
    [SerializeField] private List<InteractableItemType> words;
    [SerializeField] private SceneOverlayTile wordTilePrefab;
    [SerializeField] private int screenWidth = 1920;
    [SerializeField] private int screenHeight = 1080;

    private SceneOverlayTile[,] _fields;
    private Sequence _fieldScaleTween;
    private int _numberOfRows;
    private int _numberOfColumns;
    private int _chessBoardFieldHeight;
    private int _chessBoardFieldWidth;

    private void Start()
    {
        var chessBoardTexture = wordTilePrefab.baseRenderer.sprite.texture;
        _chessBoardFieldHeight = chessBoardTexture.height - 10;
        _chessBoardFieldWidth = chessBoardTexture.width - 10;
        _numberOfColumns = screenWidth / _chessBoardFieldWidth + 1;
        _numberOfRows = screenHeight / _chessBoardFieldHeight + 1;
        _fields = new SceneOverlayTile[_numberOfRows, _numberOfColumns];

        for (int row = 0; row < _numberOfRows; row++)
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                var newField = Instantiate(
                    wordTilePrefab,
                    transform
                );
                
                newField.baseRenderer.color = new Color(1, 1, 1, 0);
                var randomIndex = Random.Range(0, words.Count);
                newField.iconRenderer.sprite = words[randomIndex].itemIcon;

                var newFieldTransform = newField.transform;
                newFieldTransform.position = GetPositionForIndex(row, col) + 30 * Vector2.up;
                newFieldTransform.rotation = Quaternion.Euler(0, 90, 0);
                _fields[row, col] = newField;
            }
        }
    }

    public Tween CoverScreen()
    {
        return TurnAll(0f, 0, 1);
    }

    public Tween UncoverScreen()
    {
        return TurnAll(90f, 30, 0);
    }

    private Vector2 GetPositionForIndex(int row, int col)
    {
        return new Vector2(
            -(_numberOfColumns * _chessBoardFieldWidth) / 2 + _chessBoardFieldWidth / 2 + col * _chessBoardFieldWidth,
            -(_numberOfRows * _chessBoardFieldHeight) / 2 + _chessBoardFieldHeight / 2 + row * _chessBoardFieldHeight);
    }

    private Tween TurnAll(float rotation, float vertOffset, float opacity)
    {
        _fieldScaleTween.Kill();
        _fieldScaleTween = DOTween.Sequence();
        for (int row = 0; row < _numberOfRows; row++)
        {
            for (int col = 0; col < _numberOfColumns; col++)
            {
                var isEven = (row + col) % 2 == 0;
                var delay = (row + col + (isEven ? 4 : 0)) * 0.05f;
                _fieldScaleTween.Insert(delay, RotateFieldByDegrees(rotation, row, col));
                _fieldScaleTween.Insert(delay, MoveYField(vertOffset, row, col));
                _fieldScaleTween.Insert(delay, FadeField(opacity, row, col));
            }
        }

        _fieldScaleTween.Play();
        return _fieldScaleTween;
    }

    private TweenerCore<Color, Color, ColorOptions> FadeField(float opacity, int row, int col)
    {
        return _fields[row, col]
            .baseRenderer
            .DOFade(opacity, 0.5f)
            .SetEase(Ease.InOutExpo);
    }

    private TweenerCore<Vector3, Vector3, VectorOptions> MoveYField(float vertOffset, int row, int col)
    {
        return _fields[row, col]
            .transform
            .DOMoveY(GetPositionForIndex(row, col).y + vertOffset, 0.5f)
            .SetEase(Ease.OutSine);
    }

    private TweenerCore<Quaternion, Vector3, QuaternionOptions> RotateFieldByDegrees(float rotation, int row, int col)
    {
        return _fields[row, col]
            .transform
            .DORotate(new Vector3(0, rotation, 0), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutSine);
    }
}