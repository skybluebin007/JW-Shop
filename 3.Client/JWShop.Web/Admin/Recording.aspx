<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="Recording.aspx.cs" Inherits="JWShop.Web.Admin.Recording" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script src="/Admin/layer/layer.js"></script>
    <div class="container ease" id="container">
    	<div class="product-container">
<dl class="product-filter product-filter-pro clearfix">
    <dd>
        <div class="head">活动商品</div>
        <asp:DropDownList ID="BargainDetail" OnSelectedIndexChanged="BargainDetail_SelectedIndexChanged" AutoPostBack="true" runat="server"  CssClass="select"/>
    </dd>

    <dd>
        <div class="head">砍价发起人</div>
        <asp:DropDownList ID="BargainOrder" runat="server" OnSelectedIndexChanged="BargainOrder_SelectedIndexChanged" AutoPostBack="true"  CssClass="select"/>
    </dd>
   <%if (_bargainOrder.Id>0)
        {%>
    <dd>
        砍价状态：<%=(BargainOrderType)(_bargainOrder.Status)%>
       
    </dd>
    <%} %>
  <dd>
        参与人数：<%=recordingList.Count %>
    </dd>
</dl>
<table  cellpadding="0" cellspacing="0" border="0" class="product-list product-list-img" width="100%">
<thead>
<tr>
<%--	<td style="width:auto">ID</td>--%>
	<td style="width:auto">微信昵称</td>	
    <td style="width:auto">头像</td>  
	<td style="width:auto">砍价金额</td>               
</tr>
</thead>
    <tbody>
<asp:Repeater ID="RecordList" runat="server">
	<ItemTemplate>	     
        	<tr height="80" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
			<%--<td height="80"><%# Eval("ID") %></td>--%>
            <td><%# Eval("UserName") %>
              &nbsp;&nbsp; <%# ShowLeader(Eval("UserId")) %>
            </td>
            <td><div class="scan-img"><img src="<%#ShopCommon.ShowImage(Eval("Photo").ToString().Replace("Original","90-90")) %>" /></div></td>   	
            <td><%# Eval("Price") %></td>        
		</tr>
     </ItemTemplate>
</asp:Repeater>        
    </tbody>
    <%if (recordingList.Count <= 0)
        { %>
    <tfoot>
        <tr>
            <td colspan="4">暂无</td>
        </tr>
    </tfoot>
    <%} %>
</table>
<%--<div class="listPage"><SkyCES:CommonPager ID="MyPager" runat="server" /></div>--%>
</div>
    </div>
<script language="javascript" type="text/javascript">
    function selectThisProduct(productID) {
   var index = parent.layer.getFrameIndex(window.name);
    //var tag=getQueryString("Tag");
    parent.loadProduct(productID);
    parent.layer.close(index);
}
</script>
</asp:Content>

