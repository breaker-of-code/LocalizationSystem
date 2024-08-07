using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor
{
    private LocalizationManager localizationManager;
    private Vector2 scrollPos;

    private void OnEnable()
    {
        localizationManager = (LocalizationManager)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Localization Manager", EditorStyles.boldLabel);

        localizationManager.localizationData = (LocalizationData)EditorGUILayout.ObjectField("Localization Data",
            localizationManager.localizationData, typeof(LocalizationData), false);

        if (localizationManager.localizationData == null)
        {
            EditorGUILayout.HelpBox("No LocalizationData ScriptableObject assigned.", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();

        if (localizationManager.localizationData.texts.Count == 0)
        {
            EditorGUILayout.LabelField("No data available.");
        }
        else
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (LocalizedText localizedText in localizationManager.localizationData.texts)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Key:", localizedText.key);

                foreach (Translation translation in localizedText.translations)
                {
                    EditorGUILayout.LabelField($"{translation.language}:", translation.text);
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}