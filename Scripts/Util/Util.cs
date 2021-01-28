using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static GameObject FindChildObject(GameObject go, string objectName)
    {
        var childs = go.GetComponentsInChildren<Transform>();
        for(int i = 0; i < childs.Length; i++)
        {
            if (childs[i].name.Equals(objectName))
                return childs[i].gameObject;
        }
        return null;
    }
    public static bool IsFloatEqual(float a, float b)
    {
        if (a >= b - Mathf.Epsilon && a <= b + Mathf.Epsilon)
            return true;
        else
            return false;
    }
    public static T GetEnum<T>(string str)
    {
        var array = System.Enum.GetValues(typeof(T));

        for(int i = 0; i < array.Length; i++)
        {
            if (array.GetValue(i).ToString().Equals(str))
                return (T)array.GetValue(i);
        }
        return default(T);
    }
}
