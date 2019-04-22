var colorHex=new Array('00','33','66','99','CC','FF');
var spColorHex=new Array('FF0000','00FF00','0000FF','FFFF00','00FFFF','FF00FF');
function colorOpen(obj,prefix,styleID,valueID){
    var objID=prefix+"_ColorPane";
    if(document.getElementById(objID)==null){
        var divObj = document.createElement("div"); 
        divObj.id=objID;
        //颜色块
        var colorTable=''
        for (i=0;i<2;i++){
            for (j=0;j<6;j++){
                colorTable=colorTable+'<tr height=10>';
                colorTable=colorTable+'<td width=10 style="background-color:#000000">';
                if (i==0){
                    colorTable=colorTable+'<td width=10 style="cursor:pointer;background-color:#'+colorHex[j]+colorHex[j]+colorHex[j]+'" onclick="selectColor(\''+styleID+'\',\''+valueID+'\',\''+divObj.id+'\',this.style.backgroundColor)">';
                }
                else{
                    colorTable=colorTable+'<td width=10 style="cursor:pointer;background-color:#'+spColorHex[j]+'" onclick="selectColor(\''+styleID+'\',\''+valueID+'\',\''+divObj.id+'\',this.style.backgroundColor)">';
                }
                colorTable=colorTable+'<td width=10 style="background-color:#000000">';
                for (k=0;k<3;k++){
                    for (l=0;l<6;l++){
                        colorTable=colorTable+'<td width=10 style="cursor:pointer;background-color:#'+colorHex[k+i*3]+colorHex[l]+colorHex[j]+'" onclick="selectColor(\''+styleID+'\',\''+valueID+'\',\''+divObj.id+'\',this.style.backgroundColor)">';
                    }
                }
            }
        }
        colorTable='<table border="0" cellspacing="0" cellpadding="0" style="border-collapse: collapse;width:232px;" bordercolor="000000">'
        +'<tr height=15><td colspan=21 bgcolor=#ffffff style="font:12px tahoma;padding-left:2px;">'
        +'<span style="float:right;padding-right:3px;cursor:pointer;color:#ff0000" onclick="colorClose(\''+divObj.id+'\')">×关闭</span>'
        +'</td></table>'
        +'<table border="1" cellspacing="0" cellpadding="0" style="border-collapse: collapse" bordercolor="000000" style="cursor:pointer;">'
        +colorTable+'</table>';    
        //赋值属性
        divObj.innerHTML=colorTable;
        divObj.style.left = obj.offsetLeft + "px";
        divObj.style.top =  obj.offsetTop + "px";
        divObj.style.position="absolute";
        divObj.style.zIndex=999;
        document.body.appendChild(divObj);
     }
     else{
        document.getElementById(objID).style.display = "";
     }
}
function selectColor(styleID,valueID,colorID,color){
   if(document.getElementById(styleID)!=null){
        document.getElementById(styleID).style.color =color;
    }
    if(document.getElementById(valueID)!=null){
        document.getElementById(valueID).value = color;
    }
    colorClose(colorID);
}
function colorClose(colorID){
    document.getElementById(colorID).style.display = "none";
}