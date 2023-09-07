using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private float timeScore;

    [SerializeField]
    TextMeshProUGUI timeScoreText = null;

    public GameObject gameOverPanel = null;

    public PlayerController player = null;

    private void FixedUpdate()
    {
        timeScore += Time.deltaTime;
        int timeScoreValue = Mathf.FloorToInt(timeScore);
        timeScoreText.text = "���� �ð� : " + timeScoreValue.ToString() + "��";
    }

    private void Update()
    {
        if(Time.timeScale == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            ReStart();
        }
    }
    void ReStart()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale= 1.0f;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
