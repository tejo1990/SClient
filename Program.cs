using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var clientId = "user001"; // 클라이언트 ID 설정
        var serverIp = "192.168.45.97"; // 서버 IP 주소
        var serverPort = 7775; // 서버 포트 번호

        var client = new SimpleTcpClient(clientId);
        await client.ConnectAsync(serverIp, serverPort);

        
        while (true)
        {
            Console.WriteLine("보낼 요청을 선택하세요: 1) 로그인 2) 저장 3) 로그인된 사용자 목록 4) 사용자 검색");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int requestNumber))
            {
                string message = string.Empty;

                try
                {
                    switch (requestNumber)
                    {
                        case 1:
                            message = RequestList.CreateLoginRequest(clientId);
                            break;
                        case 2:
                            // 필요한 매개변수를 입력받아 저장 요청 생성
                            Console.WriteLine("사용자 ID를 입력하세요:");
                            var userId = Console.ReadLine();
                            Console.WriteLine("비밀번호를 입력하세요:");
                            var password = Console.ReadLine();
                            Console.WriteLine("smallLevel을 입력하세요:");
                            var smallLevel = Console.ReadLine();
                            Console.WriteLine("bigLevel을 입력하세요:");
                            var bigLevel = Console.ReadLine();
                            Console.WriteLine("contentID를 입력하세요:");
                            var contentID = Console.ReadLine();
                            Console.WriteLine("EndEud를 입력하세요:");
                            var EndEud = Console.ReadLine();
                            Console.WriteLine("CurrentEud를 입력하세요:");
                            var CurrentEud = Console.ReadLine();
                            Console.WriteLine("parID를 입력하세요:");
                            var parID = Console.ReadLine();

                            message = RequestList.CreateSaveRequest(userId, password, smallLevel, bigLevel, contentID, EndEud, CurrentEud, parID);
                            break;
                        case 3:
                            message = RequestList.GetCurrentLoggedInList();
                            break;
                        case 4:
                            Console.WriteLine("검색할 사용자 ID를 입력하세요:");
                            var searchUserId = Console.ReadLine();
                            message = RequestList.SearchUser(searchUserId);
                            break;
                        default:
                            Console.WriteLine("유효하지 않은 번호입니다.");
                            continue;
                    }

                    await client.SendMessageAsync(message);

                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"요청 생성 중 오류 발생: {ex.Message}");
                }

                
            }
            else
            {
                Console.WriteLine("유효한 숫자를 입력하세요.");
            }
            
        }
    }
}
