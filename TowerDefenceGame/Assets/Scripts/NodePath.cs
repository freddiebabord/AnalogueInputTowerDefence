using UnityEngine;
using System.Collections;

public class NodePath : MonoBehaviour
{
    public enum PathType
    {
        EnemyPath,
        FriendlyPath,
        Grass,
        Water
    }

    public int posX;
    public int posY;
    public PathType pathType;

	public bool towerPlaced = false;
}
