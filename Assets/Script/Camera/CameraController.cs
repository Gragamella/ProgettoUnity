using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform playerPosition;
    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPos = transform.position;

        cameraPos.x = playerPosition.position.x;
        cameraPos.y = playerPosition.position.y;
        

        transform.position = cameraPos;
    }
}
