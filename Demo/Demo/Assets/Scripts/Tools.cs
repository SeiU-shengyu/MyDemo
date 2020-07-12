using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools{

    public static int GetRate()
    {
        return Random.Range(1, 101);
    }

    public static T GetValue<T>(params T[] values)
    {
        return values[Random.Range(0, values.Length)];
    }
}
