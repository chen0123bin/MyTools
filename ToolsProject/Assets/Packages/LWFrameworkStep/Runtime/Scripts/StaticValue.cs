using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode {
    waitMode,   //观摩
    watchMode ,   //观摩
    virtualMode,//教学
    independentMode,//练习
    testMode//测试
}
public class StaticValue {
    private static Mode currMode = Mode.virtualMode;

    public static Mode CurrMode
    {
        get
        {
            return currMode;
        }

        set
        {
            currMode = value;
        }
    }
    
}
