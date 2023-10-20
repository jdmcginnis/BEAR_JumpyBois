using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

// Handles all of the user input with an adjustable samplingRate
// Informs relevant scripts of user input as needed
public class InputManager : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    private PlayerInput playerInput;

    // Defaults back to Rest if not receiving any input
    public GameLookup.graspNamesEnum currentGraspInput = GameLookup.graspNamesEnum.Rest;

    [SerializeField] private int bufferSize;
    public Queue<GameLookup.graspNamesEnum> buffer { get; private set; }
    public int[] bufferNums { get; private set; } // For data logging purposes

    // The majority from the buffer; Starts off as in rest
    public GameLookup.graspNamesEnum graspPrediction { get; private set; } = GameLookup.graspNamesEnum.Rest;
    public bool wantsToSkip { get; private set; } = false;


    private void Awake()
    {
        buffer = new Queue<GameLookup.graspNamesEnum>();
        bufferNums = new int[bufferSize];

        // Default values are the rest state
        for (int i = 0; i < bufferSize; i++)
        {
            bufferNums[i] = 10;
        }
    }


    // Handling Input Events
    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            playerInput.KeyboardInput_Universal.PauseGame.performed += i => pauseMenu.Pause();

            playerInput.KeyboardInput_Universal.Skip.performed += i => wantsToSkip = true;
            playerInput.KeyboardInput_Universal.Skip.canceled += i => wantsToSkip = false;

            // Keyboard & Delsys Specific Bindings
            if (PlayerData.PlayerDataRef.usingDelsys)
            {
                Debug.Log("TODO: Add Delsys Specific Bindings!");
            } else
            {
                // Consult GameLookup -> graspNamesEnum to see what grasps these are binding to
                playerInput.KeyboardInput_Grasps.GraspInput_Key0.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)0;
                playerInput.KeyboardInput_Grasps.GraspInput_Key0.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key1.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)1;
                playerInput.KeyboardInput_Grasps.GraspInput_Key1.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key2.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)2;
                playerInput.KeyboardInput_Grasps.GraspInput_Key2.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key3.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)3;
                playerInput.KeyboardInput_Grasps.GraspInput_Key3.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key4.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)4;
                playerInput.KeyboardInput_Grasps.GraspInput_Key4.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key5.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)5;
                playerInput.KeyboardInput_Grasps.GraspInput_Key5.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key6.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)6;
                playerInput.KeyboardInput_Grasps.GraspInput_Key6.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key7.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)7;
                playerInput.KeyboardInput_Grasps.GraspInput_Key7.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key8.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)8;
                playerInput.KeyboardInput_Grasps.GraspInput_Key8.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;

                playerInput.KeyboardInput_Grasps.GraspInput_Key9.performed += i => currentGraspInput = (GameLookup.graspNamesEnum)9;
                playerInput.KeyboardInput_Grasps.GraspInput_Key9.canceled += i => currentGraspInput = GameLookup.graspNamesEnum.Rest;
            }
        }

        playerInput.Enable();
    }

    // Input events are taken care of in the beginning of FixedUpdate()
    // This means that we are always acting on the current input and not a frame behind
    private void FixedUpdate()
    {
        AddToBuffer(currentGraspInput);
    }

    // Appends current input to end of queue; removes whatever is in the front of the queue
    // Re-evaluates what the majority value in the queue is following this change
    private void AddToBuffer(GameLookup.graspNamesEnum grasp)
    {
        buffer.Enqueue(grasp);
        if (buffer.Count > bufferSize)
            buffer.Dequeue();

        MajorityFromBuffer();
    }


    // Retreives the most frequently occuring value in the buffer
    private void MajorityFromBuffer()
    {
        Dictionary<GameLookup.graspNamesEnum, int> frequencyMap = new Dictionary<GameLookup.graspNamesEnum, int>();

        int i = 0;
        foreach (GameLookup.graspNamesEnum grasp in buffer)
        {
            if (frequencyMap.ContainsKey(grasp)) {
                int freq = frequencyMap[grasp];
                freq++;
                frequencyMap[grasp] = freq;
            } else
            {
                frequencyMap.Add(grasp, 1);
            }

            // For purposes of printing out logging contents of buffer
            bufferNums[i] = (int)grasp;
            i++;
        }

        int minCount = 0;
        GameLookup.graspNamesEnum result = GameLookup.graspNamesEnum.Rest;

        foreach (KeyValuePair<GameLookup.graspNamesEnum, int> pair in frequencyMap)
        {
            if (minCount < pair.Value)
            {
                result = pair.Key;
                minCount = pair.Value;
            }
        }

        graspPrediction = result;
    }

    // Handling Input Events
    private void OnDisable()
    {
        playerInput.Disable();
    }





}
