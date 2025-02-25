using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    private Color _originalColor;
    private bool _canHighlight = false; // Prevents unnecessary highlighting

    public virtual void Init(int x, int y)
    {
        _originalColor = _renderer.color;
    }

    void Start()
    {
        if (_renderer == null)
        {
            Debug.LogError($"[Tile] Missing _renderer on {gameObject.name}! Assign it in the prefab.");
            return;
        }
        _originalColor = _renderer.color;
    }

    void OnMouseEnter()
    {
        if (_canHighlight)
        {
            _renderer.color = Color.green; // Light green for hover
        }
    }

    void OnMouseExit()
    {
        if (_canHighlight)
        {
            _renderer.color = _originalColor; // Reset to original only if it was hoverable
        }
    }


    void OnMouseDown()
    {
        if (_canHighlight) // ✅ Only allow selection of valid tiles
        {
            PlayerManager.Instance?.SelectTile((Vector2)transform.position);
        }
    }

    public void SetHighlight(bool canHighlight)
    {
        _canHighlight = canHighlight;
    }

    public void SetColor(Color color)
    {
        if (_renderer != null)
        {
            _renderer.color = color;
        }
    }

}
