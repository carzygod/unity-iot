using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;
using UnityEngine.UI;
using System;
using System.Text;

using System.Net.WebSockets;
using System.Threading;
public class canmer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
      WebSocket();
       
        Debug.Log("初始化");
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
              sendWebSocket();
                    Debug.Log(gameObject.name);
                    
            }
        }

}

//🔥websocket相关

    public string urll = "ws://tmp.sidcloud.cn:33000";
 
    public String msg =  "wph003";
 
    public Text log;
 
             ClientWebSocket ws = new ClientWebSocket();
            CancellationToken ct = new CancellationToken();
               public async void sendWebSocket()
               {
 await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("wph003")), WebSocketMessageType.Binary, true, ct); //发送数据
               }
   public async void WebSocket()
    {
        try
        {

            //添加header
            //ws.Options.SetRequestHeader("X-Token", "eyJhbGciOiJIUzI1N");
            Uri url = new Uri(urll);
            await ws.ConnectAsync(url, ct);
            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("wph003")), WebSocketMessageType.Binary, true, ct); //发送数据
            while (true)
            {
                var result = new byte[1024];
                await ws.ReceiveAsync(new ArraySegment<byte>(result), new CancellationToken());//接受数据
                var str = Encoding.UTF8.GetString(result, 0, result.Length);
                Debug.Log(str);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


}
