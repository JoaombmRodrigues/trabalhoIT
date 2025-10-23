using System;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [field: SerializeField] public float ComboValue { get; private set; } = 0;
    [SerializeField] private float decayRate = 0.2f;
    [field: SerializeField] public float MaxCombo { get; private set; } = 100f;

    public enum Difficulty{Easy,Medium,Hard,Extreme};
    public event Action<Difficulty> OnDifficultyChanged;
    public Difficulty CurrentDifficulty { get; private set; } = Difficulty.Easy;

    // Update is called once per frame
    void Update()
    {
        if (ComboValue > 0)
        {
            ComboValue -= decayRate * Time.deltaTime;
            ComboValue = Mathf.Max(ComboValue, 0);
            UpdateDifficulty();
        }
    }

    private void UpdateDifficulty()
    {
        Difficulty newDifficulty;

        if (ComboValue < 20) newDifficulty = Difficulty.Easy;
        else if (ComboValue < 50) newDifficulty = Difficulty.Medium;
        else if (ComboValue < 90) newDifficulty = Difficulty.Hard;
        else newDifficulty = Difficulty.Extreme;

        if (newDifficulty != CurrentDifficulty)
        {
            CurrentDifficulty = newDifficulty;
            OnDifficultyChanged?.Invoke(CurrentDifficulty);
        }
    }
    
    public void ModifyComboValue(float value)
    {
        ComboValue += value;
        ComboValue = Mathf.Clamp(ComboValue, 0, MaxCombo);
        UpdateDifficulty();
    }
}
