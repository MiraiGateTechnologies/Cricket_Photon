using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance;
    public int myBallCount;
    public int oppBallCount;
    public GameObject resultPanel;
    public GameObject waitPanel;
    public GameObject winPanel;
    public GameObject lostPanel;
    public GameObject tiePanel;
    public TextMeshProUGUI yourEndScore;
    public TextMeshProUGUI oppEndScore;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI opponentName;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI opponentScore;
    public TextMeshProUGUI playerScore;
    public int coins;
    public List<TextMeshProUGUI> ballsText;
    public int total;
    public bool isNumberCick;
    public int currentBallIndex = -1;
    public int wikcetCount = 0;
    int totalBallsInOver = 6;
    public int currentBallsCountInSession = 0;
    public int totalBallCountInOneSession = 12;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        coins = 1000;
        resultPanel.SetActive(false);
    }
    public void UpdateCoins(int diduct)
    {
        coinsText.text = diduct.ToString();
    }
    public void UpdateOpponentName(string name)
    {
        opponentName.text = name;
    }
    public void UpdatePlayerName(string name)
    {
        playerName.text = name;
    }
    public void UpdateOpponentScore(int score)
    {
        opponentScore.text = score.ToString();
    }
    public void UpdatePlayerScore(string score)
    {
        playerScore.text = score.ToString();
    }
    // Method to update balls text
    public void UpdateBallsText(int index, string text)
    {
        ballsText[index].text = text;
        UpdatePlayerScore(total + "/" + wikcetCount);
        currentBallsCountInSession += 1;
        if (currentBallsCountInSession >= totalBallCountInOneSession)
        {
            StartCoroutine(ShowGameOverPanal());
        }
    }
    private IEnumerator ShowGameOverPanal()
    {
        yield return new WaitForSeconds(1f);
        resultPanel.SetActive(true);
        waitPanel.SetActive(true);

        yield return new WaitUntil(() => myBallCount == oppBallCount);
        waitPanel.SetActive(false);
        string[] parts = playerScore.text.Split('/');
        int currentPlayerScore = int.Parse(parts[0]);
        int opponentPlayerScore = int.Parse(opponentScore.text.ToString());
        Debug.Log(currentPlayerScore + "/" + opponentPlayerScore);
        yourEndScore.text = playerName.text + " Scored: " + currentPlayerScore.ToString();
        oppEndScore.text = opponentName.text + " Scored: " + opponentPlayerScore.ToString();
        yourEndScore.gameObject.SetActive(true);
        oppEndScore.gameObject.SetActive(true);
        if (currentPlayerScore == opponentPlayerScore)
        {
            tiePanel.SetActive(true);
        }
        else if (currentPlayerScore > opponentPlayerScore)
        {
            winPanel.SetActive(true);
        }
        else
        {
            lostPanel.SetActive(true);
        }
        Invoke("ReloadScene", 5f);
    }
    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void GetHitcount(int batHit)
    {
        switch (batHit)
        {
            case 1:
                total += 1;
                break;
            case 2:
                total += 2;
                break;
            case 3:
                total += 3;
                break;
            case 4:
                total += 4;
                break;
            case 6:
                total += 6;
                break;
        }
        currentBallIndex += 1;
        if (currentBallIndex >= totalBallsInOver)
        {
            currentBallIndex = 0;
            foreach (var item in ballsText)
            {
                item.text = string.Empty;
            }
        }
        UpdateBallsText(currentBallIndex, batHit.ToString());
    }
}