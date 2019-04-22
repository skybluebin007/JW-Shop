<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/MasterPage.Master" CodeBehind="WeixinConfig.aspx.cs" Inherits="JWShop.Web.Admin.WexinConfig" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script language="javascript" type="text/javascript" src="/Admin/Js/Color.js"></script>
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur">微站管理</span>
        </div>
        <div class="product-container product-container-border">
            <div class="product-main">
                <div class="form-row">
                    <div class="head">
                        AppID(应用ID)：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="80" ID="AppID" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        AppSecret：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="80" ID="AppSecret" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        Token(令牌)：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="80" ID="Token" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        AESKey(密钥)：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="80" ID="EncodingAESKey" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        微信登录URL：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="80" ID="WechatLoginURL" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        关注回复标题：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="80" ID="AttentionTitle" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        关注回复内容：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" Height="60px" MaxLength="100" ID="AttentionSummary" runat="server" TextMode="MultiLine"/>
                </div>
                <div class="form-row">
                    <div class="head">
                       关注回复图片：
                    </div>
                     <a <%if (!string.IsNullOrEmpty(AttentionPicture.Text)){%>href="<%=AttentionPicture.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(AttentionPicture.Text)%>" class="icon"  height="50" id="nailimg"/></a>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="80" ID="AttentionPicture" runat="server" style="display:none;"/>
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=AttentionPicture&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no">
                        </iframe>
                    </div>建议图片尺寸330X220
                </div>
                 <div class="form-row">
                    <div class="head">
                        默认回复内容：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" Height="60px" MaxLength="100" ID="DefaultReply" runat="server" TextMode="MultiLine"/>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server"
                OnClick="SubmitButton_Click" />
        </div>
    </div>
    <link rel="stylesheet" href="/Admin/static/css/plugin.css" />
</asp:Content>
