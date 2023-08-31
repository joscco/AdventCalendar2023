using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene.Facts;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class HintText : MonoBehaviour
{
    [SerializeField] private LocalizedString text;
    [TextArea(3, 10)]
    [SerializeField] private String defaultText;
    [SerializeField] private PaletteColor color;
    [SerializeField] private TextMeshPro textObject;
    [SerializeField] private FactCondition factToHideOn;

    private void Start()
    {
        UpdateText();
        LocalizationSettings.SelectedLocaleChanged += (_) => UpdateText();
        FactManager.onNewFacts += CheckAndHideIfNecessary;
    }

    private void CheckAndHideIfNecessary(List<Fact> newFacts)
    {
        if (null != factToHideOn && newFacts.Any(fact => fact.id == factToHideOn.id && fact.value == factToHideOn.value))
        {
            Hide();
        }
    }

    private void UpdateText()
    {
        textObject.text = text.GetLocalizedString();
    }

    private void OnValidate()
    {
        textObject.color = color.value;
        textObject.text = defaultText;
    }

    private void Hide()
    {
        textObject.DOFade(0f, 1f).SetEase(Ease.InOutQuad);
    }
}
