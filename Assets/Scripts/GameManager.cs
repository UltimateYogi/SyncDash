using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameOverUI;
    public GameObject gamePauseUI;
    private float score = 0;
    
    public static GameManager instance;
    public bool isGamerunning;
    public float gameSpeed = 5f;
    public float speedIncreaseRate = 0.1f; // Increase per second
    private float distanceTraveled = 0f;
    private void Start()
    {
        isGamerunning = true;
        if (instance == null)
            instance = this;
        distanceTraveled = 0;
    }
    private void Update()
    {
        if (!isGamerunning)
            return;

        // Increase game speed over time
        gameSpeed += speedIncreaseRate * Time.deltaTime;

        // Increase score based on distance traveled (using gameSpeed)
        distanceTraveled += gameSpeed * Time.deltaTime;
        score = (int)distanceTraveled; // Base score on distance

        AddScore(score);
    }

    public void PauseGame()
    {
        isGamerunning = false;
        Time.timeScale = 0;
        gamePauseUI.SetActive(true);
    }
    public void ResumeGame()
    {
        isGamerunning = true;
        Time.timeScale = 1;
        gamePauseUI.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }   
    public void GameOver()
    {
        isGamerunning = false;
        gameOverUI.SetActive(true);
    }

    public void AddScore(float scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }   
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }

}
