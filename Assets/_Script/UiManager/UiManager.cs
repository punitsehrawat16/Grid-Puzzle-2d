using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private GameObject _winPanel;


    public void LoadNextLevel()
    {
        int nextScene = (SceneManager.GetActiveScene().buildIndex + 1) 
                        % SceneManager.sceneCountInBuildSettings;

        Time.timeScale = 1;
        SceneManager.LoadScene(nextScene);        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnEnable()
    {
        GameStateManager.OnGameStateChanged += OnGameStateUpdated;
    }
    private void Start()
    {
        GameStateManager.ChangeGameState(GameStates.OnGame);
    }
    private void OnDisable()
    {
        GameStateManager.OnGameStateChanged -= OnGameStateUpdated;
    }

    void Reset()
    {
        _gamePanel.SetActive(false);
        _winPanel.SetActive(false);
    }
    void OnGameStateUpdated(GameStates state)
    {
        Reset();
        switch (state)
        {
            case GameStates.OnGame:
                _gamePanel.SetActive(true);
                break;
            case GameStates.OnWin:
                _winPanel.SetActive(true);
                Time.timeScale = 0;
                break;
        }
    }
}
