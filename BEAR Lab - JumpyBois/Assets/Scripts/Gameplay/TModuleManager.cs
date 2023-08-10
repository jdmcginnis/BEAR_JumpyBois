using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TModuleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] reindeerModules;
    [SerializeField] private GameObject[] sealModules;

    // link enum to arrays holding module arrays?

    public int testCounter { get; private set; }

    private void Start()
    {
        testCounter = 0;

        
    }

    // Called from CameraMotor.cs upon collision trigger
    public void LoadNextModule()
    {
        if (testCounter >= GlobalStorage.GameSettings.numTests)
        {
            LoadEndingModule();
            return;
        }

        Debug.Log("Loading a new module!");

        // int nextTModuleInd = rnd.Next(1, )



        testCounter++;
    }

    private void LoadEndingModule()
    {
        Debug.Log("This is the end!");
    }

    private void GetRandomNumber()
    {

    }
}
