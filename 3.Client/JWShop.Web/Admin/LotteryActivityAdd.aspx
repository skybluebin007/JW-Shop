<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="MasterPage.Master" CodeBehind="LotteryActivityAdd.aspx.cs" Inherits="JWShop.Web.Admin.LotteryActivityAdd" %>


<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
        <style>
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <link rel="stylesheet" href="/Admin/kindeditor/themes/default/default.css" />
    <link rel="stylesheet" href="/Admin/kindeditor/plugins/code/prettify.css" />
    <script type="text/javascript" src="/Admin/kindeditor/kindeditor.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/plugins/code/prettify.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/kindeditor-content.js"></script>
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <script>
        $(function () {
            $("#StartDate").datepicker({ changeMonth: true, changeYear: true });
            $("#EndDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <script>
        var KE, KE1, KE3, KE4, KE5, KE6, KE7, KE8;
        $(document).ready(function () {
            KindEditor.ready(function (K) {

                KE = K.create('#ctl00_ContentPlaceHolder_Content', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                KE1 = K.create('#ctl00_ContentPlaceHolder_MobileContent', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                prettyPrint();
            });
        });   
    </script>
    <div class="container ease" id="container">

        <div class="product-container product-container-border product-container-mt70">         
            <div class="form-row">
                <div class="head">
                    活动名称：</div>
                <input type="text" id="Title" name="Title" class="txt" style="width:400px" maxlength="30" value="<%=lotteryActivity.ActivityName%>" /><span class="red">*</span>              
            </div>
            <div class="form-row">
                <div class="head">
                    关键字：
                </div>
                <input type="text" id="Keyword" name="Keyword" class="txt" style="width: 400px" maxlength="30" value="<%=lotteryActivity.ActivityKey%>"/><span class="red">*</span>
            </div>
             <div class="form-row">
                <div class="head">
                    开始时间：</div>
                  <input type="text" id="StartDate" name="StartDate" class="txt" style="width:200px" <%if(lotteryActivity.Id>0){ %>value="<%=lotteryActivity.StartTime.ToString("yyyy-MM-dd")%><%} %>"/><span class="red">*</span>
            </div>
            <div class="form-row">
                <div class="head">
                    结束时间：
                </div>
                <input type="text" id="EndDate" name="EndDate" class="txt" style="width:200px"  <%if(lotteryActivity.Id>0){ %>value="<%=lotteryActivity.EndTime.ToString("yyyy-MM-dd")%><%} %>"/><span class="red">*</span>              
            </div>

            <div class="form-row">
                <div class="head">
                    可抽奖次数：</div>
                 <input type="text" id="MaxNum" name="MaxNum" class="txt" style="width:100px"  <%if(lotteryActivity.Id>0){ %>value="<%=lotteryActivity.MaxNum%>"<%} %> onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"  onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'0')}else{this.value=this.value.replace(/\D/g,'')}"/><span class="red">*</span>            
            </div>
            <div class="form-row">
                <div class="head">
                  活动简介：</div>               
                  <textarea class="input" id="Content" name="Content" style="width: 500px; height: 300px" runat="server" ></textarea>
            </div>
            <div class="form-row">
                <div class="head">
                    图片：</div>
                 <a <%if (!string.IsNullOrEmpty(Photo.Text)){%>href="<%=Photo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Photo.Text)%>" class="icon"  height="50" id="nailimg"/></a>
                <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
            </div>
            <div class="form-row">
                <div class="head">
                    上传图片：</div>
                <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=LotteryActivityBLL.TableID%>&FilePath=LotteryActivityPhoto/Original&NeedMark=0&NeedNail=0"
                        width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no">
                    </iframe>
                </div>
                <span class="red" id="photoMsg">建议上传图片320×200为最佳视觉效果</span>
            </div>
            <div class="form-row">
                <div class="head">
                    一等奖名称：
                </div>
                <input type="text" id="txtPrizeTitle1" name="txtPrizeTitle1" class="txt" style="width:400px" maxlength="30" <%if (prizeList.Count>0){ %>value="<%=prizeList[0].PrizeTitle %>"<%} %>/><span class="red">*</span>  
            </div>
            <div class="form-row">
                <div class="head">
                    一等奖描述：
                </div>
                <textarea id="txtPrize1" name="txtPrize1" class="txt" style="width:400px;height:60px;" ><%if (prizeList.Count>0){ %><%=prizeList[0].PrizeName %><%} %></textarea>
            </div>
            <div class="form-row">
                <div class="head">
                    奖品数量：</div>
                <input type="text" id="txtPrize1Num" name="txtPrize1Num" class="txt" style="width:100px" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"  onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'0')}else{this.value=this.value.replace(/\D/g,'')}"/  <%if (prizeList.Count>0){ %>value="<%=prizeList[0].PrizeNum %>"<%} %>><span class="red">*</span>              
            </div>
            <div class="form-row">
                <div class="head">
                    中奖概率：
                </div>
                 <input type="text" id="txtProbability1" name="txtProbability1" class="txt" style="width:100px" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>0){ %>value="<%=prizeList[0].Probability %>"<%} %>/> %<span class="red">*</span>            
            </div>
            <div class="form-row">
                <div class="head">
                    二等奖名称：
                </div>
                <input type="text" id="txtPrizeTitle2" name="txtPrizeTitle2" class="txt" style="width:400px" maxlength="30" <%if (prizeList.Count>1){ %>value="<%=prizeList[1].PrizeTitle %>"<%} %>/><span class="red">*</span>               
            </div>
            <div class="form-row">
                <div class="head">
                    二等奖描述：
                </div>
                 <textarea id="txtPrize2" name="txtPrize2" class="txt" style="width:400px;height:60px;" ><%if (prizeList.Count>1){ %><%=prizeList[1].PrizeName %><%} %></textarea>
            </div>
            <div class="form-row">
                <div class="head">
                    奖品数量：
                </div>
                <input type="text" id="txtPrize2Num" name="txtPrize2Num" class="txt" style="width: 100px" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'0')}else{this.value=this.value.replace(/\D/g,'')}" <%if (prizeList.Count>1){ %>value="<%=prizeList[1].PrizeNum %>"<%} %>/><span class="red">*</span>             
            </div>
            <div class="form-row">
                <div class="head">
                    中奖概率：
                </div>
                <input type="text" id="txtProbability2" name="txtProbability2" class="txt" style="width: 100px" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>1){ %>value="<%=prizeList[1].Probability %>"<%} %>/>%<span class="red">*</span>              
            </div>
            <div class="form-row">
                <div class="head">
                    三等奖名称：
                </div>
                 <input type="text" id="txtPrizeTitle3" name="txtPrizeTitle3" class="txt" style="width:400px" maxlength="30" <%if (prizeList.Count>2){ %>value="<%=prizeList[2].PrizeTitle %>"<%} %>/><span class="red">*</span>
                </div>
            <div class="form-row">
                <div class="head">
                    三等奖描述：
                </div>
                 <textarea id="txtPrize3" name="txtPrize3" class="txt" style="width:400px;height:60px;" ><%if (prizeList.Count>2){ %><%=prizeList[2].PrizeName %><%} %></textarea>
               </div>
            <div class="form-row">
                <div class="head">
                    奖品数量：
                </div>
                <input type="text" id="txtPrize3Num" name="txtPrize3Num" class="txt" style="width: 100px" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'0')}else{this.value=this.value.replace(/\D/g,'')}" <%if (prizeList.Count>2){ %>value="<%=prizeList[2].PrizeNum %>"<%} %>/><span class="red">*</span>
                </div>
            <div class="form-row">
                <div class="head">
                    中奖概率：
                </div>
                <input type="text" id="txtProbability3" name="txtProbability3" class="txt" style="width: 100px" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>2){ %>value="<%=prizeList[2].Probability %>"<%} %>/>%<span class="red">*</span>               
            </div>
            <div class="form-row">
                <input type="checkbox" id="ChkOpen" name="ChkOpen" onclick="CheckOpen()" value="1" <%if (prizeList.Count>3){ %>checked<%} %> />开启四五六七八等奖
            </div>
            <div id="morePrize">
                <div class="form-row">
                    <div class="head">
                        四等奖名称：
                    </div>
                    <input type="text" id="txtPrizeTitle4" name="txtPrizeTitle4" class="txt" style="width:400px" maxlength="30" <%if (prizeList.Count>3){ %>value="<%=prizeList[3].PrizeTitle %>"<%} %>/><span class="red">*</span>
                   </div>
                <div class="form-row">
                    <div class="head">
                        四等奖描述：
                    </div>
                    <textarea id="txtPrize4" name="txtPrize4" class="txt" style="width:400px;height:60px;" ><%if (prizeList.Count>3){ %><%=prizeList[3].PrizeName %><%} %></textarea>                
                </div>
                <div class="form-row">
                    <div class="head">
                        奖品数量：
                    </div>
                    <input type="text" id="txtPrize4Num" name="txtPrize4Num" class="txt" style="width: 100px" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'0')}else{this.value=this.value.replace(/\D/g,'')}" <%if (prizeList.Count>3){ %>value="<%=prizeList[3].PrizeNum %>"<%} %>/><span class="red">*</span>
                   </div>
                <div class="form-row">
                    <div class="head">
                        中奖概率：
                    </div>
                    <input type="text" id="txtProbability4" name="txtProbability4" class="txt" style="width: 100px" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>3){ %>value="<%=prizeList[3].Probability %>"<%} %>/>%<span class="red">*</span>
                   </div>
                <div class="form-row">
                    <div class="head">
                        五等奖名称：
                    </div>
                    <input type="text" id="txtPrizeTitle5" name="txtPrizeTitle5" class="txt" style="width:400px" maxlength="30" <%if (prizeList.Count>4){ %>value="<%=prizeList[4].PrizeTitle %>"<%} %>/><span class="red">*</span>
                    </div>
                <div class="form-row">
                    <div class="head">
                        五等奖描述：
                    </div>
                    <textarea id="txtPrize5" name="txtPrize5" class="txt" style="width:400px;height:60px;" ><%if (prizeList.Count>4){ %><%=prizeList[4].PrizeName %><%} %></textarea>
                    </div>
                <div class="form-row">
                    <div class="head">
                        奖品数量：
                    </div>
                    <input type="text" id="txtPrize5Num" name="txtPrize5Num" class="txt" style="width: 100px" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>4){ %>value="<%=prizeList[4].PrizeNum %>"<%} %>/><span class="red">*</span>
                  </div>
                <div class="form-row">
                    <div class="head">
                        中奖概率：
                    </div>
                    <input type="text" id="txtProbability5" name="txtProbability5" class="txt" style="width: 100px" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>4){ %>value="<%=prizeList[4].Probability %>"<%} %>/>%<span class="red">*</span>
                    </div>
                <div class="form-row">
                    <div class="head">
                        六等奖名称：
                    </div>
                    <input type="text" id="txtPrizeTitle6" name="txtPrizeTitle6" class="txt" style="width:400px" maxlength="30"  <%if (prizeList.Count>5){ %>value="<%=prizeList[5].PrizeTitle %>"<%} %>/><span class="red">*</span>
                    </div>
                <div class="form-row">
                    <div class="head">
                        六等奖描述：
                    </div>
                    <textarea id="txtPrize6" name="txtPrize6" class="txt" style="width:400px;height:60px;" ><%if (prizeList.Count>5){ %><%=prizeList[5].PrizeName %><%} %></textarea>
                    </div>
                <div class="form-row">
                    <div class="head">
                        奖品数量：
                    </div>
                    <input type="text" id="txtPrize6Num" name="txtPrize6Num" class="txt" style="width: 100px"  onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'0')}else{this.value=this.value.replace(/\D/g,'')}" <%if (prizeList.Count>5){ %>value="<%=prizeList[5].PrizeNum %>"<%} %>/><span class="red">*</span>
                   </div>
                <div class="form-row">
                    <div class="head">
                        中奖概率：
                    </div>
                    <input type="text" id="txtProbability6" name="txtProbability6" class="txt" style="width: 100px"  onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>5){ %>value="<%=prizeList[5].Probability %>"<%} %>/>%<span class="red">*</span>
                    </div>
                <div class="form-row">
                    <div class="head">
                        七等奖名称：
                    </div>
                    <input type="text" id="txtPrizeTitle7" name="txtPrizeTitle7" class="txt" style="width:400px" maxlength="30" <%if (prizeList.Count>6){ %>value="<%=prizeList[6].PrizeTitle %>"<%} %>/><span class="red">*</span>
                    </div>
                <div class="form-row">
                    <div class="head">
                        七等奖描述：
                    </div>
                    <textarea id="txtPrize7" name="txtPrize7" class="txt" style="width:400px;height:60px;" ><%if (prizeList.Count>6){ %><%=prizeList[6].PrizeName %><%} %></textarea>
                    </div>
                <div class="form-row">
                    <div class="head">
                        奖品数量：
                    </div>
                     <input type="text" id="txtPrize7Num" name="txtPrize7Num" class="txt" style="width: 100px" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'0')}else{this.value=this.value.replace(/\D/g,'')}" <%if (prizeList.Count>6){ %>value="<%=prizeList[6].PrizeNum %>"<%} %>/><span class="red">*</span>
                   </div>
                <div class="form-row">
                    <div class="head">
                        中奖概率：
                    </div>
                    <input type="text" id="txtProbability7" name="txtProbability7" class="txt" style="width: 100px" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>6){ %>value="<%=prizeList[6].Probability %>"<%} %>/>%<span class="red">*</span>
                    </div>
                <div class="form-row">
                    <div class="head">
                        八等奖名称：
                    </div>
                     <input type="text" id="txtPrizeTitle8" name="txtPrizeTitle8" class="txt" style="width:400px" maxlength="30" <%if (prizeList.Count>7){ %>value="<%=prizeList[7].PrizeTitle %>"<%} %>/><span class="red">*</span>
                    </div>
                <div class="form-row">
                    <div class="head">
                        八等奖描述：
                    </div>
                    <textarea id="txtPrize8" name="txtPrize8" class="txt" style="width:400px;height:60px;" ><%if (prizeList.Count>7){ %><%=prizeList[7].PrizeName %><%} %></textarea>
                    </div>
                <div class="form-row">
                    <div class="head">
                        奖品数量：
                    </div>
                    <input type="text" id="txtPrize8Num" name="txtPrize8Num" class="txt" style="width: 100px" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'0')}else{this.value=this.value.replace(/\D/g,'')}" <%if (prizeList.Count>7){ %>value="<%=prizeList[7].PrizeNum %>"<%} %>/><span class="red">*</span>
                    </div>
                <div class="form-row">
                    <div class="head">
                        中奖概率：
                    </div>
                    <input type="text" id="txtProbability8" name="txtProbability8" class="txt" style="width: 100px" onkeyup="clearNoNum(this)" onafterpaste="clearNoNum(this)" onblur="clearNoNum(this)" <%if (prizeList.Count>7){ %>value="<%=prizeList[7].Probability %>"<%} %>/>%<span class="red">*</span>
                    </div>
            </div>
            

        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton"  Text=" 确 定 " runat="server"  OnClientClick="return checkSubmit();" OnClick="SubmitButton_Click" />
        </div>
    </div>
