using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class TModuleManager : MonoBehaviour
{
    [SerializeField] private GameObject[] reindeerModules;
    [SerializeField] private GameObject[] sealModules;

    public Random rnd = new Random();
    public int testCounter { get; private set; }
    public int numTModules { get; private set; }
    public int currentTModuleInd { get; private set; }

    private GameObject[] playerModules;

    public float[] playerModulesLength;
    public float[] playerModulesOffset;

    private float cellSize;

    public float spawnLocation; // Center position of current module



    private void Start()
    {
        testCounter = 0;

        switch (GlobalStorage.GameSettings.characterType)
        {
            case GlobalStorage.characterTypes.reindeer:
                playerModules = reindeerModules;
                break;
            case GlobalStorage.characterTypes.seal:
                playerModules = sealModules;
                break;
            default:
                Debug.Log("ERROR: Character not found, add this functionality!");
                break;
        }

        numTModules = playerModules.Length;

        cellSize = this.GetComponent<Grid>().cellSize.x;
        playerModulesLength = new float[numTModules];
        playerModulesOffset = new float[numTModules];
        RecordTModuleLengths();

        currentTModuleInd = 0;
        // spawnLocation = playerModules[0].GetComponent<Transform>().position.x;
        // currentTModRPos = (playerModules[0].GetComponent<BoxCollider2D>().bounds.max.x / cellSize);
        spawnLocation = 0;

    }

    // Called from CameraMotor.cs upon collision trigger
    public void LoadNextModule()
    {
        if (testCounter >= GlobalStorage.GameSettings.numTests)
        {
            LoadEndingModule();
            return;
        }

        int nextTModuleInd = GetRandomNumber();

        Instantiate(playerModules[nextTModuleInd], FetchNewModuleLocation(currentTModuleInd, nextTModuleInd), Quaternion.identity, this.transform);

        currentTModuleInd = nextTModuleInd;
        testCounter++;
    }

    private void LoadEndingModule()
    {
        Debug.Log("This is the end!");

        int nextTModuleInd = playerModules.Length - 1;

        Instantiate(playerModules[nextTModuleInd], FetchNewModuleLocation(currentTModuleInd, nextTModuleInd), Quaternion.identity, this.transform);

    }

    private int GetRandomNumber()
    {
        // Exclude the starting TModule and ending TModule
        int rndNum = rnd.Next(1, numTModules - 1);

        // Ensure modules do not repeat
        while (rndNum == currentTModuleInd)
            rndNum = rnd.Next(1, numTModules - 1);

        return rndNum;
    }

    private Vector3 FetchNewModuleLocation(int currentInd, int nextInd)
    {
        spawnLocation += ((0.5f * playerModulesLength[currentInd]) + playerModulesOffset[currentInd]);
        spawnLocation += ((0.5f * playerModulesLength[nextInd]) + Mathf.Abs(playerModulesOffset[nextInd]));

        return new Vector3(spawnLocation, 0, 0);
    }

    private void RecordTModuleLengths()
    {
        int i = 0;

        foreach (GameObject pModule in playerModules)
        {
            playerModulesLength[i] = (pModule.GetComponent<BoxCollider2D>().size.x);
            playerModulesOffset[i] = (pModule.GetComponent<BoxCollider2D>().offset.x);

            i++;
        }
        
    }

}


