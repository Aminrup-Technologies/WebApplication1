using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApplication1.bussiness.production
{
    public class NotificationHub : Hub
    {
        public void SendNotification(string message)
        {
            Clients.All.newNotification(message);
        }
    }
}