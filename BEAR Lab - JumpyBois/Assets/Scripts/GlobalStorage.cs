using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


// Singleton Design Pattern for Storing Game Settings
public class GlobalStorage : MonoBehaviour
{
    public static GlobalStorage GameSettings { get; private set; } 
    public enum graspNamesEnum
    {
        IndexFlexion, // 0
        Key, // 1
        Pinch, // 2
        Point, // 3
        Power, // 4
        Tripod, // 5
        WristExtension, // 6
        WristFlexion, // 7
        WristRotation, // 8
        WristRotation_Power // 9
    }

    public List<graspNamesEnum> activeGrasps;
    public List<graspNamesEnum> inactiveGrasps;

    // Indices map to graspNamesEnum
    public int[] graspProbs = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    public int numTests = 0;
    public bool usingDelsys = false; // using keyboard input by default

    public string characterSelection;
    public string sceneSelection;

    public enum characterTypes
    {
        reindeer, // 0
        seal // 1
    }

    private void Start()
    {
        activeGrasps = new List<graspNamesEnum> { };

        inactiveGrasps = new List<graspNamesEnum> { };

        foreach (graspNamesEnum graspName in Enum.GetValues(typeof(graspNamesEnum)))
        {
            inactiveGrasps.Add(graspName);
        }
    }



    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (GameSettings != null && GameSettings != this)
        {
            Destroy(this);
        } else
        {
            GameSettings = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
