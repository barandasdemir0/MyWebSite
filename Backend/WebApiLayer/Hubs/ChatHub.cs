using BusinessLayer.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace WebApiLayer.Hubs;

public class ChatHub:Hub
{
    private readonly IChatbotManagerService _chatbotManagerService;

    public ChatHub(IChatbotManagerService chatbotManagerService)
    {
        _chatbotManagerService = chatbotManagerService;
    }

    public async Task SendMessage(string message,string currentUrl)
    {
        try
        {
            string response = await _chatbotManagerService.ProcessUserMessageAsync(message,currentUrl);
            await Clients.Caller.SendAsync("ReceiveBotMessage", response);
        }
        catch (Exception ex)
        {

            await Clients.Caller.SendAsync("ReceiveError", "Asistan şu an hizmet veremiyor."+ex.Message);
        }
    }
}
