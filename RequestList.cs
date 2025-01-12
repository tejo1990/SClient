using System;
using System.Collections.Generic;
using System.Text.Json;

public class RequestList
{
    public static string CreateLoginRequest(string clientId)
    {
        var request = new
        {
            Type = "login",
            Data = new Dictionary<string, object>
            {
                { "id", clientId }
            }
        };

        return JsonSerializer.Serialize(request);
    }

    public static string CreateSaveRequest(string userId, string password, string smallLevel, string bigLevel, string contentID, string EndEud, string CurrentEud, string parID)
    {
        var request = new
        {
            Type = "save",
            Data = new Dictionary<string, object>
            {
                { "id", userId },
                { "PW", password },
                { "smalllevel", smallLevel },
                { "biglevel", bigLevel },
                { "contentID", contentID },
                { "EndEud", EndEud },
                { "CurrentEud", CurrentEud },
                { "parID", parID }
            }
        };

        return JsonSerializer.Serialize(request);
    }


    public static string GetCurrentLoggedInList()
    {
        var request = new
        {
            Type = "List"
        };

        return JsonSerializer.Serialize(request);
    }

    public static string SearchUser(string userId)
    {
        var request = new
        {
            Type = "Search",
            Data = new Dictionary<string, object>
            {
                { "id", userId }
            }
        };

        return JsonSerializer.Serialize(request);
    }


    // 추가적인 요청 유형을 여기에 정의할 수 있습니다.
} 