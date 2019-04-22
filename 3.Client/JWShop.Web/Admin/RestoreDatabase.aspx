<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="RestoreDatabase.aspx.cs" Inherits="JWShop.Web.Admin.RestoreDatabase" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
	<div class="path-title"></div>
    <div class="product-container product-container-border"> 
    <div class="title" style=" margin-bottom:20px;"> 
        <h1>数据恢复</h1>
        <span>通过本界面对以往备份的数据库进行恢复或下载，请注意在恢复后，所有数据库信息包括管理员用户名密码都会恢复成备份时的状态。<br /><span style="color:red;font-size:14px;">数据还原功能仅对拥有服务器权限的网站有用，虚拟空间的站点无法使用</span></span>
    </div>
    <table class="product-list">
                <thead>
                    <tr>       
	                    <td>备份文件名</td>
	                    <td>版本号</td>
	                    <td>大小(字节)</td> 
	                    <td>备份时间</td>  
	                    <td>操作</td>        
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>	     
                            <tr>
		                        <td><%#Eval("BackupName") %></td>
		                        <td><%# Eval("Version") %></td>
		                        <td><%# Eval("FileSize")%></td> 
	                            <td><%# Eval("BackupTime")%></td> 
		                        <td class="link"><a href="RestoreDatabase.aspx?Action=Restore&filename=<%#Eval("BackupName") %>" onclick="return confirm('您确定要执行恢复操作吗？执行后数据将恢复到备份时的状态！')">恢复</a>                   
                    <a href="RestoreDatabase.aspx?Action=Delete&filename=<%#Eval("BackupName") %>" onclick="return confirm('您确定要删除此备份文件吗？此操作不可还原！')">删除</a>    
		                        </td>
	                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>                
            </table>   

        </div>
        </div>
</asp:Content>
