
function PreviewDocument(param_documentid, param_templatename) {
    
    jQuery("#iframePdfDocument").attr('src', sRootPath + 'temp/document' + param_documentid + '.pdf?v=' + new Date().getTime() + '#toolbar=0&statusbar=0&navpanes=0&view=fit&zoom=200');
    
    $('.dialogPreviewBox').attr("title", param_templatename);
    $(".dialogPreviewBox").dialog("option", "title", param_templatename); 

    jQuery('.dialogPreviewBox').dialog({
        autoOpen: true,
        closeOnEscape: false,
        //position: [300, 50],
        width: 600, //$(window).width()*0.48
        height: 750,//$(window).height()*0.83,
        modal: true,
        draggable: false,
        resizable: false
    });
}

function CancelPreviewDialog() {

    jQuery(".dialogPreviewBox").dialog("close");
    jQuery("#iframePdfDocument").removeAttr("src");

    var templatenumber =  $('#templatenumber').val();
    var templatebrand = $('#templatebrand').val();
    var bPXBrand;
    if (templatebrand == 'xeroxbrand')
        bPXBrand = false;
    else if (templatebrand == 'partnerbrand')
        bPXBrand = true;

    //Update templateButton details
    if (templatenumber != undefined || templatebrand != undefined) {
        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateTemplateButton",
            cache: false,
            data: JSON.stringify({ iTemplateID: templatenumber, sChannelPartnerLoginID: sCLoginId, bCustomize: true, bPartnerBrand: bPXBrand }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {
                    BuildTemplateButton(sCLoginId);
                    
                    //default selected product
                    $("#lblselecteditem").text($("#productoptions").find("div[class=\"product-category\"]").find("span[nodeid=\"" + response.TemplateNodeID[1] + "\"]").text());
                    $("#productoptions").find("div[class=\"product-category\"]").find("span[class=\"producttext enableitemtext\"]").removeClass('enableitemtext');
                    $("#productoptions").find("div[class=\"product-category\"]").find("span[nodeid=\"" + response.TemplateNodeID[1] + "\"]").addClass('enableitemtext');

                    LoadMenuTreeTemplates(response.TemplateNodeID[1]);
                }
            }
        });
    }
}
