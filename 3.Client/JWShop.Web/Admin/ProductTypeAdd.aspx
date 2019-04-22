<%@ Page Language="C#" MasterPageFile="MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductTypeAdd.aspx.cs" Inherits="JWShop.Web.Admin.ProductTypeAdd" %>
<%@ Register Namespace="SkyCES.EntLib" Assembly="SkyCES.EntLib" TagPrefix="SkyCES"%>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<%@ Import Namespace="JWShop.Business" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <script src="Js/ProductTypeAdd.js"></script>
    <div class="container ease" id="container">
    	<div class="tab-title" style="display:none;">
        	<span class="cur">添加类型名</span>
            <span>添加属性</span>
            <span>关联规格</span>
        </div>
        <div id="Type-add">
            <div class="product-container">

                    <!--<div class="product-tip">01</div>-->
                    <div class="product-head"><h2><%if( RequestHelper.GetQueryString<int>("ID")>0){ %>修改商品类型<%}else{ %>添加商品类型<%} %></h2></div>
                    <div class="product-row">
                    <div class="product-main">
                        <div class="form-row" style="margin-bottom: 20px;">
                            <div class="head">类型名称：</div>
                            <SkyCES:TextBox ID="Name" CssClass="txt" placeholder="长度限制1-30个字符之间" MaxLength="30" runat="server" Width="250px"  CanBeNull="必填"/>                        
                        </div>
                        <div class="clear"></div>
                        <!--<div class="form-tag">商品类型名称不能为空，长度限制在1-30个字符之间</div>-->
                        <div class="clear"></div>
                        <div class="form-checkbox">
                            <div class="head">
                            	<label>关联品牌：</label>
                                <ul id="brandGroup">
                                    <%for (char i = 'A'; i <= 'Z'; i++){%>
                                    <li <%if (GetBrandCountByFirstLetter(i.ToString().ToLower()) > 0){%>
                                        <%if(i=='A'){ %>class="on"<%} %><%}else{ %>class="no"<%} %>><%=i.ToString()%></li>                                        
                                      <%}%>
                                  <li <%if (GetBrandCountByFirstLetter("+")<= 0){%>
                                       class="no"<%} %>>+</li>
  <%--                                	<li class="on">A</li>
                                   </li><li class="no">+</li>--%>
                                </ul>
                            </div>
                            <div class="label_box" id="brandContainer">
                            <%List<ProductBrandInfo> proBrandList = ProductBrandBLL.ReadList().Where(k=>k.Spelling.ToLower().StartsWith("a")).OrderBy(k=>k.Spelling).ToList();
                              if (proBrandList.Count > 0)
                              { 
                              foreach(ProductBrandInfo proBrand in proBrandList){
                                  if (!string.IsNullOrEmpty(pageAttr.BrandIds))
                                  {
                                      string brands = ";" + pageAttr.BrandIds + ";";
                                      
                                      if(brands.IndexOf(";"+proBrand.Id.ToString()+";")>=0)
                                      {%>
                                        <label class="ig-checkbox checked"><input type="checkbox" checked="checked" name="proBrand" value="<%=proBrand.Id%>" onclick="chooseBrand(this,'<%=proBrand.Name%>')"/><%=proBrand.Name%></label>
                                      <%
                                        }
                                      else
                                      {%>
                                      <label class="ig-checkbox" ><input type="checkbox" name="proBrand" value="<%=proBrand.Id%>" onclick="chooseBrand(this,'<%=proBrand.Name%>')"/><%=proBrand.Name%></label>
                                      <%}
                                    }
                                    else{%>
                            <label class="ig-checkbox"><input type="checkbox" name="proBrand" value="<%=proBrand.Id%>" onclick="chooseBrand(this,'<%=proBrand.Name%>    ')"/><%=proBrand.Name%></label>
                                    <%} 
                                      }}else{%>没有符合条件的数据<%} %>
                             </div>
                             <div class="label_shose">
                             	<label>已选择：</label>
                                 <input type="hidden" name="choosedBrnadIds" id="choosedBrnadIds" value="<%=pageAttr.BrandIds%>" />
                                <ul id="choosedBrand">
                                  <% if (!string.IsNullOrEmpty(pageAttr.BrandIds))
                                     {
                                         foreach (string bid in pageAttr.BrandIds.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                         {%>
                                	<li><%=ProductBrandBLL.Read(Convert.ToInt32(bid)).Name %></li>
                                <%}
                                     }else{ %>
                                    <li id="nobrand">暂无</li>
                                    <%} %>
                                   
                                </ul>
                             </div>
                        </div>
                        <div class="clear"></div>
                    </div>
                    

                    <!--<div class="product-tip">02</div>-->
                    <div class="form-row" style="display:none;">
                    	<div class="head">属性：</div>
                        <div id="btn">添加</div>
                    </div>
                    <div class="product-main"  style="display:none;">
                        <div class="type-ads type-ads1">
                        	<div class="tc_close">X</div>
                            <div class="title">新建属性</div>
                            <div class="part1">
                            	<div class="box_main">
                                    <input type="hidden" id="hattrid" name="hattrid" />
                                    <div class="head2">属性名</div>
                                    <SkyCES:TextBox ID="AttrName" MaxLength="20" CssClass="txt" runat="server"/>
                                </div>  
                                <p>填写属性的名称</p>                                         
                            </div>
                            <div class="clear"></div>
                            <div class="part1">
                            	<div class="box_main">
                                <div class="head2">属性值</div>
                                <SkyCES:TextBox ID="InputValue" CssClass="text" runat="server" TextMode="MultiLine" />
                                </div>
                                <p>多个属性值可用";"(英文分号)隔开，每个值的字符数字最多15个字符</p>                                        
                            </div>
                            <div class="clear"></div>
                            <div class="form-checkbox">
                            	<div class="part1" style="padding:0 30px">
                                <div class="box_main">
                                <div class="head2">是否多选</div>
                                <label class="ig-checkbox"><input type="checkbox" name="InputType" id="InputType" value="6" />是</label>
                                </div>
                                </div>                                    
                            </div>
                            <div class="clear"></div>                            
                            <input type="button" onclick="addAttribute()" value="确定" class="submit ease" />
                            <div id="box_type_close">取消</div>
                        </div>                    
                        <table cellpadding="0" cellspacing="0" border="0" class="form-table" width="100%">
                            <thead>
                                <tr>
                                    <td width="150" height="40">属性名称</td>
                                    <td width="100">录入方式</td>
                                    <td width="">属性值</td>
                                    <td width="200">操作</td>
                                </tr>
                            </thead>
                            <tbody id="attributeBox">                            
                                <asp:Repeater ID="Repeater1" runat="server">
                                    <ItemTemplate>
                                        <tr id="tr<%#Container.ItemIndex+1%>">
                                            <td height="60"><input type="text" name="NameList" placeholder="" value="<%#Eval("Name") %>" class="ig-txt" maxlength="20" /></td>
                                            <td ><%#EnumHelper.ReadEnumChineseName<InputType>(Convert.ToInt32(Eval("InputType"))) %><input type="hidden" name="TypeList" value="<%#Eval("InputType") %>"></td>
                                            <td >
                                                <dl class="ig-group-del clearfix">
                                                    <script>document.write(getSectionList('<%#Eval("InputValue")%>'));</script>
                                                    <input type="hidden" name="ValueList" value="<%#Eval("InputValue").ToString().Replace(';','|') %>">
                                                </dl>
                                            </td>                                        
                                            <td >
                                                <input type="hidden" name="IdList" value="<%#Eval("id") %>" />
                                                <a href="javascript:editAttr(<%#Container.ItemIndex+1%>,'<%#Eval("Name") %>',<%#Eval("InputType")%>,'<%#Eval("InputValue")%>')" class="ig-colink" title="编辑">编辑</a>
                                                <a href="javascript:void(0)" onclick="deleteAttr(this)" atid="<%#Eval("id") %>" id="aa" class="ig-colink" title="删除">删除</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>


                    <!--<div class="product-tip">03</div>-->
                    <div class="form-row" style="margin-top: 30px;">
                    	<div class="head">关联规格：</div>
                        <div id="btn2">添加</div>
                    </div>
                    <div class="product-main">
                    	<div class="type-ads type-ads2">
                    		<div class="tc_close">X</div>
                            <div class="title">新建规格</div>
                            <input type="hidden" id="hstandid" name="hstandid" />
                            <div class="part1">
                            	<div class="box_main">
                                    <input type="hidden" id="hattrid" name="hattrid" />
                                    <div class="head2">规格名</div>
                                   <SkyCES:TextBox ID="StandardName" MaxLength="30" CssClass="txt" runat="server"  />
                                </div>  
                                <p>填写规格的名称</p>                                         
                            </div>
                            <div class="part1">
                            	<div class="box_main">
                                <div class="head2">规格值</div>
                               <SkyCES:TextBox ID="StandardValue" CssClass="text text-600" runat="server"  TextMode="MultiLine" />  
                                </div>
                                <p>多个规格值可用";"(英文分号)隔开，每个值的字符数字最多15个字符</p>                                        
                            </div>
                          
                            <div class="clear"></div>                            
                            <input type="button" onclick="addStandard()" value="确定" class="submit ease" />
                            <div id="box_type_close2">取消</div>
                        </div>
                        <table cellpadding="0" cellspacing="0" border="0" class="form-table" width="100%">
                            <thead>
                                <tr>
                                    <td width="150" height="40">规格名称</td>
                                    <td width="">规格值</td>                                
                                    <td width="200">操作</td>
                                </tr>
                            </thead>
                            <tbody id="standardBox">
                                <asp:Repeater ID="Repeater2" runat="server">
                                    <ItemTemplate>
                                        <tr id="trs<%#Container.ItemIndex+1%>">
                                            <td height="60"><input type="text" name="SNameList" placeholder="" value="<%#Eval("Name") %>" class="ig-txt" maxlength="20" /></td>                                        
                                            <td >
                                                <dl class="ig-group-del clearfix">
                                                    <script>document.write(getSectionList('<%#Eval("ValueList")%>'));</script>
                                                    <input type="hidden" name="SValueList" value="<%#Eval("ValueList").ToString().Replace(';','|') %>">
                                                </dl>
                                            </td>                                        
                                            <td >
                                                <input type="hidden" name="SIdList" value="<%#Eval("id") %>" />
                                                <a href="javascript:editStand(<%#Container.ItemIndex+1%>,'<%#Eval("Name") %>',1,'<%#Eval("ValueList")%>')" class="ig-colink" title="编辑">编辑</a>
                                                <a href="javascript:void(0)" onclick="deleteAttr(this)" skuid="<%#Eval("id") %>" id="aa" class="ig-colink" title="删除">删除</a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <div class="clear"></div> 
                    <div class="form-foot" style="margin-left:90px; top: 0; margin-top: 30px;">
			            <asp:Button CssClass="form-submit ease" style=" margin:0;" ID="SubmitButton" Text=" 确 定 " runat="server"  OnClick="SubmitButton_Click" />
			        </div>
				</div>
				
            </div>
        </div>
        
    </div>
	<script>
    $("#btn").click(function () {
        $(".type-ads1").show();
        $(".type-ads1 .title").text("新建属性");
    });
    $("#box_type_close").click(function () {
        $(".type-ads1").hide();
        $("#ctl00_ContentPlaceHolder_AttrName").val("");
        $("#InputType").removeAttr("checked");
        $("#InputType").parent().removeClass("checked");
        $("#ctl00_ContentPlaceHolder_InputValue").val("");
        $("#hattrid").val("");
    });
    $(".type-ads1 .tc_close").click(function () {
        $(".type-ads1").hide();
        $("#ctl00_ContentPlaceHolder_AttrName").val("");
        $("#InputType").removeAttr("checked");
        $("#InputType").parent().removeClass("checked");
        $("#ctl00_ContentPlaceHolder_InputValue").val("");
        $("#hattrid").val("");
    });
    
    
    $("#btn2").click(function () {
        $(".type-ads2").show();
        $(".type-ads2 .title").text("新建规格");
    });
    $("#box_type_close2").click(function () {
        $(".type-ads2").hide();
        $("#ctl00_ContentPlaceHolder_StandardName").val("");
        $("#ctl00_ContentPlaceHolder_StandardValue").val("");
        $("#hstandid").val("");
    });
    $(".type-ads2 .tc_close").click(function () {
        $(".type-ads2").hide();
        $("#ctl00_ContentPlaceHolder_StandardName").val("");
        $("#ctl00_ContentPlaceHolder_StandardValue").val("");
        $("#hstandid").val("");
    });
    if(!$("#attributeBox tr").length){
    	$("#attributeBox").parent("table").hide();
    }
    if(!$("#standardBox tr").length){
    	$("#standardBox").parent("table").hide();
    }
	    //品牌分组筛选
    $("#brandGroup li").click(function () {
        //console.log($(this).html());
        if($(this).attr("class")=="no"){$(this).siblings().removeClass("on");}
        else
        {
            $(this).addClass("on").siblings().removeClass("on");
        }
        $("#brandContainer").html("记载中，请稍后");
        $.ajax({
            url:"?Action=GetBrandsByFirstLetter<%=(!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.ToString())?"&"+HttpContext.Current.Request.QueryString.ToString():"")%>",
            type:"get",
            data:{FirstLetter:$(this).html()},
            dataType:"json",
            success:function(data){
                if(data.flag==true){
                    if(data.content.length>0){$("#brandContainer").html(data.content);}
                    else{$("#brandContainer").html("没有符合条件的数据");}
                }
                else{
                    alertMessage("系统忙，请稍后重试");
                }
            },
            error:function(){alertMessage("系统忙，请稍后重试");},
        })
    })
        //选择（取消选择）品牌
    function chooseBrand(obj,_bname) {        
        $("#nobrand").remove();
        if($(obj).is(":checked")) {  $("#choosedBrand").append("<li>" + _bname + "</li>");
            $(obj).parent().addClass("checked");
            $("#choosedBrnadIds").val($("#choosedBrnadIds").val()+";"+$(obj).val()+";");
            $("#choosedBrnadIds").val($("#choosedBrnadIds").val().replace(";;",";"))
        }
        else{ 
            $("#choosedBrand").html( $("#choosedBrand").html().replace("<li>" + _bname + "</li>",""));
            $(obj).parent().removeClass("checked");
            $("#choosedBrnadIds").val($("#choosedBrnadIds").val().replace($(obj).val()+";","").replace(";;",";"))
        }       
        if( $("#choosedBrand li").length<=0){
            $("#choosedBrand").append(" <li id=\"nobrand\">暂无</li>");
        }
    }
    </script>    
</asp:Content>


