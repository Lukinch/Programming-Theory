using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject wonOrLostMenu;
    [SerializeField] GameObject countersSection;
    [SerializeField] GameObject crossHairSection;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] TextMeshProUGUI escSection;
    [SerializeField] TextMeshProUGUI jumpersCounter;
    [SerializeField] TextMeshProUGUI floatersCounter;

    [SerializeField] PlayerMovementRB player;

    [SerializeField] int floatersCount;
    [SerializeField] int jumpersCount;

    [SerializeField] bool didLost = false;
    [SerializeField] bool didWon = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
        updateCounters();
        ManageWinCondition();
        ManageGameOverCondition();
    }

    private void ManageWinCondition()
    {
        if (jumpersCount < 1 && floatersCount < 1)
        {
            didWon = true;
            winText.SetActive(true);
            OnWinOrLost();
        }
    }

    private void ManageGameOverCondition()
    {
        if (player.health < 0)
        {
            didLost = true;
            gameOverText.SetActive(true);
            OnWinOrLost();
        }
    }

    private void OnWinOrLost()
    {
        countersSection.SetActive(false);
        crossHairSection.SetActive(false);
        escSection.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        wonOrLostMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void updateCounters()
    {
        var floaters = GameObject.FindObjectsOfType<Floater>();
        var jumpers = GameObject.FindObjectsOfType<Jumper>();

        floatersCount = floaters.Length;
        jumpersCount = jumpers.Length;

        floatersCounter.text = $"Floaters: {floatersCount}";
        jumpersCounter.text = $"Jumpers: {jumpersCount}";
    }

    private void PauseGame()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
