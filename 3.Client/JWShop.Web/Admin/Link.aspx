<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="Link.aspx.cs" Inherits="JWShop.Web.Admin.Link" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="container ease" id="container">
        <div class="product-container" style="padding-top: 24px;">        
    <div class="add-button"><a href="LinkAdd.aspx?CID=<%=RequestHelper.GetQueryString<string>("ClassID")%>" title="添加新数据" class="ease"> 添 加 </a></div>
    <table class="product-list link-add-list">
        <thead>
                    <tr>  
          
            <td style="width:10%">ID</td>
	        <td style="width:30%; text-align:left;text-indent:8px;">标题</td>
	        <td style="width:40%;">URL</td>	       
	        <td style="width:15%">管理</td>
        </tr>
            </thead>
    <asp:Repeater ID="RecordList" runat="server">
	    <ItemTemplate>	     
            <tr class="listTableMain" onmousemove="changeColor(this,'#FFFDD7')" onmouseout="changeColor(this,'#FFF')">
            
                  <td style="width:10%"><%#Eval("Id") %></td>
			    <td style="width:30%; text-align:left;text-indent:8px;"><%# Eval("Display") %></td>
			    <td style="width:40%;"><%#Eval("URL") %></a></td>
	          
			    <td style="width:15%" class="imgCz">
                     <a href="LinkAdd.aspx?ID=<%#Eval("ID")%>&ClassID=<%=classID %>"  class="ig-colink">修改</a> | 
                     <a href="javascript:void(0);" class="ig-colink pmore">更多</a>
		                        	<div class="list">
	                 <a href='?Action=Delete&Id=<%# Eval("Id") %>' onclick="return check()" class="ig-colink">删除</a>
                     <a href="?Action=Up&ID=<%# Eval("ID") %>&ClassID=<%=classID%>" class="ig-colink">上移</a> 
                    <a href="?Action=Down&ID=<%# Eval("ID") %>&ClassID=<%=classID%>" class="ig-colink">下移</a>
                                    </div>
                  
			    </td>
		    </tr>
            </ItemTemplate>
    </asp:Repeater>
    </table>
    <div class="listPage"><SkyCES:CommonPager ID="MyPager" runat="server" /></div>    
            </div>
    </div>
        <script type="text/javascript">
            $(".pmore").mouseenter(function(){
				$(this).next().show();
			});
			
			$(".imgCz .list,.imgCz").mouseleave(function(){
				$(".imgCz .list").hide();
			});
    </script>
</asp:Content>
