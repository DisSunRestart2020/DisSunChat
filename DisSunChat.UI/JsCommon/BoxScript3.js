
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

var leftHtml = '<div class="dialogBox leftDialog"><div class="imgBox"></div><div class="msgBox"><div class="timeRow">IP=$clientName$</div><div class="msgRow">$chatContent$</div></div><div class="clear"></div></div>';
var rightHtml = '<div class="dialogBox rightDialog"><div class="imgBox"></div><div class="msgBox"><div class="timeRow">IP=$clientName$</div><div class="msgRow">$chatContent$</div></div><div class="clear"></div></div>';


var wsMessage = function (msg) {
    var responseStr = msg.data;
    console.log("接收到消息:" + responseStr);

    var responseJson = $.parseJSON(responseStr); 
    if (responseJson == null) return false;

    var subHtml = leftHtml.replace("$clientName$", responseJson.ClientName + "(" + responseJson.ChatTime + ")").replace("$chatContent$", responseJson.ChatMsg);

    $(".contentDiv .dialogGap").before(subHtml);
    
}
