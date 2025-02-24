using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public GameState GameState;

    void Awake()
    {
        Instance = this;
    } 
    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }
    void Update(){
        // Check if the "R" key is pressed to restart the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        // Check if the "Q" key is pressed to quit the game
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.SpawnHeroes:
                break;
            case GameState.SpawnEnemies:
                break;
            case GameState.HeroesTurn:
                break;
            case GameState.EnemiesTurn:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    public void endGame(){

    }
    public void winGame(){

    }
    public void ResetGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game Restarted!");
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        // Quit the application
        Application.Quit();

        // If running in the Unity Editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

public enum GameState
{
    GenerateGrid = 0,
    SpawnHeroes = 1,
    SpawnEnemies = 2,
    HeroesTurn = 3,
    EnemiesTurn = 4
}
