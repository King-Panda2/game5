using UnityEngine;

public class WinTile : Tile
{
    private bool isUnlocked = false; // ✅ Locked until keys are collected

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"➡️ WinTile at {transform.position} triggered by {collision.gameObject.name}");

        if (collision.CompareTag("Player"))
        {
            if (isUnlocked)
            {
                Debug.Log("🎉 Player reached WinTile! You win!");
                GameManager.Instance.winGame();
            }
            else
            {
                Debug.Log("❌ You need to collect both keys first!");
            }
        }
    }

    public void UnlockWinTile()
    {
        isUnlocked = true;
        SetColor(Color.yellow); // ✅ Change color to indicate activation
        Debug.Log("🚪 WinTile is now unlocked!");
    }
}
