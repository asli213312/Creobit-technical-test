using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MiniGameClickerView : AbstractMiniGameView
{
    [Header("Data")]
    [SerializeField] private MiniGameClickerData data;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI counterText;

    protected override AbstractMiniGameData Data { get => data; set => data = value as MiniGameClickerData; }

    private int _counter;

    protected override void OnRender(List<UnityEngine.Object> assets) 
    {
        Image buttonGraphic = button.GetComponent<Image>();
        buttonGraphic.sprite = GetAssetFromLoaded<Sprite>();

        button.onClick.AddListener(OnClick);

        _counter = PlayerPrefs.GetInt(Constants.MiniGamesPrefs.CLICKER_PREFS);
        UpdateText();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
        PlayerPrefs.SetInt(Constants.MiniGamesPrefs.CLICKER_PREFS, _counter);
    }

    private void OnClick() 
    {
        _counter++;
        UpdateText();
    }

    private void UpdateText() => counterText.text = _counter.ToString();
}