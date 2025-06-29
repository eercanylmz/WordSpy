using System;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public GameData currentGameData;      // Zaman bilgisi buradan al�n�r
    public Text timerText;                // Zaman� g�sterecek UI Text

    private float _timerLeft;             // Geriye kalan s�re
    private bool _timeOut;                // S�re doldu mu kontrol�
    private bool _stopTimer;              // Saya� durduruldu mu kontrol�

    void Start()
    {
        _stopTimer = false;
        _timeOut = false;

        _timerLeft = currentGameData.SelectedBoardData.timeInSeconds;

        // Event'lere abone ol
        GameEvents.OnBoardCompleted += StopTimer;
        GameEvents.OnUnlockNextCategory += StopTimer;

        UpdateTimerUI(); // Ba�lang��ta s�reyi g�ster
    }

    private void OnDisable()
    {
        // Event'lerden ��k
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
            UpdateTimerUI(); // 00:00 g�ster
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
        GameEvents.GameOverMethod(); // GameOver ekran�n� aktif et
    }
}
