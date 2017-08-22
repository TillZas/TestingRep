$(document).ready(function () {
    $("#btnLogin").click(function () {
        var nickName = $("#userNameDropList").val();
        if (nickName) {
            var href = "/Chat?user=" + encodeURIComponent(nickName);
            href = href + "&logOn=true";
            $("#LoginButton").attr("href", href);
            $("#LoginButton").click();
 
            $("#Username").text(nickName);
        }
    });
});
 
function LoginOnSuccess(result) {
 
    //Scroll();
    //ShowLastRefresh();
 
    //setTimeout("Refresh();", 5000);

    $("#updtMessage").click(function () {
        $("#messages").empty();
        $.getJSON('/Chat/Messages', function (data) {
            for (var i = 0; i < data.length; i++) {
                var nickname = "server";
                if (data[i].User != null) { 
                    nickname = data[i].User.Name;
                }
                $("#messages").append('<li>' + nickname + ' : '
                    + data[i].Text + '</li>');
            }
        });
    });

    $('#txtMessage').keydown(function (e) {
        /*if (e.keyCode == 13) {
            e.preventDefault();
            $("#btnMessage").click();
        }*/
    });
 
    $("#btnMessage").click(function () {
        var text = $("#txtMessage").val();
        if (text) {
 
            //var href = "/Chat?user=" + encodeURIComponent($("#Username").text());
            //href = href + "&chatMessage=" + encodeURIComponent(text);
            $.get('/Chat', { user: $("#Username").text(), chatMessage: text });
            //$("#ActionLink").attr("href", href).click();
        }
    });
 
    $("#btnLogOff").click(function () {
 
        var href = "/Chat?user=" + encodeURIComponent($("#Username").text());
        href = href + "&logOff=true";
        $("#ActionLink").attr("href", href).click();
 
        document.location.href = "Home";
    });
}
 
function LoginOnFailure(result) {
    $("#Username").val("");
    $("#Error").text(result.responseText);
    setTimeout("$('#Error').empty();", 2000);
}

/*
function Refresh() {
    var href = "/Chat?user=" + encodeURIComponent($("#Username").text());
 
    $("#ActionLink").attr("href", href).click();
    //setTimeout("Refresh();", 5000);
}
 */

function ChatOnFailure(result) {
    $("#Error").text(result.responseText);
    setTimeout("$('#Error').empty();", 2000);
}
 
function ChatOnSuccess(result) {
   // Scroll();
   // ShowLastRefresh();
}
 
function Scroll() {
    var win = $('#Messages');
    var height = win[0].scrollHeight;
    win.scrollTop(height);
}
 
function ShowLastRefresh() {
    var dt = new Date();
    var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
    $("#LastRefresh").text("Последнее обновление было в " + time);
}