using UnityEngine;
using System.Globalization;

public class MyMessageListener : MonoBehaviour
{
    [SerializeField] private Frog1Script frog1;
    [SerializeField] private Frog2ScriptNew frog2;

    void OnMessageArrived(string msg)
    {
        if (string.IsNullOrEmpty(msg))
            return;


        if (TryParseCSV(msg, out Vector2 v1, out Vector2 v2))
        {
            // Inverte eixo X 
            v1.x = -v1.x;
            v2.x = -v2.x;

            frog1.OnAimArduino(v1);
            frog2.OnAimArduino(v2);
        }
    }

    bool TryParseCSV(string input, out Vector2 v1, out Vector2 v2)
    {
        v1 = Vector2.zero;
        v2 = Vector2.zero;

        try
        {
            // Divide a linha CSV em 4 valores
            string[] parts = input.Trim().Split(',');

            if (parts.Length < 4)
                return false;

            // Usa InvariantCulture para garantir que o '.' é aceito como separador decimal
            float lr1 = float.Parse(parts[0], CultureInfo.InvariantCulture);
            float ud1 = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float lr2 = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float ud2 = float.Parse(parts[3], CultureInfo.InvariantCulture);

            v1 = new Vector2(lr1, ud1);
            v2 = new Vector2(lr2, ud2);

            return true;
        }
        catch
        {
            return false;
        }
    }

    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? " Arduino conectado" : "Arduino desconectado");
    }
}
