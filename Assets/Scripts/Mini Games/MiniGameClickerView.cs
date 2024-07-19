using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MiniGameClickerView : AbstractMiniGameView
{
    #region Serialized Fields
    [Header("Data")]
    [SerializeField] private MiniGameClickerData data;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI counterText;

    #endregion Serialized Fields

    #region Properties
    protected override AbstractMiniGameData Data { get => data; set => data = value as MiniGameClickerData; }

    #endregion Properties

    #region Private Fields
    private int _counter;

    #endregion Private Fields

    #region UnityLoop Events

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
        PlayerPrefs.SetInt(Constants.MiniGamesPrefs.CLICKER_PREFS, _counter);
    }

    #endregion UnityLoop Events

    #region Protected Methods
    protected override void OnRender(List<UnityEngine.Object> assets) 
    {
        Image buttonGraphic = button.GetComponent<Image>();
        buttonGraphic.sprite = GetAssetFromLoaded<Sprite>();

        button.onClick.AddListener(OnClick);

        if (PlayerPrefs.HasKey(Constants.MiniGamesPrefs.CLICKER_PREFS)) 
        {
            _counter = PlayerPrefs.GetInt(Constants.MiniGamesPrefs.CLICKER_PREFS);
        }

        UpdateText();
    }

    #endregion Protected Methods

    #region Private Methods
    private void OnClick() 
    {
        _counter++;
        UpdateText();
    }

    private void UpdateText() => counterText.text = _counter.ToString();

    #endregion Private Methods
}