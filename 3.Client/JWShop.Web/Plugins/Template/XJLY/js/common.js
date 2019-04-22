//取得对象
function o(id) {
    return document.getElementById(id);
}
//加载数据
function loading(id) {
    try{
        document.getElementById(id).innerHTML = "<div style='margin:10px auto;text-align:center;'><div style='border:2px solid #ff3c3c; padding:8px; width:260px;display:inline-block; background:#fff9f1;color:#ff3c3c;font-weight:bold'><img src='/admin/style/images/loading.gif' align='absmiddle'/> 加载中, 请稍候……</div></div>";
    }
    catch (e) { }
}

//提示信息
function alertMessage(message, checked) {
    var alertDiv = document.createElement("div");
    var divWidth = 200;
    var divHeight = 50;
    var time = 2000;
    var screenWidth = (window.innerWidth || document.documentElement && document.documentElement.clientWidth || document.body.clientWidth);
    var screenHeight = (window.innerHeight || document.documentElement && document.documentElement.clientHeight || document.body.clientHeight);
    var scrollLeft = (document.documentElement.scrollLeft || document.body.scrollLeft);
    var scrollTop = (document.documentElement.scrollTop || document.body.scrollTop);
    var top = (screenHeight - divHeight) / 2 + scrollTop;
    var left = (screenWidth - divWidth) / 2 + scrollLeft;
    with (alertDiv) {
        style.border = "2px solid #ff3c3c";
        style.background = "#fff9f1";
        style.position = "absolute";
        style.padding = "12px";
        style.left = left + "px";
        style.top = top + "px";
        style.width = divWidth + "px";
        style.textAlign = "center";
        style.color = "#ff5c4d";
        style.fontWeight = "bold";
        id = "AlertMessage";
        innerHTML = message;
    }
    document.body.appendChild(alertDiv);
    if (checked != "1") {
        setTimeout("closeAlertDiv()", time);
    }
}
function closeAlertDiv() {
    var alerObj = document.getElementById("AlertMessage");
    if (alerObj != null) {
        document.body.removeChild(alerObj);
    }
}
//验证类
var Validate = {
    isEmpty: function (val) {
        switch (typeof (val)) {
            case 'string':
                return Utils.trim(val).length == 0 ? true : false;
                break;
            case 'number':
                return val == 0;
                break;
            case 'object':
                return val == null;
                break;
            case 'array':
                return val.length == 0;
                break;
            default:
                return true;
        }
    },
    isNumber: function (val) {
        var reg = /^[|+\-]*[\d|\.|,]+$/;
        return reg.test(val);
    },

    isInt: function (val) {
        if (val == "") {
            return false;
        }
        var reg = /\D+/;
        return !reg.test(val);
    },

    isEmail: function (email) {
        var reg1 = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/;
        return reg1.test(email);
    },

    isTel: function (tel) {
        var reg = /^[\d|\-|\s|\_]+$/; //只允许使用数字-空格等
        return reg.test(tel);
    },
    isTime: function (val) {
        var reg = /^\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}$/;
        return reg.test(val);
    },
    isDate: function (val) {
        var reg = /^\d{4}-\d{1,2}-\d{1,2}$/;
        return reg.test(val);
    },
    reg: function (v, r) {
        var re = r;
        if (!re.test(v)) {
            return false;
        }
        return true;
    },
    fixEvent: function (e) {
        var evt = (typeof e == "undefined") ? window.event : e;
        return evt;
    },
    srcElement: function (e) {
        if (typeof e == "undefined") e = window.event;
        var src = document.all ? e.srcElement : e.target;

        return src;
    }
}

function getQueryString(url, parmName) {
    var result = '';
    if (url != "undefined") {
        if (url.substr(0, 1) == "?") {
            url = url.substr(1);
        }
        var arrParam = url.split("&");
        for (var i = 0; i < arrParam.length; i++) {
            if (arrParam[i].split("=")[0].toLowerCase() == parmName.toLowerCase()) {
                result = arrParam[i].replace(parmName + "=", "");
                break;
            }
        }
    }
    return result;
}

//添加cookie
function addCookie(objName, objValue, objMinutes) {
    var str = objName + "=" + escape(objValue);
    if (objMinutes > 0) {   //为0时不设定过期时间，浏览器关闭时cookie自动消失
        var date = new Date();
        var ms = objMinutes * 60 * 1000;
        date.setTime(date.getTime() + ms);
        str += "; expires=" + date.toGMTString();
    }
    str += ";path=/";
    document.cookie = str;
}
//获取指定名称的cookie的值 
function getCookie(objName) {
    var arrStr = document.cookie.split("; ");
    for (var i = 0; i < arrStr.length; i++) {
        var temp = arrStr[i].split("=");
        if (temp[0] == objName) return unescape(temp[1]);
    }
    return "";
}
//删除指定名称的cookie
function delCookie(name) {
    var date = new Date();
    date.setTime(date.getTime() - 10000);
    document.cookie = name + "=a; expires=" + date.toGMTString();
}
//添加事件
function addEvent(obj, evt, fn) {
    if (typeof obj.attachEvent != 'undefined') {
        obj.attachEvent("on" + evt, fn);
    }
    else if (typeof obj.addEventListener != 'undefined') {
        obj.addEventListener(evt, fn, false);
    }
    else {
    }
}

Math.Round = function Round(d, x, z) {
    var _d = Math.round(d * Math.pow(10, x)) / Math.pow(10, x);
    if (z) { _d = ((Number)(_d)).padZero(2); }
    return _d;
};
// 小数数位补零
Number.prototype.padZero = function (oCount) {
    var strText = this.toString();
    if (strText.indexOf('.') < 0) {
        strText += ".";
    }
    while (strText.length - strText.indexOf('.') - 1 < oCount) {
        strText += '0';
    }
    return strText;
};

//删除自身节点
function removeSelf(obj) {
    obj.parentNode.removeChild(obj);
}