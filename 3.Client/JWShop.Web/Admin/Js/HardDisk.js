//返回上级
function goBack() {
    var url = window.location.href;
    if (getQueryString("Path")) {
        url = url.substr(0, url.length - 1);
        url = url.substr(0, url.lastIndexOf("/") + 1);
        if (url.split("?")[1].split("/").length > 3) {
            window.location.href = url;
        }
        else {
            alertMessage("已经到顶层目录了");
        }
    }
    else {
        alertMessage("已经到顶层目录了");
    }        
}
//增加文件夹
function addDirectory() {
    pop("DirectoryAdd.aspx?Path="+path,380,120,"创建文件夹", "AddDirectory");
}
 //增加文件
 function addFile() {
    pop("FileAdd.aspx?Path="+path,380,120,  "上传文件", "AddFile");
 }
 //剪切
 function cut() {
     var objs=os("name","HardDisk");
     var valueList = getCheckboxValue(objs).join("|");
     if(valueList==""){
         alertMessage("请选择要剪切的对象", 500);
     }
     else{
         addCookie("FileList", valueList, 10);
         addCookie("Action", "Cut", 10);
         alertMessage("剪切成功");
     }      
 }
 //复制
 function copy() {
     var objs=os("name","HardDisk");
     var valueList = getCheckboxValue(objs).join("|");
     if (valueList == "") {
         alertMessage("请选择要复制的对象", 500);             
     }
     else {
         addCookie("FileList", valueList, 10);
         addCookie("Action", "Copy", 10);
         alertMessage("复制成功");
     }   
 }
 //粘贴
 function paste() {
     var fileList = getCookie("FileList");
     var sourceAction=getCookie("Action");
     if (fileList != "") {
         Ajax.requestURL("HardDisk.aspx?Action=Paste&Path="+path+"&FileList=" + fileList+"&SourceAction="+sourceAction, function(data) {
             if (data == "ok") {
                 alertMessage("粘贴成功");
                 reloadPage();
             }
             else{
                 alertMessage("粘贴失败");
             }
         });
     }
     else {
         alertMessage("请选择要粘贴的对象", 500);              
     }
 } 
  ///全选    
 function selectAll() {
    var objs=os("name","HardDisk");
    if (objs != null && objs.length > 0) {
        for (var i = 0; i < objs.length; i++) {            
            objs[i].checked=true;            
        }
    }
 }
 //不选
 function selectNo() {
    var objs=os("name","HardDisk");
    if (objs != null && objs.length > 0) {
        for (var i = 0; i < objs.length; i++) {            
            objs[i].checked=false;            
        }
    }
 }
 //文件夹重命名
 function changeDirectoryName(obj, newValue, oldValue, id) {
     if (newValue != "" && newValue != oldValue) {
          Ajax.requestURL("HardDisk.aspx?Action=ChangeDirectoryName&NewName=" + encodeURI(newValue) + "&OldName=" + encodeURI(oldValue) + "&Path="+path, function(data) {
             if (data == "ok") {
                 alertMessage("修改成功");
                 reloadPage();
             }
             else {
                 alertMessage("修改失败",500);
                 obj.innerHTML = oldValue;
             }
         });
     }
     else {
         obj.innerHTML = oldValue;
     }
 }
 ///文件重命名
 function changeFileName(obj, newValue, oldValue, id) {
     if (newValue != "" && newValue != oldValue && newValue.indexOf(".")>-1) {
         Ajax.requestURL("HardDisk.aspx?Action=ChangeFileName&NewName=" + encodeURI(newValue) + "&OldName=" + encodeURI(oldValue) + "&Path="+path, function(data) {
             if (data == "ok") {
                 alertMessage("修改成功");
                 reloadPage();
             }
             else {
                 alertMessage("修改失败",500);
                 obj.innerHTML = oldValue;
             }
         });
     }
     else {
         obj.innerHTML = oldValue; 
     }
 }     