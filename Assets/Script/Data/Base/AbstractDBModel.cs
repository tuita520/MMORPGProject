// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-22 16:53:12
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 数据管理基类
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="P"></typeparam>
public abstract class AbstractDBModel<T, P>
    where T : class, new()
    where P : AbstractEntity
{
    protected List<P> m_List;
    protected Dictionary<int, P> m_Dic;

    public AbstractDBModel()
    {
        m_List = new List<P>();
        m_Dic = new Dictionary<int, P>();

        LoadData();
    }

    #region 单例
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
                //instance.Load();
            }
            return instance;
        }
    }
    #endregion

    #region 需要子类实现的属性或方法

    /// <summary>
    /// 数据文件名称
    /// </summary>
    protected abstract string FileName { get; }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected abstract P MakeEntity(GameDataTableParser parse);

    #endregion

    #region 加载数据 LoadData
    /// <summary>
    /// 加载数据
    /// </summary>
    private void LoadData()
    {
        //读取文件
        //路径问题
        using (GameDataTableParser parse = new GameDataTableParser(string.Format(@"D:\Github项目\MMORPGProject\www\Data\{0}", FileName)))
        {
            while (!parse.Eof)
            {
                P p = MakeEntity(parse);
                m_List.Add(p);
                m_Dic[p.Id] = p;

                parse.Next();
            }
        }
    }
    #endregion

    #region 通过编号查询实体 Get
    /// <summary>
    /// 通过编号查询实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public P Get(int id)
    {
        if (m_Dic.ContainsKey(id))
        {
            return m_Dic[id];
        }
        return null;
    }
    #endregion

    #region 获取集合数据 GetList
    /// <summary>
    /// 获取集合数据
    /// </summary>
    /// <returns></returns>
    public List<P> GetList()
    {
        return m_List;
    }
    #endregion

}
