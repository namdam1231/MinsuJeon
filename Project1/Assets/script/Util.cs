using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util 
{
    public static GameObject FindChildObject(GameObject gObj, string childName)
    {
        var childObjs = gObj.GetComponentsInChildren<Transform>();
        for(int i=0; i<childObjs.Length; i++)
        {
            if (childObjs[i].name.Equals(childName))
                return childObjs[i].gameObject;
        }
        return null;
    }
}
