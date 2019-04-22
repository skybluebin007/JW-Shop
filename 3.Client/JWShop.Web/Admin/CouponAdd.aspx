<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="CouponAdd.aspx.cs" Inherits="JWShop.Web.Admin.CouponAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <style type="text/css">
        .form-txt .head, .form-txt2 .head { left: -40px; width: 120px; }
    </style>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_UseStartDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_UseEndDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>

    <div class="container ease" id="container">
    	<div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row" style="display:none;">
                <div class="head">
                    类别：
                </div>
                <asp:DropDownList ID="Type" runat="server" CssClass="select">
                    <asp:ListItem Value="1">通用</asp:ListItem>
                    <asp:ListItem Value="2">注册赠送</asp:ListItem>
                    <asp:ListItem Value="3">确认收货赠送</asp:ListItem>
                    <asp:ListItem Value="4">生日赠送</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-row">
                <div class="head">名称：</div>
                <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="400px" CanBeNull="必填" maxlength="20"/>
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">优惠券金额：</div>
                <SkyCES:TextBox ID="Money" CssClass="txt" runat="server" CanBeNull="必填" RequiredFieldType="金额" Text="0" Width="100px"/> 元 
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">最小订单金额：</div>
                <SkyCES:TextBox ID="UseMinAmount" CssClass="txt" runat="server"  CanBeNull="必填" RequiredFieldType="金额" Text="0" HintInfo="只有订单产品金额达到该值的订单才能使用这种优惠券 " Width="100px" /> 元 
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">使用日期：</div>
                <SkyCES:TextBox ID="UseStartDate" CssClass="txt" runat="server" Width="120px" RequiredFieldType="日期时间" CanBeNull="必填"/> <span>到</span> <SkyCES:TextBox ID="UseEndDate" CssClass="txt" runat="server" Width="120px" RequiredFieldType="日期时间" CanBeNull="必填" />
            </div>
            <div class="clear"></div>
            <!--商家优惠券可以设置图片、总数量-->
            <%if (couponKind ==(int)CouponKind.Common) {%>
            <div class="form-row">
                <div class="head">总数：</div>
                <SkyCES:TextBox ID="TotalCount" CssClass="txt" runat="server" CanBeNull="必填" RequiredFieldType="数据校验" Text="0" Width="100px" />            
            </div>
            <div class="clear"></div>
            <div class="form-row">
                <div class="head">
                    图片：
                </div>
                    <a id="imgurl_Photo" <%if (!string.IsNullOrEmpty(Photo.Text))
                                   {%>href="<%=Photo.Text%>" target="_blank" <%} %>>
                    <img src="<%=ShopCommon.ShowImage(Photo.Text)%>" class="icon" height="50" id="img_Photo" /></a>
                <div class="opr <%if(string.IsNullOrEmpty(Photo.Text)){%>hidden<%}%>" >
                    <span class="delete" onclick="deleteCouponPhoto()" title="删除" style="cursor:pointer;">删除</span>
                </div>
                <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" Style="display: none;" />
            </div>
            <div class="form-row">
                <div class="head">
                    上传图片：
                </div>
                <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=CouponBLL.TableID%>&FilePath=CouponPhoto/Original&NeedMark=0"
                        width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                </div>
            </div>
            <%} %>
             <div class="form-row">
                <div class="head"> 已发放(领取)：</div>
             <span class="red"><%=coupon.UsedCount%></span>
            </div>
        </div>
        <div class="form-foot">
            
            <asp:Button CssClass="form-submit ease" Style="margin: 0;" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
            <br />
            <span class="red">*如已发放优惠券，请慎重操作</span>
        </div>
    </div>
    <script>
        //删除优惠券主图
        function deleteCouponPhoto() {
            var photo = $("#ctl00_ContentPlaceHolder_Photo").val();
            if(confirm("确定删除吗?")){
            $.ajax({
                url: "Ajax.aspx?Action=DeleteCouponPhoto&photo=" + photo,
                type: 'get',
                cache: false,
                success: function (data) {
                    if (data == "ok") {
                        $("#ctl00_ContentPlaceHolder_Photo").val("");
                        $("#img_Photo").attr("src", "/Admin/Images/nopic.gif");
                        $(".opr").hide();
                        $("#imgurl_Photo").attr("href", "javascript:void(0)").attr("target", "");
            }
            else {
                        alert("系统忙，请稍后重试1");
            }

            },
                error: function () {
                    alert("系统忙，请稍后重试2");
            }
            })
                }
        }
    </script>
</asp:Content>
