using UnityEngine;

public static class MathfExtetntions
{
    public static Vector3 ClampInCircle(Vector3 position, Vector3 circleCenter, float radius)
    {
        Vector3 direction = position - circleCenter;

        if (direction.magnitude <= radius)
            return position;

        direction.Normalize();
        direction *= radius;

        return circleCenter + direction;
    }
}
