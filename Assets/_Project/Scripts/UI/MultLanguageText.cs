using UnityEngine;

public class MultLanguageText : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _myLabel;

    private void Awake()
    {
        GameCEO.onLanguageSelected += GameCEO_onLanguageSelected;

        _myLabel = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public string[] text;

    private void GameCEO_onLanguageSelected(int p_language)
    {
        _myLabel.text = text[p_language];
    }
}
