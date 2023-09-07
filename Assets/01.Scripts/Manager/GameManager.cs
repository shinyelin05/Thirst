using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private float timeScore;

    [SerializeField]
    TextMeshProUGUI timeScoreText = null;

    public GameObject gameOverPanel = null;

    PlayerController player = null;

    private void FixedUpdate()
    {
        timeScore += Time.deltaTime;
        int timeScoreValue = Mathf.FloorToInt(timeScore);
        timeScoreText.text = "생존 시간 : " + timeScoreValue.ToString() + "초";
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayerInit(PlayerController onwer)
    {
        player = onwer;
    }
}
