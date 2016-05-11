using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class NodePath : MonoBehaviour
{
    public enum PathType
    {
        EnemyPath,
        FriendlyPath,
        Grass,
        Water,
        Rock,
        Tree
    }

    public int posX;
    public int posY;
    public PathType pathType;

	public bool towerPlaced = false;

    public TowerClass placedTower;
}
