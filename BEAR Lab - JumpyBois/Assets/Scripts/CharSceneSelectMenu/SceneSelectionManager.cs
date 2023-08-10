using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelectionManager : MonoBehaviour
{
    public enum sceneChoices
    {
        arctic // 0
    }

    int defaultScene = (int)sceneChoices.arctic;

    public void LogSceneData()
    {
        GlobalStorage.GameSettings.sceneSelection = ((sceneChoices)defaultScene).ToString();
    }
}
