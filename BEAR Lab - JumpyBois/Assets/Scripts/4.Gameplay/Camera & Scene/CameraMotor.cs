using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    [SerializeField] private int cameraOffset; // 12 is a good value
    [HideInInspector] public bool enableCameraMove;

    [SerializeField] private Transform lookAt;
    [SerializeField] private TModuleManager tmoduleManager;

    private void Start()
    {
        enableCameraMove = true;
    }

    private void LateUpdate()
    {
        if (enableCameraMove)
            MoveCamera();
    }

    private void MoveCamera()
    {
        float deltaX = lookAt.position.x - this.transform.position.x + cameraOffset;
        this.transform.position += new Vector3(deltaX, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tmoduleManager.LoadNextModule();
    }
}
