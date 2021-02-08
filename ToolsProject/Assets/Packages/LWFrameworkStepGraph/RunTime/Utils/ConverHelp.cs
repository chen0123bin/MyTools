using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ConverHelp : Singleton<ConverHelp>
{
    private Assembly m_Assembly;
    public Assembly CurrAssembly {
        get {
            if (m_Assembly == null) {
                m_Assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            }
            return m_Assembly;
        }
    }
    public T CreateInstance<T>(string scriptName) {
        object o = CurrAssembly.CreateInstance(scriptName);
        return (T)o;
    }
}
