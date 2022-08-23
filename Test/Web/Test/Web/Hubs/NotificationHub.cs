using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Test.Modal.Entities;

namespace Web.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        public override Task OnConnectedAsync()
        {
            try
            {

                string reciver = (string)Context.GetHttpContext().Items["User"];

                var connectionId = Context.ConnectionId;

                if (!_ConnectionsMap.ContainsKey(reciver) && reciver != null)
                {
                    _ConnectionsMap.Add(reciver, connectionId);
                    Groups.AddToGroupAsync(connectionId, reciver);
                }
                else
                {
                    Groups.AddToGroupAsync(connectionId, reciver);
                }

                if (reciver == null)
                {
                    reciver = "all";
                }

                var listNoti = GetNotifications(reciver).Result;
                if(listNoti != null)
                {
                    List<Notification> list = new List<Notification>();
                    foreach(var i in listNoti)
                    {
                        list.Add(new Notification
                        {
                            Id = i.Id,
                            Title = i.Title,
                            Content = i .Content,
                            Sender = i.Sender,
                            Reciver = i.Reciver,
                            DateCreate = i.DateCreate
                        });
                    }
                    string message = JsonConvert.SerializeObject(list);
                    Clients.Caller.SendAsync("ReceiveMessage", message);
                }

                return base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private async Task<List<Notification>> GetNotifications(string reciver)
        {
            using var client = new HttpClient();
            string url = $"https://localhost:44373/api/Notification/getNotificationByReciver/{reciver}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                List<Notification> list = JsonConvert.DeserializeObject<List<Notification>>(res);
                return list;
            }
            return null;
        }


    }
}
