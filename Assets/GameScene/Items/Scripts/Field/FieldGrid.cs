using System;
using GameScene.Items.Scripts.Field;
using UnityEngine;

// 3D Field references with very basic information about all Field Entities positioned on Field Spots
// The Field Contains an Array of freely movable FieldEntity Renderers

public class FieldGrid : MonoBehaviour
{
    public int columns = 1;
    public int rows = 1;
    public int layers = 1;

    public float spotWidth = 100;
    public float spotHeight = 70;

    public FieldSpot spotPrefab;
    private FieldSpot[,,] _plantFields;

    public SpriteRenderer preview;

    private void OnValidate()
    {
        preview.size = new Vector2(columns * spotWidth, rows * spotHeight);
    }

    private void Start()
    {
        DeactivatePreview();
        _plantFields = CreateFieldSpots();
    }

    private void DeactivatePreview()
    {
        preview.gameObject.SetActive(false);
    }

    private FieldSpot[,,] CreateFieldSpots()
    {
        FieldSpot[,,] result = new FieldSpot[rows, columns, layers];

        // All layers above the first are unimportent at the beginning
        for (int layer = 0; layer < layers; layer++)
        {
            for (int row = 0; row < rows; row++)
            {
                float positionY = transform.position.y + row * spotHeight;
                float offsetY = -(spotHeight * (rows - 1)) / 2;
                for (int column = 0; column < columns; column++)
                {
                    float positionX = transform.position.x + column * spotWidth;
                    float offsetX = -(spotWidth * (columns - 1)) / 2;
                    Vector3 pos = new Vector3(positionX + offsetX, positionY + offsetY, row);
                    FieldSpot newSpot = Instantiate(spotPrefab, transform);
                    result[row, column, layer] = newSpot;
                    newSpot.transform.position = pos;
                }
            }
        }

        return result;
    }
}