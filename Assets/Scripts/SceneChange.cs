using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string NewLevel;
    public int XPos;
    public int YPos;

    private GameObject player;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            SceneManager.LoadScene(NewLevel);
            player = GameObject.Find("Player");
            Vector3 newPosition = new Vector3(XPos, YPos, 0);
            player.transform.position = newPosition;
        }
    }
}
