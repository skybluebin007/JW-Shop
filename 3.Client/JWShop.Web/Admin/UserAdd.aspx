<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="JWShop.Web.Admin.UserAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
        <style>
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_Birthday").datepicker({ changeMonth: true, changeYear: true });
         
        });
    </script>
<script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
<div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">选择会员类型：</div>
                <asp:DropDownList ID="usertype" runat="server" CssClass="txt">
                    </asp:DropDownList>
            </div>
            
            <div class="form-row">
                <div class="head">所属经销商ID：</div>
                <SkyCES:TextBox ID="FromUserid" CssClass="txt" runat="server" Width="100px" />
                &nbsp;<font color="red">*非经销商类型用户，需填写所属经销商ID</font>
            </div>

            <div class="form-row">
                <div class="head">用户名：</div>
                <SkyCES:TextBox ID="UserName" CssClass="txt" runat="server" Width="100px" CanBeNull="必填" onblur="checkUserName(this.value)" />
            </div>
           
	<asp:PlaceHolder ID="Add" runat="server">
	<div class="form-row">
		<div class="head">密码：</div>
		<SkyCES:TextBox ID="UserPassword" CssClass="txt" runat="server" Width="100px" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password"/>
        </div>
            <div class="form-row">
		<div class="head">重复密码：</div>
		<SkyCES:TextBox CssClass="txt" Width="100px" ID="UserPassword2" runat="server" CanBeNull="必填" RequiredFieldType="自定义验证表达式" ValidationExpression="^[\W\w]{6,16}$" CustomErr="密码长度大于6位少于16位" TextMode="Password"/>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="两次密码不一致" ControlToCompare="UserPassword" ControlToValidate="UserPassword2" Display="Dynamic"></asp:CompareValidator>        
	</div>
	</asp:PlaceHolder>
            <div class="form-row">
                <div class="head">性别：</div>
                <asp:RadioButtonList ID="Sex" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">男</asp:ListItem>
                    <asp:ListItem Value="2">女</asp:ListItem>
                    <asp:ListItem Value="3" Selected="True">保密</asp:ListItem>
                </asp:RadioButtonList>
            </div>
          
            <div class="form-row">
                <div class="head">头像：</div>
                <a <%if (!string.IsNullOrEmpty(Photo.Text))
                     {%>href="<%=Photo.Text%>" target="_blank" <%} %>>
                    <img class="icon" height="50" src="<%=ShopCommon.ShowImage(user.Photo) %>" /></a>
                <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" Style="display: none;" />
            </div>

            <%if (RequestHelper.GetQueryString<int>("ID") <= 0)
                { %>
            <div class="form-upload" >
                <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=UserBLL.TableID%>&FilePath=UserPhoto" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
            </div>
            <%} %>
            <div class="form-row" style="display:none;">
                <div class="head">MSN：</div>
                <SkyCES:TextBox ID="MSN" CssClass="txt" runat="server" Width="100px" />
            </div>
            <div class="form-row"  style="display:none;">
                <div class="head">QQ：</div>
                <SkyCES:TextBox ID="QQ" CssClass="txt" runat="server" Width="100px" />
            </div>
            <div class="form-row"  style="display:none;">
                <div class="head">固定电话：</div>
                <SkyCES:TextBox ID="Tel" CssClass="txt" runat="server" Width="100px" />
            </div>
            <div class="form-row">
                <div class="head">手机号码：</div>
                <SkyCES:TextBox ID="Mobile" CssClass="txt" runat="server" Width="100px" CanBeNull="必填" RequiredFieldType="移动手机" />
            </div>
            <div class="form-row">
                <div class="head">真实姓名：</div>
                <SkyCES:TextBox ID="RealName" CssClass="txt" runat="server" Width="140px" />
            </div>
             <div class="form-row">
                <div class="head">生日：</div>
                <SkyCES:TextBox ID="Birthday" CssClass="txt" runat="server" Width="140px" />
            </div>
             <div class="form-row">
                <div class="head">电子邮箱：</div>
                <SkyCES:TextBox ID="Email" CssClass="txt" runat="server" Width="400px"  RequiredFieldType="电子邮箱" />
            </div>
            <div class="form-row">
                <div class="head">所在地区：</div>
                <SkyCES:SingleUnlimitControl ID="UserRegion" runat="server" />
                &nbsp;<font color="red">*经销商类型用户，必须选择所属地区</font>
            </div>
            <div class="form-row">
                <div class="head">详细地址：</div>
                <SkyCES:TextBox ID="Address" CssClass="txt" runat="server" Width="400px" />
            </div>
           
              <div class="form-row">
                <div class="head">备注：</div>
                <SkyCES:TextBox ID="Introduce" CssClass="txt" runat="server" Width="400px" TextMode="MultiLine" Height="100px" />
            </div>
            <div class="form-row">
                <div class="head">状态：</div>
                <asp:RadioButtonList ID="Status" runat="server" RepeatDirection="Horizontal">
                  <%--  <asp:ListItem Value="1">未验证</asp:ListItem>--%>
                    <asp:ListItem Value="2" Selected="True">正常</asp:ListItem>
                    <asp:ListItem Value="3">冻结</asp:ListItem>
                </asp:RadioButtonList>
            </div>
</div>
<div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
     <input type="button"  value="返回用户列表" class="form-submit ease"  onclick="window.location.href = 'User.aspx'" style="background: #cecece;width: 110px;"/>
        </div>

    </div>
<script language="javascript" type="text/javascript">
    var isCheckUserName = false;
    function checkUserName(userName) {
        isCheckUserName = false;
        if (userName != "") {
            var reg = /^([a-zA-Z0-9_\u4E00-\u9FA5])+$/;
            if (reg.test(userName)) {
                Ajax.requestURL("Ajax.aspx?Action=CheckUserName&UserName=" + encodeURI(userName), dealCheckUserName);
            }
            else {
                alertMessage("用户名只能包含字母、数字、下划线、中文");
                return false;
            }
        }
    }
    function dealCheckUserName(data) {
        if (data == "ok") {
            isCheckUserName = true;
        }
        else {
            alertMessage("该用户名已经被占用");
        }
    }
    function checkForm() {
        if (Page_ClientValidate()) {
            if (getQueryString("ID") == "") {
                if (!isCheckUserName) {
                    alertMessage("该用户名已经被占用");
                    return false;
                }
                return true;
            }
            return true;
        }
        else {
            return false;
        }
    }
</script>
</asp:Content>
