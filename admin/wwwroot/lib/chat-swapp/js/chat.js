"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/admin/message/chatter").build();

//Disable send button until connection is established
document.getElementById("btn-send-message").disabled = true;

connection.on("ReceiveMessage", function (message, roomId) {
    alert(roomId);
    $(".chat-body ul").append("<li><div class='chat-message d-flex'> <div class='left-side'> <img src='/admin/img/admin/omer-faruk-coskun.webp' class='message-avatar'> </div> <div class='message-content d-flex flex-column'> <div class='d-flex justify-content-between'> <span class='author'>Ömer Faruk Coşkun</span> <span class='timestamp'> <i class='far fa-clock'></i> <span>20.01.2021</span> </span> </div> <span class='content'> " + message + " </span> </div> </div> </li>");
});

//If connection is success, you can add some code at the bottom.
connection.start().then(function () {
    document.getElementById("btn-send-message").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("btn-send-message").addEventListener("click", function (event) {
    const message = document.getElementById('chat-message').value;
    const roomId = document.getElementById('hidden-room-id').value;
    connection.invoke("SendMessage", message, 1).catch(function (err) {
        return console.error(err.toString());
    });
    
    event.preventDefault();
});