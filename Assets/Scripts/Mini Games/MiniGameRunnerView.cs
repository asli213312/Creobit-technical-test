using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MiniGameRunnerView : AbstractMiniGameView
{
    #region Serialized Fields
    [Header("Data")]
    [SerializeField] private MiniGameRunnerData data;

    [Header("Game components")]
    [SerializeField] private SkinnedMeshRenderer runnerSkin;
    [SerializeField] private CharacterMover mover;
    [SerializeField] private CameraFollower cameraFollower;
    [SerializeField] private CollisionChecker collisionChecker;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI finishTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    [Header("Events")]
    [SerializeField] private UnityEvent onFinish;
    #endregion Serialized Fields
    
    #region Properties
    protected override AbstractMiniGameData Data { get => data; set => data = value as MiniGameRunnerData; }

    #endregion Properties

    #region Private Fields
    private SimpleStopWatch _stopWatch;

    #endregion Private Fields

    #region UnityLoop Events
    private void OnDestroy() 
    {
        collisionChecker.OnCollide -= OnFinish;
    }

    #endregion UnityLoop Events

    #region Protected Methods
    protected override void OnRender(List<UnityEngine.Object> assets) 
    {
        mover.Initialize(data.moveSpeed);
        cameraFollower.Initialize(data.mouseSensitivity);

        collisionChecker.OnCollide += OnFinish;

        runnerSkin.material = GetAssetFromLoaded<Material>();

        _stopWatch = new SimpleStopWatch();
        _stopWatch.Start();
    }

    #endregion Protected Methods

    #region Private Methods
    private void OnFinish() 
    {
        onFinish?.Invoke();
        _stopWatch.Stop();

        finishTimeText.text = $"{_stopWatch.ElapsedTime.Seconds:00}.{_stopWatch.ElapsedTime.Milliseconds:000}";

        UpdateBestTime();
    }

    private void UpdateBestTime() 
    {
        if (PlayerPrefs.HasKey(Constants.MiniGamesPrefs.RUNNER_PREFS)) 
        {
            float savedTotalMilliseconds = PlayerPrefs.GetFloat(Constants.MiniGamesPrefs.RUNNER_PREFS);

            if (_stopWatch.ElapsedTime.TotalMilliseconds < savedTotalMilliseconds) 
            {
                PlayerPrefs.SetFloat(Constants.MiniGamesPrefs.RUNNER_PREFS, (float)_stopWatch.ElapsedTime.TotalMilliseconds);
                Debug.Log("Runner best time updated");
                savedTotalMilliseconds = (float)_stopWatch.ElapsedTime.TotalMilliseconds;
            }

            UpdateText(savedTotalMilliseconds);
        }
        else 
        {
            PlayerPrefs.SetFloat(Constants.MiniGamesPrefs.RUNNER_PREFS, (float)_stopWatch.ElapsedTime.TotalMilliseconds);
            UpdateText((float)_stopWatch.ElapsedTime.TotalMilliseconds);
        }

        void UpdateText(float milliseconds) 
        {
            TimeSpan savedTimeSpan = TimeSpan.FromMilliseconds(milliseconds);
            bestTimeText.text = $"Best: {savedTimeSpan.Seconds:00}.{savedTimeSpan.Milliseconds:000}";
            this.Activate(bestTimeText.transform);
        }
    }

    #endregion Private Methods
}