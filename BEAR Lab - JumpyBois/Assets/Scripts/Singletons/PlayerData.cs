using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

// Singleton Design Pattern for Storing Game Settings
[Serializable] // Serializable, so we can save data
public class PlayerData : MonoBehaviour
{
    public static PlayerData PlayerDataRef;

    // for preserving grasps between GraspSelectionMenu and ControlsAndSetup
    [HideInInspector] public bool graspsChosen = false;

    // Holds the various grasps we are testing for (active) and not testing for (inactive)
    public List<GameLookup.graspNamesEnum> activeGrasps = new List<GameLookup.graspNamesEnum> { };
    public List<GameLookup.graspNamesEnum> inactiveGrasps = new List<GameLookup.graspNamesEnum> { };

    // We are using the keyboard input by default
    public bool usingDelsys = false;

    // Number of total tests/trials; default value set in 2.ControlsAndSetup -> SettingsContainer/.../Placeholder
    public int numTests = 20;

    // Maps the grasp (enum) to how many times it'll appear in the game
    public IDictionary<GameLookup.graspNamesEnum, int> numTestsPerGrasp = new Dictionary<GameLookup.graspNamesEnum, int>();

    // Dictionary can't be saved, so we'll unpack it into two arrays
    // Declaring empty ones to be written to/read from upon save/load
    public GameLookup.graspNamesEnum[] graspNameDictKeys = new GameLookup.graspNamesEnum[10];
    public int[] graspNumDictValues = new int[10];

    // characterType tells us which selection we should read
    public GameLookup.characterTypes characterType;
    public GameLookup.reindeerTypes reindeerSelection;
    public GameLookup.sealTypes sealSelection;


    // Ensures only one instance of this singleton exists
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (PlayerDataRef != null && PlayerDataRef != this)
        {
            Destroy(this);
        } else
        {
            PlayerDataRef = this;
            DontDestroyOnLoad(gameObject); // Preserves this instance across scenes
        }
    }
}
