// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-22 10:35:26
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// 测试byte数组和其他类型数据的转换
/// </summary>
public class TestMMOMemory : MonoBehaviour {

	void Start () {
        ////int 4个字节 （byte）
        //int a = 10;

        ////把int 转换成byte数组
        //byte[] arr = BitConverter.GetBytes(a);

        //for (int i = 0; i < arr.Length; i++)
        //{
        //    Debug.Log(string.Format("arr{0}={1}",i,arr[i]));
        //}

        //byte[] arr2 = new byte[4];
        //arr2[0] = 10;
        //arr2[1] = 0;
        //arr2[2] = 0;
        //arr2[3] = 0;

        //Debug.Log(BitConverter.ToInt32(arr2,0));

        //Item item = new Item() { ID = 1,Name = "测试"};
        //byte[] arr = null;

        ////将 类 转换成字节数组
        //using (MMO_MemoryStream ms = new MMO_MemoryStream())
        //{
        //    ms.WriteInt(item.ID);
        //    ms.WriteUTF8String(item.Name);

        //    arr = ms.ToArray();
        //};

        //Item item2 = new Item();

        //using (MMO_MemoryStream ms = new MMO_MemoryStream(arr))
        //{
        //    item2.ID = ms.ReadInt();
        //    item2.Name = ms.ReadUTF8String();
        //}

        //Debug.Log(item2.ID);
        //Debug.Log(item2.Name);

        //测试获取商品本地数据表的所有数据
        //List<ProductEntity> list = ProductDBModel.Instance.GetList();

        //for (int i = 0; i < list.Count; i++)
        //{
        //    Debug.Log(list[i].Name);
        //}

        //Debug.Log(list.Count);

        //测试获取商品本地数据表中某已条件下的商品
        ProductEntity entity = ProductDBModel.Instance.Get(5);
        if(entity != null)
        {
            Debug.Log(entity.Name);
        }
    }
}

public class Item
{
    public int ID;
    public string Name;
}
