<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Helps.aspx.cs" Inherits="JWShop.Web.Admin.Helps" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
<div class="helper-container" >
	<div class="helper-main" >
		<iframe width="100%" height="100%" frameborder="0" src="http://v.hnjing.com/helps.aspx?cid=<%=cid%>"></iframe>
    </div>
</div>
    <%--<div class="wrapper" id="wrapper">
	<div class="home-container" style="display:none;">
    	<div class="help-title"></div>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <dl class="help-list">
        	        <dt>
            	        <span><%#Container.ItemIndex+1 %>、</span><%#Eval("n_title") %>
                        <a href="#" class="video">查看视频教程</a>
                    </dt>
                    <dd><span>1.1</span><a href="#">二级标题</a></dd>
                    <dd><span>1.2</span><a href="#">二级标题</a></dd>
                    <dd><span>1.3</span><a href="#">二级标题</a></dd>
                    <dd><span>1.4</span><a href="#">二级标题</a></dd>
                    <dd><span>1.5</span><a href="#">二级标题</a></dd>
                    <dd><span>1.6</span><a href="#">二级标题</a></dd>
                </dl>
            </ItemTemplate>
        </asp:Repeater>            
		
        <%=commonPager.ShowPageAdmin() %>
	</div>
</div>--%>
</asp:Content>