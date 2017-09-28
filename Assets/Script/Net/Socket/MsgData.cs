using System.IO;
using System;
using UnityEngine;
using Google.Protobuf;
using protocal_loginserver;

public class MsgData
{
	private byte[] bytes;
	private IMessage pData;
	private Int32 msgId; 

	public byte[] Bytes{
		get { return bytes; }
	}

	public IMessage PData {
		get { return pData; }
	}

	public MsgData(byte[] bytes,int msgId){
		this.bytes = bytes;
		this.msgId = msgId;
	}

	public MsgData(IMessage pData,int msgId){
		this.pData = pData;
		byte[] tmp = pData.ToByteArray ();
		int length = tmp.GetLength () + 8;
		this.bytes = new byte[length];
		MemoryStream ms = new MemoryStream ();
		StreamWriter writer = new StreamWriter (ms);
		writer.Write ((Int32)length);
		writer.Write ((Int32)msgId);
		writer.Write (tmp);
		writer.Flush ();
		this.bytes = ms.ToArray ();
	}
}

