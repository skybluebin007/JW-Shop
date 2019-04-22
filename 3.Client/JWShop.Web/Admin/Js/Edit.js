function edit(obj, functionName, id) {
    var tag = obj.firstChild.tagName;
    
    if (typeof (tag) != "undefined" && tag.toLowerCase() == "input") {
        return;
    }

    /* 保存原始的内容 */
    var org = obj.innerHTML;
    var val = Browser.isIE ? obj.innerText : obj.textContent;

    /* 创建一个输入框 */
    var txt = document.createElement("input");
    txt.value = (val == 'N/A') ? '' : val;
    txt.style.width = (obj.offsetWidth + 12) + "px";
  
    /* 隐藏对象中的内容，并将输入框加入到对象中 */
    obj.innerHTML = "";
    
    obj.appendChild(txt);
    txt.focus();

    /* 编辑区输入事件处理函数 */
    txt.onkeypress = function(e) {
        var evt = Validate.fixEvent(e);
        var obj = Validate.srcElement(e);

        if (evt.keyCode == 13) {
            obj.blur();

            return false;
        }

        if (evt.keyCode == 27) {
            obj.parentNode.innerHTML = org;
        }
    }

    /* 编辑区失去焦点的处理函数 */
    txt.onblur = function() {
        functionName(obj, txt.value, org, id);
    }
}

document.onmousemove = function(e) {
    var obj = Validate.srcElement(e);
    if (typeof (obj.onclick) == 'function' && obj.onclick.toString().indexOf('edit') != -1) {
        obj.title = '点击修改内容';
        obj.style.cssText = 'background: #B6CEE6;';
        obj.onmouseout = function(e) {
            this.style.cssText = '';
        }
    }
}