// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-22 16:18:31
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

public static class StringUtil {

    /// <summary>
    /// 把string类型转换成int
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        int temp = 0;
        int.TryParse(str, out temp);
        return temp;
    }

    /// <summary>
    /// 把string类型转换成float
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float ToFloat(this string str)
    {
        float temp = 0;
        float.TryParse(str, out temp);
        return temp;
    }
}
