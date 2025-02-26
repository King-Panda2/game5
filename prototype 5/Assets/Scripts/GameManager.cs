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
        UnitManager.Instance.SpawnEnemy();

        // ✅ Reset timer if TimerManager exists
        Timer timer = FindFirstObjectByType<Timer>();
        if (timer != null)
        {
            timer.ResetTimer();
        }
        else
        {
            Debug.LogWarning("⏳ TimerManager not found! Timer won't reset.");
        }

        // delay to let grid load first
        StartCoroutine(DelayedPlayerRespawn());
    }

    private IEnumerator DelayedPlayerRespawn()
    {
        yield return new WaitForSeconds(0.1f); // ✅ Small delay ensures grid loads first
        PlayerManager.Instance.RespawnPlayer();
    }

    public void UnlockWinTile()
    {
        Debug.Log("🔓 Attempting to unlock WinTile...");

        if (GridManager.Instance == null)
        {
            Debug.LogError("❌ GridManager.Instance is null! Cannot find WinTile.");
            return;
        }

        var allTiles = GridManager.Instance.GetAllTiles();

        if (allTiles == null || allTiles.Count == 0)
        {
            Debug.LogError("❌ No tiles found in GridManager! Cannot unlock WinTile.");
            return;
        }

        bool foundWinTile = false;

        foreach (var tile in allTiles.Values)
        {
            if (tile is WinTile winTile)
            {
                winTile.UnlockWinTile(); // ✅ Unlock it
                Debug.Log("✅ WinTile successfully unlocked!");
                foundWinTile = true;
                break;
            }
        }

        if (!foundWinTile)
        {
            Debug.LogError("❌ No WinTile found in the grid! Check if it's being spawned.");
        }
    }



    public void winGame()
    {
        Debug.Log("🎉 You Win! Restarting Game...");

        // store best time
        Timer timer = FindFirstObjectByType<Timer>();
        if (timer != null)
        {
            timer.CheckForBestTime(); // ✅ Check if this is the best time
        }
        else
        {
            Debug.LogWarning("⏳ TimerManager not found! Best time won't update.");
        }

        SoundManager.Instance.PlayWinSound();
        ResetGame();
    }

    public void endGame()
    {
        Debug.Log("💀 Player Died! Restarting Game...");
        SoundManager.Instance.PlayDeathSound();
        ResetGame();
    }
}
