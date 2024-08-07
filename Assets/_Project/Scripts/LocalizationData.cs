using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationData", menuName = "Localization Manager/Localization Data", order = 1)]
public class LocalizationData : ScriptableObject
{
    public List<LocalizedText> texts = new();
}