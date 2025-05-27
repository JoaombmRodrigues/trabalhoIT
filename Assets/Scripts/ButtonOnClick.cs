using UnityEngine;

public class ButtonOnClick : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        SceneLoader.Instance.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        SceneLoader.Instance.ReloadScene();
    }

    public void QuitGame()
    {
        SceneLoader.Instance.QuitGame();
    }
}
