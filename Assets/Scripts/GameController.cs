using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIController uiController;

    private bool isPaused = false;

    private void Start()
    {
        isPaused = true;
    }

    private void Update()
    {
        PauseGame();

        if (!isPaused)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void PlayGame()
    {
        isPaused = false;
        uiController.mainMenuPanel.SetActive(false);
        uiController.pauseMenuPanel.SetActive(false);
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = true;
            uiController.pauseMenuPanel.SetActive(true);
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();  // Stops play mode in Editor
#else
            Application.Quit();  // Quits the game in a build
#endif
    }
}
