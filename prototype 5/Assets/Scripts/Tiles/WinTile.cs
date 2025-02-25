using UnityEngine;

public class WinTile : Tile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ✅ Check if the player steps on it
        {
            GameManager.Instance.winGame(); // ✅ Calls win function
        }
    }
}
