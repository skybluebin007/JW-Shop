//页面切换
var PreTitleBlock = "TitleDefault";
var PreContentBlock = "ContentDefault";
function switchBlock(Name){
    var TitleBlock = "Title" + Name;
    var ContentBlock = "Content" + Name;
    o(PreTitleBlock).className = "";
    o(PreContentBlock).style.display = "none";
    o(TitleBlock).className = "listOn";
    o(ContentBlock).style.display = "";
    PreTitleBlock=TitleBlock;
    PreContentBlock=ContentBlock;
}

//全选全不选
function selectAll(obj){
    var objs=os("name","SelectID");
    if (objs != null && objs.length > 0) {
        for (var i = 0; i < objs.length; i++) {            
            objs[i].checked=obj.checked;            
        }
    }
}
//检查是否选择
function checkSelect(){
    var objs=os("name","SelectID");
    var result=getCheckboxValue(objs);
    if (result.length==0){
        alert("请选择要操作的记录");
        return false;
    }
    else{
        if (confirm("注意：请问确定要操作吗")){
            return true;
        }
    }
    return false;
}
//确认检查
function check(){
	if (confirm("注意：请问确定要操作吗")){
        return true;
    }
	return false;
}
//删除记录
function deleteRecord(obj){
    if(confirm("确定要删除")){
        Ajax.requestURL(obj.href,function(data){if(data=="ok"){reloadPage();}else{alertMessage("删除失败");}});
    }
    return false;
}
//显示提示层
function showHintInfo(obj, objleftoffset,objtopoffset, title, info , objheight, showtype ,objtopfirefoxoffset){   
   var p = getPosition(obj);   
   if((showtype==null)||(showtype =="")) 
   {
       showtype =="up";
   }
   o('hintiframe'+showtype).style.height= objheight + "px";
   o('hintinfo'+showtype).innerHTML = info;
   o('hintdiv'+showtype).style.display='block';   
   if(objtopfirefoxoffset != null && objtopfirefoxoffset !=0 && !isie()){
        o('hintdiv'+showtype).style.top=p['y']+parseInt(objtopfirefoxoffset)+"px";
   }
   else{
        if(objtopoffset == 0){ 
			if(showtype=="up"){
				 o('hintdiv'+showtype).style.top=p['y']-o('hintinfo'+showtype).offsetHeight-40+"px";
			}
			else{
				 o('hintdiv'+showtype).style.top=p['y']+obj.offsetHeight+5+"px";
			}
        }
        else{
			o('hintdiv'+showtype).style.top=p['y']+objtopoffset+"px";
        }
   }
   o('hintdiv'+showtype).style.left=p['x']+objleftoffset+"px";
}
//隐藏提示层
function hideHintInfo(){
    o('hintdivup').style.display='none';
    o('hintdivdown').style.display='none';
}
//取得位置
function getPosition(obj){
	var r = new Array();
	r['x'] = obj.offsetLeft;
	r['y'] = obj.offsetTop;
	while(obj = obj.offsetParent)
	{
		r['x'] += obj.offsetLeft;
		r['y'] += obj.offsetTop;
	}
	return r;
}
// 返回某年某月的天数
function getDays() {
    var days = 30;
    var year = o("Year").value;
    var month = o("Month").value;
    var day = o("Day"); 
    if (month == "2")
    days = 28;  
    if ( (",1,3,5,7,8,10,12,").indexOf(","+month+",") > -1 )
    days = 31;  
    if (month == "2" && parseInt(year) % 4 ==0 && !(parseInt(year) % 100 == 0 && !(parseInt(year) % 400 == 0)))
    days = 29;  
    day.options.length=0;
    for(i=1;i<=days;i++){
      day.options.add(new Option(i,i));
    }
}
//改变背景颜色
function changeColor(e,color){
    e.style.backgroundColor=color;
}
//更改商品状态：新品、推荐等
function updateStatus(id, action) {
    var status = 0;
    var obj;
    try {
        obj = o(action + id);
    } catch (e) { alertMessage("系统忙，请稍后重试"); }
    if (obj.innerHTML != '是') {
        status = 1;
    } 
    url = "Ajax.aspx?Action=ChangeProductStatus&field=" + action + "&ID=" + id + "&Status=" + status;
    Ajax.requestURL(url,dealUpdateStatus);
}
function dealUpdateStatus(data) {  
    var list = data.split('|');  
    try {
        obj = o(list[0] + list[1]);      
        if (obj.innerHTML == '是') {
            obj.innerHTML = '否';
        }
        else {
            obj.innerHTML = '是'
        }
    } catch (e) { alertMessage("系统忙，请稍后重试"); }
}
function reloadPage(){
    //window.location.href=document.location;   
    window.location.reload(); 
}
function copyText(fileName){
    var isMinIE4 = (document.all) ? 1 : 0;
    var isMinIE5 = (isMinIE4 && navigator.appVersion.indexOf("5.") >= 0) ? 1 : 0;
    if ( isMinIE5 ){
        var js="<script language=\"javascript\" src=\"" + fileName + "\" type=\"text/javascript\"><\/script>";
	    window.clipboardData.setData("Text", js);
	    alert("复制成功！请粘贴到您所需要的位置中。\n内容如下：\n" + js);
	    return;
    }	    
}
//弹窗
function pop(url,width,height,title,id){   
    var dg;
    if(globalPopCloseRefresh==1){
        dg = new J.dialog({ id:id, skin:'chrome',page:url ,rang: true,width: width, height: height,title:title,cover: true,iconTitle:false,btnBar:false,onXclick:reloadPage });
    }
    else{
        dg = new J.dialog({ id:id, skin:'chrome',page:url ,rang: true,width: width, height: height,title:title,cover: true,iconTitle:false,btnBar:false,onXclick:null });
    }
    dg.ShowDialog();
}
//弹窗(返回不刷新页面)
function popPageOnly(url,width,height,title,id){  
    var dg = new J.dialog({ id:id, skin:'chrome',page:url ,rang: true,width: width, height: height,title:title,cover: true,iconTitle:false,btnBar:false });
    dg.ShowDialog();
}
//弹子窗
function childPop(url,width,height,title,id){
    var DG = frameElement.lhgDG;
    var dg = new DG.curWin.J.dialog({ id:id, skin:'chrome',page:url ,rang: true,width: width, height: height,title:title,cover: false,iconTitle:false,btnBar:false ,parent:DG });
    dg.ShowDialog();
}
