using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMenuManager : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync("4.Game");
    }
}
