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
using LitJson;

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
        //ProductEntity entity = ProductDBModel.Instance.Get(5);
        //if(entity != null)
        //{
        //    Debug.Log(entity.Name);
        //}

        //测试http get请求
        //if (!NetWorkHttp.Instance.IsBusy)
        //{
        //    NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/account?id=100", CallBack);
        //    Debug.Log("访问账号服务器URL " + GlobalInit.WebAccountUrl + "api/account?id=100");
        //}

        //测试http post请求
        //if (!NetWorkHttp.Instance.IsBusy)
        //{
        //    JsonData jsonData = new JsonData();
        //    jsonData["type"] = 0;//0 注册 1 登录
        //    jsonData["UserName"] = "哈哈";
        //    jsonData["Pwd"] = "213";
        //    NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/account", PostCallBack,true,jsonData.ToJson());
        //}

        //测试客户端与服务器的连接 和通信
        //NetWorkSocket.Instance.Connect("127.0.0.1",111);

        //using (MMO_MemoryStream ms = new MMO_MemoryStream())
        //{
        //    ms.WriteUTF8String("你好啊");

        //    NetWorkSocket.Instance.SendMsg(ms.ToArray());
        //}

        TestProto proto = new TestProto();
        proto.Id = 1;
        proto.Name = "测试";
        proto.Type = 0;
        proto.Price = 99.99f;

        byte[] buffer = null;

        //1、json 方式
        //string json = JsonMapper.ToJson(proto);
        //using (MMO_MemoryStream ms = new MMO_MemoryStream())
        //{
        //    ms.WriteUTF8String(json);
        //    buffer = ms.ToArray();
        //}
        //Debug.Log("buffer = "+buffer.Length);

        //2、自定义
        buffer = proto.ToArray();
        Debug.Log("buffer = "+buffer.Length);

        TestProto proto2 = TestProto.GetProto(buffer);
        Debug.Log(proto2.Name);

        //自定义协议传输 相比Json传输 体积更小 而且json不能传输float类型的数据
    }

    private void GetCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (obj.HasError)
        {
            Debug.Log("找不到用户");
        }
        else
        {
            AccountEntity entity = JsonMapper.ToObject<AccountEntity>(obj.Json);
            Debug.Log(entity.UserName);
        }
        
    }

    private void PostCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (obj.HasError)
        {
            Debug.Log("找不到用户");
        }
        else
        {
            RetValue ret = JsonMapper.ToObject<RetValue>(obj.Json);
            if (!ret.HasError)
            {
                Debug.Log("用户编号= "+ret.RetData);
            }
        }

    }
}

public class Item
{
    public int ID;
    public string Name;
}