<script type="text/javascript">
    $(function () {
        CheckOpen();
    })
    function CheckOpen() {
        if ($("#ChkOpen").is(":checked")) {
            //开启4-8等奖
            $("#morePrize").show();
        }
        else {           
            $("#morePrize").hide();
        }
    }
    function checkSubmit() {
        if ($("#Title").val() == "") {
            $("#Title").focus();
            alertMessage("请输入活动名称");
            return false;
        }
        if ($("#Keyword").val() == "") {
            $("#Keyword").focus();
            alertMessage("请输入活动关键字");
            return false;
        }
        if ($("#StartDate").val() == "") {
            $("#StartDate").focus();
            alertMessage("请选择开始时间");
            return false;
        }
        if (!Validate.isDate($("#StartDate").val())) {
            $("#StartDate").focus();
            alertMessage("开始时间错误");
            return false;
        }
        if ($("#EndDate").val() == "") {
            $("#EndDate").focus();
            alertMessage("请选择结束时间");
            return false;
        }
        if (!Validate.isDate($("#EndDate").val())) {
            $("#EndDate").focus();
            alertMessage("结束时间错误");
            return false;
        }
        if ($("#MaxNum").val()=="" ) {
            $("#MaxNum").focus();
            alertMessage("请输入可抽奖次数");
            return false;
        }
        if (!Validate.isBigInt($("#MaxNum").val()) || isNaN($("#MaxNum").val())) {
            $("#MaxNum").focus();
            alertMessage("可抽奖次数必须是正整数");
            return false;
        }
        if ($("#ctl00_ContentPlaceHolder_Photo").val() == "") {
            $("#ctl00_ContentPlaceHolder_Photo").focus();
            alertMessage("请上传图片");
            return false;
        }
        //一二三等奖名称、奖品数量、获奖概率
        for (var i = 1; i <= 3; i++) {
            var _t=i==1?"一":i==2?"二":i==3?"三":"";
         
        if ($("#txtPrizeTitle"+i).val() == "") {
            $("#txtPrizeTitle"+i).focus();
            alertMessage("请输入"+_t+"等奖名称");
            return false;
        }
        if ($("#txtPrize"+i+"Num").val() == "") {
            $("#txtPrize" + i + "Num").focus();
            alertMessage("请输入" + _t + "等奖奖品数量");
            return false;
        }
        if (!Validate.isBigInt($("#txtPrize" + i + "Num").val()) || isNaN($("#txtPrize" + i + "Num").val())) {
            $("#txtPrize" + i + "Num").focus();
            alertMessage(_t+"等奖奖品数量必须是正整数");
            return false;
        }
        if ($("#txtProbability"+i).val() == "") {
            $("#txtProbability" + i).focus();
            alertMessage("请输入"+_t+"等奖中奖概率");
            return false;
        }
        if (parseFloat($("#txtProbability" + i).val()) > 100 || parseFloat($("#txtProbability" + i).val()) < 0 || isNaN($("#txtProbability" + i).val())) {
            $("#txtProbability" + i).focus();
            alertMessage(_t + "等奖中奖概率范围必须在0-100之间");
            return false;
        }      
    }
        if ($("#ChkOpen").is(":checked")) {
            //开启4-8等奖
            //4-8奖名称、奖品数量、获奖概率
            for (var i = 4; i <= 8; i++) {
                var _t = i == 4 ? "四" : i == 5 ? "五" : i == 6 ? "六" : i == 7 ? "七" : i == 8 ? "八" :"";

                if ($("#txtPrizeTitle" + i).val() == "") {
                    $("#txtPrizeTitle" + i).focus();
                    alertMessage("请输入" + _t + "等奖名称");
                    return false;
                }
                if ($("#txtPrize" + i + "Num").val() == "") {
                    $("#txtPrize" + i + "Num").focus();
                    alertMessage("请输入" + _t + "等奖奖品数量");
                    return false;
                }
                if (!Validate.isBigInt($("#txtPrize" + i + "Num").val()) || isNaN($("#txtPrize" + i + "Num").val())) {
                    $("#txtPrize" + i + "Num").focus();
                    alertMessage(_t + "等奖奖品数量必须是正整数");
                    return false;
                }
                if ($("#txtProbability" + i).val() == "") {
                    $("#txtProbability" + i).focus();
                    alertMessage("请输入" + _t + "等奖中奖概率");
                    return false;
                }
                if (parseFloat($("#txtProbability" + i).val()) > 100 || parseFloat($("#txtProbability" + i).val()) < 0 || isNaN($("#txtProbability" + i).val())) {
                    $("#txtProbability" + i).focus();
                    alertMessage(_t + "等奖中奖概率范围必须在0-100之间");
                    return false;
                }
            }
        }
       
        return true;
        
    }
</script>
   
</asp:Content>
