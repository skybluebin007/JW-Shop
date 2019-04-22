<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="UserBatchAdd.aspx.cs" Inherits="JWShop.Web.Admin.UserBatchAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<script language="javascript" type="text/javascript" src="/Admin/js/UnlimitClass.js"></script>
<link rel="stylesheet" href="/Admin/static/css/plugin.css" />
<script src="/Admin/layer/layer.js"></script>
<script src="/Admin/static/js/colpick.js"></script>   
     
<link rel="stylesheet" href="/Admin/static/css/jquery.validator.css" />
 <script type="text/javascript" src="/Admin/js/jquery.tmpl.min.js"></script>
    <script type="text/javascript" src="/Admin/js/layout.js"></script>
    <script type="text/javascript" src="/Admin/js/jquery.validator.js"></script>
<script type="text/javascript" src="/Admin/js/zh-CN.js"></script>
   <script src="/laydate/laydate.js"></script>
    <style>
        .form-upload:before { content:'上  传'; position:absolute; left:0; top:0; z-index:0; width:100%; height:28px; line-height:28px; text-align:center; }
    </style>

    <div class="container ease" id="container">
    
    	<div class="product-container product-container-border">
        	<div class="product-row">
            	              
                <div class="product-main clearfix">

<div class="form-row">
                        <div class="head">导入EXCEL：</div>
                        <div class="form-upload form-upload-product">
                            <iframe src="/Admin/UploadUser.aspx?FilePath=UserBatch" width="300" height="40px" frameborder="0" allowTransparency="true" scrolling="no" id="uploadIFrame"></iframe>
                        </div>
                        <a href="/Admin/userbatchadd/userbatch.xls" class="form-cut" style="vertical-align: top; height: 28px; width: 80px; text-align:center">下载模版文件</a>
                    </div>
                    <div class="form-row" style="float:none;">
                        <div class="head">批量添加：</div>
                        <table class="product-list">
                            <thead>
                                <tr>
                                    <td style="width: 25%">姓名</td>
                                    <td style="width: 35%">用户名/微信昵称</td>
                                    <td style="width: 15%">手机</td>
                                    <td style="width: 15%">生日</td>
                                    <td style="width: 10%">操作</td>
                                </tr>
                            </thead>
                            <tbody id="eachRow"></tbody>
                            <tfoot style="display:none;">
                                <tr>
                                    <td colspan="8">
                                        <div class="button">
                                            <input type="button" class="button-2" value="再加一个" onclick="addRow();" />
                                        </div>
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </div>

        </div>
        <div class="form-foot">
            
        <input type="submit" class="form-submit ease" value=" 确 定 " /><span style="display:inline-block;margin: 10px 20px 0 0 !important;">初始密码均为“123456”</span>

        </div>
    </div>
<script type="text/javascript" src="/Admin/js/userbatch.add.js"></script>

<script id="tmplRow" type="text/x-jquery-tmpl">
    <tr>
         <td><input type="text" class="txt s_txt" style="width: 92%" name="realname" id="realname${$data.row}" value="${$data.RealName}" /></td>
        <td><input type="text" class="txt s_txt" style="width: 92%" name="username" id="username${$data.row}" value="${$data.UserName}" /></td>
        <td><input type="text" class="txt s_txt" style="width: 90%" name="mobile" id="mobile${$data.row}" value="${$data.Mobile}" maxlength="11" /></td>
        <td><input type="text" class="txt s_txt" style="width: 90%" name="birthday" id="birthday${$data.row}" value="${$data.Birthday}" /></td>
        <td>
            <a href="javascript:void(0);" onclick="delRow(this);" class="num ico del" title="删除" style="margin-left: 5px;">-</a>
        </td>
    </tr>
</script>

    

    <!--menus templates end-->
    <script>
        $(function () {
            Layout.init();
        });

    </script>

</asp:Content>
