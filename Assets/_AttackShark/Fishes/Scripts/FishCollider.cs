using UnityEngine;

public class FishCollider : MonoBehaviour
{
    private Collider _collider;

    private Collider Collider
    {
        get
        {
            if (_collider == null)
                _collider = GetComponent<Collider>();

            return _collider;
        }
    }
}