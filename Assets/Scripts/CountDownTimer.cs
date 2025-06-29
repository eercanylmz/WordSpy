using System;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public GameData currentGameData;      // Zaman bilgisi buradan alýnýr
    public Text timerText;                // Zamaný gösterecek UI Text

    private float _timerLeft;             // Geriye kalan süre
    private bool _timeOut;                // Süre doldu mu kontrolü
    private bool _stopTimer;              // Sayaç durduruldu mu kontrolü

    void Start()
    {
        _stopTimer = false;
        _timeOut = false;

        _timerLeft = currentGameData.SelectedBoardData.timeInSeconds;

        // Event'lere abone ol
        GameEvents.OnBoardCompleted += StopTimer;
        GameEvents.OnUnlockNextCategory += StopTimer;

        UpdateTimerUI(); // Baþlangýçta süreyi göster
    }

    private void OnDisable()
    {
        // Event'lerden çýk
        GameEvents.OnBoardCompleted -= StopTimer;
        GameEvents.OnUnlockNextCategory -= StopTimer;
    }

    public void StopTimer()
    {
        _stopTimer = true;
    }

    void Update()
    {
        if (_stopTimer || _timeOut) return;

        _timerLeft -= Time.deltaTime;

        if (_timerLeft <= 0)
        {
            _timerLeft = 0;
            _timeOut = true;
            _stopTimer = true;
            UpdateTimerUI(); // 00:00 göster
            ActivateGameOverGUI();
        }
        else
        {
            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_timerLeft / 60f);
        int seconds = Mathf.FloorToInt(_timerLeft % 60f);
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void ActivateGameOverGUI()
    {
        GameEvents.GameOverMethod(); // GameOver ekranýný aktif et
    }
}
