<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="NavigationAdd.aspx.cs" Inherits="JWShop.Web.Admin.NavigationAdd"  %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Register Assembly="SkyCES.EntLib" Namespace="SkyCES.EntLib" TagPrefix="SkyCES" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>网站导航<%=GetAddUpdate()%></div>

    <div class="add">
	    <ul>
		    <li class="left">导航类别：</li>
		    <li class="right"><asp:DropDownList ID="ddlNavigationType" runat="server" /></li>
	    </ul>
        <ul>
		    <li class="left">父类导航：</li>
		    <li class="right"><asp:DropDownList ID="ddlParent" runat="server" /></li>
	    </ul>
	    <ul>
		    <li class="left">导航名称：</li>
		    <li class="right"><SkyCES:TextBox  ID="Name" CssClass="input" Width="300px" CanBeNull="必填" runat="server" /></li>
	    </ul>
	    <ul>
		    <li class="left">导航描述：</li>
		    <li class="right"><SkyCES:TextBox ID="Remark" CssClass="input" runat="server" Width="300px" TextMode="MultiLine" Height="80px"/></li>
	    </ul>
	    <ul>
		    <li class="left">是否可见：</li>
		    <li class="right">
                <asp:RadioButtonList ID="IsVisible" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="true" Selected="True">是</asp:ListItem>
                    <asp:ListItem Value="false">否</asp:ListItem>
                </asp:RadioButtonList>
		    </li>
	    </ul>
	    <ul>
		    <li class="left">排序ID：</li>
		    <li class="right"><SkyCES:TextBox ID="OrderId" CssClass="input" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" HintInfo="数字越小越排前"/></li>
	    </ul>
	    <ul>
		    <li class="left">匹配分类内容ID：</li>
		    <li class="right"><asp:DropDownList ID="ClassId" runat="server" /></li>
	    </ul>
	    <ul>
		    <li class="left">链接类型：</li>
		    <li class="right">
                <asp:RadioButtonList ID="LinkType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="LinkType_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="1" Selected="True">使用URL地址</asp:ListItem>
                    <asp:ListItem Value="2">使用文章分类</asp:ListItem>
                    <asp:ListItem Value="3">使用产品分类</asp:ListItem>
                </asp:RadioButtonList>
		    </li>
	    </ul>
        <div id="LinkTypeForURL" runat="server">
	        <ul>
		        <li class="left">URL地址：</li>
		        <li class="right"><SkyCES:TextBox ID="URL"  CssClass="input" Width="300px" HintInfo="如果是外部地址，请在地址前带上http://" runat="server" /></li>
	        </ul>
        </div>
        <div id="LinkTypeForCustom" runat="server">
	        <ul>
		        <li class="left">内容组成：</li>
		        <li class="right">
                    <asp:RadioButtonList ID="radioIsSingle" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radioIsSingle_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="false" Selected="True">列表</asp:ListItem>
                        <asp:ListItem Value="true">单一的</asp:ListItem>
                    </asp:RadioButtonList>
		        </li>
	        </ul>
	        <ul id="ShowTypeForCustom" runat="server">
		        <li class="left">显示方式：</li>
		        <li class="right">
                    <asp:RadioButtonList ID="radioNavigationShowType" runat="server" RepeatDirection="Vertical" />
		        </li>
	        </ul>
        </div>
        <ul><SkyCES:Hint ID="Hint" runat="server"/></ul>
    </div>
    <div class="action">
        <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
    </div>
</asp:Content>
