using UnityEngine;

public class KeyTile : Tile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.CompareTag("Player"))
        {
            
        }
    }
}
