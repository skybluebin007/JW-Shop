
var tempObjID='Default1';
var tempSmallObjID='Default1Menu-1';
function show(objID){
    var divObj=o(objID+"Div");
    var tempDivObj=o(tempObjID+"Div");
    var menuObj=o(objID+"Menu");
    var tempMenuObj=o(tempObjID+"Menu");    
    tempDivObj.className="";
    divObj.className="on";
    tempMenuObj.style.display="none";
    menuObj.style.display="";
    tempObjID=objID;
}
function shwoSmall(smallObjID){
    var smallMenuObj=o(smallObjID);
    var tempSmallMenuObj=o(tempSmallObjID);   
    tempSmallMenuObj.className="";
     smallMenuObj.className="smallOn";
    tempSmallObjID=smallObjID;
}
function goUrl(url){
    top.window.o("RightFrame").src = url;
}
show(tempObjID);