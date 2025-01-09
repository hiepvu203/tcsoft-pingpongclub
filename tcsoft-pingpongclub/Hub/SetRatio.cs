
using Microsoft.AspNetCore.SignalR;

namespace tcsoft_pingpongclub.Hubs
{
    public class SetRatio : Hub
    {
        public async Task SendRatio(string idMatch, string ratio1, string ratio2, string ratio3)
        {
            await Clients.All.SendAsync("GetRatio",idMatch ,ratio1, ratio2, ratio3);
        }
    }
}

