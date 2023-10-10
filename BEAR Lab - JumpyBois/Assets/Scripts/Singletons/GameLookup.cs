using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose: Serves as a single point of reference for many scripts
public class GameLookup : MonoBehaviour
{
    public static GameLookup GameRef { get; private set; }

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

    public enum characterTypes
    {
        reindeer, // 0
        seal // 1
    }

    public enum reindeerTypes
    {
        brown, // 0
        grey, // 1
        ruldolph, // 2
        white, // 3
        whiteAndBrown // 4
    }

    public enum sealTypes
    {
        black, // 0
        cream, // 1
        grey, // 2
        white // 3
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (GameRef != null && GameRef != this)
        {
            Destroy(this);
        } else
        {
            GameRef = this;
            DontDestroyOnLoad(gameObject);
        }
    }

}
