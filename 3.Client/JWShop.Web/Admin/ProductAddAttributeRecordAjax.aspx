<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductAddAttributeRecordAjax.aspx.cs"  Inherits="JWShop.Web.Admin.ProductAddAttributeRecordAjax" %>
<%@ Import Namespace="JWShop.Common" %>
<%@ Import Namespace="JWShop.Entity" %>

<% foreach (var attribute in attributeList){%>
    <% var _index = attributeList.IndexOf(attribute); %>
    <ul>
        <li class="left" style="width: 80px;" id="att<%=_index %>name"><%=attribute.Name%>：</li>
        <li class="right">
        <%
            switch (attribute.InputType)
            {
                case (int)InputType.Text:
                    %>
                        <input name="att<%=_index %>" class="input" type="text" value="<%=attribute.AttributeRecord.Value %>"  style="width:200px"/>
                    <%
                    break;
                case (int)InputType.KeyWord:%>
                    <input name="att<%=_index %>" class="input" type="text" value="<%=attribute.AttributeRecord.Value %>" style="width:200px" id="<%=attribute.Id %>Value" />
                    <%foreach (string inputValue in attribute.InputValue.Split(';')){%>
                        <span onclick="selectKeyword(<%=attribute.Id %>,'<%=inputValue%>')" style="cursor:pointer">
                            <%=inputValue%>
                        </span>
                    <%} %>
                    <%
                    break;
                case (int)InputType.Select:
                    %>
                    <select name="att<%=_index %>" style="width: 200px;">
                        <%foreach (string inputValue in attribute.InputValue.Split(';'))
                        {%>
                            <option value="<%=inputValue%>" <%if(attribute.AttributeRecord.Value==inputValue){%>selected="selected"<%} %>>
                                <%=inputValue%>
                            </option>
                        <%} %>
                    </select>
                    <%    
                    break;
                case (int)InputType.Textarea:%>
                    <textarea name="att<%=_index %>" class="input" style="width:400px; height:50px"><%=attribute.AttributeRecord.Value%></textarea>
                    <%
                        break;
                case (int)InputType.Radio:%>
                    <select name="att<%=_index %>" style="width: 200px;" class="select">
                        <option value="">请选择</option>
                        <%foreach (string inputValue in attribute.InputValue.Split(';'))
                        {%>
                            <option value="<%=inputValue%>" <%if(attribute.AttributeRecord.Value==inputValue){%>selected="selected"<%} %>>
                                <%=inputValue%>
                            </option>
                        <%} %>
                    </select>
                <%
                    break;
                case (int)InputType.CheckBox:%>
                    <% var _attrInputValue = attribute.InputValue.Split(';'); %> 
                    <%foreach (string inputValue in _attrInputValue)
                    {%>
                        <input name="att<%=_index %>" type="checkbox" <%if((";"+attribute.AttributeRecord.Value+";").IndexOf(";"+inputValue+";")>-1){%> checked="checked"<%} %>  value="<%=inputValue %>" id="cbattr<%=attribute.Id %><%=_attrInputValue.ToList().IndexOf(inputValue) %>" /><label for="cbattr<%=attribute.Id %><%=_attrInputValue.ToList().IndexOf(inputValue) %>"><%=inputValue %></label>
                    <%} %>
                <%
                    break;
                    default:
                    break;
                }%>
        </li>
    </ul>       
<%}%>