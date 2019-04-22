<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="BackUp.aspx.cs" Inherits="JWShop.Web.Admin.BackUp" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
	<div class="path-title"></div>
    <div class="product-container product-container-border"> 
    
    <div class="title"> 
        <h1>数据备份</h1>
        <span>在本处可备份所有的数据库数据与结构,备份完成后可通过数据库恢复功能进行恢复，备份过程中请勿进行其他页面操作。</span>
    </div>    
        </div>
        <div class="form-foot">
            <asp:Button ID="btnBackup" runat="server" Text="开始备份" CssClass="form-submit ease" Style="margin: 0; position: static;" OnClick="btnBackup_Click"  />
            </div>
<div class="product-container product-container-border"> 
    <div class="title" style=" margin-bottom:20px;"> 
        <h1>数据还原</h1>
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
		                        <td class="link"><a href="?Action=Restore&filename=<%#Eval("BackupName") %>" onclick="return confirm('您确定要执行还原操作吗？执行后数据将恢复到备份时的状态！')">还原</a>                   
                    <a href="?Action=Delete&filename=<%#Eval("BackupName") %>" onclick="return confirm('您确定要删除此备份文件吗？此操作不可还原！')">删除</a>    
		                        </td>
	                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>                
            </table>  
    </div>
        </div>
</asp:Content>
