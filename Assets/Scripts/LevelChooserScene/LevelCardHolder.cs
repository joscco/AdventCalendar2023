using System;
using System.Collections;
using System.Collections.Generic;
using Code;
using Code.GameScene.Items.Item;
using Code.LevelChooserScene;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelCardHolder : MonoBehaviour
{
    [SerializeField]
    private List<LevelCardData> levelDataCards;

    [SerializeField]
    private LevelCard levelCardPrefab;

    private List<LevelCard> _levelCards;

    private void Start()
    {
        _levelCards = new List<LevelCard>();
        foreach (var cardData in levelDataCards)
        {
            LevelCard newCard = Instantiate(levelCardPrefab, transform);
            newCard.SetLevelCardData(cardData);
            _levelCards.Add(newCard);
        }

        UpdateCards();
    }

    private void UpdateCards()
    {
        foreach (var card in _levelCards)
        {
            if (Game.Instance.PlayerData.GetLevelUnlocked() >= card.GetLevelNumber())
            {
                card.BlendIn();
            }
            else
            {
                card.BlendOut();
            }
        }
    }
}
