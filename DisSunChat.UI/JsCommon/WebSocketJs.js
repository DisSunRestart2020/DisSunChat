var socket;
var websocketInit = function (wsPath) {
    if (typeof (WebSocket) === "undefined") {
        socket = null;
        console.log("浏览器不支持websocket");
    } else {
        // 实例化socket
        socket = new WebSocket(wsPath);
        // 监听socket连接
        socket.onopen = wsOnOpen;
        //监听socket关闭
        socket.onclose = wsOnClose;
        // 监听socket错误信息
        socket.onerror = wsOnError;
        // 监听socket消息
        socket.onmessage = wsOnMessage;
    }
}


var wsOnOpen = function () {
    console.log("已经成功连接");
    var sMsg = "";

    var sendMsg = "{\"IdentityMd5\":\"" + identityMd5 + "\",\"SMsg\":\"\",\"ImgIndex\":\"" + imgIndex + "\",\"IsConnSign\":\"true\"}";
    socket.send(sendMsg);
}

var wsOnClose = function () {
    console.log("已经关闭连接");
}


var wsOnError = function (evt) {
    console.log("异常:" + evt);
}

var wsSend = function (sMsg) {     
    if (socket == null || sMsg == "") return false;
    console.log("消息成功发出");
    socket.send(sMsg);
}
 