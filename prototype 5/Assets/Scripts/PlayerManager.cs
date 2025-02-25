using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private Vector2 _startPosition = new Vector2(0, 0);
    private Vector2 _playerPosition;
    private List<Vector2> _selectedPath = new List<Vector2>();
    private int _remainingSteps;
    private bool _canMove = false;
    
    [SerializeField] private Color _selectedColor;
    private Dictionary<Vector2, Tile> _tiles => GridManager.Instance.GetAllTiles();
    private HashSet<Vector2> _validMoves = new HashSet<Vector2>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(true);
        RespawnPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollDice();
        }

        if (_canMove && Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(MovePlayer());
        }
    }

    public void RespawnPlayer()
    {
        Debug.Log("🚀 Respawning player at starting position...");

        gameObject.SetActive(true); // ✅ Ensure player is active
        
        transform.position = _startPosition;  // ✅ Reset position
        _playerPosition = _startPosition;
        transform.localScale = Vector3.one;  // ✅ Reset scale in case of shrinking

        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        if (playerSprite != null)
        {
            playerSprite.enabled = true;  // ✅ Make sure the sprite is visible
            playerSprite.color = Color.cyan;  // ✅ Change to a visible color (e.g., black)
        }
        else
        {
            Debug.LogError("❌ Player SpriteRenderer is missing!");
        }

        _selectedPath.Clear();
        _canMove = false;
        HighlightAvailableTiles();
    }



    private void RollDice()
    {
        _remainingSteps = Random.Range(1, 7);
        Debug.Log($"🎲 Rolled a {_remainingSteps}");
        HighlightAvailableTiles();
    }

    private void HighlightAvailableTiles()
    {
        _validMoves.Clear();

        foreach (var tile in _tiles.Values)
        {
            tile.SetHighlight(false);
        }

        foreach (Vector2 direction in GetValidMoves(_playerPosition))
        {
            if (_tiles.TryGetValue(direction, out Tile tile))
            {
                tile.SetHighlight(true);
                _validMoves.Add(direction);
            }
        }
    }

    private List<Vector2> GetValidMoves(Vector2 currentPosition)
    {
        List<Vector2> possibleMoves = new List<Vector2>
        {
            currentPosition + Vector2.up,
            currentPosition + Vector2.down,
            currentPosition + Vector2.left,
            currentPosition + Vector2.right
        };

        possibleMoves.RemoveAll(pos => _selectedPath.Contains(pos) || !_tiles.ContainsKey(pos));
        return possibleMoves;
    }

    public void SelectTile(Vector2 tilePosition)
    {
        if (_selectedPath.Count < _remainingSteps && _validMoves.Contains(tilePosition))
        {
            _selectedPath.Add(tilePosition);
            _playerPosition = tilePosition;

            if (_tiles.TryGetValue(tilePosition, out Tile tile))
            {
                tile.SetColor(_selectedColor);
                tile.SetHighlight(false);
            }

            if (_selectedPath.Count >= _remainingSteps)
            {
                _canMove = true;
            }

            HighlightAvailableTiles();
        }
    }

    private IEnumerator MovePlayer()
    {
        foreach (var step in _selectedPath)
        {
            transform.position = step;

            if (_tiles.TryGetValue(step, out Tile tile))
            {
                if (tile is LavaTile)
                {
                    StartCoroutine(PlayerDeath());
                    yield break;
                }

                tile.SetColor(Color.white);
            }
            yield return new WaitForSeconds(0.3f);
        }

        _selectedPath.Clear();
        _canMove = false;
        _remainingSteps = 0;
        HighlightAvailableTiles();
    }

    private IEnumerator PlayerDeath()
    {
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();

        if (playerSprite == null)
        {
            Debug.LogError("❌ Player SpriteRenderer is missing!");
            yield break;
        }

        playerSprite.color = Color.grey; // ✅ Ensure color change before disappearing

        for (float i = 1f; i > 0; i -= 0.1f)
        {
            transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForSeconds(0.05f);
        }

        gameObject.SetActive(false);  // ✅ Hide player instead of destroying

        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManager is missing! Cannot restart game.");
            yield break;
        }

        GameManager.Instance.endGame(); // ✅ Restart game after player "dies"
    }




}
