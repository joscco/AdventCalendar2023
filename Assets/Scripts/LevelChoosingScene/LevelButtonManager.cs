using System;
using System.Collections;
using System.Collections.Generic;
using LevelChoosingScene;
using SceneManagement;
using UnityEngine;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private int rowHeight;
    [SerializeField] private int colWidth;

    private void Start()
    {
        for (int i = 0; i < levelConfig.levels.Count; i++)
        {
            var singleLevelConfig = levelConfig.levels[i];
            var levelButton = Instantiate(levelButtonPrefab, transform);
            levelButton.SetFrontSprite(singleLevelConfig.levelIcon);
            levelButton.SetLevelName(SceneReference.GetReferenceForLevel(singleLevelConfig.levelName));
            levelButton.transform.position = new Vector3(
                (-cols / 2f + 0.5f + i % cols) * colWidth, 
                (rows / 2f - 0.5f - i / cols) * rowHeight, 
                0);
        }
    }
}
