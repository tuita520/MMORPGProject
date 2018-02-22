// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-22 15:57:32
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 商品实体
/// </summary>
public partial class ProductEntity : AbstractEntity
{
    /// <summary>
    /// 商品名称
    /// </summary>
    public string Name
    {
        get;
        set;
    }

    /// <summary>
    /// 商品价格
    /// </summary>
    public int Price
    {
        get;
        set;
    }

    /// <summary>
    /// 商品图片名称
    /// </summary>
    public string PicName
    {
        get;
        set;
    }

    /// <summary>
    /// 商品描述
    /// </summary>
    public string Desc
    {
        get;
        set;
    }
}
