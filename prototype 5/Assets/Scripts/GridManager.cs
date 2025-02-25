using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _groundTile,_keyTile,_lavaTile,_winTile;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;


    void Awake()
    {
        Instance = this;
    }
    public void GenerateGrid(){

        // debug (ensure tile prefabs exist)
        if (_groundTile == null || _keyTile == null)
        {
            Debug.LogError("tile prefab isnt assigned");
            return;
        }

        _tiles = new Dictionary<Vector2, Tile>();
        
        for(int x = 0; x < _width;x++){
            for(int y = 0; y < _height;y++){
                var randomTile = Random.Range(0,6) == 3 ? _keyTile : _lavaTile;
                var spawnedTile = Instantiate(randomTile, new Vector3(x,y),Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.Init(x,y);

                _tiles[new Vector2(x,y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width/2-0.5f, (float)_height/2-0.5f, -10);

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetTileAtPostition(Vector2 pos){
        if(_tiles.TryGetValue(pos, out Tile tile)){
            return tile;
        }

        return null;
    }
 }
