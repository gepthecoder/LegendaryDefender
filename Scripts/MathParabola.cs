using UnityEngine;

public class MathParabola
{
    private static float xpos;
    private static float zpos;

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float f(float x) => -4 * height * x * x + 4 * height * x;

        if (t > 0)
        {
            xpos = (end.x - start.x) * t + start.x;
            zpos = (end.z - start.z) * t + start.z;
        }

        return new Vector3(xpos, f(t) + Mathf.Lerp(start.y, end.y, t), zpos);
    }
}