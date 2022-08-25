"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

//function notification(Id, Title, Content, Sender, Reciver, DateCreate) {
//    this.Id = Id,
//    this.Title = Title,
//    this.Content = Content,
//    this.Sender = Sender,
//    this.Reciver = Reciver,
//    this.DateCreate = DateCreate
//}

connection.on("sendToUser", (notificationJson) => {
    var noti = JSON.parse(notificationJson)

    var strongtitle = document.createElement("strong");
    strongtitle.textContent = `${noti.Title}`;

    var smalldatecreate = document.createElement("small");
    smalldatecreate.textContent = `${noti.DateCreate}`;

    var divtitle = document.createElement("div");
    divtitle.appendChild(strongtitle);
    divtitle.appendChild(smalldatecreate);

    var divcontent = document.createElement("div");
    divcontent.textContent = `${noti.Content}`;

    var divnoti = document.createElement("div");
    divnoti.appendChild(divtitle);
    divnoti.appendChild(divcontent);

    document.getElementById("articleList").appendChild(divnoti);

    var badge = document.getElementById("badgecounter");
    var count = parseInt(badge.innerText);
    count++;
    if (count < 5) {
        badge.textContent = count.toString();
    } else if (count >= 5) {
        badge.textContent = "5+";
    }
    else {
        badge.textContent = "1";
    }
});

connection.on("ReceiveMessage", (message) => {
    let count = 0;
    var listNoti = JSON.parse(message);
    if (listNoti !== null) {
        for (let x of listNoti) {

            var strongtitle = document.createElement("strong");
            strongtitle.textContent = `${x.Title}`;

            var smalldatecreate = document.createElement("small");
            smalldatecreate.textContent = `${x.DateCreate}`;

            var divtitle = document.createElement("div");
            divtitle.appendChild(strongtitle);
            divtitle.appendChild(smalldatecreate);

            var divcontent = document.createElement("div");
            divcontent.textContent = `${x.Content}`;

            var divnoti = document.createElement("div");
            divnoti.appendChild(divtitle);
            divnoti.appendChild(divcontent);

            document.getElementById("articleList").appendChild(divnoti);

            var badge = document.getElementById("badgecounter");
            count++;
            if (count < 5) {
                badge.textContent = count.toString();
            } else if (count >= 5) {
                badge.textContent = "5+";
            }
            else {
                badge.textContent = "1";
            }
        }      
    }
    
});

connection.start().then(function () {
    console.log(connection.Id);
}).catch(function (err) {
    return console.error(err.toString());
});

connection.onreconnecting(error => {
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
});





