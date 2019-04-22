<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttributeRecordAjax.aspx.cs"  Inherits="JWShop.Web.Admin.AttributeRecordAjax" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>
<% foreach (ProductTypeAttributeInfo attribute in attributeList)
   {%>
<div class="form-checkbox">
                    	<div class="head"><%=attribute.Name%>：</div>    
        <%
            switch (attribute.InputType)
            {
                case (int)InputType.Text:
        %>
        <input name="<%=attribute.Id %>Value" class="input" type="text" value="<%=attribute.AttributeRecord.Value %>"  style="width:200px"/>
        <%
            break;
            case (int)InputType.KeyWord:%>
        <input name="<%=attribute.Id %>Value" class="input" type="text" value="<%=attribute.AttributeRecord.Value %>" style="width:200px" id="<%=attribute.Id %>Value" />
        <%foreach (string inputValue in attribute.InputValue.Split(';')){%><span onclick="selectKeyword(<%=attribute.Id %>,'<%=inputValue%>')" style="cursor:pointer"><%=inputValue%></span> <%} %>
        <%
            break;
        case (int)InputType.Select:
        %>
        <select name="<%=attribute.Id %>Value" style="width: 200px;">
            <%foreach (string inputValue in attribute.InputValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
              {%>
            <option value="<%=inputValue%>" <%if(attribute.AttributeRecord.Value==inputValue){%>selected="selected"<%} %>>
                <%=inputValue%></option>
            <%} %>
        </select>
        <%    
            break;
            case (int)InputType.Textarea:%>
        <textarea name="<%=attribute.Id %>Value" class="input" style="width:400px; height:50px"><%=attribute.AttributeRecord.Value%></textarea>
        <%
            break;
            case (int)InputType.Radio:%>
            <select name="<%=attribute.Id %>Value" style="width: 200px;" class="select">
                <%foreach (string inputValue in attribute.InputValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                  {%>
                <option value="<%=inputValue%>" <%if(attribute.AttributeRecord.Value==inputValue){%>selected="selected"<%} %>>
                    <%=inputValue%></option>
                <%} %>
            </select>             
        <%
            break;
            case (int)InputType.CheckBox:%> 
            <div class="checkList" id='attr<%=attribute.Id%>'>
            <label class="ig-checkbox"><input type="checkbox" name="chooseAll<%=attribute.Id%>" _atid="<%=attribute.Id%>" onclick="chooseAllAttr(this)" /><span class="red">全选</span></label>
                            <%foreach (string inputValue in attribute.InputValue.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries))
                               {%>
                        <label <%if((";"+attribute.AttributeRecord.Value+";").IndexOf(";"+inputValue+";")>=0){%> class="ig-checkbox checked"<%}else{ %>class="ig-checkbox"<%} %>><input name="<%=attribute.Id %>Value" type="checkbox" <%if((";"+attribute.AttributeRecord.Value+";").IndexOf(";"+inputValue+";")>=0){%> checked="checked"<%} %>  value="<%=inputValue %>" /><%=inputValue %></label> &nbsp;&nbsp;
                        <%} %>                       
                        
            </div>          
        <%
            break;
            default:
            break;
        }%>
</div>       
<%    }    %>