// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-22 10:35:26
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 测试byte数组和其他类型数据的转换
/// </summary>
public class TestMMOMemory : MonoBehaviour {

	void Start () {
        //int 4个字节 （byte）
        int a = 10;

        //把int 转换成byte数组
        byte[] arr = BitConverter.GetBytes(a);

        for (int i = 0; i < arr.Length; i++)
        {
            Debug.Log(string.Format("arr{0}={1}",i,arr[i]));
        }

        byte[] arr2 = new byte[4];
        arr2[0] = 10;
        arr2[1] = 0;
        arr2[2] = 0;
        arr2[3] = 0;

        Debug.Log(BitConverter.ToInt32(arr2,0));
    }
	
	void Update () {
	
	}
}
