//SIGNALR NOTIFICATIONS

$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/admin/notification/hub").build();

    connection.start().then(function () {
        console.log('SignalR Notification Started...')
        //viewModel.notificationList();
    }).catch(function (err) {
        return console.error(err);
    });

    //function AppViewModel() {
    //    var self = this;

    //    self.notificationList = function () {
    //        connection.invoke("GetNotifications").then(function (result) {

    //        });
    //    }

    //}

    //var viewModel = new AppViewModel();
    //ko.applyBindings(viewModel);
});
