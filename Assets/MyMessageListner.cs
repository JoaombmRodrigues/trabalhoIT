using UnityEngine;

public class MyMessageListener : MonoBehaviour
{
    [SerializeField] private Frog1Script frog1;
    [SerializeField] private Frog2ScriptNew frog2;
    void OnMessageArrived(string msg)
    {
        Debug.Log("poop");
        if (string.IsNullOrEmpty(msg))
            return;

        Debug.Log($"Raw message: {msg}");

        if (TryParseVectors(msg, out Vector2 v1, out Vector2 v2))
        {
            Debug.Log($"V1: {v1} | V2: {v2}");
            frog1.OnAimArduino(v1);
            frog2.OnAimArduino(v2);
        }
    }

    bool TryParseVectors(string input, out Vector2 v1, out Vector2 v2)
    {
        v1 = Vector2.zero;
        v2 = Vector2.zero;

        try
        {
            string[] parts = input.Split(';');
            if (parts.Length < 2)
                return false;

            string[] v1parts = parts[0].Replace("V1:", "").Split(',');
            string[] v2parts = parts[1].Replace("V2:", "").Split(',');

            v1 = new Vector2(float.Parse(v1parts[0]), float.Parse(v1parts[1]));
            v2 = new Vector2(float.Parse(v2parts[0]), float.Parse(v2parts[1]));
            return true;
        }
        catch
        {
            return false;
        }
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Arduino connected" : "Arduino disconnected");
    }
}