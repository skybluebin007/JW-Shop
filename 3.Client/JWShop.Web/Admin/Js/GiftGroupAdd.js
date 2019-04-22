function addGiftGroupAll(candidateObjName, selectedObjName) {
    var strID = getSelectedID(selectedObjName);
    var candidateObj = o(candidateObjName);
    var selectedObj = o(selectedObjName);
    for (var i = 0; i < candidateObj.length; i++) {
        if (strID.indexOf("|" + candidateObj.options[i].value + "|") == -1) {
            selectedObj.options[selectedObj.length] = new Option(candidateObj.options[i].text, candidateObj.options[i].value);
         }
    }
}
function addGiftGroupSingle(candidateObjName, selectedObjName) {
    var strID = getSelectedID(selectedObjName);
    var candidateObj = o(candidateObjName);
    var selectedObj = o(selectedObjName);
    for (var i = 0; i < candidateObj.length; i++) {
        if (candidateObj.options[i].selected && strID.indexOf("|" + candidateObj.options[i].value + "|") == -1) {
            selectedObj.options[selectedObj.length] = new Option(candidateObj.options[i].text, candidateObj.options[i].value);
        }
    }
}
//增加一条礼品组
function addGiftGroup(name,count,idList,nameList,photoList,giftPackID){
    var countObj=o("GiftGroupCount");
    var parentObj=o("GiftGroup");
    var span = document.createElement("span");
    with (span) {
        id = "GiftGroup" + countObj.value;
        className="themeActivityBlock";
    }
    parentObj.appendChild(span);   
   // alert(readContent(name,count,idList,nameList,photoList,countObj.value,giftPackID));
    span.innerHTML =readContent(name,count,idList,nameList,photoList,countObj.value,giftPackID) ;	
    o("GiftGroupCount").value=1+parseInt(countObj.value);
}
//修改一条礼品组
function updateGiftGroup(name,count,idList,nameList,photoList,id,giftPackID){
    var span = o("GiftGroup"+id);
    span.className="themeActivityBlock";
    span.innerHTML =readContent(name,count,idList,nameList,photoList,id,giftPackID) ;	
}
//删除一条礼品组
function deleteGiftGroup(id){
    var obj = o("GiftGroup" + id);
    obj.parentNode.removeChild(obj); 
}
//提交检查
function checkSubmit() {
    checkProductHandler(globalIDPrefix+"Product", "RelationProduct");
    Page_ClientValidate();
}
//读取内容
function readContent(name,count,idList,nameList,photoList,id,giftPackID){
    var result="<input type=\"button\" class=\"button\" onclick=\"childPop('GiftGroupAdd.aspx?Action=Update&ID="+id+"&GiftPackID="+giftPackID+"',650,600,'修改礼品组','GiftGroupAdd')\" value=\"修改产品组\"  style=\"width:80px\" /> ";
    result+="<input type=\"button\" class=\"button\" onclick=\"deleteGiftGroup("+id+")\" value=\"删除礼品组\"  style=\"width:80px\" />";
    result+="<input name=\"GiftGroupValue"+id+"\" id=\"GiftGroupValue"+id+"\"  type=\"hidden\"  value=\""+name+"|"+count+"|"+idList +"\"/>";
	result+="<ul>";
	result+="<li class=\"left\">名称：</li>";
	result+="<li class=\"right\">"+name+"</li>";
	result+="</ul>";	
	result+="<ul>";
	result+="<li class=\"left\">购买数量：</li>";
	result+="<li class=\"right\">"+count +"</li>";
	result+="</ul>";	
	result+="<ul>";
	result+="<li class=\"left\">商品：</li>";
	result+="<li class=\"right\">";
	if(nameList!=""){
	    for(var i=0;i<nameList.split(",").length;i++)	{
	        result+="<div class=\"giftGroupPhoto\"><img  src=\""+photoList.split(",")[i]+"\" alt=\"\" onload=\"photoLoad(this,60,60)\" title=\""+nameList.split(",")[i]+"\"/><br />"+nameList.split(",")[i]+"</div>";
	    }
	}
    result+="</li>";
	result+="</ul>";
	result+="<input name=\"GiftGroupNameValue"+id+"\" id=\"GiftGroupNameValue"+id+"\"  type=\"hidden\"  value=\""+nameList +"\"/>";
	return result;
}
//读取一条礼品组数据
function readGiftGroup(id,giftPackID){
    var DG = frameElement.lhgDG;
    var giftGroupValue=DG.iDoc("GiftPackAdd"+giftPackID).getElementById("GiftGroupValue"+id).value;  
    var giftGroupNameValue=DG.iDoc("GiftPackAdd"+giftPackID).getElementById("GiftGroupNameValue"+id).value;   
    var valueArray=giftGroupValue.split("|");
    var nameArray=giftGroupNameValue.split(",");
    o(globalIDPrefix+"Name").value=valueArray[0];
    o(globalIDPrefix+"Count").value=valueArray[1]; 
    var productObj=o(globalIDPrefix+"Product");
    for (var i = 0; i < valueArray[2].split(",").length; i++) {
        var id=valueArray[2].split(",")[i];
        productObj.options.add(new Option(nameArray[i], id));
    }  
}