using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class FaceGameMenuManager : MonoBehaviour
{
    static void LoadScene(string sceneName)
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    
    public void OnPlayButtonPressed()
    {
        LoadScene("FaceGame");
    }
    
    public void OnCreateButtonPressed()
    {
        LoadScene("FaceGameCreation");
    }
}
