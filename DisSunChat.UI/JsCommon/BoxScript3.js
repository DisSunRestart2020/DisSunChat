
$(function () {
    websocketInit();
});


var PraiseEvent = function () {
    $(".praiseDiv").show();
    $(".contentDiv").addClass("blur");
    $(".keyboardDiv").addClass("blur");
}

var CloseEvent = function () {

    $(".praiseDiv").hide();
    $(".contentDiv").removeClass("blur");
    $(".keyboardDiv").removeClass("blur");
}

var SendMsgEvent = function () {
    //alert("别点了，还没做好呢！");

    var sMsg = $("#contentTxt").val().trim();
    if (sMsg != null && sMsg != "") {
        wsSend(sMsg);
    }
    $("#contentTxt").val("");

}

var wsMessage = function (msg) {
    var responseStr = msg.data;
    console.log("接收到消息:" + responseStr);

    var responseJson = $.parseJSON(responseStr); 
    if (responseJson == null) return false;

    
}
