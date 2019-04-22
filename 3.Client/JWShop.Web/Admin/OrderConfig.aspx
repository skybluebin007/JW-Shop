<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="OrderConfig.aspx.cs" Inherits="JWShop.Web.Admin.OrderConfig" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <style>
        .form-row table { box-shadow: none; }
        .form-row table td { border: none; }
    </style>
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur">订单设置</span>
        </div>
        <div class="product-container product-container-border">
            <div class="product-main">
                <div class="form-row">
                    <div class="head">
                        订单付款时限：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="OrderPayTime" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" MaxLength="3" />
                    （分钟） （时间限制:0表示不限制）
                </div>


                <div class="form-row">
                    <div class="head">
                        订单收货时限：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="OrderRecieveShippingDays" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" MaxLength="3" />
                    （天） （自发货之日开始计算，0表示不启用自动收货）
                </div>
                <div class="form-row">
                    <div class="head">
                        付款时积分抵现：
                    </div>
                    <div class="og-radio">
                        <label og-show="pointpercent" class="item  <%if (ShopConfig.ReadConfigInfo().EnablePointPay == 1)
                            { %>checked<%}%>">
                            开启<input type="radio" name="ctl00$ContentPlaceHolder$EnablePointPay" value="1" <%if (ShopConfig.ReadConfigInfo().EnablePointPay == 1)
                                  { %>checked<%}%> /></label>
                        <label og-hide="pointpercent" class="item <%if (ShopConfig.ReadConfigInfo().EnablePointPay == 0)
                            { %>checked<%}%>">
                            关闭<input type="radio" name="ctl00$ContentPlaceHolder$EnablePointPay" value="0" <%if (ShopConfig.ReadConfigInfo().EnablePointPay == 0)
                                  { %>checked<%}%> /></label>

                    </div>
                </div>
                <div class="form-row <%if (ShopConfig.ReadConfigInfo().EnablePointPay == 0)
                    { %>hidden<%}%>"
                    id="pointpercent">
                    <div class="head" style="left: -20px;">
                        积分抵现百分比%：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="PointToMoney" runat="server" Text="10" />
                </div>

                <div class="form-row">
                    <div class="head">
                        订单支付模板Id：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="OrderPayTemplateId" runat="server" MaxLength="50" />
                    （微信支付后推送模板消息）
                </div>
                <div class="form-row">
                    <div class="head">
                        订单自提模板Id：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="SelfPickTemplateId" runat="server" MaxLength="50" />
                    （自提订单付款后推送模板消息）
                </div>
                <div class="form-row">
                    <div class="head">
                        开团成功模板Id：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="OpenGroupTemplateId" runat="server" MaxLength="50" />
                    （开团成功[拼满]后推送模板消息）
                </div>
                <div class="form-row">
                    <div class="head">
                        参团成功模板Id：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="GroupSignTemplateId" runat="server" MaxLength="50" />
                    （参团付款后推送模板消息）
                </div>
                <div class="form-row">
                    <div class="head">
                        砍价成功模板Id：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="BarGainTemplateId" runat="server" MaxLength="50" />
                    （砍价成功后推送模板消息）
                </div>
                
                <div class="form-row" style="display:none;">
                    <div class="head">
                        拼团成功模板Id：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="GroupSuccessTemplateId" runat="server" MaxLength="50" />
                    （拼团成功推送模板消息）
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        拼团失败模板Id：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="GroupFailTemplateId" runat="server" MaxLength="50" />
                    （拼团失败后推送模板消息）
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


    <script language="javascript" type="text/javascript">

        //是否启用积分抵现
        var enablepointpay = $("input[name='ctl00$ContentPlaceHolder$EnablePointPay']:checked").val();
        $(function () {
            $("input[name='ctl00$ContentPlaceHolder$EnablePointPay']").click(function () {
                enablepointpay = $("input[name='ctl00$ContentPlaceHolder$EnablePointPay']:checked").val();
                //如果选择关闭，则将积分抵现比例设置为0
                if (enablepointpay == 0) {
                    $("#<%=PointToMoney.ClientID%>").val(0).focus();
                }
            });
        })
        //提交保存
        function checksubmit() {
            //正整数
            var reg2 = /^[0-9]*[1-9][0-9]*$/;
            if (enablepointpay == 1 && !reg2.test($("#<%=PointToMoney.ClientID%>").val())) {
                alert("积分抵现百分比必填且必须是正整数");
                $("#<%=PointToMoney.ClientID%>").focus();
                  return false;
              }
          }


    </script>
</asp:Content>
