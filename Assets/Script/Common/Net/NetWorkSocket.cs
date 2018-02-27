// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-24 15:00:24
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System;
using System.Text;

/// <summary>
/// 网络传输Socket
/// </summary>
public class NetWorkSocket : MonoBehaviour
{

    #region 单例
    private static NetWorkSocket instance;

    public static NetWorkSocket Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("NetWorkSocket");
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<NetWorkSocket>();
            }
            return instance;
        }
    }
    #endregion


    private byte[] buffer = new byte[10240];

    #region 发送消息所需变量
    //发送消息队列
    private Queue<byte[]> m_SendQueue = new Queue<byte[]>();

    //检查队列的委托
    private Action m_CheckSendQueue;

    //压缩数组的长度界限
    private const int m_CompressLen = 200;

    #endregion

    #region 接收消息所需的变量
    //接收数据包的字节数组缓冲区
    private byte[] m_ReceiveBuffer = new byte[10240];

    //接收数据包的缓冲数据流
    private MMO_MemoryStream m_ReceiveMs = new MMO_MemoryStream();

    //接收消息的队列
    private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();

    private int m_ReceiveCount = 0;
    #endregion

    /// <summary>
    /// 客户端Socket
    /// </summary>
    private Socket m_Client;

    #region Connect 连接到Socket服务器

    /// <summary>
    /// 连接到Socket服务器
    /// </summary>
    /// <param name="ip">ip</param>
    /// <param name="port">端口号</param>
    public void Connect(string ip, int port)
    {
        //如果Socket已经存在 并且处于连接状态 直接返回
        if (m_Client != null && m_Client.Connected) return;

        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            m_Client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            m_CheckSendQueue = OnCheckSendQueueCallBack;

            ReceiveMsg();
        }
        catch (System.Exception ex)
        {
            Debug.Log("连接失败 " + ex.Message);
        }
    }

    #endregion

    #region OnCheckSendQueueCallBack  检查队列的委托回调
    /// <summary>
    /// 
    /// </summary>
    private void OnCheckSendQueueCallBack()
    {
        lock (m_SendQueue)
        {
            //如果队列中有数据包则发送数据包
            if (m_SendQueue.Count > 0)
            {
                //发送数据包
                Send(m_SendQueue.Dequeue());
            }
        }
    }
    #endregion

    #region MakeData 封装数据包
    /// <summary>
    /// 封装数据包
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private byte[] MakeData(byte[] data)
    {
        byte[] retBuffer = null;

        //1、对数据进行压缩检测
        bool isCompress = data.Length > m_CompressLen ? true : false;
        if (isCompress)
        {
            data = ZlibHelper.CompressBytes(data);
        }

        //2、先异或
        data = SecurityUtil.Xor(data);

        //3、计算Crc校验值
        ushort crc = Crc16.CalculateCrc16(data);

        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort((ushort)(data.Length + 3));

            ms.WriteBool(isCompress);

            ms.WriteUShort(crc);

            ms.Write(data, 0, data.Length);

            retBuffer = ms.ToArray();
        }

        return retBuffer;

    }
    #endregion

    #region SendMsg 发送消息 将消息加入队列
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="buffer"></param>
    public void SendMsg(byte[] buffer)
    {
        //得到封装后的数据包
        byte[] sendBuffer = MakeData(buffer);

        lock (m_SendQueue)
        {
            //将数据包加入队列
            m_SendQueue.Enqueue(sendBuffer);

            //启动委托（执行委托）
            m_CheckSendQueue.BeginInvoke(null, null);
        }
    }
    #endregion

    #region Send 真正发送数据包到服务器
    /// <summary>
    /// 真正发送数据包到服务器
    /// </summary>
    /// <param name="buffer"></param>
    private void Send(byte[] buffer)
    {
        m_Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
    }
    #endregion

    #region SendCallBack 发送数据包的回调
    /// <summary>
    /// 发送数据包的回调
    /// </summary>
    /// <param name="ar"></param>
    private void SendCallBack(IAsyncResult ar)
    {
        m_Client.EndSend(ar);

        //继续检查队列
        OnCheckSendQueueCallBack();
    }
    #endregion

    //==============================================================

    #region ReceiveMsg 接收数据
    /// <summary>
    /// 接收数据
    /// </summary>
    private void ReceiveMsg()
    {
        //异步接收数据
        m_Client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);
    }
    #endregion

    #region ReceiveCallBack 接收数据回调
    /// <summary>
    /// 接收数据回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            int len = m_Client.EndReceive(ar);
            if (len > 0)
            {
                //已经接收到数据
                //把接收到的数据 写入缓冲数据流的尾部
                m_ReceiveMs.Position = m_ReceiveMs.Length;

                //把指定长度的字节写入数据流
                m_ReceiveMs.Write(m_ReceiveBuffer, 0, len);

                //byte[] buffer = m_ReceiveMs.ToArray();

                //如果缓存数据流的长度 大于 2 说明至少有个不完整的包过来
                //我们客户端封装数据包 用的ushort 长度就是2
                if (m_ReceiveMs.Length > 2)
                {
                    //循环拆分数据包
                    while (true)
                    {
                        //把数据流指针位置放在0处
                        m_ReceiveMs.Position = 0;

                        //当前包体长度
                        int currMsgLen = m_ReceiveMs.ReadUShort();

                        //当前总包的长度
                        int currFullMsgLen = 2 + currMsgLen;

                        //如果数据流长度大于或等于总包长度 说明至少收到一个完整包
                        if (m_ReceiveMs.Length >= currMsgLen)
                        {
                            //收到完整包
                            byte[] buffer = new byte[currMsgLen];

                            //把数据流指针放在包体位置
                            m_ReceiveMs.Position = 2;

                            //把包体读取到byte数组中
                            m_ReceiveMs.Read(buffer, 0, currMsgLen);

                            //将消息放入接收消息的队列
                            lock (m_ReceiveQueue)
                            {
                                m_ReceiveQueue.Enqueue(buffer);
                            }

                            //这里的buffer就是我们拆分的数据包
                            //using (MMO_MemoryStream ms2 = new MMO_MemoryStream(buffer))
                            //{
                            //    string msg = ms2.ReadUTF8String();
                            //    Debug.Log("接收的消息是 " + msg);
                            //}

                            //处理剩余字节长度
                            int remainLen = (int)(m_ReceiveMs.Length - currFullMsgLen);
                            if (remainLen > 0)
                            {
                                //把指针放在第一个包的尾部
                                m_ReceiveMs.Position = currFullMsgLen;

                                //定义剩余字节数组
                                byte[] remainBuffer = new byte[remainLen];

                                //将数据流读取到剩余字节数组当中
                                m_ReceiveMs.Read(remainBuffer, 0, remainLen);

                                //清空数据流
                                m_ReceiveMs.Position = 0;
                                m_ReceiveMs.SetLength(0);

                                //将剩余字节数组重新写入数据流
                                m_ReceiveMs.Write(remainBuffer, 0, remainBuffer.Length);

                                remainBuffer = null;
                            }
                            else
                            {
                                //没有剩余字节
                                //清空数据流
                                m_ReceiveMs.Position = 0;
                                m_ReceiveMs.SetLength(0);

                                break;
                            }
                        }
                        else
                        {
                            //还没有完整包
                            break;
                        }
                    }
                }

                //进行下一次接收数据包
                m_Client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);
            }
            else
            {
                //说明服务器断开连接了
                Debug.Log(string.Format("服务器{0}断开连接了", m_Client.RemoteEndPoint.ToString()));
            }
        }
        catch
        {
            //说明客户端断开连接了
            Debug.Log(string.Format("服务器{0}断开连接了", m_Client.RemoteEndPoint.ToString()));
        }


    }
    #endregion


    #region Update 真正从队列中读取数据
    void Update()
    {
        #region 从队列中获取数据
        while (true)
        {
            if (m_ReceiveCount <= 5)
            {
                m_ReceiveCount++;

                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {
                        //得到队列中的数据包
                        byte[] buffer = m_ReceiveQueue.Dequeue();

                        //异或之后的数组
                        byte[] bufferNew = new byte[buffer.Length - 3];

                        bool isCompress = false;
                        ushort crc = 0;

                        using(MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
                        {
                            isCompress = ms.ReadBool();
                            crc = ms.ReadUShort();
                            ms.Read(bufferNew, 0, bufferNew.Length);
                            
                        }

                        //1、CRC 计算
                        int newCrc = Crc16.CalculateCrc16(bufferNew);
                        if(newCrc == crc)
                        {
                            //异或得到原始数据
                            bufferNew = SecurityUtil.Xor(bufferNew);

                            if (isCompress)
                            {
                                bufferNew = ZlibHelper.DeCompressBytes(bufferNew);
                            }

                            ushort protoCode = 0;
                            byte[] protoContent = new byte[bufferNew.Length - 2];
                            using (MMO_MemoryStream ms = new MMO_MemoryStream(bufferNew))
                            {
                                //协议编号
                                protoCode = ms.ReadUShort();
                                //将协议内容写入字节数组
                                ms.Read(protoContent, 0, protoContent.Length);

                                //观察者 分发协议
                                EventDispatcher.Instance.Dispatch(protoCode, protoContent);
                            }
                        }
                        else
                        {
                            break;//校验失败
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                m_ReceiveCount = 0;
                break;
            }

        }
        #endregion

    }
    #endregion

    /// <summary>
    /// 用户退出网络
    /// </summary>
    void OnDestroy()
    {
        if (m_Client != null && m_Client.Connected)
        {
            m_Client.Shutdown(SocketShutdown.Both);
            m_Client.Close();
        }
    }
}
