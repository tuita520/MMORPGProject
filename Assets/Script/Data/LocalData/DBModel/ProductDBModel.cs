// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-22 16:04:23
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 商品数据管理
/// </summary>
public class ProductDBModel : IDisposable{

    private List<ProductEntity> list;
    private Dictionary<int, ProductEntity> dic;

    private static ProductDBModel instance;

    public static ProductDBModel Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ProductDBModel();
                instance.Load();
            }
            return instance;
        }
    }


    private ProductDBModel()
    {
        list = new List<ProductEntity>();
        dic = new Dictionary<int, ProductEntity>();
    }

    private void Load()
    {
        //读取文件
        //路径问题
        using (GameDataTableParser parse = new GameDataTableParser(@"D:\Github项目\MMORPGProject\www\Data\Product.data"))
        {
            while (!parse.Eof)
            {
                ProductEntity entity = new ProductEntity();
                entity.Id = parse.GetFieldValue("Id").ToInt();
                entity.Name = parse.GetFieldValue("Name");
                entity.Price = parse.GetFieldValue("Price").ToInt();
                entity.PicName = parse.GetFieldValue("PicName");
                entity.Desc = parse.GetFieldValue("Desc");

                list.Add(entity);
                dic[entity.Id] = entity;

                parse.Next();
            }
        }
    }

    /// <summary>
    /// 通过商品ID查询商品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ProductEntity Get(int id)
    {
        if (dic.ContainsKey(id))
        {
            return dic[id];
        }
        return null;
    } 

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public List<ProductEntity> GetList()
    {
        return list;
    }

    public void Dispose()
    {
        list.Clear();
        list = null;
    }
}
