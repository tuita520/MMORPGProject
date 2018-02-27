// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-11 17:09:10
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 公共资源管理
/// </summary>
public class GlobalInit : MonoBehaviour {

    public delegate void OnReceiveProtoHandler(ushort protoCode, byte[] buffer);

    public OnReceiveProtoHandler OnReceiveProto;

    public static GlobalInit Instance;

    /// <summary>
    /// 昵称 Key
    /// </summary>
    public const string MMO_NICKNAME = "MMO_NICKNAME";

    /// <summary>
    /// 密码 key
    /// </summary>
    public const string MMO_PWD = "MMO_PWD";

    public const string WebAccountUrl = "http://localhost:9855/";

    public const string SocketIP = "127.0.0.1";

    public const ushort Port = 111;

    /// <summary>
    /// 玩家注册的昵称
    /// </summary>
    [HideInInspector]
    public string CurrRoleNickName;

    /// <summary>
    /// 当前玩家
    /// </summary>
    [HideInInspector]
    public RoleCtrl CurrPlayer;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

}
