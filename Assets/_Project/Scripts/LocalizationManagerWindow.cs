using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LocalizationManagerWindow : EditorWindow
{
    private LocalizationManager localizationManager;
    private Vector2 scrollPos;
    private string newKey = "";
    private string newLanguage = "";
    private List<string> newTranslations = new();
    private int selectedTab = 0;
    private List<string> languages = new();

    [MenuItem("Localization Manager/Manage Localization")]
    public static void ShowWindow()
    {
        GetWindow<LocalizationManagerWindow>("Localization Manager");
    }

    private void OnEnable()
    {
        localizationManager = FindObjectOfType<LocalizationManager>();
        if (localizationManager == null)
        {
            GameObject manager = new GameObject("LocalizationManager");
            localizationManager = manager.AddComponent<LocalizationManager>();
        }
    }

    private void OnGUI()
    {
        if (localizationManager == null) return;

        if (localizationManager.localizationData == null)
        {
            EditorGUILayout.HelpBox("No LocalizationData ScriptableObject assigned.", MessageType.Warning);
            localizationManager.localizationData = (LocalizationData)EditorGUILayout.ObjectField("Localization Data",
                localizationManager.localizationData, typeof(LocalizationData), false);
            return;
        }

        selectedTab = GUILayout.Toolbar(selectedTab, new string[] { "Add Language", "Add Text", "Show Data" });

        switch (selectedTab)
        {
            case 0:
                ShowAddLanguageTab();
                break;
            case 1:
                ShowAddTextTab();
                break;
            case 2:
                ShowShowDataTab();
                break;
        }
    }

    private void ShowAddLanguageTab()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.LabelField("Add New Language", EditorStyles.boldLabel);
        newLanguage = EditorGUILayout.TextField("Language", newLanguage);

        GUILayout.Space(10); // Add vertical space

        EditorGUILayout.LabelField("Languages", EditorStyles.boldLabel);
        foreach (string language in languages)
        {
            EditorGUILayout.LabelField(language);
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Language", GUILayout.Height(40)))
        {
            if (!string.IsNullOrEmpty(newLanguage))
            {
                languages.Add(newLanguage);
                newLanguage = ""; // Clear the input field
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Language name is required.", "OK");
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(localizationManager.localizationData);
        }

        EditorGUILayout.EndScrollView();
    }

    private void ShowAddTextTab()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.LabelField("Add New Text", EditorStyles.boldLabel);
        newKey = EditorGUILayout.TextField("Key", newKey);

        // Ensure newTranslations has the correct number of entries
        while (newTranslations.Count < languages.Count)
        {
            newTranslations.Add("");
        }

        for (int i = 0; i < languages.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(languages[i], GUILayout.Width(100));
            newTranslations[i] = EditorGUILayout.TextField(newTranslations[i]);
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Text", GUILayout.Height(40)))
        {
            if (!string.IsNullOrEmpty(newKey))
            {
                LocalizedText newText = new LocalizedText
                {
                    key = newKey,
                    translations = new List<Translation>()
                };

                for (int i = 0; i < languages.Count; i++)
                {
                    newText.translations.Add(new Translation { language = languages[i], text = newTranslations[i] });
                }

                localizationManager.localizationData.texts.Add(newText);

                // Reset fields
                newKey = "";
                newTranslations =
                    new List<string>(newTranslations.Count); // Reset translations list with the correct count
                for (int i = 0; i < languages.Count; i++)
                {
                    newTranslations.Add("");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Key is required.", "OK");
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(localizationManager.localizationData);
        }

        EditorGUILayout.EndScrollView();
    }

    private void ShowShowDataTab()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (localizationManager.localizationData.texts.Count == 0)
        {
            EditorGUILayout.LabelField("No data available.");
        }
        else
        {
            foreach (LocalizedText localizedText in localizationManager.localizationData.texts)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Key:", localizedText.key);

                foreach (Translation translation in localizedText.translations)
                {
                    EditorGUILayout.LabelField($"{translation.language}:", translation.text);
                }

                if (GUILayout.Button("Delete Text"))
                {
                    if (EditorUtility.DisplayDialog("Confirm Delete", "Are you sure you want to delete this text?",
                            "Yes", "No"))
                    {
                        localizationManager.localizationData.texts.Remove(localizedText);
                        break;
                    }
                }

                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.EndScrollView();
    }
}