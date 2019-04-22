<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="UserMessageAdd.aspx.cs" Inherits="JWShop.Web.Admin.UserMessageAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <div class="head">留言类型：</div>
                <%=UserMessageBLL.ReadMessageType(userMessage.MessageClass)%>
            </div>

            <div class="clear"></div>
             <%if(userMessage.MessageClass<=5) {%>
            <div class="form-row">
                <div class="head">标题：</div>
                <%=userMessage.Title %>
            </div>
            <div class="clear"></div>
            <%} %>  
              <div class="form-row">
                <div class="head">姓名：</div>
                <%=userMessage.UserName %>
            </div>


             <%if (styleid != 3)
               {%>
            <div class="clear"></div>
                <div class="form-row">
                <div class="head">联系电话：</div>
                <%=userMessage.Tel%>
            </div>
            <div class="clear"></div>   
            <%} %>



             <%if (styleid == 1)
               { %>
              <div class="form-row">
                <div class="head"> <%if (userMessage.MessageClass == 7)
                                     { %>到岗日期<%}
                                     else
                                     { %> 出生日期 <%} %>：</div>
                <%=userMessage.Birthday%>
            </div>
            <%} %>

            <div class="clear"></div>

              <%if (userMessage.MessageClass == 7)
                { %>
             <div class="form-row">
                <div class="head">服务天数：</div>
                <%=userMessage.Servedays%> 天
            </div>
            <div class="clear"></div>
            <%} %>

            <%if (styleid != 3)
              { %>
             <div class="form-row">
                <div class="head"><%if (userMessage.MessageClass == 7)
                                    { %>服务方式<%}
                                    else if (userMessage.MessageClass == 8)
                                    { %> 职业技能 <%}
                                    else
                                    { %> 邮箱地址 <%} %>：</div>
                <%=userMessage.Email%>
            </div>
            <div class="clear"></div>
           
           <%} %>

            <%if (styleid == 1)
              { %>
              <div class="form-row">
                <div class="head"><%if (userMessage.MessageClass == 7)
                                    { %>服务地区<%}
                                    else
                                    { %> 籍贯 <%} %>：</div>
                <%=userMessage.AddCol1%>
            </div>
            <div class="clear"></div>
            <%} %>


            <%if(userMessage.MessageClass==8){ %>
             <div class="form-row">
                <div class="head">现居地：</div>
                <%=userMessage.AddCol2%>
            </div>
            <div class="clear"></div>
           <%} %>

           <%if (styleid == 1)
             { %>
             <div class="form-row">
                <div class="head">具体服务地址：</div>
                <%=userMessage.Address%>
            </div>
            <div class="clear"></div>
             <%} %>
            
             <%if(userMessage.MessageClass==8){ %>
             <div class="form-row">
                <div class="head">性别：</div>
                <%=userMessage.Liveplace%>
            </div>
            <div class="clear"></div>
           <%} %>
            <div class="form-row">
                <div class="head">内容：</div>
                <%=userMessage.Content %>
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">留言IP：</div>
                <%=userMessage.UserIP %>
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">留言时间：</div>
                <%=userMessage.PostDate %>
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">是否处理：</div>
                <div class="og-radio clearfix">
                    <label class="item <%if(userMessage.Id == 0 || userMessage.IsHandler == 1){ %>checked<%} %>">是<input type="radio" name="IsHandler" value="1" <%if(userMessage.Id == 0 || userMessage.IsHandler == 1){ %>checked<%} %>></label>
                    <label class="item <%if(userMessage.Id > 0 && userMessage.IsHandler == 0){ %>checked<%} %>">否<input type="radio" name="IsHandler" value="0" <%if(userMessage.Id > 0 && userMessage.IsHandler == 0){ %>checked<%} %>></label>
                </div>
            </div>
            <div class="clear"></div>
            <div class="form-row" style="display:none;">
                <div class="head">回复：</div>
                <SkyCES:TextBox ID="AdminReplyContent" CssClass="txt" runat="server" Width="300px" TextMode="MultiLine" Height="80px" />
            </div>
            <div class="clear"></div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
        </div>
    </div>
</asp:Content>