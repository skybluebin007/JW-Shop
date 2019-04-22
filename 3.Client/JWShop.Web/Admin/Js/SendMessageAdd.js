var tempPage=1;
var SendMessageAdd={  
    //搜索用户
    searchUser:function(){
        var userName=o("UserName").value;
        var url = "UserAjax.aspx?Action=SearchUser&UserName=" + userName+"&Page="+tempPage;
        Ajax.requestURL(url, this.dealSearchUser);
        alertMessage("正在搜索...", 1); 
    },
    dealSearchUser:function(data){
        closeAlertDiv();
        o("SearchUser").innerHTML=data;
    },
    //分页
    goPage:function(page){
        tempPage=page;
        SendMessageAdd.searchUser();
    },
    selectUser:function(obj){
        var userArray=obj.value.split("|");
        if(obj.checked){
            if(o("User"+userArray[0])==null){
                var content="<div id=\"User"+userArray[0]+"\" style=\"float:left;line-height:20px;\">"+userArray[1]+"<span onclick=\"SendMessageAdd.deleteUser("+userArray[0]+")\" style=\"cursor:pointer\"><img src=\"/Admin/style/images/delete.gif\" /></span><input name=\"UserIDList\" value=\""+userArray[0]+"\" type=\"hidden\"><input name=\"UserNameList\" value=\""+userArray[1]+"\" type=\"hidden\"></div>";
                o("SendMessageUser").innerHTML=o("SendMessageUser").innerHTML+content;
            }
           else{
                alertMessage("已存在该用户"); 
           }
       }
       else{
            try{
                this.deleteUser(userArray[0]);
            }catch(e){}
       }
    },
    deleteUser:function(id){
        var obj = o("User" + id);
        obj.parentNode.removeChild(obj); 
    }
}