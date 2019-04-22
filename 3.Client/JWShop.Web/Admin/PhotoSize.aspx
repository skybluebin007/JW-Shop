<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="PhotoSize.aspx.cs" Inherits="JWShop.Web.Admin.PhotoSize" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/js/jqdate/demos.css">
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_StartPostDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndPostDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <div class="container ease" id="container">
        <asp:Label Text="" ID="lab_sty"  runat="server" Visible="False" />
        <div class="product-container" style="padding: 20px 0 0;">
         <dl class="product-filter clearfix" style="float: left;">
             <dd>
                 <div class="head">
                     图片类别：
                 </div>
                 <asp:DropDownList ID="Type" runat="server" CssClass="select">
                     <asp:ListItem Value="0">全部</asp:ListItem>
                     <asp:ListItem Value="1">文章封面图</asp:ListItem>
                     <asp:ListItem Value="2">产品封面图</asp:ListItem>
                     <asp:ListItem Value="3">产品图集</asp:ListItem>
                 </asp:DropDownList>
             </dd>
               
                <dt>
                    <asp:Button CssClass="submit ease" ID="SearchButton" Text=" 搜 索 " runat="server"
                        OnClick="SearchButton_Click" />

                </dt>
             <dt style="line-height: 30px;">
                  <span id="" style="color:red;float:right;font-size: 16px; margin-left: 10px;">一键生成本站所有文章、产品图片（含缩略图、水印图）,请谨慎操作!</span> 
                   <input class="form-submit" id="RecreatePhotos" style="width: 140px; margin:0px 0 0 20px !important; cursor: pointer;" type="button" value="重新生成本站图片" />
             </dt>
             <dt id="dtMsg" style="width:500px; position:absolute; left:0;  height:30px; left:50%; margin-left:-388px; line-height:30px; text-align:center; top:122px; z-index:99; padding:10px 20px; background:#fff; border:1px solid #eee; display:none;">
               <img style="width:30px; height:30px;vertical-align:-8px; margin-right:6px; " id="loading" /> <span id="allPhotosMsg" style="color:red;font-size: 18px;"></span>
             </dt>
            </dl>
            <table class="product-list" cellpadding="0" cellspacing="0" border="0" width="100%">
                <thead>
                    <tr>
                        <td style="width: 5%">Id
                        </td>
                        <td style="width: 300px;">类别
                        </td>
                       
                        <td style="width: 10%">宽X高
                        </td>
                         <td style="width: 15%">说明
                        </td>
                        <td style="width: 100px">管理
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RecordList" runat="server">
                        <ItemTemplate>                        
                               <tr class="listTableMain" onmousemove="changeColor(this,'#f1f1f1')" onmouseout="changeColor(this,'#FFF')">
                                    <td><%# Eval("ID")%></td>
                                    <td><%# PhotoSizeBLL.ReadPhotoType(Convert.ToInt32(Eval("Type")))%></td>
                                   <td><%# Eval("Width")%> X <%# Eval("Height")%></td>
                                    <td title="<%#Eval("Introduce") %>"><%# StringHelper.Substring(Eval("Introduce").ToString(),19)%></td>                                    
                                    <td>
                                        <a href="PhotoSizeAdd.aspx?ID=<%# Eval("ID")%>" class="ig-colink">修改</a> |
                                        
                                       <a href="?Action=Delete&Id=<%# Eval("Id") %>" onclick="return check()" class="ig-colink">删除</a>  
                                    </td>
                                </tr>
                           
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr> 
                        <td colspan="5"> 
                            <div class="button"><input type="button" onclick="javascript: window.location.href = 'PhotoSizeAdd.aspx'" class="form-submit ease" style="display: inline-block;
    float: left;margin-top: 10px;" value="添 加" /></div>
                            <SkyCES:CommonPager ID="MyPager" runat="server" />
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
     
    </div>
       <script type="text/javascript">
           $(function () {
               //重新生成所有文章产品图片
               $("#RecreatePhotos").click(function () {
                   if (confirm("确定要进行此操作吗？")) {
                       $("#dtMsg").show();
                       $("#allPhotosMsg").text("正在生成图片，请耐心等待...");
                       $("#loading").attr("src", "/admin/images/loading.gif").show();
                       $(".product-list").hide();
                       $.ajax({
                           url: 'Ajax.aspx?Action=CreateAllPhotos',
                           type: 'GET',
                           //data: $("form").serialize(),
                           dataType: "JSON",
                           success: function (result) {
                               $("#allPhotosMsg").text("");
                               $("#loading").attr("src", "").hide();
                               if (result.flag == "ok") {
                                   $("#allPhotosMsg").text("操作成功");
                                   $("#dtMsg").hide();
                                   $(".product-list").show();
                               }
                               else {
                                   $("#allPhotosMsg").text("系统忙，请稍后重试");

                               }
                           }
                       });

                   }
               })
           })
    </script>
</asp:Content>
