<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="PayAdd.aspx.cs" Inherits="JWShop.Web.Admin.PayAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
            <style>
 .form-row table {
            box-shadow: none;
        }
 .form-row table td {
                border: none;
            }
    </style>
    <div class="container ease" id="container">
    	<!--<div class="path-title"></div>-->
        <div class="product-container product-container-border product-container-mt70">
            <div class="form-row">
                <div class="head">名称：</div>
                <%=payPlugins.Name%>
            </div>
            <div class="form-row">
                <div class="head">图片：</div>
                <img src="<%=payPlugins.Photo %>" />
            </div>
            <div class="form-row">
                <div class="head">描述：</div>
                <SkyCES:TextBox ID="Description" CssClass="txt" runat="server" Width="400px" Height="100px" TextMode="MultiLine" />
            </div>
            <div class="form-row">
                <div class="head">是否货到付款：</div>
                <%=ShopCommon.GetBoolText(payPlugins.IsCod)%>
            </div>
            <div class="form-row">
                <div class="head">是否在线支付：</div>
                <%=ShopCommon.GetBoolText(payPlugins.IsOnline)%>
            </div>
            <div class="form-row">
                <div class="head">是否启用：</div>
                <asp:RadioButtonList ID="IsEnabled" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                    <asp:ListItem Value="0">否</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <%  string nameList = "|"; 
	            foreach (KeyValuePair<string, string> keyValue in nameDic){
                nameList += keyValue.Key + "|";
                    %>
                <div class="form-row">
                    <div class="head"><%=keyValue.Value%>：</div>
	                <%if(selectValueDic[keyValue.Key]==string.Empty){ %>
	                    <input name="<%=keyValue.Key %>" value="<%=valueDic[keyValue.Key] %>" style="width:400px" class="txt"/>
	                <%}else{ %>
	                    <select class="select" name="<%=keyValue.Key %>" style="width:410px">
	                        <%foreach(string temp in selectValueDic[keyValue.Key].Split('#')){%>
	                            <option value="<%=temp.Split('|')[0] %>" <%if(temp.Split('|')[0]==valueDic[keyValue.Key]){%> selected="selected" <%} %>><%=temp.Split('|')[1] %></option>
	                        <%} %>
	                    </select>
	                <%} %>	                
                </div>
            <%} %>
            <input name="ConfigNameList" value="<%=nameList %>" type="hidden" />
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
            <br />
            <span class="red">*非专业人士请勿擅自修改</span>
        </div>
    </div>
</asp:Content>
