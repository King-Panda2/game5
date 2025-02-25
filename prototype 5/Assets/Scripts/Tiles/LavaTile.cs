using UnityEngine;

public class LavaTile : Tile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ✅ Check if it's the player
        {
            Debug.Log("🔥 Player stepped on lava!");
            GameManager.Instance.endGame(); // ✅ Calls the end game function
        }
    }
}
