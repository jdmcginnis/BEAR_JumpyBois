using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    [SerializeField] private Transform lookAt;
    [SerializeField] private int cameraOffset = 3;
    private BoxCollider2D camCollider; // For loading new tilemap modules

    [SerializeField] private GameObject tmapContainer;
    private TModuleManager tmodManager;


    private void Start()
    {
        camCollider = GetComponent<BoxCollider2D>();
        tmodManager = tmapContainer.GetComponent<TModuleManager>();
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        // deltaX = playerPositionX - cameraPositionX + adjustableOffset
        float deltaX = lookAt.position.x - this.transform.position.x + cameraOffset;
        this.transform.position += new Vector3(deltaX, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tmodManager.LoadNextModule();
    }
}
