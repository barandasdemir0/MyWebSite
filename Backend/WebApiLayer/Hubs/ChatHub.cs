using BusinessLayer.Abstract;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiLayer.Hubs;

public class ChatHub:Hub
{
    private readonly IChatbotManagerService _chatbotManagerService;
    //private readonly IMemoryCache _memoryCache;

    public ChatHub(IChatbotManagerService chatbotManagerService/*, IMemoryCache memoryCache*/)
    {
        _chatbotManagerService = chatbotManagerService;
        //_memoryCache = memoryCache;
    }

    public async Task SendMessage(string message,string currentUrl)
    {
        try
        {
            string cacheKey = $"chat_limit_{Context.ConnectionId}";
            //if (_memoryCache.TryGetValue(cacheKey,out _))
            //{
                //await Clients.Caller.SendAsync("ReceiveError", "Çok hızlı mesaj gönderiyorsunuz. Lütfen biraz bekleyin.");
                //return;
            //}

            //_memoryCache.Set(cacheKey, true, TimeSpan.FromSeconds(5));
            string response = await _chatbotManagerService.ProcessUserMessageAsync(message,currentUrl);
            await Clients.Caller.SendAsync("ReceiveBotMessage", response);
        }
        catch (Exception)
        {
            await Clients.Caller.SendAsync("ReceiveError", "Asistan şu an hizmet veremiyor.");
        }
    }
}
