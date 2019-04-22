<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="EditPhoto.aspx.cs" Inherits="SocoShop.Web.Admin.EditPhoto" %>
<html>
    <head>
        <script src="/Admin/Js/jquery-1.7.2.min.js"></script>
        <script src="/Admin/js/jquery.Jcrop.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/Admin/js/jquery.Jcrop.css" type="text/css" />        
    <script type="text/javascript">

        jQuery(function ($) {

            // Create variables (in this scope) to hold the API and image size
            var jcrop_api, boundx, boundy;

            $('#target').Jcrop({
                onChange: updatePreview,
                onSelect: updatePreview,
                setSelect: [250, 250, 250, 350],
                minSize: [200, 200],
                maxSize: [750, 750],
                aspectRatio: 1
            }, function () {
                // Use the API to get the real image size
                var bounds = this.getBounds();
                boundx = bounds[0];
                boundy = bounds[1];
                // Store the API in the jcrop_api variable
                jcrop_api = this;
            });

            function updatePreview(c) {
                $('#x1').val(c.x);
                $('#y1').val(c.y);
                $('#w').val(c.w);
                $('#h').val(c.h);

                if (parseInt(c.w) > 0) {
                    var rx = 160 / c.w;
                    var ry = 120 / c.h;

                    $('#preview').css({
                        width: Math.round(rx * boundx) + 'px',
                        height: Math.round(ry * boundy) + 'px',
                        marginLeft: '-' + Math.round(rx * c.x) + 'px',
                        marginTop: '-' + Math.round(ry * c.y) + 'px'
                    });

                }
            };

        });

        function showvalue() {
            //alert($('#w').val() + $('#h').val());
            return true;
        }

  </script></head>
    <body>
        <form id="form1" runat="server">
            <div class="wrapper" id="wrapper">

        <div class="container ease" id="container">
            <div class="product-container">
                <div id="outer" style=" margin:0 auto;">
                  <div class="jcExample">
                  <div class="article">
                    <table>
                      <tr>
                        <td>
          
                            <asp:Image runat="server" ID="target" alt="Flowers" />
                        </td>
                        <td>
                          <div style="width:160px;height:120px;overflow:hidden;">
            
                                <asp:Image runat="server"  ID="preview" alt="Preview" class="jcrop-preview" /><asp:HiddenField
                            ID="himg" runat="server" />
                          </div>
                          <div style=" padding-top:20px;"><asp:Button ID="Button1" runat="server" Text="确认修改" 
                            onclick="Button1_Click" OnClientClick="return showvalue()" /></div>
                        </td>
                      </tr>
                    </table>   
   

                    <div>
                      <input type="hidden" id="x1" name="x1" />
                      <input type="hidden" id="y1" name="y1" />
                      <input type="hidden" id="w" name="w" />
                      <input type="hidden" id="h" name="h" />
      
                      </div>

                  </div>
                  </div>
                  </div>
                </div>
        </div>
    </div>
        </form>
    </body>
</html>    
    
