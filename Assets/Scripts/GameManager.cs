using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTextInGame;
    public Button restartButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        UpdateScoreUI();
    }
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }
    void UpdateScoreUI()
    {
        if (scoreTextInGame != null)
            scoreTextInGame.text = "Score: " + score.ToString();
    }
    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();

        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
