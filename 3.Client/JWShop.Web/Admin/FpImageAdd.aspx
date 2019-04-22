<%@ Page Language="C#" Debug="true" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="FpImageAdd.aspx.cs" Inherits="JWShop.Web.Admin.AdImageAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
<div class="container ease" id="container">
	<!--<div class="path-title"></div>-->
    <div class="product-container product-container-border product-container-mt70"> 
	    <div class="form-row">
		    <div class="head">标题：</div>
		    <SkyCES:TextBox ID="Title" CssClass="txt" runat="server" Width="300px" MaxLength="20"/>
	    </div>	
	    <div class="form-row"<%if(_adType!=6){%>style=" display:none"<%} %>>
		    <div class="head">显示方式：</div>
            <asp:RadioButtonList ID="SubTitle" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">普通</asp:ListItem>
                    <asp:ListItem Value="2">横幅</asp:ListItem>
                </asp:RadioButtonList>
	    </div>	
        <div class="form-row" <%if(_adType!=10&&_adType!=6){%>style=" display:none"<%} %>>
		    <div class="head">所属分类：</div>
            <asp:DropDownList ID="ProductClass" runat="server"></asp:DropDownList>
	    </div>	
	    <div class="form-row">
		    <div class="head">链接地址：</div>
		    <SkyCES:TextBox ID="LinkUrl" CssClass="txt" runat="server" Width="300px" maxlength="192"/>小程序商品列表页"/pages/product/list",商品详情页："/pages/product/detail?id=1015"
	    </div>
        <div class="form-row">
            <div class="head"><span class="red">*</span>上传图片：</div>
            <SkyCES:TextBox ID="ImageUrl" CssClass="txt" runat="server" Width="300px" Style="display: none;" />
            <a id="imgurl_ImageUrl" <%if (!string.IsNullOrEmpty(ImageUrl.Text))
                {%>href="<%=ImageUrl.Text%>" target="_blank" <%} %>>
                <img src="<%=ImageUrl.Text%>" class="icon" height="50" id="img_ImageUrl" /></a>
            <div class="form-upload">
                <iframe src="UploadAdd.aspx?Control=ImageUrl&TableID=<%=AdImageBLL.TableID%>&FilePath=FpImage/Original&NeedMark=0&NeedNail=0" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
            </div>
        </div>
            <div class="form-row" style="display:none;">
		    <div class="head">背景颜色：</div>
		    <SkyCES:TextBox ID="BgColor" CssClass="txt" runat="server" Width="300px" Text="ffffff" /> 如：ff9966
	    </div>
        <div class="form-row">
            <div class="head">排序：</div>
            <SkyCES:TextBox ID="OrderId" CssClass="txt" runat="server" Width="300px" CanBeNull="必填" RequiredFieldType="数据校验" />
        </div>
      
    </div>
    <div class="form-foot">
	<asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClientClick="return check();" OnClick="SubmitButton_Click" />
    </div>
</div>
    <script type="text/javascript">
        function check() {
            var _img = $("#<%=ImageUrl.ClientID%>").val();
            if (_img == "") {
                alertMessage("请上传图片");
                $("#<%=ImageUrl.ClientID%>").focus();
                return false;
            }
            if ($("#<%=OrderId.ClientID%>").val() == "") {
                alertMessage("请填写排序号");
                $("#<%=OrderId.ClientID%>").focus();
                return false;
            }
        }
        $(function () {
            $("#<%=ImageUrl.ClientID%>").change(function () {
                $("#nailimg").attr("src", $("#<%=ImageUrl.ClientID%>").val());
            })
        })
    </script>
</asp:Content>
