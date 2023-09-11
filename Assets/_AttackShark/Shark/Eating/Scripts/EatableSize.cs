using UnityEngine;

public class EatableSize : MonoBehaviour
{
    [SerializeField] private int _size;

    public int Size => _size;
}