using System.IO;
using System;
using UnityEngine;

public class Databuff 
{
  private int size = 0;
  private int buffLen = 0;
  private byte[] buff; 
  /**
   * 构造函数
   * @param minBuffLen最小缓存区大小
   */
  public void DataBuff(int size = 1600){
    this.size = size;
    this.buff = new byte [size];
  }
  
  public void AddBuff(byte[] data, int len){
    // 超过当前缓存
    if(len > this.size-this.buffLen){
      byte[] tmpBuff = new byte[len + this.buffLen];
      Array.Copy(buff,tmpBuff,this.buffLen);
      Array.Copy(data,tmpBuff,this.buffLen,len);
      this.buff = tmpBuff;
    }else{
      Array.Copy(data,0,this.buff,curPos,len);
    }
    this.buffLen += len;
  }
  
  public bool GetData(out MsgData msgData){
    // 长度不够或者为空
    if(this.buffLen < 4 ){
      return false;
    }
    
    byte[] tmpBuff = new byte[4];
    Array.Copy(this.buff,0,tmpBuff,4);
    int32 msgLen = BitConverter.ToInt32(tmpBuff,0);
    // 包没有完
    if(msgLen > this.buffLen){
      return false;
    }
    
    Array.Copy(this.buff,4,tmpbuff,4);
    int32 msgId = BitConverter.ToInt32(tmpBuff,0);
    byte[] tmpData = new byte[msgLen - 4];
    Array.Copy(this.tmpData,this.buff,msgLen-4);
    
    msgData = new MsgData(tmpData,msgId);
    
    if(this.buffLen > 0 ){
      Array.Copy(this.buff,msgLen,this.buff,0,this.buffLen-msgLen);
    }
    this.buffLen -= msgLen;
    return true;
  }
  
}