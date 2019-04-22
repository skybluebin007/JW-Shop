<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="OtherConfig.aspx.cs" Inherits="JWShop.Web.Admin.OtherConfig" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
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
        <div class="tab-title">
            <span class="cur">系统设置</span>
        </div>
        <div class="product-container product-container-border">
            <div class="product-main">
                <div class="form-row">
                    <div class="head">
                        评论默认状态：
                    </div>
                    <div class="og-radio">
                        <label class="item  <%if (ShopConfig.ReadConfigInfo().CommentDefaultStatus == 2)
                            { %>checked<%}%>">
                            显示<input type="radio" name="ctl00$ContentPlaceHolder$CommentDefaultStatus" value="2" <%if (ShopConfig.ReadConfigInfo().CommentDefaultStatus == 2)
                                  { %>checked<%}%> /></label>
                        <label class="item <%if (ShopConfig.ReadConfigInfo().CommentDefaultStatus == 3)
                            { %>checked<%}%>">
                            不显示<input type="radio" name="ctl00$ContentPlaceHolder$CommentDefaultStatus" value="3" <%if (ShopConfig.ReadConfigInfo().CommentDefaultStatus == 3)
                                   { %>checked<%}%> /></label>

                    </div>

                </div>
                <div class="form-row">
                    <div class="head">
                        整站图片压缩：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 1)
                            { %>checked<%}%>">
                            开启<input type="radio" name="ctl00$ContentPlaceHolder$AllImageIsNail" value="1" <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 1)
                                  { %>checked<%}%> /></label>
                        <label og-hide="photoWidth" class="item <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 0)
                            { %>checked<%}%>">
                            关闭<input type="radio" name="ctl00$ContentPlaceHolder$AllImageIsNail" value="0" <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 0)
                                  { %>checked<%}%> /></label>
                        &nbsp;开启后图片将会等比压缩至指定尺寸,图片质量会有一定下降
                    </div>
                </div>
                <div class="form-row <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 0)
                    { %>hidden<%}%>"
                    id="photoWidth">
                    <div class="head">
                        图片宽度：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="AllImageWidth" runat="server" MaxLength="4"
                        Text="750" RequiredFieldType="数据校验" onblur="checkImgWidth();" />px
                </div>
                <div class="clear">
                </div>
                <div class="form-row">
                    <div class="head">
                        启用自提：
                    </div>
                    <div class="og-radio">
                        <label class="item  <%if (ShopConfig.ReadConfigInfo().SelfPick == 1)
                            { %>checked<%}%>">
                            启用<input type="radio" name="ctl00$ContentPlaceHolder$SelfPick" value="1" <%if (ShopConfig.ReadConfigInfo().SelfPick == 1)
                                  { %>checked<%}%> /></label>
                        <label class="item <%if (ShopConfig.ReadConfigInfo().SelfPick == 0)
                            { %>checked<%}%>">
                            关闭<input type="radio" name="ctl00$ContentPlaceHolder$SelfPick" value="0" <%if (ShopConfig.ReadConfigInfo().SelfPick == 0)
                                  { %>checked<%}%> /></label>

                    </div>

                </div>
                <div class="clear">
                </div>
                <div class="form-row">
                    <div class="head">
                        团购天数：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="GroupBuyDays" runat="server" MaxLength="3" Text="2" RequiredFieldType="数据校验" />
                </div>
                <div class="clear">
                </div>
                <div class="form-row">
                    <div class="head">
                        打印机SN：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="PrintSN" runat="server" MaxLength="150" />
                </div>
                <div class="clear">
                </div>
                <div class="form-row">
                    <div class="head">
                        分销商审核：
                    </div>
                    <div class="og-radio">
                        <label class="item  <%if (ShopConfig.ReadConfigInfo().CheckToBeDistributor == 1)
                            { %>checked<%}%>">
                            启用<input type="radio" name="ctl00$ContentPlaceHolder$CheckToBeDistributor" value="1" <%if (ShopConfig.ReadConfigInfo().CheckToBeDistributor == 1)
                                  { %>checked<%}%> /></label>
                        <label class="item <%if (ShopConfig.ReadConfigInfo().CheckToBeDistributor == 0)
                            { %>checked<%}%>">
                            关闭<input type="radio" name="ctl00$ContentPlaceHolder$CheckToBeDistributor" value="0" <%if (ShopConfig.ReadConfigInfo().CheckToBeDistributor == 0)
                                  { %>checked<%}%> /></label>
                        &nbsp;开启后会员需要申请成为分销商并经过后台审核，否则会员注册时即可成为分销商
                    </div>

                </div>
                <div class="clear">
                </div>
                <div class="form-row">
                    <div class="head">
                        1级分销返佣比例：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="FirstLevelDistributorRebatePercent" runat="server" MaxLength="5" Text="40" MaximumValue="40" RequiredFieldType="数据校验"  CanBeNull="必填"/>
                </div>
                <div class="clear">
                </div>
                 <div class="form-row">
                    <div class="head">
                        2级分销返佣比例：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="SecondLevelDistributorRebatePercent" runat="server" MaxLength="5" Text="40" MaximumValue="40" RequiredFieldType="数据校验" CanBeNull="必填"/>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>


        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClientClick="return checksubmit();"
                OnClick="SubmitButton_Click" />
        </div>
    </div>


    <script type="text/javascript">
        //整站图片压缩宽度
        function checkImgWidth() {
            var imgWidth = $("#<%= AllImageWidth.ClientID%>").val();
            if (imgWidth == "" || (imgWidth != "" && parseInt(imgWidth) < 600)) $("#<%= AllImageWidth.ClientID%>").val("600");
        }
        //提交保存
        function checksubmit() {
            //正整数
            var reg2 = /^[0-9]*[1-9][0-9]*$/;

            //是否启用整站图片压缩
            var enablimagenail = $("input[name='ctl00$ContentPlaceHolder$AllImageIsNail']:checked").val();
            if (enablimagenail == 1 && !reg2.test($("#<%=AllImageWidth.ClientID%>").val()) && parseInt($("#<%= AllImageWidth.ClientID%>").val() < 600)) {
                      alert("整站图片压缩宽度必填且不小于600");
                      $("#<%=AllImageWidth.ClientID%>").focus();
                  return false;
              }
              if (enablimagenail == 0) {
                  $("#<%= AllImageWidth.ClientID%>").val("600");
              }

              if (!reg2.test($("#<%=GroupBuyDays.ClientID%>").val())) {
                      alert("团购天数必填且必须大于0");
                      $("#<%=GroupBuyDays.ClientID%>").focus();
                  return false;
              }

          }
    </script>
</asp:Content>
