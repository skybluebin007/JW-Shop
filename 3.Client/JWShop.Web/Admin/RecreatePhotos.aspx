<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="RecreatePhotos.aspx.cs" Inherits="JWShop.Web.Admin.RecreatePhotos" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <div class="container ease" id="container">
        <div class="path-title"></div>
        <div class="product-container product-container-border">
            <div class="form-row">
                <span id="" style="color:red;float:right;font-size: 18px;">一键生成本站所有文章、产品图片（含缩略图、水印图）,请谨慎操作!</span> 
                </div>
            <div class="form-row">
                   
                     <input id="RecreatePhotos" style="height: 30px;background: #e64652;color: #fff;border-radius: 1px;border: 0;font-size: 16px;cursor: pointer;padding:0 5px;" type="button" value="重新生成本站图片" />
                     
                </div>
             <div class="form-row">
            <span id="allPhotosMsg" style="color:red;float:right;font-size: 18px;"></span>
                 <br />
                 <img id="loading" /> 
                 </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            //重新生成所有文章产品图片
            $("#RecreatePhotos").click(function () {
                if (confirm("确定要进行此操作吗？")) {
                    $("#allPhotosMsg").text("正在生成图片，请耐心等待...");
                    $("#loading").attr("src", "/admin/images/loading.gif").show();
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
