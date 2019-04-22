<%@ Page Language="C#" Debug="true" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="AdImageMobileAdd.aspx.cs" Inherits="JWShop.Web.Admin.AdImageMobileAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Business" %>
<asp:Content ID="MasterContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">    
    <div class="position"><img src="/Admin/Style/Images/PositionIcon.png"  alt=""/>广告<%=GetAddUpdate()%></div>

    <div class="add" >
	    <ul>
		    <li class="left">标题：</li>
		    <li class="right"><SkyCES:TextBox ID="Title" CssClass="input" runat="server" Width="300px" /></li>
	    </ul>	
	    <ul>
		    <li class="left">副标题：</li>
		    <li class="right"><SkyCES:TextBox ID="SubTitle" CssClass="input" runat="server" Width="300px" /></li>
	    </ul>	
	    <ul>
		    <li class="left">链接地址：</li>
		    <li class="right"><SkyCES:TextBox ID="LinkUrl" CssClass="input" runat="server" Width="300px" HintInfo="填写该项就表示点击图片直接链接到该地址，如果是外部地址，请在地址前带上http://"/></li>
	    </ul>
	    <ul>
		    <li class="left">上传图片：</li>
		    <li class="right">
                <% 
                    switch (_adType)
                    {
                        case (int)AdImageType.MobileTopBanner:
                            %><span class="upload-tag">建议图片最大宽度不超过640px</span><br /><%
                            break;
                        case (int)AdImageType.MobileBanner:
                            %><span class="upload-tag">建议图片最大宽度不超过640px，最大高度不超过200px，宽高比例约为2：1</span><br /><%
                            break;
                        case (int)AdImageType.MobileTopRecommend:
                            %><span class="upload-tag">建议图片最大宽度不超过250px，最大高度不超过150px，宽高比例约为5：3</span><br /><%
                            break;
                        case (int)AdImageType.MobileTopSubject:
                            %><span class="upload-tag">建议图片最大宽度不超过100px，最大高度不超过75px，宽高比例约为4：3</span><br /><%
                            break;
                        case (int)AdImageType.MobileTopNew:
                            %><span class="upload-tag">建议图片最大宽度不超过100px，最大高度不超过75px，宽高比例约为4：3</span><br /><%
                            break;
                        case (int)AdImageType.MobileFloorClass:
                            %><span class="upload-tag">建议图片最大宽度不超过140px，最大高度不超过200px，宽高比例约为1：2</span><br /><%
                            break;
                        case (int)AdImageType.MobileFloorBottom:
                            %><span class="upload-tag">建议图片最大宽度不超过110px，最大高度不超过110px，宽高比例约为1：1</span><br /><%
                            break;
                    }
                %>
                <SkyCES:TextBox ID="ImageUrl" CssClass="input" runat="server" Width="300px" CanBeNull="必填" /><br />
                <iframe src="UploadAdd.aspx?Control=ImageUrl&TableID=<%=AdImageBLL.TableID%>&FilePath=AdImage/Original" width="400" height="30px" frameborder="0" allowTransparency="true" scrolling="no"></iframe>
		    </li>
	    </ul> 
	    <ul>
		    <li class="left">排序：</li>
		    <li class="right"><SkyCES:TextBox ID="OrderId" CssClass="input" runat="server" Width="300px" CanBeNull="必填" RequiredFieldType="数据校验" HintInfo="数字越小越排前"/></li>
	    </ul>
	    <ul><SkyCES:Hint ID="Hint" runat="server"/></ul>
    </div>
    <div class="action">
        <asp:Button CssClass="button" ID="SubmitButton" Text=" 确 定 " runat="server" OnClick="SubmitButton_Click" />
    </div>
</asp:Content>
