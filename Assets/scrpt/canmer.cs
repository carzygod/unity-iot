
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP.WebSocket;
using System.Collections.Generic;
//using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading;
using SocketIO;
//using System.Net.WebSockets;

public class canmer : MonoBehaviour
{
public bool statu = false;
public  GameObject light;
    // Start is called before the first frame update
    void Start()
    {
       Init();
       Connect();
        Debug.Log("wph003初始化");
         
         //light.
         light.SetActive (statu);
    }

    // Update is called once per frame
    void Update()
    {
        //监听
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray,out hitInfo)){
                GameObject gameObject = hitInfo.collider.gameObject;
                    Debug.Log(gameObject.name);
                    if(gameObject.name=="light"){
                        if(statu){
                        statu=false;
                        light.SetActive (statu);
                        Send("{\"target\":\"wph002\",\"msg\":\"off\"}");
                        }else{
                        statu=true;                            
                        light.SetActive (statu);
                        Send( "{\"target\":\"wph002\",\"msg\":\"on\"}");
                        }

                    }
                  
            }
        }

}

//🔥websocket相关

    public string url = "ws://tmp.sidcloud.cn:33000";
 
    public String msg =  "wph003";
 
    //public string url = "ws://localhost:8080//websocket/";
 
    private WebSocket webSocket;
 
    private void Init()
    {
        webSocket = new WebSocket(new Uri(url));
        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessageReceived;
        webSocket.OnError += OnError;
        webSocket.OnClosed += OnClosed;    
    }
 
    private void AntiInit()
    {
        webSocket.OnOpen = null;
        webSocket.OnMessage = null;
        webSocket.OnError = null;
        webSocket.OnClosed = null;
        webSocket = null;
    }
 
    public void Connect()
    {
        webSocket.Open();
        print("发送连接");
    }
 
    private byte[] getBytes(string message)
    {
        byte[] buffer = Encoding.Default.GetBytes(message);
        return buffer;
    }
    public void Send(string str)
    {
        webSocket.Send(str);
    }
 
    public void Close()
    {
        webSocket.Close();
    }
    void OnOpen(WebSocket ws)
    {
        Debug.Log("连接成功");    
        //初始数据
         Send("wph003"); 
        
    }
   /// <summary>
    /// 接收信息
    /// </summary>
    /// <param name="ws"></param>
    /// <param name="msg">接收内容</param>
    void OnMessageReceived(WebSocket ws,string msg)
    {
        Debug.Log(msg);
    }
    /// <summary>
    /// 关闭连接
    /// </summary>
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        Debug.Log(message);

        Init();
    }
 
    private void OnDestroy()
    {
        if (webSocket != null && webSocket.IsOpen)
        {
            webSocket.Close();

        }
    }
   
    /// <summary>
    /// Called when an error occured on client side
    /// </summary>
    void OnError(WebSocket ws, Exception ex)
    {
        string errorMsg = string.Empty;
#if !UNITY_WEBGL || UNITY_EDITOR
        if (ws.InternalRequest.Response != null)
            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}",      ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);
#endif
        Debug.Log(errorMsg);
        Init();
    }
 


}
