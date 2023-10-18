using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TModuleManager : MonoBehaviour
{
    // Contains prefab tilemaps (TModules) for each type of character
    // "TModules" == GameObjects with Tilemaps Attached
    [SerializeField] private GameObject[] reindeerModules;
    [SerializeField] private GameObject[] sealModules;
    private GameObject[] playerModules; // holds TModules pertaining to our character selection

    private int numTModules;
    private int trialCounter = 0;
    private int currentTModuleInd = 0; // Start TModule
    private float[] playerModulesLength; // Tilemap lengths
    private float[] playerModulesOffset; // Tilemap offsets
    private float spawnLocation = 0; // Center position of TModule

    private void Start()
    {
        switch (PlayerData.PlayerDataRef.characterType)
        {
            case GameLookup.characterTypes.reindeer:
                playerModules = reindeerModules;
                break;
            case GameLookup.characterTypes.seal:
                playerModules = sealModules;
                break;
            default:
                Debug.Log("ERROR: Character not found, add this functionality!");
                break;
        }

        numTModules = playerModules.Length;

        playerModulesLength = new float[numTModules];
        playerModulesOffset = new float[numTModules];

        RecordTModuleLengths();
    }

    private void RecordTModuleLengths()
    {
        int i = 0;

        foreach (GameObject pModule in playerModules)
        {
            BoxCollider2D pModColl = pModule.GetComponent<BoxCollider2D>();
            playerModulesLength[i] = (pModColl.size.x);
            playerModulesOffset[i] = (pModColl.offset.x);

            i++;
        }
    }

    // Called from CameraMotor.cs upon collision w/ModuleEndTrigger
    public void LoadNextModule()
    {
        if (trialCounter >= PlayerData.PlayerDataRef.numTests)
        {
            LoadEndingModule();
            return;
        }

        int nextTModuleInd = GetRandomNumber();

        Instantiate(playerModules[nextTModuleInd], FetchNewModuleLocation(currentTModuleInd, nextTModuleInd), Quaternion.identity, this.transform);

        currentTModuleInd = nextTModuleInd;
        trialCounter++;
    }

    private void LoadEndingModule()
    {
        int nextTModuleInd = playerModules.Length - 1;
        Instantiate(playerModules[nextTModuleInd], FetchNewModuleLocation(currentTModuleInd, nextTModuleInd), Quaternion.identity, this.transform);
    }

    private int GetRandomNumber()
    {
        // Exclude the starting TModule and ending TModule
        int rndNum = Random.Range(1, numTModules - 1);
        
        // Ensure modules do not repeat
        while (rndNum == currentTModuleInd)
            rndNum = Random.Range(1, numTModules - 1);

        return rndNum;
    }

    private Vector3 FetchNewModuleLocation(int currentInd, int nextInd)
    {
        spawnLocation += ((0.5f * playerModulesLength[currentInd]) + playerModulesOffset[currentInd]);
        spawnLocation += ((0.5f * playerModulesLength[nextInd]) + Mathf.Abs(playerModulesOffset[nextInd]));

        return new Vector3(spawnLocation, 0, 0);
    }

}