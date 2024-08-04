using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static Vector2 RotateVector2(Vector2 vector, float angle)
    {
        if (angle == 0)
        {
            return vector;
        }
        float sinus = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cosinus = Mathf.Cos(angle * Mathf.Deg2Rad);

        float oldX = vector.x;
        float oldY = vector.y;
        vector.x = (cosinus * oldX) - (sinus * oldY);
        vector.y = (sinus * oldX) + (cosinus * oldY);
        return vector;
    }

    public static float Remap(float x, float A, float B, float C, float D)
    {
        float remappedValue = C + (x - A) / (B - A) * (D - C);
        return remappedValue;
    }
}
