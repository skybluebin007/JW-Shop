<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="HelpDetail.aspx.cs" Inherits="JWShop.Web.Admin.HelpDetail" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <div class="helper-container" >
	<div class="helper-main" >
        <iframe width="100%" height="100%" src="http://v.hnjing.com/Helps/Show/C<%=id%>.html"></iframe>
	<%--<div class="home-container">
        <%if(dt.Rows.Count>0){ %>
        <div style="padding:25px;">
            <div style="text-align:center; font-weight:bold; font-size:14px; margin:15px 0;"><%=dt.Rows[0]["N_Title"]%></div>
            <div style="line-height:25px; font-size:12px">
                <%=dt.Rows[0]["N_Content"]%>
            </div>
        </div>
        <%} %>
	</div>--%>
</div>
        </div>
</asp:Content>
