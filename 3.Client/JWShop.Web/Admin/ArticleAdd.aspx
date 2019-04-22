<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ArticleAdd.aspx.cs" Inherits="JWShop.Web.Admin.ArticleAdd" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
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
    <link rel="stylesheet" href="/Admin/kindeditor/themes/default/default.css" />
    <link rel="stylesheet" href="/Admin/kindeditor/plugins/code/prettify.css" />
    <script type="text/javascript" src="/Admin/kindeditor/kindeditor.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/plugins/code/prettify.js"></script>
    <script type="text/javascript" src="/Admin/kindeditor/kindeditor-content.js"></script>
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_RealDate").datepicker({ changeMonth: true, changeYear: true });


        });
    </script>
    <script>
        var KE, KE1, KE3, KE4, KE5, KE6, KE7, KE8;
        $(document).ready(function () {
            KindEditor.ready(function (K) {

                KE = K.create('#ctl00_ContentPlaceHolder_Content', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                KE1 = K.create('#ctl00_ContentPlaceHolder_MobileContent', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                prettyPrint();
            });
        });
        $(document).ready(function () {
            KindEditor.ready(function (K) {

                KE2 = K.create('#ctl00_ContentPlaceHolder_Content1', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                KE3 = K.create('#ctl00_ContentPlaceHolder_MobileContent1', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                prettyPrint();
            });
        });
        $(document).ready(function () {
            KindEditor.ready(function (K) {

                KE4 = K.create('#ctl00_ContentPlaceHolder_Content2', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                KE5 = K.create('#ctl00_ContentPlaceHolder_MobileContent2', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                prettyPrint();
            });
        });
        $(document).ready(function () {
            KindEditor.ready(function (K) {

                KE7 = K.create('#ctl00_ContentPlaceHolder_Content3', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                KE8 = K.create('#ctl00_ContentPlaceHolder_MobileContent3', {
                    cssPath: '/Admin/kindeditor/plugins/code/prettify.css',
                    uploadJson: '/Admin/kindeditor/asp.net/upload_json.ashx',
                    fileManagerJson: '/Admin/kindeditor/asp.net/file_manager_json.ashx',
                    allowFileManager: true
                });
                prettyPrint();
            });
        });
    </script>
    <div class="container ease" id="container">
        <!--<div class="path-title">
        </div>-->
        <div class="product-container product-container-border product-container-mt70">
            <div class="form-row">
                <div class="head">
                    分类：</div>
                <asp:DropDownList ID="ClassID" runat="server" CssClass="select" />
            </div>
            <div class="form-row">
                <div class="head">
                    标题：</div>
                <SkyCES:TextBox ID="Title" CssClass="txt" runat="server" Width="400px" CanBeNull="必填"  MaxLength="50" />
            </div>
            <div class="form-row">
                <div class="head">
                    发布者：</div>
                <SkyCES:TextBox ID="Author" CssClass="txt" runat="server" Width="100px" Text="原创" />
            </div>
            <div class="form-row">
                <div class="head">
                    来源：</div>
                <SkyCES:TextBox ID="Resource" CssClass="txt" runat="server" Width="100px" Text="本站" />
            </div>
            <div class="form-row">
                <div class="head">
                    排序：</div>
                <SkyCES:TextBox ID="OrderID" CssClass="txt" runat="server" Width="400px" Text="0"
                    CanBeNull="必填" RequiredFieldType="数据校验" />
            </div>
            <div class="form-row">
                <div class="head">
                    实际日期：</div>
                <SkyCES:TextBox ID="RealDate" CssClass="txt" runat="server" Width="400px" CanBeNull="必填" />
            </div>
            <div class="form-row" style="display:none;">
                <div class="head">
                    链接地址：</div>
                <SkyCES:TextBox ID="Url" CssClass="txt" runat="server" Width="400px" />如果是外部地址，请在地址前带上Http://
            </div>
            <div class="form-row" id="btdx" style="display: none;">
                <div class="head">
                    补贴对象：</div>
                <SkyCES:TextBox ID="FilePath" CssClass="txt" runat="server" Width="400px" />
            </div>
            <div class="form-row">
                <div class="head">
                    图片：</div>
                 <a id="imgurl_Photo" <%if (!string.IsNullOrEmpty(Photo.Text)){%>href="<%=Photo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Photo.Text)%>" class="icon"  height="50" id="img_Photo"/></a>
                <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
            </div>
            <div class="form-row">
                <div class="head">
                    上传图片：</div>
                <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=ArticleBLL.TableID%>&FilePath=ArticlePhoto/Original&NeedMark=0"
                        width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no">
                    </iframe>
                </div>
                <span class="red" id="photoMsg"><%if (thisClass.ImageWidth > 0)
                                    {%>建议上传图片<%=thisClass.ImageWidth%>×<%=thisClass.ImageHeight%>为最佳视觉效果<%}
                                    else
                                    {%>建议上传图片600×450为最佳视觉效果<%} %></span>
            </div>
            <div class="form-row" style="display:none;">
                    	<div class="head">案例图集：</div>
                        <div class="ig-multigraph" id="ContentPhoto">
                            <ul class="search clearfix" id="ProductPhotoList">
								<%foreach (ProductPhotoInfo productPhoto in productPhotoList){ %>
                                <li class="productPhoto" id="ProductPhoto<%=productPhoto.Id%>">
                                    <img src="<%=productPhoto.ImageUrl.Replace("Original", "75-75")%>" alt=""  title="<%=productPhoto.Name%>" onload="photoLoad(this,90,90)" id="photo<%=productPhoto.Id %>"/>
                                    <%=StringHelper.Substring(productPhoto.Name, 6)%>
                                    <div class="opr">
                                        <span class="delete" onclick="deleteProductPhoto(<%=productPhoto.Id %>,1)" title="删除">删除</span>
                                        <a class="cut" href="javascript:loadCut('<%=productPhoto.ImageUrl.Replace("75-75", "Original")%>','photo<%=productPhoto.Id %>')" title="裁剪">裁剪</a>
                                    </div>
                                </li>
                                <%} %>
                                <li class="add" id="ProductPhotoListAdd">
                                    <iframe src="ProductPhotoAdd.aspx?proStyle=1" width="90" height="90" frameborder="0" allowTransparency="true" scrolling="no"></iframe>
                                </li>
                            </ul>
                            <div class="clear"></div>
                            <div class="upload-tag"> <span class="red">图片比例4:3，建议上传图片600×450为最佳视觉效果</span> </div>
                        </div>
                    </div>
            <div class="form-row">
                <div class="head">
                    关键字：</div>
                <SkyCES:TextBox ID="Keywords" CssClass="txt" runat="server" Width="400px" />
            </div>
            <div class="form-row">
                <div class="head">
                    摘要：</div>
                <SkyCES:TextBox ID="Summary" CssClass="text" runat="server" Width="690px" TextMode="MultiLine"
                    Height="" />
            </div>
            <div class="form-row" style="float: none;">
                <div class="head">
                    内容：</div>
                <div class="content-box" id="ig-tab">
                    <ul class="tab clearfix"  style="display:none;">
                        <li class="cur">PC端</li>
                        <li>移动端</li>
                    </ul>
                    <div class="main" style="border: 0;">
                        <div class="row">
                            <textarea class="input" id="Content" name="Content" style="width: 700px; height: 400px"
                                runat="server"></textarea></div>
                        <div class="row hidden">
                            <textarea class="input" id="MobileContent" name="Content" style="width: 640px; height: 400px"
                                runat="server"></textarea></div>
                    </div>
                </div>
            </div>
            <div class="form-row" style="float: none; display:none">
                <div class="head">
                    服务亮点：</div>
                <div class="content-box" id="ig-tab1">
                    <ul class="tab clearfix">
                        <li class="cur">PC端</li>
                        <li>移动端</li>
                    </ul>
                    <div class="main" style="border: 0;">
                        <div class="row">
                            <textarea class="input" id="Content1" name="Content1" style="width: 700px; height: 400px"
                                runat="server"></textarea></div>
                        <div class="row hidden">
                            <textarea class="input" id="MobileContent1" name="Content1" style="width: 640px; height: 400px"
                                runat="server"></textarea></div>
                    </div>
                </div>
            </div>
            <div class="form-row" style="float: none;display:none">
                <div class="head">
                    服务保障：</div>
                <div class="content-box" id="ig-tab2">
                    <ul class="tab clearfix">
                        <li class="cur">PC端</li>
                        <li>移动端</li>
                    </ul>
                    <div class="main" style="border: 0;">
                        <div class="row">
                            <textarea class="input" id="Content2" name="Content2" style="width: 700px; height: 400px"
                                runat="server"></textarea></div>
                        <div class="row hidden">
                            <textarea class="input" id="MobileContent2" name="Content2" style="width: 640px; height: 400px"
                                runat="server"></textarea></div>
                    </div>
                </div>
            </div>
            <div class="form-row" style="float: none;display:none;">
                <div class="head">
                    服务亮点：</div>
                <div class="content-box" id="ig-tab3">
                    <ul class="tab clearfix">
                      <%--  <li class="cur">PC端</li>--%>
                      <%--  <li>移动端</li>--%>
                    </ul>
                    <div class="main" style="border: 0;">
                        <div class="row">
                            <textarea class="input" id="Content3" name="Content3" style="width: 700px; height: 400px"
                                runat="server"></textarea></div>
                        <div class="row hidden">
                            <textarea class="input" id="MobileContent3" name="Content3" style="width: 640px; height: 400px"
                                runat="server"></textarea></div>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="head">
                    是否推荐：</div>
                <asp:RadioButtonList ID="IsTop" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
                    <asp:ListItem Value="0">否</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" Style="margin: 0; position: static;" ID="SubmitButton"
                Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
        </div>
    </div>
        <script language="javascript" type="text/javascript">var productID = ""; var _selectclassid = ""; var isUpdate = 0;</script>	 
    <script type="text/javascript" src="/Admin/Js/ProductAdd.js"></script>
    <script type="text/javascript">
        $(function () {
            var chooseCid = $("#<%=ClassID.ClientID%>").find("option:selected").val();
            getThisClass(chooseCid);

            $("#<%=ClassID.ClientID%>").change(function () {
                getThisClass($("#<%=ClassID.ClientID%>").find("option:selected").val());
            })
        })
        function getThisClass(_cid){
            $.ajax({
                type: 'post',
                url: "?Action=GetThisClass&classId=" + _cid,
                data: {},
                cache: false,
                //dataType: 'json',
                success: function (data) {
                    var str = data.split("|");
                    if (str[0]=="ok") {
                        $("#photoMsg").html(str[1]);
                    }
                    else {
                        $("#photoMsg").html("建议上传图片600×450为最佳视觉效果");
                    }
                },
                error: function () {
                    $("#photoMsg").html();
                }
            });
        }
    </script>
</asp:Content>
