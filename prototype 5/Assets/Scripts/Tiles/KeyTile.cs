using UnityEngine;

public class KeyTile : Tile
{
    private static int keysCollected = 0; // ✅ Tracks collected keys

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"➡️ KeyTile at {transform.position} triggered by {collision.gameObject.name}");

        if (collision.CompareTag("Player"))
        {
            keysCollected++;
            
            Debug.Log($"🔑 Key Collected at {transform.position}! ({keysCollected}/2)");
            SoundManager.Instance.PlayKeySound();
            SetColor(Color.gray);

            if (keysCollected == 2)
            {
                Debug.Log("✅ All keys collected! WinTile is now active.");
                GameManager.Instance.UnlockWinTile();
            }
        }
    }

}
