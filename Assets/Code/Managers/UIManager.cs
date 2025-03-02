using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject startPanel;
    public GameObject gameOverPanel;

    public GameObject UIPanel;
    public GameObject gameplayPanel;

    [Header("Gameplay Components")]
    public bool isGameStarted;
    public string[] preGameWords;

    void Start()
    {
        Time.timeScale = 1;
        isGameStarted = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return) && !isGameStarted)
        {
            StartGame();
        }
    }

    public string GetWittyWords()
    {
        return preGameWords[Random.Range(0, preGameWords.Length)];
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        isGameStarted = true;
        startPanel.SetActive(false);
        UIPanel.SetActive(true);
        gameplayPanel.SetActive(true);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        UIPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        startPanel.SetActive(false);
        Time.timeScale = 0;
        isGameStarted = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
