using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        UpdateText();
        LocalizationSettings.SelectedLocaleChanged += (_) => UpdateText();
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
}
