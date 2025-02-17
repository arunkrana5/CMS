

//$(document).ready(function () {
//    setInterval(function () {
//        GetNotificationCount();
//    }, 30000);
//});  


function GetNotificationCount() {
   
    var Category = "";
        $.ajax({
            url: "/CommonAjax/GetPushNotificationListJSON",
            type: "Get",
            async: true,
            data: { Category: "", IsRecent:1},
            success: function (data) {
                GetRecentPushNotificationCallBack(data)
            },
            error: function (er) {
                console.log(er)
            }
        });
}

function GetRecentPushNotificationCallBack(args)
{
    var InfotabHTML = "";
    var count = 0;
    var TopMessage = "";
    $.each(args, function (k, v) {
        count++;
        $.notify(this.Subject, "error");
        InfotabHTML += "<li> <a><span><span>" + this.Subject+"</span><span class='time'>" + this.CreatedDate+"</span></span >";
        InfotabHTML += " <span class='message'><strong> " + this.Status + "</strong><span>" + this.MessageContent+"</span></span></a></li>";
    });
   
    if (InfotabHTML != "")
    {
        $("#ULNotification").prepend(InfotabHTML);
        $("#spnNotiCount").html(parseInt($("#spnNotiCount").html()) + count);
    }
}

//function ClearRecentNotification(ID) {
//    $.ajax({
//        url: "/CommonAjax/ClearRecentNotificationJSON",
//        type: "Get",
//        async: true,
//        data: { ID: ID },
//        success: function (data) {
       
//        },
//        error: function (er) {
         
//        }
//    });
//}