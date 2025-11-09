using UnityEngine;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSettings playerSettings;
    [SerializeField]
    private Frog1Script frog1Script;
    [SerializeField]
    private Frog2ScriptNew frog2Script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frog1Script.SetToAutomaticOrFalse(playerSettings.isAutomatic);
        frog2Script.SetToAutomaticOrFalse(playerSettings.isAutomatic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
