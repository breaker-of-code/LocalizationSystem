using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public LocalizationData localizationData;
    [HideInInspector] public List<LocalizedText> texts = new();

    private void Awake()
    {
        if (localizationData != null)
        {
            texts = localizationData.texts;
        }
    }
}

[System.Serializable]
public class LocalizedText
{
    public string key;
    public List<Translation> translations = new();
}

[System.Serializable]
public class Translation
{
    public string language;
    public string text;
}
