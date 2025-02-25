using UnityEngine;


[CreateAssetMenu(fileName = "New unit", menuName = "Scriptable Unit")]
public class ScriptableUnit : ScriptableObject
{
    public Faction faction;
    public BaseUnit unitPrefab;

}

public enum Faction{
    Hero =0,
    Enemy =1
}