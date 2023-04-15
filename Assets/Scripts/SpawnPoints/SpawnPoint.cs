using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    [field: SerializeField] public int SpawnPointIndex { get; set; }
}
