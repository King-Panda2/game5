using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _groundTile, _keyTile, _lavaTile, _winTile;
    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles = new Dictionary<Vector2, Tile>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.Instance.ResetGame();
    }

    public void GenerateGrid()
    {

        if (_groundTile == null || _keyTile == null || _lavaTile == null || _winTile == null)
        {
            Debug.LogError("❌ Tile prefabs are not assigned!");
            return;
        }

        _tiles = new Dictionary<Vector2, Tile>();

        Vector2 playerStartPosition = new Vector2(0, 0);
        Vector2 winTilePosition = new Vector2(_width - 2, Random.Range(1, _height - 1));
        Vector2 keyTilePosition1, keyTilePosition2;

        // ✅ Ensure KeyTiles do NOT spawn at (0,0) or too close
        do
        {
            keyTilePosition1 = new Vector2(Random.Range(3, _width - 3), Random.Range(3, _height - 3));
        } while (Vector2.Distance(keyTilePosition1, playerStartPosition) < 4);

        do
        {
            keyTilePosition2 = new Vector2(Random.Range(3, _width - 3), Random.Range(3, _height - 3));
        } while (Vector2.Distance(keyTilePosition2, playerStartPosition) < 4 || keyTilePosition2 == keyTilePosition1);

        Debug.Log($"📌 WIN TILE placed at {winTilePosition}");
        Debug.Log($"📌 KEY TILE 1 placed at {keyTilePosition1}");
        Debug.Log($"📌 KEY TILE 2 placed at {keyTilePosition2}");

        for (int x = -1; x < _width + 1; x++)
        {
            for (int y = -1; y < _height + 1; y++)
            {
                Vector2 tilePosition = new Vector2(x, y);
                Tile spawnedTile;

                if (tilePosition == winTilePosition)
                {
                    spawnedTile = Instantiate(_winTile, new Vector3(x, y), Quaternion.identity);
                }
                else if (tilePosition == keyTilePosition1 || tilePosition == keyTilePosition2)
                {
                    spawnedTile = Instantiate(_keyTile, new Vector3(x, y), Quaternion.identity);
                }
                else if (x == -1 || x == _width || y == -1 || y == _height)
                {
                    spawnedTile = Instantiate(_lavaTile, new Vector3(x, y), Quaternion.identity);
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
        int rand = Random.Range(0, 10);
        
        if (rand < 7) return _groundTile;
        else if (rand < 9) return _lavaTile;
        else return _groundTile; 
    }



    public Dictionary<Vector2, Tile> GetAllTiles()
    {
        return _tiles;
    }

    public Tile getEnemySpawn()
    {
        var groundTiles = _tiles.Values.Where(t => t is GroundTile).ToList();

        if (groundTiles.Count == 0)
        {
            Debug.LogError("❌ No valid ground tiles found for enemy spawn!");
            return null;
        }

        Tile randomSpawnTile = groundTiles[Random.Range(0, groundTiles.Count)];
        Debug.Log($"👾 Enemy spawned at {randomSpawnTile.transform.position}");
        return randomSpawnTile;
    }



    public Tile GetTileAtPosition(Vector2 pos)
    {
        return _tiles.TryGetValue(pos, out Tile tile) ? tile : null;
    }
}
