using UnityEngine;

[CreateAssetMenu()]
public class CardSO : ScriptableObject
{
    public GameObject Prefab;
    public string Name;
    public int Value;
    public bool IsAce;
}
