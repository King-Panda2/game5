using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Xml.Schema;

public class UnitManager : MonoBehaviour
{
    [SerializeField] int spawnEnemyCount;
    public static UnitManager Instance;
    private List<ScriptableUnit> _units;
    void Awake()
    {
        Instance = this;
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
    }

    public void SpawnEnemy()
    {
        // delete enemies first
        foreach (var enemy in FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None)) // Find all enemies in the scene
        {
            Destroy(enemy.gameObject);
        }


        for (int i = 0; i < spawnEnemyCount; i++)
        {
            var randomPrefab = GetRandomUnit<BaseEnemy>(Faction.Enemy);
            var spawnedEnemy = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.getEnemySpawn(); // ✅ Calls the fixed function

            if (randomSpawnTile != null)
            {
                randomSpawnTile.SetUnit(spawnedEnemy);
            }
            else
            {
                Debug.LogError("❌ Could not find a valid spawn tile for the enemy!");
            }
        }
    }

    
    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit {
        return (T)_units.Where(u=>u.faction == faction).OrderBy(o=>Random.value).First().unitPrefab;
    
    }
}
