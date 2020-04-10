var leftHtml = '<div class="dialogBox leftDialog "><div class="imgBox $clientHead$"></div><div class="msgBox"><div class="timeRow">IP=$clientName$</div><div class="msgRow">$chatContent$</div></div><div class="clear"></div></div>';
var rightHtml = '<div class="dialogBox rightDialog"><div class="imgBox $clientHead$"></div><div class="msgBox"><div class="timeRow">IP=$clientName$</div><div class="msgRow">$chatContent$</div></div><div class="clear"></div></div>';
var centerHtml = '<div class="dialogBox centerDialog"><div class="timeRow">$infoTime$</div><div class="alertInfo">-- $chatContent$ --</div><div class="clear"></div></div>';
var pageIndex = 0;
var pageSize = 10; 
var totalPages = 0;
var totalCount = 0;
var firstLoad = 0;

$(function () {

    txtGpu = getGlRenderer();
    txtResolution = getResolution();
    txtmodels = getModels();
    txtUserAgent = GetUserAgent();
    imgIndex = txtResolution.substr(txtResolution.length - 1, 1);   
    identityMd5 = hex_md5(txtGpu + txtResolution + txtmodels + txtUserAgent);//用手机自身的信息来产生唯一识别码，但是这个方法存在重复的可能，目前将就使用。    
    websocketInit("ws://你.自.己.的.IP:8111");    
    getDataList();  
    $("#wsContentDiv").scroll(function () {
        WinScrollEvent();
    });
});

//滚动条事件
var WinScrollEvent = function () {
    if (firstLoad == 0) return true;
    // 窗口可视高度
    var winHeight = document.documentElement.clientHeight;
    // 文档撑开高度
    var documentHeight = document.getElementById("wsContentDiv").scrollHeight;
    // 滚动条距离底部的高度
    var scrollTop = document.getElementById("wsContentDiv").scrollTop;

    if (documentHeight <= winHeight) return true;
    if (totalPages <= 1 || totalPages <= pageIndex + 1) return true;

    if (scrollTop == 0) {
        //滚动条到达顶部

        ++pageIndex;
        getDataList();
        console.log("触发分页：pageIndex(" + pageIndex + ")");
    }
}

//点赞
var PraiseEvent = function () {
    $(".praiseDiv").show();
    $(".contentDiv").addClass("blur");
    $(".keyboardDiv").addClass("blur");
}
//关闭点赞
var CloseEvent = function () {

    $(".praiseDiv").hide();
    $(".contentDiv").removeClass("blur");
    $(".keyboardDiv").removeClass("blur");
}
//触发发送按钮
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


//监听到websocket消息后响应事件
var wsOnMessage = function (msg) {
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
    TurnToBottom();     
}


//加载聊天历史数据
var getDataList = function () {
    $.ajax({
        type: "get",
        url: "/Chat/GetDataList?pageIndex=" + pageIndex + "&pageSize=" + pageSize,
        dataType: "json",
        data: {},
        contentType: 'application/json;charset=utf-8',//向后台传送格式
        success: function (sourceData) {
            //console.log("返回数据" + JSON.stringify( data));
            //"PageIndex":0,"PageSize":10,"TotalCount":18,"TotalPages":2,"ReponseObj"
            if (sourceData.Code <= 0) {
                console.log(sourceData.Msg);
                return false;
            }
            var data = sourceData.Data;

            pageIndex = data.PageIndex 
            totalPages = data.TotalPages
            totalCount = data.TotalCount
            var reponseObj = data.ReponseObj


            var printHtml = '';
            for (var i = 0; i < data.ReponseObj.length; i++) {
                printHtml += printSubDataHtml(data.ReponseObj[i]);
            }
            $(".contentDiv").prepend(printHtml);
            if (pageIndex == 0) {
                TurnToBottom();
                firstLoad = 1;
            }
        },
        error: function (e) {
            var res = (e.responseText);
            console.log("发生错误：" + res);
        }
    });
}
 

//合成消息html
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

//调整窗口的显示位置
var TurnToBottom = function () {
    // 窗口可视高度
    var winHeight = document.documentElement.clientHeight;
    // 文档撑开高度
    var documentHeight = document.getElementById("wsContentDiv").scrollHeight;
    // 滚动条距离底部的高度
    var scrollTop = document.getElementById("wsContentDiv").scrollTop;

    //判定元素是否滚动到底:element.scrollHeight - element.scrollTop === element.clientHeight
    //console.log('winHeight：' + winHeight + '，documentHeight：' + documentHeight + '，scrollTop：' + scrollTop);

    if (documentHeight > winHeight) {
        document.getElementById("wsContentDiv").scrollTop = documentHeight - winHeight;
    }
}

//调整浏览器回车事件
$(document).keyup(function (event) {
    if (event.keyCode == 13) {
        SendMsgEvent();
    }
});




 