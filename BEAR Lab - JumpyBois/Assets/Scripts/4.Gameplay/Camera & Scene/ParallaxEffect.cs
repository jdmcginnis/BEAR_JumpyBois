using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length;
    private float startPos;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float temp = (cameraTransform.position.x * (1 - parallaxEffect));
        float dist = (cameraTransform.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length) 
            startPos += length;
        else if (temp < startPos - length) 
            startPos -= length;
    }
}

// Values of Parallax Effect that seem to be fit well
// 1, 0.8, 0.7, 0.6, 0.125, 0
