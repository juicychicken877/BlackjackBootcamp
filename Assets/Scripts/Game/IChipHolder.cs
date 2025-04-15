using UnityEngine;

public interface IChipHolder
{
    public float Chips { get; }
    public void AddChips(float amount);
    public void ClearChips();
    public void PopChips();
}
