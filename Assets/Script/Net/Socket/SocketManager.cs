using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System;
using System.Text;
using System.IO;
using Google.Protobuf;

public class SocketManager
{
    private string ip;
    private int port;
	private static SocketManager instance;
    private bool isConnected = false;
    private Socket clientSocket = null;
    private Thread receiveThread = null;
	private Thread sendThread = null;
   	private DataBuffer dataBuffer = new DataBuffer();
	private byte[] receiveBuff = new byte[2048];

	public static SocketManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new SocketManager();
			}
			return instance;
		}
	}

	private void onSendSocket(){
		while (true) {
			if (!clientSocket.Connected)
			{
				isConnected = false;
				ReConnect();
				break;
			}
			lock (MessageCenter.Instance.SendMessageQueue)
			{
				while (MessageCenter.Instance.SendMessageQueue.Count > 0) {
					MsgData md = MessageCenter.Instance.SendMessageQueue.Dequeue ();
					clientSocket.Send (md.Bytes);
				}
			}
		}
	}

    /// <summary>
    /// 接受网络数据
    /// </summary>
    private void onReceiveSocket()
    {
        while (true)
        {
            if (!clientSocket.Connected)
            {
                isConnected = false;
                ReConnect();
                break;
            }
            try
			{
                int receiveLen = clientSocket.Receive(receiveBuff);
                if (receiveLen > 0)
				{
					// 将收到的数据添加到缓存器中
                    dataBuffer.AddBuffer(receiveBuff, receiveLen);
					MsgData msgData;
					// 取出一条完整数据
                    while (dataBuffer.GetData(out msgData))
                    {
                        //锁死消息中心消息队列，并添加数据
						lock (MessageCenter.Instance.RecvMessageQueue)
                        {
                            Debug.Log("收到数据:"+msgData.msgId);
							MessageCenter.Instance.RecvMessageQueue.Enqueue(msgData);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                clientSocket.Disconnect(true);
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                break;
            }
        }
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void Connect(string ip, int port){
		if (isConnected) {
			Debug.Log ("已经连接");
			return;
		}
        this.ip = ip;
        this.port = port;
		try
		{
			clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建套接字
			IPAddress ipAddress = IPAddress.Parse(ip);//解析IP地址
			IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
			IAsyncResult result = clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(onConnectSucess), clientSocket);//异步连接
			bool success = result.AsyncWaitHandle.WaitOne(5000, true);
			if (!success) //超时
			{
				Debug.Log("socket连接超时");
				Close();
			}
		}
		catch (System.Exception e)
		{
			Debug.Log ("连接失败:" + e);
			Close();
		}
    }

	private void onConnectSucess(IAsyncResult iar)
	{
		try
		{
			isConnected = true;

			Socket client = (Socket)iar.AsyncState;
			client.EndConnect(iar);

			receiveThread = new Thread(new ThreadStart(onReceiveSocket));
			receiveThread.IsBackground = true;
			receiveThread.Start();

			sendThread = new Thread(new ThreadStart(onSendSocket));
			sendThread.IsBackground = true;
			sendThread.Start();

			Debug.Log("连接成功");
		}
		catch (Exception e)
		{
			Debug.Log ("socket连接成功创建线程chucuo" + e);
			Close();
		}
	}

	public void SendMsg(MsgData msgData){
		lock (MessageCenter.Instance.SendMessageQueue)
		{
			MessageCenter.Instance.SendMessageQueue.Enqueue(msgData);
		}
	}

	/**
	 * 重连暂时直接连接后面加短线重连逻辑
	 * 
	 */ 
	public void ReConnect()
	{
		this.Connect (this.ip, this.port);
	}

	/// <summary>
	/// 断开
	/// </summary>
	private void Close()
	{
		if (!isConnected) 
		{
			return;
		}

		isConnected = false;

		if (receiveThread != null)
		{
			receiveThread.Abort();
			receiveThread.Join();
			receiveThread = null;
		}

		if (sendThread != null) {
			sendThread.Abort ();
			sendThread.Join ();
			sendThread = null;
		}

		if (clientSocket != null && clientSocket.Connected)
		{
			clientSocket.Close();
			clientSocket = null;
		}
	}

	public void SendMsgAsyc (MsgData msgData){
		if (!clientSocket || !clientSocket.Connected)
		{
			ReConnect();
			return;
		}
		clientSocket.BeginSend (
			msgData.bytes, 
			0, 
			msgData.bytes.Length, 
			SocketFlags.None, 
			new AsyncCallback (sendAsycEnd), 
			clientSocket
		);
	}
//
//	/// <summary>
//	/// 发送消息结果回掉，可判断当前网络状态
//	/// </summary>
//	/// <param name="asyncSend"></param>
	private void sendAsycEnd(IAsyncResult asyncSend)
	{
		try
		{
			Socket client = (Socket)asyncSend.AsyncState;
			client.EndSend(asyncSend);
			Debug.Log("send msg ok:",asyncSend);
		}
		catch (Exception e)
		{
			Debug.Log("send msg exception:" + e.StackTrace);
		}
	}
}