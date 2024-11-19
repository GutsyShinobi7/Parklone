using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameInstructions;
    [SerializeField] private Button questionnaireButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private Button closeInstructionsButton;

    private void Start()
    {
        pauseMenu.SetActive(false);
        // Add listeners to the button's click events
        if (questionnaireButton != null)
        {
            questionnaireButton.onClick.AddListener(OnQuestionnaireButtonClick);
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(OnResumeButtonClick);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitButtonClick);
        }

        if (closeInstructionsButton != null)
        {
            closeInstructionsButton.onClick.AddListener(onCloseInstructionsButtonClick);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameInstructions.activeSelf == false)
        {
            if (pauseMenu.activeSelf)
            {
                Resume();
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    private void Question()
    {
    
        if (gameInstructions.activeSelf)
        {   

            pauseMenu.SetActive(true);
            gameInstructions.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(false);
            gameInstructions.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Method to be called when the pause button is clicked
    private void OnQuestionnaireButtonClick()
    {
        Question();
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

    private void onCloseInstructionsButtonClick()
    {
        Question();
    }

    private void OnDestroy()
    {
        // Clean up the listeners to avoid memory leaks
        if (questionnaireButton != null)
        {
            questionnaireButton.onClick.RemoveListener(OnQuestionnaireButtonClick);
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveListener(OnResumeButtonClick);
        }
        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        if (closeInstructionsButton != null)
        {
            closeInstructionsButton.onClick.RemoveListener(onCloseInstructionsButtonClick);
        }
    }
}