using System;
using UnityEngine;

public class ButtonOnClick : MonoBehaviour
{

    [SerializeField]
    private PlayerSettings playerSettings;

    private void Start()
    {
        if (playerSettings != null)
            playerSettings.isAutomatic = false;
    }
    public void LoadScene(string sceneName)
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        SceneLoader.Instance.ReloadScene();
    }
    public void SetToManual()
    {
        playerSettings.isAutomatic = false;
    }
    public void SetToAutomatic()
    {
        playerSettings.isAutomatic = true;
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
