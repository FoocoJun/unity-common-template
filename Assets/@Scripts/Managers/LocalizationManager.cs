using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class LocalizationManager : Singleton<LocalizationManager>
{
    private ELanguage _currentLanguage = ELanguage.KOR;

    public ELanguage CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                RefreshAllUI();
                RefreshAllMaterials();
                EventManager.Instance.TriggerEvent(EEventType.LanguageChanged);
            }
        }
    }

    public string GetLocalizedText(string templateID)
    {
        if (string.IsNullOrEmpty(templateID))
            return string.Empty;

        if (DataManager.Instance.TextDict.TryGetValue(templateID, out TextData textData))
        {
            return _currentLanguage switch
            {
                ELanguage.KOR => textData.KOR,
                ELanguage.ENG => textData.ENG,
                _ => textData.ENG
            };
        }

        Debug.LogWarning($"LocalizationManager: Text not found for TemplateID: {templateID}");
        return templateID;
    }

    private void RefreshAllUI()
    {
        // Scene에 있는 모든 UI_Base 오브젝트들 찾아서 RefreshUI 호출
        UI_Base[] uiBases = Object.FindObjectsByType<UI_Base>(FindObjectsSortMode.None);
        foreach (var uiBase in uiBases)
        {
            uiBase.RefreshUI();
        }
    }

    private void RefreshAllMaterials()
    {
        if (DataManager.Instance.Config == null)
            return;

        TMP_Text[] tmpTexts = Object.FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
        
        Material targetMaterial = _currentLanguage switch
        {
            ELanguage.KOR => DataManager.Instance.Config.koreanFontMaterial,
            ELanguage.ENG => DataManager.Instance.Config.englishFontMaterial,
            _ => DataManager.Instance.Config.englishFontMaterial
        };

        if (targetMaterial == null)
            return;

        foreach (var tmpText in tmpTexts)
        {
            tmpText.fontMaterial = targetMaterial;
        }
    }

    public void SetLanguage(ELanguage language)
    {
        CurrentLanguage = language;
    }
}