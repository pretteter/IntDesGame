// using Godot;
// using System;
// using System.Net.WebSockets;
// using System.Threading;
// using System.Threading.Tasks;

// public partial class WebSocket : Node
// { private ClientWebSocket webSocket;

//     public override void _Ready()
//     {
//         ConnectToWebSocket();
//     }

//     private async void ConnectToWebSocket()
//     {
//         webSocket = new ClientWebSocket();
//         Uri serverUri = new Uri("ws://localhost:3000/game/");
// 		GD.Print("URI: "+serverUri.AbsolutePath);

//         try
//         {
//             await webSocket.ConnectAsync(serverUri, CancellationToken.None);

//             GD.Print("WebSocket connection established.");

//             // Beginnen Sie mit dem Empfangen von Nachrichten im Hintergrund
//             ReceiveWebSocketMessages();
//         }
//         catch (Exception e)
//         {
//             GD.Print("WebSocket connection failed: " + e.Message);
//         }
//     }

//     private async void ReceiveWebSocketMessages()
//     {
//         while (webSocket.State == WebSocketState.Open)
//         {
//             try
//             {
//                 // Puffer für den Nachrichtenempfang
//                 ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

//                 // Nachricht vom WebSocket-Server empfangen
//                 WebSocketReceiveResult result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

//                 // Nachrichtenverarbeitung
//                 if (result.MessageType == WebSocketMessageType.Text)
//                 {
//                     string message = System.Text.Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
//                     GD.Print("Daten empfangen: " + message);
//                 }
//             }
//             catch (Exception e)
//             {
//                 GD.Print("Fehler beim Empfangen von Daten: " + e.Message);
//                 break;
//             }
//         }
//     }

//     // Optional: WebSocket-Verbindung schließen, wenn das Skript beendet wird
//     public override void _ExitTree()
//     {
//         if (webSocket != null && webSocket.State == WebSocketState.Open)
//         {
//             webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Goodbye", CancellationToken.None).Wait();
//         }
//     }
// }

// using Godot;

// public partial class WebSocket : Node
// {
//  private Resource WebSocketGDScript;
//     private WebSocketConnection webSocketConnection;

//     public override void _Ready()
//     {
//         // Laden Sie das GDScript als Ressource
//         WebSocketGDScript = GD.Load("res://path/to/WebSocketConnection.gd");

//         // Instanziieren Sie das GDScript
//         webSocketConnection = (WebSocketConnection)WebSocketGDScript.Instance();
//         AddChild(webSocketConnection);
//     }
// }

using Godot;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
public partial class WebSocket : Node
{
    private ClientWebSocket webSocket;
    [Signal]
    public delegate void ConnectionEstablishedSignalEventHandler(string message);

    public event ConnectionEstablishedSignalEventHandler ConnectionEstablished;


    public override void _Ready()
    {
        ConnectToWebSocket();
    }

    private async void ConnectToWebSocket()
    {
        webSocket = new ClientWebSocket();
        Uri serverUri = new Uri("ws://localhost:3000/game/");

        try
        {
            await webSocket.ConnectAsync(serverUri, CancellationToken.None);
            GD.Print("WebSocket connected to /game/ path.");
            GD.Print(webSocket);

            // Hier können Sie Code ausführen, der bei erfolgreicher Verbindung ausgeführt werden soll



            // Signal auslösen, um auf erfolgreiche Verbindung zu reagieren
            EmitConnectionEstablishedSignal("You are now connected to the game server!");
            // Beispiel: Nachricht an den Server senden
            await SendWebSocketMessage("connected");

            // Beispiel: Auf Nachrichten vom Server hören
            await ReceiveWebSocketMessage();
        }
        catch (Exception e)
        {
            GD.Print("WebSocket connection error: " + e.Message);
            // Hier können Sie Code ausführen, der bei Verbindungsfehlern ausgeführt werden soll
        }
    }

    private void EmitConnectionEstablishedSignal(string message)
    {
        GD.Print("message to Server " + message);
        ConnectionEstablished?.Invoke(message);
    }

    private async Task SendWebSocketMessage(string message)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    private async Task ReceiveWebSocketMessage()
    {
        try
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    GD.Print("Received message from server: " + receivedData);

                    // Hier können Sie den empfangenen Text verarbeiten
                }
            }
        }
        catch (Exception e)
        {
            GD.Print("Error receiving WebSocket message: " + e.Message);
        }
    }

    // Optional: Schließen Sie die Verbindung, wenn das Skript beendet wird
    public override void _ExitTree()
    {
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).Wait();
        }
    }
}


