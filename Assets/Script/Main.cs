using UnityEngine;
using System.Collections;
using System.IO;
public class Main : MonoBehaviour 
{
	void Start () 
    {
        InitNet();
        InitManager();
		InitTest ();
//        ChatView.OpenView("Prefabs/ChatView/ChatView");
	}

    private void InitNet()
    {
//        gameObject.AddComponent<SocketManager>();
//        SocketManager.Instance.Connect();
    }

    private void InitManager()
    {
//        gameObject.AddComponent<LoginModel>();
//        gameObject.AddComponent<ChatModel>();
    }

	private void InitTest(){
		byte[] arr1 = new byte[10];
		byte[] arr2 = new byte[20];

		Debug.Log (arr1.Length + "-" + arr1.GetLength ());
		arr1 = arr2;
		Debug.Log (arr1.Length + "-" + arr1.GetLength ());
	}
}
