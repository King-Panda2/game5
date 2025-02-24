using UnityEngine;

public class WinTile : Tile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.CompareTag("Player"))
        {
            // Call the EndGame function in the GameManager
            GameManager.Instance.winGame();
        }
    }
}
