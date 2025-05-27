using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
    public GameObject sceneLoaderPrefab;

    void Awake()
    {
        if (SceneLoader.Instance == null)
        {
            Instantiate(sceneLoaderPrefab);
        }
    }
}
