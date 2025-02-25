using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _groundTile, _keyTile, _lavaTile, _winTile;
    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles = new Dictionary<Vector2, Tile>();
    private int _keyCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        if (_groundTile == null || _keyTile == null || _lavaTile == null || _winTile == null)
        {
            Debug.LogError("Tile prefabs are not assigned!");
            return;
        }

        _tiles = new Dictionary<Vector2, Tile>();
        _keyCount = 0;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2 tilePosition = new Vector2(x, y);
                Tile spawnedTile;

                // ✅ Ensure WinTile is placed at the top-right corner
                if (x == _width - 1 && y == _height - 1)
                {
                    spawnedTile = Instantiate(_winTile, new Vector3(x, y), Quaternion.identity);
                }
                else
                {
                    spawnedTile = Instantiate(GetRandomTile(), new Vector3(x, y), Quaternion.identity);
                }

                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init(x, y);
                _tiles[tilePosition] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public void ClearGrid()
    {
        Debug.Log("🧹 Clearing old tiles...");
        
        foreach (var tile in _tiles.Values)
        {
            Destroy(tile.gameObject);
        }
        
        _tiles.Clear();
    }

    private Tile GetRandomTile()
    {
        if (_keyCount < 2)
        {
            _keyCount++;
            return _keyTile;
        }

        int rand = Random.Range(0, 10);
        if (rand < 7) return _groundTile;
        else if (rand < 9) return _lavaTile;
        else return _groundTile; 
    }

    public Dictionary<Vector2, Tile> GetAllTiles()
    {
        return _tiles;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        return _tiles.TryGetValue(pos, out Tile tile) ? tile : null;
    }
}
