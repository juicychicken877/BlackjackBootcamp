using UnityEngine;

public interface IChipHolder
{
    public float ChipCount { get; }
    public void AddChips(float amount);
    public void ClearChips();
    public void PopChips();
}
