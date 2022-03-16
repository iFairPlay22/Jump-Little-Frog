using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class RandomStaticMethods
{
    public static Vector3 GenerateRandomPointIn2dCircle(Vector3 center, float radius)
    {
        float rho = radius * Mathf.Sqrt(Random.Range(0f, 1f));
        float theta = Random.Range(0f, 1f) * 2.0f * Mathf.PI;

        float randomX = center.x + rho * Mathf.Cos(theta);
        float randomY = center.y + rho * Mathf.Sin(theta);

        return new Vector3(randomX, randomY, 0);
    }
}
