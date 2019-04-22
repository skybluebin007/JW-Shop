<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Popularize.aspx.cs" Inherits="JWShop.Web.Admin.Popularize" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <div class="helper-container" >
	<div class="helper-main" >
        <iframe width="100%" height="100%" src="http://v.hnjing.com/Popularize.aspx?cid=<%=cid%>"></iframe>
        </div>
        </div>    
</asp:Content>

