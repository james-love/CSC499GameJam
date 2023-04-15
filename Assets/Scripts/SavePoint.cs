using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : Interactable, ISpawnPoint
{
    [field: SerializeField] public int SpawnPointIndex { get; set; }

    public override void Interact()
    {
        Player.Instance.State.SaveGame(SceneManager.GetActiveScene().buildIndex, SpawnPointIndex);
    }
}
