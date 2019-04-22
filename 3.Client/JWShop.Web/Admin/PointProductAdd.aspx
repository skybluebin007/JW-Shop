<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true" CodeBehind="PointProductAdd.aspx.cs" Inherits="JWShop.Web.Admin.PointProductAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="JWShop.Entity" %>
<asp:Content Id="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <link rel="stylesheet" href="/Admin/kindeditor/themes/default/default.css" />
    <link rel="stylesheet" href="/Admin/kindeditor/plugins/code/prettify.css" />
    <script charset="utf-8" src="/Admin/kindeditor/kindeditor.js"></script>
    <script charset="utf-8" src="/Admin/kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" src="/Admin/kindeditor/plugins/code/prettify.js"></script>
    <script charset="utf-8" src="/Admin/kindeditor/kindeditor-content.js"></script>
    <script type="text/javascript" src="/Admin/layer/layer.js"></script>
    
    <link rel="stylesheet" href="/Admin/Js/jqdate/base/jquery.ui.all.css">    
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.core.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script type="text/javascript" src="/Admin/Js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <link rel="stylesheet" href="/Admin/Js/jqdate/demos.css">
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_BeginDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_EndDate").datepicker({ changeMonth: true, changeYear: true });
        });

        var KE, KE2;
        $(document).ready(function () {
            KindEditor.ready(function (K) {
                KE = K.create('#ctl00_ContentPlaceHolder_Introduction', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp_net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp_net/file_manager_json.ashx',
                    allowFileManager: true
                });
                KE2 = K.create('#ctl00_ContentPlaceHolder_Introduction_Mobile', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp_net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp_net/file_manager_json.ashx',
                    allowFileManager: true
                });
                prettyPrint();
            });
        });
    </script>

    <div class="container ease" id="container">
