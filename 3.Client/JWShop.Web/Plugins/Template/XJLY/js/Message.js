var objID = "ReceiveMessage";
var page = 1;
var objs=os("name","SelectID");
readMessage(objID, page);
function readMessage(id, page) {
    loading("MessageAjax", "短信息");
    o(objID).className = "item";
    objID = id;
    var obj = o(objID);
    obj.className = "item cur";
    var url = "MessageAjax.html?Action=" + id + "&Page=" + page;
    Ajax.requestURL(url, dealReadMessage);
}
function dealReadMessage(content) {
    o("MessageAjax").innerHTML = content;
}
function goPage(page) {
    readMessage(objID, page);
}
function selectMessage(action) {
    var isSelct = "";
    var isRead = "";
    switch (action) {
        case "All":
            isSelct = true;
            break;
        case "None":
            isSelct = false;
            break;
        case "HasRead":
            isRead = true;
            break;
        case "NoRead":
            isRead = false;
            break;
        default:
            break;
    }
    if (objs != null) {
        if (isSelct.toString() != "") {
            if (objs.length == null) {
                objs.checked = isSelct;
            }
            else {
                for (i = 0; i < objs.length; i++) {
                    if (objs[i].checked != isSelct) {
                        objs[i].checked = isSelct;
                    }
                }
            }
        }
        else {
            selectReadMessage(isRead);
        }
    }
}
function selectReadMessage(isRead) {    
    if (isRead) {
        if (objs.length == null) {
            if (objs.value.split("|")[1] == "1") {
                objs.checked = true;
            }
            else {
                objs.checked = false;
            }
        }
        else {
            for (i = 0; i < objs.length; i++) {
                if (objs[i].value.split("|")[1] == "1") {
                    objs[i].checked = true;
                }
                else {
                    objs[i].checked = false;
                }
            }
        }
    }
    else {
        if(objs.length == null) {
            if (objs.value.split("|")[1] == "0") {
                objs.checked = true;
            }
            else {
                objs.checked = false;
            }
        }
        else {
            for (i = 0; i <objs.length; i++) {
                if (objs[i].value.split("|")[1] == "0") {
                    objs[i].checked = true;
                }
                else {
                    objs[i].checked = false;
                }
            }
        }
    }
}
function deleteMessage(action) {
    if (checkSelect()) {
        var selectID = "";
        if (objs.length == null) {
            if (objs.checked) {
                selectID += objs.value.split("|")[0];
            }
        }
        else {
            for (i = 0; i < objs.length; i++) {
                if (objs[i].checked) {
                    selectID += objs[i].value.split("|")[0] + ",";
                }
            }
        }
        if (selectID.indexOf(",") > -1) {
            selectID = selectID.substring(0, selectID.length - 1);
        }
        var url = "MessageAjax.html?Action=DeleteReceiveMessage&SelectID=" + selectID;
        if (action == "Send") {
            url = "MessageAjax.html?Action=DeleteSendMessage&SelectID=" + selectID;
        }
        Ajax.requestURL(url, goPage);
    }
}
//查找联系人
function searchFriends() {
    var url = "MessageAjax.html?Action=SearchFriend&FriendName=" + o("FriendName").value;
    Ajax.requestURL(url, dealSearchFriends);
}
function dealSearchFriends(content) {
    var userList="";
    if (content != "") {
        var userID = o("UserIDList").value;
        var userName=o("UserNameList").value;
        var userArray = content.split("||");
        for (var i = 0; i < userArray.length; i++) {
            if ((","+userID+",").indexOf("," + userArray[i].split('|')[0] + ",") > -1) {
                continue;
            }
            userList += "<li><a href=\"javascript:addUser(" + userArray[i].split('|')[0] + ",'" + userArray[i].split('|')[1] + "')\" id=\"User" + userArray[i].split('|')[0] + "\">" + userArray[i].split('|')[1] + "</a></li>";
        }
        var userIDArray = userID.split(",");
        var userNameArray = userName.split(",");
        for (i = 0; i < userIDArray.length; i++) {
            userList += "<li><a href=\"javascript:addUser(" + userIDArray[i] + ",'" + userNameArray[i] + "')\" id=\"User" + userIDArray[i] + "\" class=\"on\">" + userNameArray[i] + "</a></li>";
        }
       o("UserList").innerHTML = userList;
    }
    else {
        alertMessage("没有您搜索的数据");
    }
}
function addUser(userID,userName) {
    var userIDObj =o("UserIDList");
    var userNameObj = o("UserNameList");
    var userObj = o("User" + userID);
    if (userObj.className == "item cur") {
        userObj.className = "item";
        if (userNameObj.value.indexOf("," + userName) > -1) {
            userNameObj.value = userNameObj.value.replace("," + userName, "");
            userIDObj.value = userIDObj.value.replace("," + userID, "");
        }
        else if (userNameObj.value.indexOf(userName + ",") > -1) {
            userNameObj.value = userNameObj.value.replace(userName + ",", "");
            userIDObj.value = userIDObj.value.replace(userID+",", "");
        }
        else {
            userNameObj.value = "";
            userIDObj.value = "";
        }
    }
    else {
        userObj.className = "item cur";
        if (userNameObj.value == "") {
            userNameObj.value = userName;
            userIDObj.value = userID;
        }
        else {
            userNameObj.value += "," + userName;
            userIDObj.value +=","+ userID;
        }
    }
    o("UserNameShow").value= userNameObj.value;
}
//发送信息
function sendMessage() {
    var userIDList = o("UserIDList").value;
    var userNameList = o("UserNameList").value;
    var title = o("Title").value;
    var content = o("Content").value;
    if (userNameList != "" && title!= "" && content != "") {
        var url = "MessageAjax.html?Action=SendUserMessage&UserIDList=" + userIDList + "&UserNameList=" + encodeURI(userNameList) + "&Title=" + encodeURI(title) + "&Content=" + encodeURI(content);
        Ajax.requestURL(url, dealSendMessage);
    }
    else {
        alertMessage("请填写完整的信息");
    }
}
function dealSendMessage(content){
    if (content != "") {
        alertMessage(content);
    }
    else {
        alertMessage("发送成功");
        o("UserNameList").value = "";
        o("UserIDList").value = "";
        o("UserNameShow").value = "";
    }
}