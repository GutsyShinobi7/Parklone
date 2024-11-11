using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {   
        pauseMenu.SetActive(false);
        // Add listeners to the button's click events
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(OnPauseButtonClick);
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(OnResumeButtonClick);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Method to be called when the pause button is clicked
    private void OnPauseButtonClick()
    {
        Pause();
    }

    // Method to be called when the resume button is clicked
    private void OnResumeButtonClick()
    {
        Resume();
    }

    // Method to be called when the exit button is clicked
    private void OnExitButtonClick()
    {
        Exit();
    }

    private void OnDestroy()
    {
        // Clean up the listeners to avoid memory leaks
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveListener(OnPauseButtonClick);
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveListener(OnResumeButtonClick);
        }
        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(OnExitButtonClick);
        }
    }
}