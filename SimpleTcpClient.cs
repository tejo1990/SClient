using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class SimpleTcpClient
{
    private readonly TcpClient _tcpClient;
    private readonly string _clientId;

    public SimpleTcpClient(string clientId)
    {
        _tcpClient = new TcpClient();
        _clientId = clientId;
    }

    public async Task ConnectAsync(string serverIp, int serverPort)
    {
        try
        {
            await _tcpClient.ConnectAsync(serverIp, serverPort);
            Console.WriteLine("서버에 연결되었습니다.");

            var stream = _tcpClient.GetStream();

            // 로그인 요청 생성
            var request = new
            {
                Type = "login",
                Data = new Dictionary<string, object>
                {
                    { "id", _clientId } // 클라이언트 ID
                }
            };

            Console.WriteLine("로그인 요청 생성: " + JsonSerializer.Serialize(request));

            string jsonRequest = JsonSerializer.Serialize(request);
            byte[] requestBytes = Encoding.UTF8.GetBytes(jsonRequest + "\n"); // 종료 문자 추가

            // 서버에 요청 전송
            await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

            // 메시지 수신 시작
            _ = ReceiveMessagesAsync(stream);

            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"서버 연결 중 오류 발생: {ex.Message}");
        }
    }

    private async Task ReceiveMessagesAsync(NetworkStream stream)
    {
        var buffer = new byte[1024];
        try
        {
            while (true)
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    Console.WriteLine("서버와의 연결이 끊어졌습니다.");
                    break;
                }
                var responseMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // JSON 문자열을 파싱
                var jsonDocument = JsonDocument.Parse(responseMessage);
                var root = jsonDocument.RootElement;

                // Success 필드 출력
                Console.WriteLine($"Success: {root.GetProperty("Success").GetBoolean()}");

                // Message 필드 디코딩하여 출력
                var message = root.GetProperty("Message").GetString();
                Console.WriteLine($"Message: {message}");

                // Data 필드 출력
                var data = root.GetProperty("Data").ToString();
                Console.WriteLine($"Data: {data}");

                Console.Write("보낼 메시지를 입력하세요: ");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"메시지 수신 중 오류 발생: {ex.Message}");
        }
    }

    public async Task SendMessageAsync(string message)
    {
        try
        {
            if (_tcpClient.Connected)
            {
                var stream = _tcpClient.GetStream();
                byte[] messageBytes = Encoding.UTF8.GetBytes(message + "\n");
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                Console.WriteLine("메시지를 서버로 전송했습니다.");
            }
            else
            {
                Console.WriteLine("서버와의 연결이 끊어졌습니다.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"메시지 전송 중 오류 발생: {ex.Message}");
        }
    }
}