<%--    	<div class="tab-title">
            <span class="cur">基本信息</span>
            <span>图片和描述</span>
        </div>--%>
        <div class="product-container product-container-border">
            <div class="product-row">
                <%--<div class="product-tip">01</div>--%>
                <div class="product-head">基本信息</div>
                <div class="product-main">
                	<div class="form-row">
                    	<div class="head">商品名称：</div>
                        <SkyCES:TextBox ID="Name" CssClass="txt txt-250" runat="server" MaxLength="40" placeholder="长度限制1-40个字符之间" CanBeNull="必填"/>
                    </div>
                    <div class="clear"></div>
                	<div class="form-row"  style="display:none;">
                    	<div class="head">副标题：</div>
                        <SkyCES:TextBox ID="SubTitle" CssClass="txt txt-250" runat="server" />
                    </div>
                    <div class="clear"></div>
                	<div class="form-row"  style="display:none;">
                    	<div class="head">市场售价：</div>
                        <SkyCES:TextBox ID="MarketPrice" CssClass="txt txt-250" Text="0" runat="server" RequiredFieldType="金额" CanBeNull="必填"/>
                    </div>
                    <div class="clear"></div>
                	<div class="form-row">
                    	<div class="head">所需积分：</div>
                        <SkyCES:TextBox ID="Point" CssClass="txt txt-250" runat="server" RequiredFieldType="数据校验" CanBeNull="必填"/>
                    </div>
                    <div class="clear"></div>
                	<div class="form-row">
                    	<div class="head">可兑换总数：</div>
                        <SkyCES:TextBox ID="TotalCount" CssClass="txt txt-250" runat="server" RequiredFieldType="数据校验" CanBeNull="必填"/>
                    </div>
                    <div class="clear"></div>
                	<div class="form-row">
                    	<div class="head">兑换时间：</div>
                        <SkyCES:TextBox Id="BeginDate" CssClass="txt txt-100" runat="server" Width="140px" CanBeNull="必填" RequiredFieldType="日期时间" /> <span class="tp">到</span> <SkyCES:TextBox Id="EndDate" CssClass="txt txt-100" runat="server" Width="140px" CanBeNull="必填" RequiredFieldType="日期时间" />
                    </div>
                    <div class="clear"></div>
                    <div class="form-row">
                        <div class="head">是否上架：</div>
                        <div class="og-radio clearfix">
                            <label class="item <%if(pointProduct.Id == 0 || pointProduct.IsSale == 1){ %>checked<%} %>">是<input type="radio" name="IsSale" value="1" <%if(pointProduct.Id == 0 || pointProduct.IsSale == 1){ %>checked<%} %>></label>
                            <label class="item <%if(pointProduct.Id > 0 && pointProduct.IsSale == 0){ %>checked<%} %>">否<input type="radio" name="IsSale" value="0" <%if(pointProduct.Id > 0 && pointProduct.IsSale == 0){ %>checked<%} %>></label>
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
            </div>
            <div class="product-row">
                <%--<div class="product-tip">02</div>--%>
                <div class="product-head">图片和详情描述</div>
                <div class="product-main">
                    <div class="form-row">
                    	<div class="head" style="width: 90px;">封面图片：</div>
                    	<div id="imgUpLoad">
                            <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" />
                            <div class="form-upload">
                                <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=ProductBLL.TableID%>&FilePath=ProductCoverPhoto/Original&NeedMark=0" width="300" height="40px" frameborder="0" allowTransparency="true" scrolling="no" id="uploadIFrame"></iframe>
                            </div>
                            <input class="form-cut" type="button" id="cutImage" value="裁剪图片" />
                        </div>                      
                    </div>                    	
                    <div class="clear"></div>
                    <div class="form-images" style="display:none;">
                    	<div class="head">商品图集：</div>
                        <div class="ig-multigraph" id="ContentPhoto">
                            <ul class="search clearfix" id="ProductPhotoList">
								<%foreach (ProductPhotoInfo productPhoto in productPhotoList){ %>
                                    <li class="productPhoto" id="ProductPhoto<%=productPhoto.Id%>">
                                        <img src="<%=productPhoto.ImageUrl.Replace("Original", "75-75")%>" alt="" title="<%=productPhoto.Name%>" onload="photoLoad(this,75,75)" id="photo<%=productPhoto.Id %>"/>
                                        <%=StringHelper.Substring(productPhoto.Name, 6)%>
                                        <div class="opr">
                                            <span class="delete" onclick="deleteProductPhoto(<%=productPhoto.Id %>)" title="删除">删除</span>
                                            <a class="cut" href="javascript:loadCut('<%=productPhoto.ImageUrl.Replace("75-75", "Original")%>','photo<%=productPhoto.Id %>')" title="裁剪">裁剪</a>
                                        </div>
                                        <input type="hidden" name="ProductPhoto" value="<%=productPhoto.Name%>|<%=productPhoto.ImageUrl %>" />
                                    </li>
                                <%} %>
                                <li class="add" id="ProductPhotoListAdd">
                                    <iframe src="ProductPhotoAdd.aspx" width="90" height="90" frameborder="0" allowTransparency="true" scrolling="no"></iframe>
                                </li>
                            </ul>
                            <div class="clear"></div>
                            <div class="upload-tag"> <span class="red">图片比例1:1，建议上传图片尺寸不小于350×350</span> </div>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="form-row" style="float:none;">
                    	<div class="head">内容详细：</div>
                        <div class="content-box" id="ig-tab">
                            <ul class="tab">
                                <li class="cur">PC端</li>
                                <li>移动端</li>
                            </ul>
                            <div class="main" style="border:0;">
                                <div class="row">
                                    <textarea class="input" id="Introduction" name="Introduction" style="width:100%;height:400px" runat="server"></textarea>
                                </div>
                                <div class="row hidden">
                                    <textarea class="input" id="Introduction_Mobile" name="Introduction" style="width:640px;height:400px" runat="server"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" OnClientClick="return Page_ClientValidate()"/>
        </div>
    </div>
    
    <script type="text/javascript" src="/Admin/Js/jquery.tmpl.min.js"></script>
    <script>
        ; !function () {

            //加载扩展模块
            layer.config({
                extend: 'extend/layer.ext.js'
            });

            //页面一打开就执行，放入ready是为了layer所需配件（css、扩展模块）加载完毕
            layer.ready(function () {
                $("#cutImage").click(function () {
                    var orgImage = $("#ctl00_ContentPlaceHolder_Photo").val();
                    if (orgImage.length > 0) {
                        layer.open({
                            type: 2,
                            //skin: 'layui-layer-lan',
                            title: '图片裁剪',
                            fix: false,
                            shadeClose: true,
                            maxmin: true,
                            area: ['900px', '500px'],
                            content: 'EditPhoto.aspx?Photo=' + orgImage + '&TableID=<%=ArticleBLL.TableID%>&TargetID=ctl00_ContentPlaceHolder_Photo&MakeNail=1'
                        });
                    } else {
                        layer.alert("请先上传图片再裁剪");
                    }
                });
                //官网欢迎页              
            });

        }();


        function loadCut(orgImage, targetImg) {
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: '图片裁剪',
                fix: false,
                shadeClose: true,
                maxmin: true,
                area: ['1100px', '600px'],
                content: 'EditPhoto.aspx?Photo=' + orgImage + '&TableID=<%=ProductBLL.TableID%>&TargetID=' + targetImg + '&MakeNail=1&TargetType=1'
            });
        }

        function addProductPhoto(photo, name) {
            var data = { photo: photo, photo_org: photo.replace("75-75", "Original"), name: name.substr(0, 6) };
            $("#productPhotoForm").tmpl(data).appendTo('#ProductPhotoList');
        }
        $('#ProductPhotoList').on('click', '.delete', function () {
            $(this).parent().parent().remove();
        });
    </script>

    <!--商品图集-->
    <script id="productPhotoForm" type="text/x-jquery-tmpl">
        <li class="productPhoto" id="ProductPhoto${photo}">
            <img src="${photo}" id="photo${photo}" />
            ${name}
            <div class="opr">
                <span class="delete" title="删除">删除</span>
                <a class="cut" href="javascript:loadCut('${photo_org}','photo${photo}')" title="裁剪">裁剪</a>
            </div>
            <input type="hidden" name="ProductPhoto" value="${name}|${photo}" />
        </li>
    </script>
</asp:Content>
