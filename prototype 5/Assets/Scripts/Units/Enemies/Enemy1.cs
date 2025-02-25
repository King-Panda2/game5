using UnityEngine;

public class Enemy1 : BaseEnemy
{
    private Vector2 _direction = Vector2.up; // Initial direction (up)
    private float _moveSpeed = 1f; // Time between moves (in seconds)
    private float _timer = 0f;

    private void Start()
    {
        // Start moving immediately
        _timer = _moveSpeed;
    }

    private void Update()
    {
        // Move the enemy automatically
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            Move();
            _timer = _moveSpeed; // Reset the timer
        }
    }

    private void Move()
    {
        // Calculate the next position
        Vector2 nextPosition = (Vector2)transform.position + _direction;

        // Check if the next position is valid (not a lava tile)
        Tile nextTile = GridManager.Instance.GetTileAtPosition(nextPosition);
        if (nextTile != null && nextTile.walkable && !(nextTile is LavaTile))
        {
            // Move to the next position
            transform.position = nextPosition;
        }
        else
        {
            // Reverse direction if the next position is invalid (lava or blocked)
            _direction *= -1; // Reverse direction (up -> down or down -> up)
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ✅ Check if it's the player
        {
            Debug.Log("🔥 Player stepped on lava!");
            GameManager.Instance.endGame(); // ✅ Calls the end game function
        }
    }
}