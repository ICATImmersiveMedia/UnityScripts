using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float MapWrap(float value, float min, float max){
    	return (((value - min) % (max - min)) + (max - min)) % (max - min) + min;
    }
}
