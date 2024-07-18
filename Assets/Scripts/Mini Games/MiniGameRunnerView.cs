using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MiniGameRunnerView : AbstractMiniGameView
{
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
    protected override AbstractMiniGameData Data { get => data; set => data = value as MiniGameRunnerData; }

    private SimpleStopWatch _stopWatch;

    protected override void OnRender(List<UnityEngine.Object> assets) 
    {
        mover.Initialize(data.moveSpeed);
        cameraFollower.Initialize(data.mouseSensitivity);

        collisionChecker.OnCollide += OnFinish;

        runnerSkin.material = GetAssetFromLoaded<Material>();

        _stopWatch = new SimpleStopWatch();
        _stopWatch.Start();
    }

    private void OnFinish() 
    {
        onFinish?.Invoke();
        _stopWatch.Stop();

        finishTimeText.text = $"{_stopWatch.ElapsedTime.Seconds:00}.{_stopWatch.ElapsedTime.Milliseconds:000}";

        UpdateBestTime();
    }

    private void UpdateBestTime() 
    {
        if (_stopWatch.ElapsedTime.TotalMilliseconds < PlayerPrefs.GetFloat(Constants.MiniGamesPrefs.RUNNER_PREFS)) 
        {
            PlayerPrefs.SetFloat(Constants.MiniGamesPrefs.RUNNER_PREFS, (float)_stopWatch.ElapsedTime.TotalMilliseconds);
            Debug.Log("Runner best time updated");
        }

        if (PlayerPrefs.HasKey(Constants.MiniGamesPrefs.RUNNER_PREFS)) 
        {
            float savedTotalMilliseconds = PlayerPrefs.GetFloat(Constants.MiniGamesPrefs.RUNNER_PREFS);

            TimeSpan savedTimeSpan = TimeSpan.FromMilliseconds(savedTotalMilliseconds);

            bestTimeText.text = $"Best: {savedTimeSpan.Seconds:00}.{savedTimeSpan.Milliseconds:000}";
            this.Activate(bestTimeText.transform);
        }
    }

    private void OnDestroy() 
    {
        collisionChecker.OnCollide -= OnFinish;
    }
}