
$(function () {

    txtGpu = getGlRenderer();
    txtResolution = getResolution();
    txtmodels = getModels();
    txtUserAgent = GetUserAgent();
    imgIndex = txtResolution.substr(txtResolution.length - 1, 1);
    identityMd5 = hex_md5(txtGpu + txtResolution + txtmodels + txtUserAgent);//用手机自身的信息来产生唯一识别码，但是这个方法存在重复的可能，目前将就使用。
    
    websocketInit("ws://172.16.2.4:8181");   
    getDataList();
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
        var sendMsg = "{\"IdentityMd5\":\"" + identityMd5 + "\",\"SMsg\":\"" + sMsg + "\",\"ImgIndex\":\"" + imgIndex + "\",\"IsConnSign\":\"false\"}";
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
    
    var cIp = responseJson.CIp;
    var cPort = responseJson.CPort;
    var cGuid = responseJson.CGuidID;    
    var chatTime = responseJson.ChatTime;  
    var clientName = cIp + ":" + cPort;     

    var clientDataJson = responseJson.ClientData;

    var responseIdentity = clientDataJson.IdentityMd5;
    var responseMsg = clientDataJson.SMsg;
    var responseImgIndex = clientDataJson.ImgIndex;
    var responseIsConnSign = clientDataJson.IsConnSign;


    if (responseIsConnSign === "true") {
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


var pageIndex = 0;
var pageSize =10;
var getDataList = function () {
    //GetDataList

    $.ajax({
        type: "get",
        url: "/Chat/GetDataList?pageIndex=" + pageIndex + "&pageSize=" + pageSize,
        dataType: "json",
        data: {},
        contentType: 'application/json;charset=utf-8',//向后台传送格式
        success: function (data) {
           // console.log("返回数据" + JSON.stringify( data));
            var printHtml = '';
            for (var i = 0; i < data.length; i++) {
                printHtml += printSubDataHtml(data[i]);
            }
            $(".contentDiv .dialogGap").before(printHtml);
        },
        error: function (e) {
            var res = (e.responseText);
            console.log("发生错误：" + res);
        }
    });
}

var printSubDataHtml = function (item) {
    var imgHeadStr = "imgHead0" + item.ImgIndex;
    var contentHtml;
    if (item.IdentityMd5 === identityMd5) {
        contentHtml = rightHtml;
        imgHeadStr = "imgHead10";
    }
    else {
        contentHtml = leftHtml;
    }
    var subHtml = contentHtml.replace("$clientName$", item.ClientName + "(" + item.CreateTime + ")").replace("$chatContent$", item.ChatContent).replace("$clientHead$", imgHeadStr);
    return subHtml;
}
