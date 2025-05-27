using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Frog1Script F1S;
    [SerializeField]
    private Image bar;
    [SerializeField]
    private int score;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text finalScoreText;
    [SerializeField]
    private float timeLimit;
    private float timer;
    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private GameObject scoreScreen;

    private bool timerRunning = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = timeLimit;
        UpdateScoreText();
        UpdateTimer();
    }

    // Update is called once per frame
    void Update()
    {
        // Normalize the value for UI fill (0-1 range)
        float fillValue = (F1S.NowValue() - F1S.MinValue()) / (F1S.MaxValue() - F1S.MinValue());
        bar.fillAmount = fillValue;

        UpdateScoreText();
        UpdateTimer();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
        finalScoreText.text = "Score: " + score;
    }

    private void UpdateTimer()
    {
        if (timerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                timerRunning = false;
                EndGame();
            }
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(timer);
        timerText.text = $"Time: {seconds}";
    }

    private void EndGame()
    {
        if (scoreScreen != null)
            scoreScreen.SetActive(true);

        Time.timeScale = 0f;
    }
}