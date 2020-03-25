
$(function () {

    txtGpu = getGlRenderer();
    txtResolution = getResolution();
    txtmodels = getModels();
    txtUserAgent = GetUserAgent();
    imgIndex = txtResolution.substr(txtResolution.length - 1, 1);
    identityMd5 = hex_md5(txtGpu + txtResolution + txtmodels + txtUserAgent);//用手机自身的信息来产生唯一识别码，但是这个方法存在重复的可能，目前将就使用。

    websocketInit("ws://172.16.2.4:8181");    
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
    var sMsg = $("#contentTxt").val().trim();

    if (sMsg.length >= 80) {
        alert("有话慢慢说，不要一次性说一篇文章！");
        return true;
    }

    if (sMsg != null && sMsg != "") {
        var sendMsg = "{\"identityMd5\":\"" + identityMd5 + "\",\"sMsg\":\"" + sMsg + "\",\"imgIndex\":\"" + imgIndex + "\",\"isOpenLink\":\"false\"}";
        wsSend(sendMsg);
    }
    $("#contentTxt").val("");   
}

var leftHtml = '<div class="dialogBox leftDialog "><div class="imgBox $clientHead$"></div><div class="msgBox"><div class="timeRow">IP=$clientName$</div><div class="msgRow">$chatContent$</div></div><div class="clear"></div></div>';
var rightHtml = '<div class="dialogBox rightDialog"><div class="imgBox $clientHead$"></div><div class="msgBox"><div class="timeRow">IP=$clientName$</div><div class="msgRow">$chatContent$</div></div><div class="clear"></div></div>';
var centerHtml = '<div class="dialogBox centerDialog"><div class="timeRow">$infoTime$</div><div class="alertInfo">-- $chatContent$ --</div><div class="clear"></div></div>';

var wsMessage = function (msg) {
    var responseStr = msg.data;
    //console.log("接收到消息:" + responseStr);

    var responseJson = $.parseJSON(responseStr); 
    if (responseJson == null) return false;
    
    var cIp = responseJson.cIp;
    var cPort = responseJson.cPort;
    var cGuid = responseJson.cGuid;    
    var chatTime = responseJson.ChatTime;  
    var clientName = cIp + ":" + cPort;     

    var chatMsgJson = responseJson.ChatMsgJson;

    var responseIdentity = chatMsgJson.identityMd5;
    var responseMsg = chatMsgJson.sMsg;
    var responseImgIndex = chatMsgJson.imgIndex;
    var responseIsOpenLink = chatMsgJson.isOpenLink;


    if (responseIsOpenLink === "true") {
        //登录信息
        var contentHtml = centerHtml;
        var subHtml = contentHtml.replace("$infoTime$", chatTime).replace("$chatContent$", responseMsg);
        $(".contentDiv .dialogGap").before(subHtml);
    }
    else {
        var imgHeadStr = "imgHead0" + responseImgIndex;
        var contentHtml;
        if (responseIdentity === identityMd5) {
            contentHtml = rightHtml;
            imgHeadStr = "imgHead10";
        }
        else {
            contentHtml = leftHtml;
        }
        var subHtml = contentHtml.replace("$clientName$", clientName + "(" + chatTime + ")").replace("$chatContent$", responseMsg).replace("$clientHead$", imgHeadStr);
        $(".contentDiv .dialogGap").before(subHtml);
    }
}
