using UnityEngine;
using System.Collections;
using System.IO;
public class Main : MonoBehaviour 
{
	void Start () 
    {
        InitNet();
        InitManager();
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
		
}
