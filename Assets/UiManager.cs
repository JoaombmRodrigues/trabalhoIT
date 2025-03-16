using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    private Frog1Script F1S;
    [SerializeField]
    private Image bar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Normalize the value for UI fill (0-1 range)
        float fillValue = (F1S.NowValue() - F1S.MinValue()) / (F1S.MaxValue() - F1S.MinValue());
        bar.fillAmount = fillValue;
    }

}