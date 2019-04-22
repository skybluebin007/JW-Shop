<%@ Page Language="C#" MasterPageFile="~/Admin/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="ShopConfigAdd.aspx.cs" Inherits="JWShop.Web.Admin.ShopConfigAdd" %>

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
    <link rel="stylesheet" href="/Admin/js/jqdate/base/jquery.ui.all.css">
    <script src="/Admin/js/jqdate/js/jquery.ui.core.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.widget.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker.js"></script>
    <script src="/Admin/js/jqdate/js/jquery.ui.datepicker-zh-CN.js"></script>
    <script>
        $(function () {
            $("#ctl00_ContentPlaceHolder_VoteStartDate").datepicker({ changeMonth: true, changeYear: true });
            $("#ctl00_ContentPlaceHolder_VoteEndDate").datepicker({ changeMonth: true, changeYear: true });
        });
    </script>
    <script language="javascript" type="text/javascript" src="/Admin/Js/Color.js"></script>
    <div class="container ease" id="container">
        <div class="tab-title">
            <span class="cur">基础设置</span>
           <%--  <span>联系方式</span>
            <span>商品设置</span>
            <span>水印设置</span>
             <span>用户设置</span>
            <span>Email设置</span>
          <span>Logo尺寸</span>
            <span>通知提醒</span>   --%>        
        </div>
        <div class="product-container product-container-border">
            <div class="product-main">
                <div class="form-row">
                    <div class="head">
                        小程序标题：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="40" ID="Title" runat="server"
                        ig-max-controller onkeydown="gbcount(this,40,$('#seoTitle'));" onkeyup="gbcount(this,40,$('#seoTitle'));" onblur="gbcount(this,40,$('#seoTitle'));"/>
                        Title，还能输入<strong  id="seoTitle">40</strong>字                   
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        网站关键字：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="Keywords" runat="server" MaxLength="50"
                        ig-max-controller onkeydown="gbcount(this,50,$('#seoKeyword'));" onkeyup="gbcount(this,50,$('#seoKeyword'));" onblur="gbcount(this,50,$('#seoKeyword'));"/>
                        Keyword，还能输入<strong  id="seoKeyword">50</strong>字                  
                </div>
                <div class="form-row">
                    <div class="head">
                        小程序描述：</div>
                    <SkyCES:TextBox CssClass="text" Width="400px" ID="Description" TextMode="MultiLine"
                        Height="60px" runat="server" MaxLength="100" ig-max-controlleronkeydown="gbcount(this,100,$('#seoSummary'));" onkeyup="gbcount(this,100,$('#seoSummary'));" onblur="gbcount(this,100,$('#seoSummary'));"/>
                        Description，还能输入<strong  id="seoSummary">100</strong>字                   
                </div>
                <div class="form-row">
                    <div class="head">
                        LOGO：</div>
               <a id="imgurl_AdminLogo" <%if (!string.IsNullOrEmpty(LogoAddress.Text))
                    {%>href="<%=AdminLogo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(AdminLogo.Text)%>" class="icon"  height="50" id="img_AdminLogo"/></a>
                </div>
                <div class="form-row">
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="AdminLogo" runat="server" style="display:none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=AdminLogo&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no">
                        </iframe>
                    </div>                
                </div>
                <div class="form-row">
                    <div class="head">
                        小程序码：</div>
               <a id="imgurl_LittlePrgCode" <%if (!string.IsNullOrEmpty(LittlePrgCode.Text))
                    {%>href="<%=LittlePrgCode.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(LittlePrgCode.Text)%>" class="icon"  height="50" id="img_LittlePrgCode"/></a>
                </div>
                <div class="form-row">
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="LittlePrgCode" runat="server" Style="display: none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=LittlePrgCode&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        营业资质：
                    </div>
                    <a id="imgurl_Qualification" <%if (!string.IsNullOrEmpty(LittlePrgCode.Text))
                        {%>href="<%=Qualification.Text%>"
                        target="_blank" <%} %>>
                        <img src="<%=ShopCommon.ShowImage(Qualification.Text)%>" class="icon" height="50" id="img_Qualification"/></a>
                </div>
                <div class="form-row">
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="Qualification" runat="server" Style="display: none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=Qualification&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="form-group clearfix">
                    <div class="form-row">
                        <div class="head">
                            联系电话：
                        </div>
                        <SkyCES:TextBox CssClass="txt" Width="120px" ID="Tel" runat="server" MaxLength="13" />
                    </div>
                    <div class="form-row" style="display:none;">
                        <div class="head">
                            联系电话2：
                        </div>
                        <SkyCES:TextBox CssClass="txt" Width="120px" ID="GTel" runat="server" MaxLength="13" />
                    </div>
                </div>
                <div class="form-group clearfix">
                    <div class="form-row" style="display:none;">
                        <div class="head">
                            邮箱：</div>
                        <SkyCES:TextBox CssClass="txt" Width="120px" ID="Fax" runat="server" MaxLength="20" />
                    </div>
                    <div class="form-row">
                        <div class="head">
                            联系人：</div>
                        <SkyCES:TextBox CssClass="txt" Width="120px" ID="PostCode" runat="server" MaxLength="15" />
                    </div>
                </div>
                <div class="form-group clearfix" style="display:none;">
                    <div class="form-row">
                        <div class="head">
                            QQ：</div>
                        <SkyCES:TextBox CssClass="txt" Width="120px" ID="QQ" runat="server" MaxLength="12" />
                    </div>
                    <div class="form-row" >
                        <div class="head">
                            备案号：</div>
                        <SkyCES:TextBox CssClass="txt" Width="120px" ID="RecordCode" runat="server" MaxLength="15" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        营业时间：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="BusinessHours" runat="server" MaxLength="150" />
                </div>
                <div class="form-row">
                    <div class="head">
                        地址：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="Address" runat="server" MaxLength="150" />
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        评论默认状态：
                    </div>
                    <div class="og-radio">
                        <label  class="item  <%if (ShopConfig.ReadConfigInfo().CommentDefaultStatus == 2)
                            { %>checked<%}%>">
                            显示<input type="radio" name="ctl00$ContentPlaceHolder$CommentDefaultStatus" value="2" <%if (ShopConfig.ReadConfigInfo().CommentDefaultStatus == 2)
                                  { %>checked<%}%> /></label>
                        <label class="item <%if (ShopConfig.ReadConfigInfo().CommentDefaultStatus == 3)
                            { %>checked<%}%>">
                            不显示<input type="radio" name="ctl00$ContentPlaceHolder$CommentDefaultStatus" value="3" <%if (ShopConfig.ReadConfigInfo().CommentDefaultStatus == 3)
                                   { %>checked<%}%> /></label>

                    </div>

                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        订单付款时限：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="OrderPayTime" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" MaxLength="3"/>
                    （分钟） （时间限制:0表示不限制）
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        订单收货时限：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="OrderRecieveShippingDays" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" MaxLength="3" />
                    （天） （自发货之日开始计算，0表示不启用自动收货）
                </div>
                <div class="form-row" style="display:none;">
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
                <div style="display:none;" class="form-row <%if (ShopConfig.ReadConfigInfo().EnablePointPay == 0){ %>hidden<%}%>" id="pointpercent">
                    <div class="head" style="left: -20px;">
                        积分抵现百分比%：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="PointToMoney" runat="server" Text="10" />
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        整站图片压缩：</div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 1)
                                                                    { %>checked<%}%>">
                            开启<input type="radio" name="ctl00$ContentPlaceHolder$AllImageIsNail" value="1" <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 1){ %>checked<%}%> /></label>
                        <label og-hide="photoWidth" class="item <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 0)
                                                                      { %>checked<%}%>">
                            关闭<input type="radio" name="ctl00$ContentPlaceHolder$AllImageIsNail" value="0" <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 0){ %>checked<%}%> /></label>
                        &nbsp;开启后图片将会等比压缩至指定尺寸,图片质量会有一定下降
                    </div>
                </div>
                <div style="display:none;" class="form-row <%if (ShopConfig.ReadConfigInfo().AllImageIsNail == 0){ %>hidden<%}%>"
                    id="photoWidth">
                    <div class="head">
                        图片宽度：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="AllImageWidth" runat="server" MaxLength="4"
                        Text="750" RequiredFieldType="数据校验" onblur="checkImgWidth();" />px
                </div>

                <div class="form-row"  style="display:none;">
                    <div class="head">
                        网站名称：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="20" ID="SiteName" runat="server" />网站通用名称
                </div>
                                <div class="form-row" style="display:none;">
                    <div class="head"  >
                        版权：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="Copyright" runat="server" MaxLength="50" />
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        作者：</div>
                    <SkyCES:TextBox CssClass="txt" Width="200px" ID="Author" runat="server" MaxLength="20" />
                </div>
                <div class="form-row" style="display: none">
                    <div class="head">
                        验证码类型：</div>
                    <asp:RadioButtonList ID="CodeType" RepeatDirection="Horizontal" runat="server">
                        <asp:ListItem Value="1" Selected="True">纯数字</asp:ListItem>
                        <asp:ListItem Value="2">纯字母</asp:ListItem>
                        <asp:ListItem Value="3">数字字母混合</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="form-row" style="display: none">
                    <div class="head">
                        验证码长度：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="CodeLength" runat="server" CanBeNull="必填"
                        MaxLength="1" RequiredFieldType="数据校验" />
                </div>
                <div class="form-row" style="display: none">
                    <div class="head">
                        杂点数：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="CodeDot" runat="server" CanBeNull="必填"
                        MaxLength="4" RequiredFieldType="数据校验" />
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        开始年份：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="StartYear" runat="server" CanBeNull="必填"
                        RequiredFieldType="数据校验" MaxLength="4" />
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        结束年份：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="EndYear" runat="server" CanBeNull="必填"
                        RequiredFieldType="数据校验" MaxLength="4" />
                </div>

                <div class="form-row" style="display:none;" >
                    <div class="head">
                        热门搜索：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="HotKeyword" MaxLength="200" runat="server" />
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                        统计代码：</div>
                    <SkyCES:TextBox CssClass="text" Width="400px" ID="StaticCode" MaxLength="200" runat="server"
                        TextMode="MultiLine" Height="80px" />
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        CNZZ站点Id：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" MaxLength="20" ID="CnzzId" runat="server" />
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                        ICO地址：</div>
               <a <%if (!string.IsNullOrEmpty(ICOAddress.Text)){%>href="<%=ICOAddress.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(ICOAddress.Text)%>" class="icon"  height="50" /></a>
                </div>
                <div class="form-row" style="display:none;" >
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="ICOAddress" runat="server" style="display:none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=ICOAddress&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no">
                        </iframe>
                    </div>
                    <span style="float:left">图片尺寸，宽:</span> <SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="IcoWidth" runat="server" Text="1" CanBeNull="必填" RequiredFieldType="数据校验"/><span style="float:left">高:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4"  ID="IcoHeight" runat="server" Text="1" CanBeNull="必填" RequiredFieldType="数据校验"/>px &nbsp;&nbsp;<a href="https://www.baidu.com/s?wd=ico%E5%9B%BE%E6%A0%87%E5%88%B6%E4%BD%9C" target="_blank" style="color:#0985de;font-weight:bold;">点击制作ICO</a>
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                        PC-LOGO地址：</div>
               <a <%if (!string.IsNullOrEmpty(LogoAddress.Text))
                    {%>href="<%=LogoAddress.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(LogoAddress.Text)%>" class="icon"  height="50" /></a>
                </div>
                <div class="form-row" style="display:none;" >
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="LogoAddress" runat="server" style="display:none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=LogoAddress&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no">
                        </iframe>
                    </div>
                   <span style="float:left">图片尺寸，宽:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="LogoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/><span style="float:left">高:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="LogoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/>px
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                        PC会员LOGO：
                    </div>
                   <a <%if (!string.IsNullOrEmpty(UPLogo.Text))
                    {%>href="<%=UPLogo.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(UPLogo.Text)%>" class="icon"  height="50" style="background: #ccc;"/></a>
                </div>
                <div class="form-row" style="display:none;" >
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="UPLogo" runat="server" style="display:none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=UPLogo&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                   <span style="float:left">图片尺寸，宽:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="UPLogoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" /><span style="float:left">高:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="UPLogoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                        手机LOGO：</div>
              <a <%if (!string.IsNullOrEmpty(MobileLogoAddress.Text))
                    {%>href="<%=MobileLogoAddress.Text%>" target="_blank"<%} %>><img src="<%=ShopCommon.ShowImage(MobileLogoAddress.Text)%>" class="icon"  height="50" style="background: #ccc;"/></a>
                </div>
                <div class="form-row" style="display:none;" >
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="MobileLogoAddress" runat="server" Style="display: none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=MobileLogoAddress&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                    <span style="float: left">图片尺寸，宽:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="MobileLogoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" /><span style="float: left">高:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="MobileLogoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                     会员登录背景：
                    </div>
                    <a <%if (!string.IsNullOrEmpty(UserLoginPic.Text))
                         {%>href="<%=UserLoginPic.Text%>"
                        target="_blank" <%} %>>
                        <img src="<%=ShopCommon.ShowImage(UserLoginPic.Text)%>" class="icon" height="50" style="background: #ccc;" /></a>
                </div>
                <div class="form-row" style="display:none;" >
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="UserLoginPic" runat="server" Style="display: none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=UserLoginPic&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                    <span style="float: left">图片尺寸，宽:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="UserLoginWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" /><span style="float: left">高:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="UserLoginHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                        会员注册背景：
                    </div>
                    <a <%if (!string.IsNullOrEmpty(UserRegisterPic.Text))
                         {%>href="<%=UserRegisterPic.Text%>"
                        target="_blank" <%} %>>
                        <img src="<%=ShopCommon.ShowImage(UserRegisterPic.Text)%>" class="icon" height="50" style="background: #ccc;" /></a>
                </div>
                <div class="form-row" style="display:none;" >
                    <SkyCES:TextBox CssClass="txt" Width="310px" ID="UserRegisterPic" runat="server" Style="display: none;" />
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=UserRegisterPic&FilePath=WaterPhoto&NeedMark=0&NeedNail=0"
                            width="700" height="40px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                    <span style="float: left">图片尺寸，宽:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="UserRegisterWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" /><span style="float: left">高:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="UserRegisterHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                </div>
                 <div class="form-row" style="display:none;">
                    <div class="head">
                        投票开始：</div>
                    <SkyCES:TextBox CssClass="txt" Width="200px" ID="VoteStartDate" runat="server" CanBeNull="必填"/>
                </div>
                  <div class="form-row"  style="display:none;">
                    <div class="head">
                        投票结束：</div>
                    <SkyCES:TextBox CssClass="txt" Width="200px" ID="VoteEndDate" runat="server" CanBeNull="必填"/>
                </div>
                <div class="form-row" style="display: none">
                    <div class="head">
                        上传文件类型：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="UploadFile" runat="server" />
                </div>
                <div class="form-row" style="display: none">
                    <div class="head">
                        上传图片类型：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="UploadImage" runat="server" />
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="product-main hidden">

                <div class="form-row">
                    <div class="head">
                        网址：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="SiteLink" runat="server" MaxLength="150" />
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                        手机二维码：</div>                  
                   <a <%if (!string.IsNullOrEmpty(MobileImage.Text))
                    {%>href="<%=MobileImage.Text%>" target="_blank"<%} %>> <img class="icon" height="50" src="<%=ShopCommon.ShowImage(ShopConfig.ReadConfigInfo().MobileImage) %>" /></a>
                </div>
                <div class="form-row" style="display:none;" > 
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="MobileImage" runat="server" MaxLength="150" style="display:none;" />                 
                    <div class="form-upload">                          
                        <iframe src="UploadAdd.aspx?Control=MobileImage&FilePath=WaterPhoto&NeedMark=0&NeedNail=0" width="400"
                            height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                    <span style="float:left">图片尺寸，宽:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="MobilePhotoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" /><span style="float:left">高:</span> <SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="MobilePhotoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                </div>
                <div class="form-row" style="display:none;" >
                    <div class="head">
                        微信二维码：</div>
                    <a <%if (!string.IsNullOrEmpty(WeiXin.Text))
                    {%>href="<%=WeiXin.Text%>" target="_blank"<%} %>><img class="icon" height="50" src="<%=ShopCommon.ShowImage(ShopConfig.ReadConfigInfo().WeiXin) %>" /></a>
                    
                </div>
                <div class="form-row" style="display:none;" >  
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="WeiXin" runat="server" MaxLength="150"  style="display:none;" />              
                    <div class="form-upload">
                        <iframe src="UploadAdd.aspx?Control=WeiXin&FilePath=WaterPhoto&NeedMark=0&NeedNail=0" width="400"
                            height="30px" frameborder="0" allowtransparency="true" scrolling="no"></iframe>
                    </div>
                  <span style="float:left">图片尺寸，宽:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="WeixinWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" /><span style="float:left">高:</span><SkyCES:TextBox CssClass="txt" Width="30px" MaxLength="4" ID="WeixinHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="product-main hidden">
                <div class="add" id="ContentProduct">
                    <div class="form-row" style="display:none;">
                        <div class="head">
                            容许匿名评论：</div>
                        <asp:RadioButtonList ID="AllowAnonymousComment" RepeatDirection="Horizontal" runat="server">
                            <asp:ListItem Value="0" Selected="True">不容许</asp:ListItem>
                            <asp:ListItem Value="1">容许</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                   
                    <div class="form-row">
                        <div class="head">
                            是否提供发票：
                        </div>
                        <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().Invoicing == 1)
                                                                   { %>checked<%}%>">
                            提供<input type="radio" name="ctl00$ContentPlaceHolder$Invoicing" value="1" <%if (ShopConfig.ReadConfigInfo().Invoicing == 1)
                                                                                                             { %>checked<%}%> /></label>
                        <label og-hide="photoWidth" class="item <%if (ShopConfig.ReadConfigInfo().Invoicing == 0)
                                                                  { %>checked<%}%>">
                            不提供<input type="radio" name="ctl00$ContentPlaceHolder$Invoicing" value="0" <%if (ShopConfig.ReadConfigInfo().Invoicing == 0)
                                                                                                             { %>checked<%}%> /></label>
                    
                    </div>
                    </div>

<%--                <div class="form-row">
                    <div class="head" style="left: -40px;">
                       购物赠送积分百分比%：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="MoneyToPoint" runat="server" Text="10" CanBeNull="必填" RequiredFieldType="数据校验" />
                </div>--%>
                    

                </div>
                <div class="clear">
                </div>
            </div>
            <div class="product-main hidden">
                <div class="add" id="ContentPhoto">
                    <div class="form-row">
                        <div class="head">
                            水印方式：</div>
                        <div class="og-radio">
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterType==1){ %>checked<%}%>">
                                无水印<input type="radio" name="ctl00$ContentPlaceHolder$WaterType" value="1" <%if (ShopConfig.ReadConfigInfo().WaterType==1){ %>checked<%}%> /></label>
                            <label class="item  <%if (ShopConfig.ReadConfigInfo().WaterType==2){ %>checked<%}%>">
                                文字水印<input type="radio" name="ctl00$ContentPlaceHolder$WaterType" value="2" <%if (ShopConfig.ReadConfigInfo().WaterType==2){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterType==3){ %>checked<%}%>">
                                图片水印<input type="radio" name="ctl00$ContentPlaceHolder$WaterType" value="3" <%if (ShopConfig.ReadConfigInfo().WaterType==3){ %>checked<%}%> /></label>
                        </div>
                    </div>
                    <div class="form-row" id="WaterPossition" style="display: none;">
                        <div class="head">
                            水印位置：</div>
                        <div class="og-radio">
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==1){ %>checked<%}%>">
                                左上<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="1" <%if (ShopConfig.ReadConfigInfo().WaterPossition==1){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==2){ %>checked<%}%>">
                                左中<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="2" <%if (ShopConfig.ReadConfigInfo().WaterPossition==2){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==3){ %>checked<%}%>">
                                左下<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="3" <%if (ShopConfig.ReadConfigInfo().WaterPossition==3){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==4){ %>checked<%}%>">
                                中上<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="4" <%if (ShopConfig.ReadConfigInfo().WaterPossition==4){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==5){ %>checked<%}%>">
                                正中<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="5" <%if (ShopConfig.ReadConfigInfo().WaterPossition==5){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==6){ %>checked<%}%>">
                                中下<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="6" <%if (ShopConfig.ReadConfigInfo().WaterPossition==6){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==7){ %>checked<%}%>">
                                右上<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="7" <%if (ShopConfig.ReadConfigInfo().WaterPossition==7){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==8){ %>checked<%}%>">
                                中下<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="8" <%if (ShopConfig.ReadConfigInfo().WaterPossition==8){ %>checked<%}%> /></label>
                            <label class="item <%if (ShopConfig.ReadConfigInfo().WaterPossition==9){ %>checked<%}%>">
                                右下<input type="radio" name="ctl00$ContentPlaceHolder$WaterPossition" value="9" <%if (ShopConfig.ReadConfigInfo().WaterPossition==9){ %>checked<%}%> /></label>
                        </div>
                    </div>
                    <div id="TextWaterDiv" style="display: none;">
                        <div class="form-row">
                            <div class="head">
                                水印文字：</div>
                            <SkyCES:TextBox CssClass="txt" Width="400px" ID="Text" runat="server" CanBeNull="必填" />
                        </div>
                        <div class="form-row">
                            <div class="head">
                                文字样式：</div>
                            <asp:DropDownList ID="TextFont" runat="server" CssClass="select">
                                <asp:ListItem Value="Algerian">Algerian</asp:ListItem>
                                <asp:ListItem Value="Arial">Arial</asp:ListItem>
                                <asp:ListItem Value="Arial Black">Arial Black</asp:ListItem>
                                <asp:ListItem Value="Arial Narrow">Arial Narrow</asp:ListItem>
                                <asp:ListItem Value="Arial Unicode MS">Arial Unicode MS</asp:ListItem>
                                <asp:ListItem Value="Baskerville Old Face">Baskerville Old Face</asp:ListItem>
                                <asp:ListItem Value="Batang">Batang</asp:ListItem>
                                <asp:ListItem Value="BatangChe">BatangChe</asp:ListItem>
                                <asp:ListItem Value="Bauhaus 93">Bauhaus 93</asp:ListItem>
                                <asp:ListItem Value="Bell MT">Bell MT</asp:ListItem>
                                <asp:ListItem Value="Berlin Sans FB">Berlin Sans FB</asp:ListItem>
                                <asp:ListItem Value="Berlin Sans FB Demi">Berlin Sans FB Demi</asp:ListItem>
                                <asp:ListItem Value="Bernard MT Condensed">Bernard MT Condensed</asp:ListItem>
                                <asp:ListItem Value="Bodoni MT Poster Compressed">Bodoni MT Poster Compressed</asp:ListItem>
                                <asp:ListItem Value="Book Antiqua">Book Antiqua</asp:ListItem>
                                <asp:ListItem Value="Bookman Old Style">Bookman Old Style</asp:ListItem>
                                <asp:ListItem Value="Bookshelf Symbol 7">Bookshelf Symbol 7</asp:ListItem>
                                <asp:ListItem Value="Britannic Bold">Britannic Bold</asp:ListItem>
                                <asp:ListItem Value="Broadway">Broadway</asp:ListItem>
                                <asp:ListItem Value="Brush Script MT">Brush Script MT</asp:ListItem>
                                <asp:ListItem Value="Calibri">Calibri</asp:ListItem>
                                <asp:ListItem Value="Californian FB">Californian FB</asp:ListItem>
                                <asp:ListItem Value="Cambria">Cambria</asp:ListItem>
                                <asp:ListItem Value="Cambria Math">Cambria Math</asp:ListItem>
                                <asp:ListItem Value="Candara">Candara</asp:ListItem>
                                <asp:ListItem Value="Centaur">Centaur</asp:ListItem>
                                <asp:ListItem Value="Century">Century</asp:ListItem>
                                <asp:ListItem Value="Century Gothic">Century Gothic</asp:ListItem>
                                <asp:ListItem Value="Chiller">Chiller</asp:ListItem>
                                <asp:ListItem Value="Colonna MT">Colonna MT</asp:ListItem>
                                <asp:ListItem Value="Comic Sans MS">Comic Sans MS</asp:ListItem>
                                <asp:ListItem Value="Consolas">Consolas</asp:ListItem>
                                <asp:ListItem Value="Constantia">Constantia</asp:ListItem>
                                <asp:ListItem Value="Cooper Black">Cooper Black</asp:ListItem>
                                <asp:ListItem Value="Corbel">Corbel</asp:ListItem>
                                <asp:ListItem Value="Courier New">Courier New</asp:ListItem>
                                <asp:ListItem Value="Dotum">Dotum</asp:ListItem>
                                <asp:ListItem Value="DotumChe">DotumChe</asp:ListItem>
                                <asp:ListItem Value="Estrangelo Edessa">Estrangelo Edessa</asp:ListItem>
                                <asp:ListItem Value="Footlight MT Light">Footlight MT Light</asp:ListItem>
                                <asp:ListItem Value="Franklin Gothic Medium">Franklin Gothic Medium</asp:ListItem>
                                <asp:ListItem Value="Freestyle Script">Freestyle Script</asp:ListItem>
                                <asp:ListItem Value="Garamond">Garamond</asp:ListItem>
                                <asp:ListItem Value="Gautami">Gautami</asp:ListItem>
                                <asp:ListItem Value="Georgia">Georgia</asp:ListItem>
                                <asp:ListItem Value="Gulim">Gulim</asp:ListItem>
                                <asp:ListItem Value="GulimChe">GulimChe</asp:ListItem>
                                <asp:ListItem Value="Gungsuh">Gungsuh</asp:ListItem>
                                <asp:ListItem Value="GungsuhChe">GungsuhChe</asp:ListItem>
                                <asp:ListItem Value="Harlow Solid Italic">Harlow Solid Italic</asp:ListItem>
                                <asp:ListItem Value="Harrington">Harrington</asp:ListItem>
                                <asp:ListItem Value="High Tower Text">High Tower Text</asp:ListItem>
                                <asp:ListItem Value="Impact">Impact</asp:ListItem>
                                <asp:ListItem Value="Informal Roman">Informal Roman</asp:ListItem>
                                <asp:ListItem Value="Jokerman">Jokerman</asp:ListItem>
                                <asp:ListItem Value="Juice ITC">Juice ITC</asp:ListItem>
                                <asp:ListItem Value="Kristen ITC">Kristen ITC</asp:ListItem>
                                <asp:ListItem Value="Kunstler Script">Kunstler Script</asp:ListItem>
                                <asp:ListItem Value="Latha">Latha</asp:ListItem>
                                <asp:ListItem Value="Lucida Bright">Lucida Bright</asp:ListItem>
                                <asp:ListItem Value="Lucida Calligraphy">Lucida Calligraphy</asp:ListItem>
                                <asp:ListItem Value="Lucida Console">Lucida Console</asp:ListItem>
                                <asp:ListItem Value="Lucida Fax">Lucida Fax</asp:ListItem>
                                <asp:ListItem Value="Lucida Handwriting">Lucida Handwriting</asp:ListItem>
                                <asp:ListItem Value="Lucida Sans Unicode">Lucida Sans Unicode</asp:ListItem>
                                <asp:ListItem Value="Magneto">Magneto</asp:ListItem>
                                <asp:ListItem Value="Mangal">Mangal</asp:ListItem>
                                <asp:ListItem Value="Marlett">Marlett</asp:ListItem>
                                <asp:ListItem Value="Matura MT Script Capitals">Matura MT Script Capitals</asp:ListItem>
                                <asp:ListItem Value="Microsoft Sans Serif">Microsoft Sans Serif</asp:ListItem>
                                <asp:ListItem Value="MingLiU">MingLiU</asp:ListItem>
                                <asp:ListItem Value="Mistral">Mistral</asp:ListItem>
                                <asp:ListItem Value="Modern No. 20">Modern No. 20</asp:ListItem>
                                <asp:ListItem Value="Monotype Corsiva">Monotype Corsiva</asp:ListItem>
                                <asp:ListItem Value="MS Gothic">MS Gothic</asp:ListItem>
                                <asp:ListItem Value="MS Mincho">MS Mincho</asp:ListItem>
                                <asp:ListItem Value="MS PGothic">MS PGothic</asp:ListItem>
                                <asp:ListItem Value="MS PMincho">MS PMincho</asp:ListItem>
                                <asp:ListItem Value="MS Reference Sans Serif">MS Reference Sans Serif</asp:ListItem>
                                <asp:ListItem Value="MS Reference Specialty">MS Reference Specialty</asp:ListItem>
                                <asp:ListItem Value="MS UI Gothic">MS UI Gothic</asp:ListItem>
                                <asp:ListItem Value="MT Extra">MT Extra</asp:ListItem>
                                <asp:ListItem Value="MV Boli">MV Boli</asp:ListItem>
                                <asp:ListItem Value="Niagara Engraved">Niagara Engraved</asp:ListItem>
                                <asp:ListItem Value="Niagara Solid">Niagara Solid</asp:ListItem>
                                <asp:ListItem Value="Nina">Nina</asp:ListItem>
                                <asp:ListItem Value="Old English Text MT">Old English Text MT</asp:ListItem>
                                <asp:ListItem Value="Onyx">Onyx</asp:ListItem>
                                <asp:ListItem Value="Palatino Linotype">Palatino Linotype</asp:ListItem>
                                <asp:ListItem Value="Parchment">Parchment</asp:ListItem>
                                <asp:ListItem Value="Playbill">Playbill</asp:ListItem>
                                <asp:ListItem Value="PMingLiU">PMingLiU</asp:ListItem>
                                <asp:ListItem Value="Poor Richard">Poor Richard</asp:ListItem>
                                <asp:ListItem Value="Raavi">Raavi</asp:ListItem>
                                <asp:ListItem Value="Ravie">Ravie</asp:ListItem>
                                <asp:ListItem Value="Segoe Condensed">Segoe Condensed</asp:ListItem>
                                <asp:ListItem Value="Segoe UI">Segoe UI</asp:ListItem>
                                <asp:ListItem Value="Showcard Gothic">Showcard Gothic</asp:ListItem>
                                <asp:ListItem Value="Shruti">Shruti</asp:ListItem>
                                <asp:ListItem Value="Snap ITC">Snap ITC</asp:ListItem>
                                <asp:ListItem Value="Stencil">Stencil</asp:ListItem>
                                <asp:ListItem Value="Sylfaen">Sylfaen</asp:ListItem>
                                <asp:ListItem Value="Symbol">Symbol</asp:ListItem>
                                <asp:ListItem Value="Tahoma">Tahoma</asp:ListItem>
                                <asp:ListItem Value="Tempus Sans ITC">Tempus Sans ITC</asp:ListItem>
                                <asp:ListItem Value="Times New Roman">Times New Roman</asp:ListItem>
                                <asp:ListItem Value="Trebuchet MS">Trebuchet MS</asp:ListItem>
                                <asp:ListItem Value="Tunga">Tunga</asp:ListItem>
                                <asp:ListItem Value="Verdana">Verdana</asp:ListItem>
                                <asp:ListItem Value="Viner Hand ITC">Viner Hand ITC</asp:ListItem>
                                <asp:ListItem Value="Vivaldi">Vivaldi</asp:ListItem>
                                <asp:ListItem Value="Vladimir Script">Vladimir Script</asp:ListItem>
                                <asp:ListItem Value="Webdings">Webdings</asp:ListItem>
                                <asp:ListItem Value="Wide Latin">Wide Latin</asp:ListItem>
                                <asp:ListItem Value="Wingdings">Wingdings</asp:ListItem>
                                <asp:ListItem Value="Wingdings 2">Wingdings 2</asp:ListItem>
                                <asp:ListItem Value="Wingdings 3">Wingdings 3</asp:ListItem>
                                <asp:ListItem Value="仿宋_GB2312">仿宋_GB2312</asp:ListItem>
                                <asp:ListItem Value="华文中宋">华文中宋</asp:ListItem>
                                <asp:ListItem Value="华文仿宋">华文仿宋</asp:ListItem>
                                <asp:ListItem Value="华文宋体">华文宋体</asp:ListItem>
                                <asp:ListItem Value="华文彩云">华文彩云</asp:ListItem>
                                <asp:ListItem Value="华文新魏">华文新魏</asp:ListItem>
                                <asp:ListItem Value="华文楷体">华文楷体</asp:ListItem>
                                <asp:ListItem Value="华文琥珀">华文琥珀</asp:ListItem>
                                <asp:ListItem Value="华文细黑">华文细黑</asp:ListItem>
                                <asp:ListItem Value="华文行楷">华文行楷</asp:ListItem>
                                <asp:ListItem Value="华文隶书">华文隶书</asp:ListItem>
                                <asp:ListItem Value="宋体">宋体</asp:ListItem>
                                <asp:ListItem Value="幼圆">幼圆</asp:ListItem>
                                <asp:ListItem Value="微软雅黑">微软雅黑</asp:ListItem>
                                <asp:ListItem Value="新宋体">新宋体</asp:ListItem>
                                <asp:ListItem Value="方正姚体">方正姚体</asp:ListItem>
                                <asp:ListItem Value="方正舒体">方正舒体</asp:ListItem>
                                <asp:ListItem Value="楷体_GB2312">楷体_GB2312</asp:ListItem>
                                <asp:ListItem Value="隶书">隶书</asp:ListItem>
                                <asp:ListItem Value="黑体">黑体</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-row">
                            <div class="head">
                                文字大小：</div>
                            <SkyCES:TextBox CssClass="txt" Width="400px" ID="TextSize" runat="server" Text="14"
                                CanBeNull="必填" RequiredFieldType="数据校验" />
                        </div>
                        <div class="form-row">
                            <div class="head">
                                文字颜色：</div>
                            <div class="form-colpick" id="colpick" style="margin: 0; <%if(ShopConfig.ReadConfigInfo().TextColor.Trim()!=string.Empty){%>background-color: <%=ShopConfig.ReadConfigInfo().TextColor.Trim()%><%} %>">
                                拾色器
                                <asp:HiddenField ID="TextColor" runat="server" Value="#000" />
                            </div>
                        </div>
                    </div>
                    <div id="PhotoWaterDiv" style="display: none;">
                        <div class="form-row">
                            <div class="head">
                                水印图片：</div>
                              <a <%if (!string.IsNullOrEmpty(WaterPhoto.Text))
                    {%>href="<%=WaterPhoto.Text%>" target="_blank"<%} %>><img class="icon" height="50" src="<%=ShopCommon.ShowImage(ShopConfig.ReadConfigInfo().WaterPhoto) %>" /></a>                            
                        </div>
                        <div class="form-row">
                            <SkyCES:TextBox CssClass="txt" Width="400px" ID="WaterPhoto" runat="server" CanBeNull="必填" style="display:none;" />
                            <div class="form-upload">
                                <iframe src="UploadAdd.aspx?Control=WaterPhoto&FilePath=WaterPhoto&NeedMark=0&CurMaxWidth=200"
                                    width="400" height="30px" frameborder="0" allowtransparency="true" scrolling="no">
                                </iframe>
                            </div>
                            建议宽高小于150像素，当上传图片小于水印宽高时，将不会生成水印
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="product-main hidden">
                <div class="form-row">
                    <div class="head">
                        禁止注册用户名：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="ForbiddenName" runat="server" TextMode="MultiLine"
                        Height="80px" />
                    多个名字之间用"|"隔开
                </div>
                <div class="form-row">
                    <div class="head">
                        密码长度：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="PasswordMinLength" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" />
                    <span>到</span>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="PasswordMaxLength" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" />
                </div>
                <div class="form-row">
                    <div class="head">
                        用户名长度：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="UserNameMinLength" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" />
                    <span>到</span>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="UserNameMaxLength" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" />
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        注册验证方式：
                    </div>
                    <asp:RadioButtonList ID="RegisterType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">短信验证</asp:ListItem>
                        <asp:ListItem Value="2">邮件验证</asp:ListItem>                       
                    </asp:RadioButtonList>
                </div>
                <div class="form-row">
                    <div class="head">
                        注册验证：</div>
                    <asp:RadioButtonList ID="RegisterCheck" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Selected="True">短信验证</asp:ListItem>
                        <asp:ListItem Value="2">邮件验证</asp:ListItem>
                        <%--<asp:ListItem Value="3">人工审核</asp:ListItem>--%>
                    </asp:RadioButtonList>
                </div>
                <div class="form-row">
                    <div class="head">
                        注册协议：</div>
                    <SkyCES:TextBox CssClass="text" Width="400px" ID="Agreement" runat="server" TextMode="MultiLine"
                        Height="400px" />
                </div>
                <div class="form-row">
                    <div class="head">
                        找回密码时间：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="FindPasswordTimeRestrict" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" />
                    （小时） （时间限制:少于等于0表示不限制）
                </div>
                 <div class="form-row">
                    <div class="head">
                        邮箱绑定时间：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="BindEmailTime" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" onblur="checkBindEmailTime();"/>
                    （小时） （时间限制:最小为1）
                </div>
                <div class="form-row">
                    <div class="head">
                        留言时间限制：
                    </div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="CommentRestrictTime" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" />（秒）（少于等于0表示不限制）
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="product-main hidden">
                <div class="form-row">
                    <div class="head">
                        用户名：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="EmailUserName" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        密码：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="EmailPassword" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        服务器地址：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="EmailServer" runat="server" />
                </div>
                <div class="form-row">
                    <div class="head">
                        端口号：</div>
                    <SkyCES:TextBox CssClass="txt" Width="400px" ID="EmailServerPort" runat="server"
                        CanBeNull="必填" RequiredFieldType="数据校验" />
                </div>
                <div class="form-row">
                    <div class="head">
                        Email测试：</div>
                    <input id="ToEmail" type="text" class="txt" style="width: 200px" value="" placeholder="收件箱" />
                    <input class="form-cancel" type="button" value="测试" onclick="testSendEmail()" />
                </div>
                <div class="clear">
                </div>
            </div>

            <%--           <div class="product-main hidden">
               <div class="form-row">
                    <div class="head">
                        ICO宽X高：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="IcoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/>
                <span> X </span> 
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="IcoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/>px
                </div>
                <div class="form-row">
                    <div class="head">
                     Logo宽X高：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="LogoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/>
                <span> X </span> 
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="LogoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/>px
                </div>
                <div class="form-row">
                    <div class="head">
                     手机Logo宽X高：</div>
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="MobileLogoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/>
                <span> X </span> 
                    <SkyCES:TextBox CssClass="txt" Width="100px" ID="MobileLogoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验"/>px
                </div>
                 <div class="form-row">
                     <div class="head">
                         手机二维码宽X高：
                     </div>
                     <SkyCES:TextBox CssClass="txt" Width="100px" ID="MobilePhotoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />
                     <span> X </span>
                     <SkyCES:TextBox CssClass="txt" Width="100px" ID="MobilePhotoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                 </div>
                 <div class="form-row">
                     <div class="head">
                         微信二维码宽X高：
                     </div>
                     <SkyCES:TextBox CssClass="txt" Width="100px" ID="WeixinWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />
                     <span> X </span>
                     <SkyCES:TextBox CssClass="txt" Width="100px" ID="WeixinHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                 </div>
              <div class="form-row">
                     <div class="head" style="left: -45px;">
                         PC会员中心Logo宽X高：
                     </div>
                     <SkyCES:TextBox CssClass="txt" Width="100px" ID="UPLogoWidth" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />
                     <span>X </span>
                     <SkyCES:TextBox CssClass="txt" Width="100px" ID="UPLogoHeight" runat="server" Text="0" CanBeNull="必填" RequiredFieldType="数据校验" />px
                 </div>
                <div class="clear">
                </div>
            </div>--%>
            <div class="product-main hidden">
                <div class="form-row">
                    <div class="head">
                        支付订单：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().PayOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('PayOrder');" name="ctl00$ContentPlaceHolder$PayOrder" value="1" <%if (ShopConfig.ReadConfigInfo().PayOrder == 1)
                                                                                                                             { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('PayOrder');" class="item <%if (ShopConfig.ReadConfigInfo().PayOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$PayOrder" value="0" <%if (ShopConfig.ReadConfigInfo().PayOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="PayOrderEmail" <%if (ShopConfig.ReadConfigInfo().PayOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="PayOrderEmail" id="PayOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().PayOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="PayOrderMsg" <%if (ShopConfig.ReadConfigInfo().PayOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="PayOrderMsg" id="PayOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().PayOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        取消订单：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().CancleOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('CancleOrder');" name="ctl00$ContentPlaceHolder$CancleOrder" value="1" <%if (ShopConfig.ReadConfigInfo().CancleOrder == 1)
                                                                                                                                              { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('CancleOrder');" class="item <%if (ShopConfig.ReadConfigInfo().CancleOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$CancleOrder" value="0" <%if (ShopConfig.ReadConfigInfo().CancleOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="CancleOrderEmail" <%if (ShopConfig.ReadConfigInfo().CancleOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="CancleOrderEmail" id="CancleOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().CancleOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="CancleOrderMsg" <%if (ShopConfig.ReadConfigInfo().CancleOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="CancleOrderMsg" id="CancleOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().CancleOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        审核订单：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().CheckOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('CheckOrder');" name="ctl00$ContentPlaceHolder$CheckOrder" value="1" <%if (ShopConfig.ReadConfigInfo().CheckOrder == 1)
                                                                                                                                              { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('CheckOrder');" class="item <%if (ShopConfig.ReadConfigInfo().CheckOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$CheckOrder" value="0" <%if (ShopConfig.ReadConfigInfo().CheckOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="CheckOrderEmail" <%if (ShopConfig.ReadConfigInfo().CheckOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="CheckOrderEmail" id="CheckOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().CheckOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="CheckOrderMsg" <%if (ShopConfig.ReadConfigInfo().CheckOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="CheckOrderMsg" id="CheckOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().CheckOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        订单发货：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().SendOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('SendOrder');" name="ctl00$ContentPlaceHolder$SendOrder" value="1" <%if (ShopConfig.ReadConfigInfo().SendOrder == 1)
                                                                                                                                              { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('SendOrder');" class="item <%if (ShopConfig.ReadConfigInfo().SendOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$SendOrder" value="0" <%if (ShopConfig.ReadConfigInfo().SendOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="SendOrderEmail" <%if (ShopConfig.ReadConfigInfo().SendOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="SendOrderEmail" id="SendOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().SendOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="SendOrderMsg" <%if (ShopConfig.ReadConfigInfo().SendOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="SendOrderMsg" id="SendOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().SendOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        确认收货：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().ReceivedOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('ReceivedOrder');" name="ctl00$ContentPlaceHolder$ReceivedOrder" value="1" <%if (ShopConfig.ReadConfigInfo().ReceivedOrder == 1)
                                                                                                                                              { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('ReceivedOrder');" class="item <%if (ShopConfig.ReadConfigInfo().ReceivedOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$ReceivedOrder" value="0" <%if (ShopConfig.ReadConfigInfo().ReceivedOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="ReceivedOrderEmail" <%if (ShopConfig.ReadConfigInfo().ReceivedOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="ReceivedOrderEmail" id="ReceivedOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().ReceivedOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="ReceivedOrderMsg" <%if (ShopConfig.ReadConfigInfo().ReceivedOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="ReceivedOrderMsg" id="ReceivedOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().ReceivedOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        订单换货：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().ChangeOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('ChangeOrder');" name="ctl00$ContentPlaceHolder$ChangeOrder" value="1" <%if (ShopConfig.ReadConfigInfo().ChangeOrder == 1)
                                                                                                                                              { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('ChangeOrder');" class="item <%if (ShopConfig.ReadConfigInfo().ChangeOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$ChangeOrder" value="0" <%if (ShopConfig.ReadConfigInfo().ChangeOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="ChangeOrderEmail" <%if (ShopConfig.ReadConfigInfo().ChangeOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="ChangeOrderEmail" id="ChangeOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().ChangeOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="ChangeOrderMsg" <%if (ShopConfig.ReadConfigInfo().ChangeOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="ChangeOrderMsg" id="ChangeOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().ChangeOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
                <div class="form-row" style="display:none;">
                    <div class="head">
                        订单退货：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().ReturnOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('ReturnOrder');" name="ctl00$ContentPlaceHolder$ReturnOrder" value="1" <%if (ShopConfig.ReadConfigInfo().ReturnOrder == 1)
                                                                                                                                              { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('ReturnOrder');" class="item <%if (ShopConfig.ReadConfigInfo().ReturnOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$ReturnOrder" value="0" <%if (ShopConfig.ReadConfigInfo().ReturnOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="ReturnOrderEmail" <%if (ShopConfig.ReadConfigInfo().ReturnOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="ReturnOrderEmail" id="ReturnOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().ReturnOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="ReturnOrderMsg" <%if (ShopConfig.ReadConfigInfo().ReturnOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="ReturnOrderMsg" id="ReturnOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().ReturnOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        撤销订单操作：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().BackOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('BackOrder');" name="ctl00$ContentPlaceHolder$BackOrder" value="1" <%if (ShopConfig.ReadConfigInfo().BackOrder == 1)
                                                                                                                                              { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('BackOrder');" class="item <%if (ShopConfig.ReadConfigInfo().BackOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$BackOrder" value="0" <%if (ShopConfig.ReadConfigInfo().BackOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="BackOrderEmail" <%if (ShopConfig.ReadConfigInfo().BackOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="BackOrderEmail" id="BackOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().BackOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="BackOrderMsg" <%if (ShopConfig.ReadConfigInfo().BackOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="BackOrderMsg" id="BackOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().BackOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
                <div class="form-row">
                    <div class="head">
                        订单退款：
                    </div>
                    <div class="og-radio">
                        <label og-show="photoWidth" class="item  <%if (ShopConfig.ReadConfigInfo().RefundOrder == 1)
                                                                   { %>checked<%}%>">
                            通知<input type="radio" onclick="OrderNotice('RefundOrder');" name="ctl00$ContentPlaceHolder$RefundOrder" value="1" <%if (ShopConfig.ReadConfigInfo().RefundOrder == 1)
                                                                                                                                              { %>checked<%}%> /></label>
                        <label style="margin-right:10px" og-hide="photoWidth" onclick="OrderNotice('RefundOrder');" class="item <%if (ShopConfig.ReadConfigInfo().RefundOrder == 0)
                                                                                        { %>checked<%}%>">
                            不通知<input type="radio" name="ctl00$ContentPlaceHolder$RefundOrder" value="0" <%if (ShopConfig.ReadConfigInfo().RefundOrder == 0)
                                                                                                        { %>checked<%}%> /></label>

                        <label class="ig-checkbox"  for="RefundOrderEmail" <%if (ShopConfig.ReadConfigInfo().RefundOrder == 0)
                                                     { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="RefundOrderEmail" id="RefundOrderEmail" value="1" <%if (ShopConfig.ReadConfigInfo().RefundOrderEmail == 1)
                                                                                                       { %>checked<%} %> />邮件通知</label>
                        <label class="ig-checkbox"  for="RefundOrderMsg" <%if (ShopConfig.ReadConfigInfo().RefundOrder == 0)
                                                   { %>style="display:none;" <%} %>>
                            <input type="checkbox" name="RefundOrderMsg" id="RefundOrderMsg" value="1" <%if (ShopConfig.ReadConfigInfo().RefundOrderMsg == 1)
                                                                                                   { %>checked<%} %> />短信通知</label>
                    </div>
                </div>
            </div>

        </div>
        <div class="form-foot">
            <asp:Button CssClass="form-submit ease" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClientClick="return checksubmit();"
                OnClick="SubmitButton_Click" />
        </div>
    </div>
    <link rel="stylesheet" href="/Admin/static/css/plugin.css" />
    <script src="/Admin/static/js/colpick.js"></script>
      <script language="javascript" type="text/javascript">     
          $('#seoTitle').text(40 - $("#<%=Title.ClientID%>").val().length);
          $('#seoKeyword').text(50 - $("#<%=Keywords.ClientID%>").val().length);
          $('#seoSummary').text(100 - $("#<%=Description.ClientID%>").val().length);
          //操作订单是否通知（邮件、短信）
          function OrderNotice(action) {
              var _orderAction = $("input[name='ctl00$ContentPlaceHolder$"+action+"']:checked").val();
              //console.log(_orderAction);
              if (_orderAction == 1) {
                  $("#" + action + "Email").show().attr("checked","true").parent().show();
                  $("#" + action + "Msg").show().attr("checked", "true").parent().show();
              }
              else {
                  $("#" + action + "Email").hide().removeAttr("checked").parent().hide();
                  $("#" + action + "Msg").hide().removeAttr("checked").parent().hide();
              }
          }
          
             
              /*********************************发送email******************************/
              function testSendEmail() {
                  var emailUserName = o(globalIDPrefix + "EmailUserName").value;
                  var emailPassword = o(globalIDPrefix + "EmailPassword").value;
                  var emailServer = o(globalIDPrefix + "EmailServer").value;
                  var emailServerPort = o(globalIDPrefix + "EmailServerPort").value;
                  var toEmail = o("ToEmail").value;
                  var url = "Ajax.aspx?Action=TestSendEmail&EmailUserName=" + emailUserName + "&EmailPassword=" + emailPassword + "&EmailServer=" + emailServer + "&EmailServerPort=" + emailServerPort + "&ToEmail=" + toEmail;
                  Ajax.requestURL(url, dealTestSendEmail);
              }
              function dealTestSendEmail(data) {
                  if (data == "ok") {
                      layer.msg('发送成功');
                  }
                  else {
                      layer.msg("发送失败");
                  }
              }
          /*********************************发送email end******************************/
              function selectWaterType() {
                  var waterType = $("input[name='ctl00$ContentPlaceHolder$WaterType']:checked").val();
                  switch (waterType) {
                      case "1":
                          $("#WaterPossition").fadeOut();
                          $("#TextWaterDiv").fadeOut();
                          $("#PhotoWaterDiv").fadeOut();
                          break;
                      case "2":
                          $("#WaterPossition").fadeIn();
                          $("#TextWaterDiv").fadeIn();
                          $("#PhotoWaterDiv").fadeOut();
                          break;
                      case "3":
                          $("#WaterPossition").fadeIn();
                          $("#TextWaterDiv").fadeOut();
                          $("#PhotoWaterDiv").fadeIn();
                          break;
                      default:
                          break;
                  }
              }
              selectWaterType();

              $("input[name='ctl00$ContentPlaceHolder$WaterType']").bind("click", function () {
                  selectWaterType();
              });
          //整站图片压缩宽度
              function checkImgWidth() {
                  var imgWidth = $("#<%= AllImageWidth.ClientID%>").val();
                  if (imgWidth=="" ||(imgWidth != "" && parseInt(imgWidth) < 600)) $("#<%= AllImageWidth.ClientID%>").val("600");
              }

              function checkBindEmailTime() {
                  if ($("#<%= BindEmailTime.ClientID%>").val()=="" || parseInt($("#<%= BindEmailTime.ClientID%>").val()) < 1) { $("#<%= BindEmailTime.ClientID%>").val("1") }
              }

          //是否启用积分抵现
          var enablepointpay = $("input[name='ctl00$ContentPlaceHolder$EnablePointPay']:checked").val();
        $(function(){
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

              //是否启用整站图片压缩
              var enablimagenail = $("input[name='ctl00$ContentPlaceHolder$AllImageIsNail']:checked").val();
              if (enablimagenail == 1  && !reg2.test($("#<%=AllImageWidth.ClientID%>").val()) && parseInt($("#<%= AllImageWidth.ClientID%>").val()<600))
              {
                  alert("整站图片压缩宽度必填且不小于600");                
              }
              else
              {
                  $("#<%= AllImageWidth.ClientID%>").val("600");
              }

          }
              //$("input[name='ctl00$ContentPlaceHolder$WaterType']").click(function(){
              //    var waterType=$(this).val();   
              //    switch(waterType){
              //        case "1":
              //            $("#WaterPossition").fadeOut();
              //            $("#TextWaterDiv").fadeOut();
              //            $("#PhotoWaterDiv").fadeOut();
              //            break;
              //        case "2":
              //            $("#WaterPossition").fadeIn();
              //            $("#TextWaterDiv").fadeIn();
              //            $("#PhotoWaterDiv").fadeOut();
              //            break;
              //        case "3":
              //            $("#WaterPossition").fadeIn();
              //            $("#TextWaterDiv").fadeOut();
              //            $("#PhotoWaterDiv").fadeIn();
              //            break;
              //        default:
              //            break;
              //    }  
              //})  

    </script>
</asp:Content>
