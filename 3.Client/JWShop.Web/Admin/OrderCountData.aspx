<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderCountData.aspx.cs" Inherits="JWShop.Web.Admin.OrderCountData" %>
<%@ Import Namespace="JWShop.Common" %>
<chart showFCMenuItem='0' lineThickness='2' showValues='0' anchorRadius='4' divLineAlpha='20' divLineColor='6DA7D5' divLineIsDashed='1' showAlternateHGridColor='1' alternateHGridAlpha='5' alternateHGridColor='6FA4D8' shadowAlpha='40' labelStep='2' numvdivlines='25' showAlternateVGridColor='1' chartsshowShadow='1' chartRightMargin='20' chartTopMargin='15' chartLeftMargin='0' chartBottomMargin='3' bgColor='FFFFFF' canvasBorderThickness='1' showBorder='0' legendBorderAlpha='0' bgAngle='360' showlegend='1' borderColor='FFFFFF' toolTipBorderColor='cccc99' canvasPadding='0' toolTipBgColor='ffffcc' legendShadow='0' baseFontSize='12' canvasBorderAlpha='20' outCnvbaseFontSize='10' outCnvbaseFontColor='020202' numberScaleValue='10000,1,1,1000' formatNumberScale='1' palette='2' numberScaleUnit=' , ,万,千万' lineColor='DADADC'  >
    <%if(month==int.MinValue){ %>
    <categories  >        
            <%for(int i=1;i<=12;i++){ %><category label='<%=i %>'/><%} %>
    </categories> 
    <dataset  color='#0033CC'   >   
        <%for(int i=1;i<=12;i++){ %>     
        <set  value='<%=ShopCommon.ReadValue<int,int>(orderCountDic,i) %>' toolText='month:<%=i %> ; count:<%=ShopCommon.ReadValue<int,int>(orderCountDic,i) %>'  />
        <%} %>
    </dataset>      
    <%}else{ %>
     <categories  >        
            <%for(int i=1;i<=days;i++){ %><category label='<%=i %>'/><%} %>
    </categories> 
    <dataset  color='#FF6600'   >   
        <%for(int i=1;i<=days;i++){ %>     
        <set  value='<%=ShopCommon.ReadValue<int,int>(orderCountDic,i) %>' toolText='day:<%=i %> ; count:<%=ShopCommon.ReadValue<int,int>(orderCountDic,i) %>'  />
        <%} %>
    </dataset>
    <%} %>      
    <styles>
        <definition>
            <style name='CaptionFont' type='font' size='12'  />
            <style name='myLegendFont' type='font' size='11'  />
        </definition>
        <application>
            <apply toObject='CAPTION' styles='CaptionFont'  />
            <apply toObject='SUBCAPTION' styles='CaptionFont'  />
            <apply toObject='Legend' styles='myLegendFont'  />
        </application>
    </styles>
</chart>
