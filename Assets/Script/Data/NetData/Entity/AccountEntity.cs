// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-23 17:34:39
// 版 本：v 1.0
// ========================================================
using System;
using System.Collections.Generic;

public class AccountEntity
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Pwd { get; set; }

    public int YuanBao { get; set; }

    public int LastServerId { get; set; }

    public string LastServerName { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
