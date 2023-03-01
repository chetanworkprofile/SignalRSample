using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
    public class UserHub : Hub
    {
        public static int TotalViews { get; set; } = 0;
        public static int TotalUsers { get; set; } = 0;

        public override Task OnConnectedAsync()
        {
            TotalUsers++;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            Clients.All.SendAsync("updateTotalViews", TotalViews).GetAwaiter().GetResult();
            // (all) can be replaced with (caller) to send only to requeter or (other) to send to all except caller
            //and we can use  await Clients.Clients("Connection Id - A","Connection Id - B").SendAsync("updateTotalViews", TotalViews).GetAwaiter().GetResult();
            //and we can use  await Clients.AllExcept("Connection Id - A", "Connection Id - B").SendAsync("updateTotalViews", TotalViews).GetAwaiter().GetResult();
            //can uniquely identify with user id
            //await Clients.User("ben@gmail.com").SendAsync("updateTotalViews", TotalViews).GetAwaiter().GetResult();0
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            TotalUsers--;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            Clients.All.SendAsync("updateTotalViews", TotalViews).GetAwaiter().GetResult();
            return base.OnDisconnectedAsync(exception);
        }
        public async Task NewWindowLoaded()         //NewWindowLoaded(string param)  for invoke method with paramateres
        {
            TotalViews++;
            //send update to all clients that total views have been updated
            await Clients.All.SendAsync("updateTotalViews", TotalViews);
            Console.WriteLine(TotalViews.ToString());
            //return $"totalviews from {parameter} - {TotalViews}";
        }
    }
}
