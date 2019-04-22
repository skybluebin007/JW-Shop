
$(function () {
    /*属性列表*/
    $('#btnAttribute').click(function () {
        var len = document.getElementById("tbodyAttribute").rows.length;
        $("#genAttTr").tmpl({ 'row': len, 'name': '', 'way': '', 'value': '' }).appendTo('#tbodyAttribute');
    });

    $("input[name=attribute_cbway]").live("click", function () {
        var that = $(this).parent().parent();
        $("#attribute_way" + $(that).attr("rowNum")).val(this.checked ? "6" : "5");
    });

    /*规格列表*/
    $('#btnStandard').click(function () {
        var len = document.getElementById("tbodyStandard").rows.length;
        $("#genStdTr").tmpl({ 'row': len, 'name': '', 'value': '' }).appendTo('#tbodyStandard');
    });

    /*submit*/
    $("#btnSubmit").click(function () {
        $.ajax({
            url: 'ProductTypeAdd.aspx?Action=Submit',
            type: 'POST',
            data: $("#aspnetForm").serialize(),
            success: function (result) {
                var arr = result.split('|');

                if (arr[0] == "ok") {
                    alert(arr[2]);
                    self.location.reload();
                }
                else {
                    alertMessage(arr[2], 500);
                    var trd = arr[1];
                    if (trd != '') {
                        $("table input[type=text]").css("border", "1px solid #D3D3D3");

                        var trds = trd.split('-');
                        $("#" + trds[1] + trds[0]).css("border", "1px solid #FF3C3C");
                    }
                }
            }
        });
    });
});

function delRow(that, which) {
    $(that).parent().parent().remove();

    if (which == "att") {
        $("#tbodyAttribute tr").each(function (i) {
            $(this).find("input[name=attribute_name]").attr("id", "attribute_name" + i);
            $(this).find("input[name=attribute_way]").attr("id", "attribute_way" + i);
            $(this).find("input[name=attribute_value]").attr("id", "attribute_value" + i);
        });
    }
    if (which == "std") {
        $("#tbodyStandard tr").each(function (i) {
            $(this).find("input[name=standard_name]").attr("id", "standard_name" + i);
            $(this).find("input[name=standard_value]").attr("id", "standard_value" + i);
        });
    }
}