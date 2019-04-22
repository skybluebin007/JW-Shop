var PreAname = "aCommon";
function switchBlock(Oname,MenuURL,RightURL){
    Aname = "a" + Oname;
    o(PreAname).className = "";
    o(Aname).className = "on";
    PreAname=Aname;
    o("LeftFrame").src = MenuURL;
    o("RightFrame").src = RightURL;
}
function goUrl(url){
    o("RightFrame").src = url;
}
function countHeight(){
    var bodyHeight=(document.body.clientHeight-85)+"px";
    o("Body").style.height=bodyHeight;
    o("LeftFrame").style.height=bodyHeight;
}
countHeight();
bindEvent("resize", countHeight);