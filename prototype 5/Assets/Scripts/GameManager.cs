using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ✅ Prevents GameManager from being destroyed
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("🔄 Resetting game...");
            ResetGame();
        }
    }

    public void ResetGame()
    {
        Debug.Log("🔄 Resetting Game and Regenerating Grid...");

        // Reset grid and player properly
        GridManager.Instance.ClearGrid();
        GridManager.Instance.GenerateGrid();

        StartCoroutine(DelayedPlayerRespawn());
    }

    private IEnumerator DelayedPlayerRespawn()
    {
        yield return new WaitForSeconds(0.1f); // ✅ Small delay ensures grid loads first
        PlayerManager.Instance.RespawnPlayer();
    }

    public void winGame()
    {
        Debug.Log("🎉 You Win! Restarting Game...");
        ResetGame();
    }

    public void endGame()
    {
        Debug.Log("💀 Player Died! Restarting Game...");
        ResetGame();
    }
}
