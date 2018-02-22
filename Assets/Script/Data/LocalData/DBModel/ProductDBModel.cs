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
public class ProductDBModel : AbstractDBModel<ProductDBModel, ProductEntity>
{
    protected override string FileName{ get { return "Product.data"; } }

    protected override ProductEntity MakeEntity(GameDataTableParser parse)
    {
        ProductEntity entity = new ProductEntity();
        entity.Id = parse.GetFieldValue("Id").ToInt();
        entity.Name = parse.GetFieldValue("Name");
        entity.Price = parse.GetFieldValue("Price").ToInt();
        entity.PicName = parse.GetFieldValue("PicName");
        entity.Desc = parse.GetFieldValue("Desc");

        return entity;
    }
}
