<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ThemeActivityAdd.aspx.cs" Inherits="JWShop.Web.Admin.ThemeActivityAdd" %>

<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script language="javascript" type="text/javascript" src="/Admin/Js/Color.js"></script>
    <script language="javascript" type="text/javascript" src="/Admin/js/ProductGroupAdd.js"></script>
    <link rel="stylesheet" href="/Admin/static/css/plugin.css" />
    <script src="/Admin/static/js/colpick.js"></script>   
    <style>
        .themeActivityBlock {
    margin-top: 15px;
    display: block;
    border-bottom:1px solid #dbe1e6;
}
        .themeActivityPhoto {
            width: 70px;
            height: 95px;
            float: left;
            text-align: center;
            line-height: 30px;
            border: 1px solid #EEEEEE;
            margin: 5px;
            overflow: hidden;
        }
    </style>
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur">基本设置</span>
            <span>专题商品</span>
            <span>样式设置</span>
        </div>
        <div class="product-container product-container-border">
            <div class="product-row" id="ContentDefault">
                <div class="form-row">
                    <div class="head">活动名称：</div>
                    <SkyCES:TextBox ID="Name" CssClass="txt" runat="server" Width="400px" CanBeNull="必填" />
                </div>
                <div class="form-row">
                    <div class="head">图片：</div>
                     <a <%if (!string.IsNullOrEmpty(Photo.Text)){%>href="<%=Photo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(Photo.Text)%>" class="icon"  height="50" /></a>
                    <SkyCES:TextBox ID="Photo" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
                </div>
                <div class="form-upload">                   
                    <iframe src="UploadAdd.aspx?Control=Photo&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityPhoto/Original" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                </div>
                <div class="form-row">
                    <div class="head">描述：</div>
                    <SkyCES:TextBox ID="Description" CssClass="txt" runat="server" Width="400px" TextMode="MultiLine" Height="100px" />
                </div>
            </div>
            <div class="product-row" id="ContentProduct" style="display: none">
                <div class="form-row">
                    <div class="head"></div>
                    <input type="button" class="button-2 addThemeAdd"  value="增加产品组" style="width: 80px" />
                </div>
                <div id="ProductGroup">
                    <%int i = 0; if (photoArray.Length > 0)
                      {
                          for (i = 0; i < photoArray.Length; i++)
                          { %>
                    <span id="ProductGroup<%=i %>" class="themeActivityBlock">
                        <div class="form-row">
                        <input type="button" class="button-2 updateThemeAdd" onclick="updateThemeAdd(<%=i%>)" indexVal="<%=i%>" value="修改产品组" style="width: 80px" />
                        <input type="button" class="button-2" onclick="deleteProductGroup(<%=i %>)" value="删除产品组" style="width: 80px" />
                        <input name="ProductGroupValue<%=i %>" id="ProductGroupValue<%=i %>" type="hidden" value="<%=photoArray[i]%>|<%=linkArray[i] %>|<%=photoMobileArray[i]%>|<%=linkMobileArray[i] %>|<%=idArray[i]%>" />
                        </div>
                        <div class="form-row">
                            <div class="head">PC端图片：</div>
                        <img src="<%=ShopCommon.ShowImage(photoArray[i])%>" height="60" />
                        </div>
                        <div class="form-row">
                            <div class="head">移动端图片：</div>
                           <img src="<%=ShopCommon.ShowImage(photoMobileArray[i])%>" height="60" />
                        </div>
                        <div class="form-row">
                            <div class="head">PC端更多地址：</div>
                            <%=linkArray[i]%>
                        </div>
                        <div class="form-row">
                            <div class="head">移动端更多地址：</div>
                            <%=linkMobileArray[i]%>
                        </div>
                        <div class="form-row">
                            <div class="head">商品：</div>
                            <% string nameList = string.Empty; if (idArray[i] != string.Empty)
                               {
                                   foreach (string id in idArray[i].Split(','))
                                   {
                                       nameList += ReadProduct(productList, Convert.ToInt32(id)).Name.Replace(",", "") + ",";%>
                            <div class="themeActivityPhoto">
                                <img src="<%=ShopCommon.ShowImage(ReadProduct(productList,Convert.ToInt32(id)).Photo)%>" alt="" onload="photoLoad(this,60,60)" title="<%=ReadProduct(productList, Convert.ToInt32(id)).Name%>" /><br />
                                <%=ReadProduct(productList, Convert.ToInt32(id)).Name%></div>
                            <%}
                           } %>
                        </div>
                        <input name="ProductGroupNameValue<%=i %>" id="ProductGroupNameValue<%=i %>" type="hidden" value="<%if (nameList != string.Empty) { ResponseHelper.Write(nameList.Substring(0, nameList.Length - 1)); }%>" />
                        <div class="clear"></div>
                    </span>
                    <%}
          }%>
                </div>
                <input type="hidden" name="ProductGroupCount" id="ProductGroupCount" value="<%=i %>" />
            </div>
            <div class="product-row" id="ContentCss" style="display: none">
                <div class="form-row">
                    <div class="head">PC端Css样式：</div>
                    <SkyCES:TextBox ID="Css" CssClass="txt" runat="server" Width="700px" TextMode="MultiLine" Height="200px" />
                </div>
                <div class="form-row">
                    <div class="head">移动端Css样式：</div>
                    <SkyCES:TextBox ID="CssMobile" CssClass="txt" runat="server" Width="700px" TextMode="MultiLine" Height="200px" />
                </div>
                <div class="form-row">
                    <div class="head">PC端顶部图片：</div>
                   <a <%if (!string.IsNullOrEmpty(TopImage.Text)){%>href="<%=TopImage.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(TopImage.Text)%>" class="icon"  height="50" /></a>                    
                    <SkyCES:TextBox ID="TopImage" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
                    <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=TopImage&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityCss/Original&NeedMark=0&NeedNail=0" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                     </div>
                </div>
                <div class="form-row">
                    <div class="head">移动端顶部图片：</div>
                   <a <%if (!string.IsNullOrEmpty(TopImageMobile.Text)){%>href="<%=TopImageMobile.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(TopImageMobile.Text)%>" class="icon"  height="50" /></a>
                    
                    <SkyCES:TextBox ID="TopImageMobile" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
                    <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=TopImageMobile&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityCss/Original" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">PC端背景图片：</div>
<a <%if (!string.IsNullOrEmpty(BackgroundImage.Text)){%>href="<%=BackgroundImage.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(BackgroundImage.Text)%>" class="icon"  height="50" /></a>
                    
                    <SkyCES:TextBox ID="BackgroundImage" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
                    <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=BackgroundImage&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityCss/Original&NeedMark=0&NeedNail=0" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">移动端背景图片：</div>
<a <%if (!string.IsNullOrEmpty(BackgroundImageMobile.Text)){%>href="<%=BackgroundImageMobile.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(BackgroundImageMobile.Text)%>" class="icon"  height="50" /></a>
                    
                    <SkyCES:TextBox ID="BackgroundImageMobile" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
                    <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=BackgroundImageMobile&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityCss/Original" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                        </div>
                </div>
                <div class="form-row">
                    <div class="head">PC端底部图片：</div>
<a <%if (!string.IsNullOrEmpty(BottomImage.Text)){%>href="<%=BottomImage.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(BottomImage.Text)%>" class="icon"  height="50" /></a>
                    
                    <SkyCES:TextBox ID="BottomImage" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
                   <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=BottomImage&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityCss/Original&NeedMark=0&NeedNail=0" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                        </div>
                </div>
                <div class="form-row">
                    <div class="head">移动端底部图片：</div>
<a <%if (!string.IsNullOrEmpty(BottomImageMobile.Text)){%>href="<%=BottomImageMobile.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(BottomImageMobile.Text)%>" class="icon"  height="50" /></a>
                    
                    <SkyCES:TextBox ID="BottomImageMobile" CssClass="txt" runat="server" Width="400px" style="display:none;"/>
                    <div class="form-upload">
                    <iframe src="UploadAdd.aspx?Control=BottomImageMobile&TableID=<%=ThemeActivityBLL.TableID %>&FilePath=ThemeActivityCss/Original" width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                        </div>
                </div>
                <div class="form-row">
                    <div class="head">商品名称颜色：</div>
                    <SkyCES:TextBox ID="ProductColor" CssClass="txt" runat="server" Width="100px" Text="#000000" CanBeNull="必填" />
                    <div class="form-colpick form-button2 colpickMult" tarColr="<%=IDPrefix%>ProductColor">拾色器</div>                    
                    </div>
                <div class="form-row">
                    <div class="head">商品名称大小：</div>
                    <SkyCES:TextBox ID="ProductSize" CssClass="txt" runat="server" Width="100px" Text="12" CanBeNull="必填" RequiredFieldType="数据校验" />
                </div>
                <div class="form-row">
                    <div class="head">商品价格颜色：</div>
                    <SkyCES:TextBox ID="PriceColor" CssClass="txt" runat="server" Width="100px" Text="#000000" CanBeNull="必填" />  
                    <div class="form-colpick form-button2 colpickMult" tarColr="<%=IDPrefix%>PriceColor">拾色器</div>
                </div>
                <div class="form-row">
                    <div class="head">商品价格大小：</div>
                    <SkyCES:TextBox ID="PriceSize" CssClass="txt" runat="server" Width="100px" Text="12" CanBeNull="必填" RequiredFieldType="数据校验" />
                </div>
                <div class="form-row">
                    <div class="head">其他文字颜色：</div>
                    <SkyCES:TextBox ID="OtherColor" CssClass="txt" runat="server" Width="100px" Text="#000000" CanBeNull="必填" />
                    <div class="form-colpick form-button2 colpickMult" tarColr="<%=IDPrefix%>OtherColor">拾色器</div>
                    </div>
                <div class="form-row">
                    <div class="head">其他文字大小：</div>
                    <SkyCES:TextBox ID="OtherSize" CssClass="txt" runat="server" Width="100px" Text="12" CanBeNull="必填" RequiredFieldType="数据校验" />
                </div>
            </div>
    </div>    
            
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" OnClientClick="return Page_ClientValidate()" />
        </div>
    </div>
    <script>
        function updateThemeAdd(indexVal) {            
            //var indexVal=$(this).attr("indexVal");
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: '修改产品组',
                fix: false,
                shadeClose: true,
                maxmin: true,
                area: ['1000px', '500px'],
                content: 'ProductGroupAdd.aspx?Action=Update&Id='+indexVal+'&ThemeActivityId=<%=strThemeActivityId %>'
            });    
        }
        
        $(".addThemeAdd").click(function addThemeAdd() {            
            layer.open({
                type: 2,
                //skin: 'layui-layer-lan',
                title: '添加产品组',
                fix: false,
                shadeClose: true,
                maxmin: true,
                area: ['1000px', '500px'],
                content: 'ProductGroupAdd.aspx?ThemeActivityId=<%=strThemeActivityId %>'
            });                    
        });
       
    </script>
</asp:Content>
