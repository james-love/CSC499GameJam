using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float ScaleX;
    public float ScaleY;
    public Transform TfPlayer;
    private float origXPos;
    private float origYPos;
    private void Start()
    {
        origXPos = transform.position.x;
        origYPos = transform.position.y;

    }

    private void LateUpdate()
    {
        transform.position = new Vector3(origXPos - (TfPlayer.position.x * ScaleX), origYPos - (TfPlayer.position.y * ScaleY), transform.position.z);
    }
}
