using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class KillSteamVRProcess : MonoBehaviour {

    private void OnApplicationQuit()
    {
        KillProcess();
    }



    //查找进程、结束进程
    void KillProcess()
    {
        Process[] pro = Process.GetProcesses();//获取已开启的所有进程

        //遍历所有查找到的进程

        for (int i = 0; i < pro.Length; i++)
        {
            UnityEngine.Debug.Log(pro[i].ProcessName.ToString());
            //判断此进程是否是要查找的进程
            if (pro[i].ProcessName.ToString().ToLower() == "vrmonitor")
            {
                pro[i].Kill();//结束进程
            } //判断此进程是否是要查找的进程
            if (pro[i].ProcessName.ToString().ToLower() == "vrserver")
            {
                pro[i].Kill();//结束进程
            }
        }
    }
}
