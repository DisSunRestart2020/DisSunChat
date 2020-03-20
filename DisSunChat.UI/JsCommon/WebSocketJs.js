var socket;
var websocketInit = function () {
    if (typeof (WebSocket) === "undefined") {
        socket = null;
        alert("您的浏览器不支持本功能");
    } else {
        // 实例化socket
        socket = new WebSocket("ws://172.16.2.4:8181");
        // 监听socket连接
        socket.onopen = wsOpen;
        //监听socket关闭
        socket.onclose = wsClose;
        // 监听socket错误信息
        socket.onerror = wsError;
        // 监听socket消息
        socket.onmessage = wsMessage;
    }
}

var wsOpen = function () {
    console.log("已经成功连接");
}

var wsClose = function () {
    console.log("已经关闭连接");
}


var wsError = function (evt) {
    console.log("异常:" + evt);
}

var wsSend = function (sMsg) {     
    if (socket == null || sMsg == "") return false;
    console.log("消息成功发出");
    socket.send(sMsg);
}
 