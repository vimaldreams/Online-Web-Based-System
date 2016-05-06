//Member Variables
var templatebutton;
var globalflag;
var selectedproduct;

jQuery(document).ready(function () {

    //Check Address, Custom, Logo Details
    if (sNoDetailsFlag == 'Error') {
        $('#litNoDetailMessage').html(sNoDetailMessage).text();
    }

    //Get templateButton details
    BuildTemplateButton(sCLoginId);
   
    $("#productoptions").find("div[class=\"product-category\"] > span").each(function () {

        var nodeID = $("#productoptions").find("div[class=\"product-category\"] > span").attr("nodeid");
       
        $("#productoptions").find("div[class=\"product-category\"]").find("span[nodeid=\"" + nodeID + "\"]").addClass('enableitemtext');

        //default selected product
        $("#lblselecteditem").text($("#productoptions").find("div[class=\"product-category\"]").find("span[nodeid=\"" + nodeID + "\"]").text());
        
        selectedproduct = nodeID;

        return;
    });
        
    //load templates on page load
    LoadMenuTreeTemplates(selectedproduct);
    
    var mainProduct;
    jQuery("div.product-category").each(function () {

        // Add a mouseover cursor change to the span text
        jQuery(this).children("img").mouseover(function () {
            jQuery(this).css('cursor', 'pointer');
        });

        jQuery(this).children("span").mouseover(function () {
            jQuery(this).css('cursor', 'pointer');
            jQuery(this).css('text-decoration', 'underline');
        });

        jQuery(this).children("span").mouseout(function () {
            jQuery(this).css('text-decoration', 'none');
        });

        // Add a click event to the span text to expand or 
        // collapse the category div
        jQuery(this).children("img").click(function () {

            // First hide all the previous / next category boxes
            jQuery(this).parent().prevAll().find("div.categorybox").hide();
            jQuery(this).parent().nextAll().find("div.categorybox").hide();

            // And also set their Images to be expandable (plus.gif)
            jQuery(this).parent().prevAll().find("img.image").attr("src", "images/portal/icons/plus.gif");
            jQuery(this).parent().nextAll().find("img.image").attr("src", "images/portal/icons/plus.gif");

            // Toggle the state of the current category box
            jQuery(this).parent().nextAll('div.categorybox').toggle();

            // Toggle the state of the image
            var img = jQuery(this).attr('src');
            if (img == "images/portal/icons/plus.gif") {
                jQuery(this).attr("src", "images/portal/icons/minus.gif");
                jQuery(this).parent().find('div.categorybox').toggle();
            }
            else {
                jQuery(this).attr("src", "images/portal/icons/plus.gif");
                jQuery(this).parent().find('div.categorybox').hide();
            }
        });

        // Add a mouseover cursor change to the span text
        jQuery(this).children("span").mouseover(function () {
            jQuery(this).css('cursor', 'pointer');
        });

        // Add a click event to the span text to expand or 
        // collapse the category div

        jQuery(this).children("span").click(function () {

            if (mainProduct != jQuery(this).text()) {

                jQuery(this).parent().children().find('span.subitemtext').removeClass('enableitemtext');
                jQuery(this).parent().children().find("div.subcategorybox").hide();
                var img = jQuery(this).parent().children().find("img.categoryimage").attr('src');
                if (img == "images/portal/icons/minus.gif") {
                    jQuery(this).parent().children().find("img.categoryimage").attr("src", "images/portal/icons/plus.gif");
                }

                //remove all enabled product-category itemtext 
                jQuery(this).parent().nextAll().children().children().find('span.itemtext').removeClass('enableitemtext');
                jQuery(this).parent().prevAll().children().children().find('span.itemtext').removeClass('enableitemtext');


                jQuery(this).parent().children().find('span.itemtext').removeClass('enableitemtext');
                jQuery(this).parent().find('div.categorybox').hide();
                var img = jQuery(this).parent().find("img.image").attr('src');
                if (img == "images/portal/icons/minus.gif") {
                    jQuery(this).parent().find("img.image").attr("src", "images/portal/icons/plus.gif");
                }
                var previmg = jQuery(this).parent().prevAll().find("img.image")
                jQuery.each(previmg, function (index, value) {
                    if ($(this).attr('src') == "images/portal/icons/minus.gif") {
                        jQuery(this).attr("src", "images/portal/icons/plus.gif");
                    }
                });
                var nextimg = jQuery(this).parent().nextAll().find("img.image")
                jQuery.each(nextimg, function (index, value) {
                    if ($(this).attr('src') == "images/portal/icons/minus.gif") {
                        jQuery(this).attr("src", "images/portal/icons/plus.gif");
                    }
                });

                jQuery(this).parent().nextAll().find('div.categorybox').hide();
                jQuery(this).parent().prevAll().find('div.categorybox').hide();

                //remove the enabled category div within the product
                jQuery(this).parent().prevAll().children("span").removeClass('enableitemtext');
                jQuery(this).parent().nextAll().children("span").removeClass('enableitemtext');

                jQuery(this).addClass('enableitemtext');
                mainProduct = jQuery(this).text();
                var ProductNodeId = jQuery(this).attr('nodeid');
                LoadCustomTemplates(mainProduct, ProductNodeId, "", 0, "", 0);
            }
        });
    });
    
    jQuery("div.item").each(function () {

        jQuery(this).children("span").mouseover(function () {
            jQuery(this).css('cursor', 'pointer');
            jQuery(this).css('text-decoration', 'underline');
        });

        jQuery(this).children("span").mouseout(function () {
            jQuery(this).css('text-decoration', 'none');
        });

        // Add a click event to the span text to expand or 
        // collapse the category div
        jQuery(this).children("span").click(function () {
            var subCategory;
            if (subCategory != jQuery(this).text()) {

                //remove enabled all product options
                jQuery(this).parent().parent().parent().find("span.producttext").removeClass('enableitemtext');
                jQuery(this).parent().parent().parent().nextAll().find("span.producttext").removeClass('enableitemtext');
                jQuery(this).parent().parent().parent().prevAll().find("span.producttext").removeClass('enableitemtext');

                ///remove the enabled category div from all product options
                jQuery(this).parent().parent().parent().nextAll().children().children().find("span").removeClass('enableitemtext');
                jQuery(this).parent().parent().parent().prevAll().children().children().find("span").removeClass('enableitemtext');

                jQuery(this).parent().parent().find("span.subitemtext").removeClass("enableitemtext")
                jQuery(this).parent().prevAll().find('div.subcategorybox').hide();
                var img = jQuery(this).parent().prevAll().find("img.categoryimage").attr('src');
                if (img == "images/portal/icons/minus.gif") {
                    jQuery(this).parent().prevAll().find("img.categoryimage").attr("src", "images/portal/icons/plus.gif");
                }
                jQuery(this).parent().nextAll().find('div.subcategorybox').hide();
                var img = jQuery(this).parent().nextAll().find("img.categoryimage").attr('src');
                if (img == "images/portal/icons/minus.gif") {
                    jQuery(this).parent().nextAll().find("img.categoryimage").attr("src", "images/portal/icons/plus.gif");
                }

                ///remove the enabled subcategory div from all product options
                jQuery(this).parent().children().children().find("span").removeClass('enableitemtext');
                jQuery(this).parent().children().children().find("span").removeClass('enableitemtext');

                jQuery(this).parent().find('div.subcategorybox').hide();
                var img = jQuery(this).parent().find("img.categoryimage").attr('src');
                if (img == "images/portal/icons/minus.gif") {
                    jQuery(this).parent().find("img.categoryimage").attr("src", "images/portal/icons/plus.gif");
                }

                //remove the enabled category div within the product-category
                jQuery(this).parent().prevAll().children("span").removeClass('enableitemtext');
                jQuery(this).parent().nextAll().children("span").removeClass('enableitemtext');

                jQuery(this).addClass('enableitemtext');

                var Product = jQuery(this).parent().parent().parent().find("span.producttext").text();
                subCategory = jQuery(this).text();
                var CategoryNodeId = jQuery(this).attr('nodeid');
                LoadCustomTemplates(Product, 0, subCategory, CategoryNodeId, "", 0);
                mainProduct = '';
            }
        });

        jQuery(this).children("img.categoryimage").click(function () {

            var img = jQuery(this).attr('src');
            if (img == "images/portal/icons/plus.gif") {
                jQuery(this).attr("src", "images/portal/icons/minus.gif");
                jQuery(this).parent().find('div.subcategorybox').toggle();
            }
            else {
                jQuery(this).attr("src", "images/portal/icons/plus.gif");
                jQuery(this).parent().find('div.subcategorybox').hide();
            }
        });

    });

    jQuery("div.subitem").each(function () {

        jQuery(this).children("span").mouseover(function () {
            jQuery(this).css('cursor', 'pointer');
            jQuery(this).css('text-decoration', 'underline');
        });

        jQuery(this).children("span").mouseout(function () {
            jQuery(this).css('text-decoration', 'none');
        });

        jQuery(this).children("span").click(function () {

            jQuery(this).parent().parent().parent().parent().parent().find("span.producttext").removeClass('enableitemtext');
            jQuery(this).parent().parent().parent().find("span.itemtext").removeClass('enableitemtext');

            //remove the enabled category div within the product-category
            jQuery(this).parent().prevAll().children("span").removeClass('enableitemtext');
            jQuery(this).parent().nextAll().children("span").removeClass('enableitemtext');

            jQuery(this).parent().parent().parent().nextAll().find("span.itemtext").removeClass('enableitemtext');
            jQuery(this).parent().parent().parent().nextAll().find("span.itemtext").removeClass('enableitemtext');


            jQuery(this).addClass('enableitemtext');
            var Product = jQuery(this).parent().parent().parent().parent().parent().find("span.producttext").text();
            var Category = jQuery(this).parent().parent().parent().find("span.itemtext").text();
            var SubCategory = jQuery(this).text();
            var SubCategoryNodeId = jQuery(this).attr('nodeid');
            LoadCustomTemplates(Product, 0, Category, 0, SubCategory, SubCategoryNodeId);
            mainProduct = '';
        });
    });    

});


function BuildTemplateButton(sCLoginId) {
    templatebutton = new Array();
    jQuery.ajax({
        type: "POST",
        url: sRootPath + "services/CollateralHome.asmx/GetTemplateButtons",
        cache: false,
        data: JSON.stringify({ sChannelPartnerLoginID: sCLoginId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response.TemplateButtonDetails) {
                templatebutton = response.TemplateButtonDetails;
            }
        }
    });
}

$('#NoDetail').click(
        function (e) {
            $('html, body').animate({ scrollTop: $('#bottomPanel').height() });
        }
);

function LoadCustomTemplates(product, productnodeid, category, categorynodeid, subcategory, subcategorynodeid) {
    //call when the product option is selected
    if (category == "" && subcategory == "") {
        $("#lblselecteditem").text(product);
        LoadMenuTreeTemplates(productnodeid);
        selectedproduct = productnodeid;
    }
    //call when the category option under the product is selected
    else if (subcategory == "") {
        $("#lblselecteditem").text(product + " - " + category);
        LoadMenuTreeTemplates(categorynodeid);
        selectedproduct = categorynodeid;
    }
    //call when the sub-category option under the product category is selected    
    else {
        $("#lblselecteditem").text(product + " - " + category + " - " + subcategory);
        LoadMenuTreeTemplates(subcategorynodeid);
        selectedproduct = subcategorynodeid;
    }   
}

function LoadMenuTreeTemplates(nodeID) {

    jQuery.ajax({
        type: "POST",
        url: sRootPath + "services/CollateralHome.asmx/GetMenuTreeTemplateSection",
        cache: false,
        data: JSON.stringify({ nodeID: nodeID, language: sLanguage }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response) {
                
                BuildTemplateForm(response.CustomTemplates);
            }
        }
    });
}

function BuildTemplateForm(paramTemplates) {
    
    //remove and rebuild templates whenever a new product is selected
    jQuery("#templateform").find("div[id=\"template\"]").remove();
   
    for (i = 0; i < paramTemplates.length; i++)
    {
        var leftimage;

        if ((paramTemplates[i].IsXeroxBranded && paramTemplates[i].IsPartnerBranded) || (paramTemplates[i].IsPartnerBranded))
            leftimage = "<div id=\"divimgtemplate\" class=\"divimgtemplate\"><img id=\"imgtemplate\" class=\"imgtemplate\" src=\"" + sRootPath + "images/templates/partnerbrand/thumbnail_" + paramTemplates[i].TemplateID + ".jpg?v=" + new Date().getTime() + "\" alt=\"image_template\"/></div>";
        else if (paramTemplates[i].IsXeroxBranded)
            leftimage = "<div id=\"divimgtemplate\" class=\"divimgtemplate\"><img id=\"imgtemplate\" class=\"imgtemplate\" src=\"" + sRootPath + "images/templates/xeroxbrand/thumbnail_" + paramTemplates[i].TemplateID + ".jpg?v=" + new Date().getTime() + "\" alt=\"image_template\"/></div>";

        var tempcontent1 = "<div id=\"tempcontentsection\" class=\"tempcontentsection\"><strong><label id=\"lbltemplateheader\">" + paramTemplates[i].Name + "</label></strong>";

        var tempcontent2 = "<p class=\"templatepara\"><label id=\"lbltemplatepara\">" + paramTemplates[i].Description + "</label></p>";

        var tempcontent3; var tempcontent4; var brandtempcontent; var brandheader;

        if (paramTemplates[i].IsXeroxBranded && paramTemplates[i].IsPartnerBranded) {

            brandheader = "<strong><label id=\"lblbrandheader\">" + sBrandHeader + "</label></strong>";
            tempcontent3 = "<p><label id=\"lblpartnerbrand\"><input id=\"rdbtn_partnerbrand\" type=\"radio\" value=\"partner\" checked=\"checked\" onclick=\"javascript:ChangeBrand(" + paramTemplates[i].TemplateID + ", 'partner');\" />" + sPartnerBrand + "</label></p>";
            tempcontent4 = "<p class=\"radiobutton\"><label id=\"lblxeroxbrand\"><input id=\"rdbtn_xeroxbrand\" type=\"radio\" value=\"xerox\" onclick=\"javascript:ChangeBrand(" + paramTemplates[i].TemplateID + ", 'xerox');\" />" + sXeroxBrand + "</label></p>";
            brandtempcontent = brandheader + tempcontent3 + tempcontent4;
        }
        else if (paramTemplates[i].IsXeroxBranded) {

            if (paramTemplates[i].Name.indexOf('ConnectKey') > -1)
                tempcontent3 = "<p class=\"radiobutton\"><label id=\"lblxeroxbrand\">" + sResellerBrand + "</label></p>";
            else
                tempcontent3 = "<p class=\"radiobutton\"><label id=\"lblxeroxbrand\">" + sXeroxBrand + "</label></p>";

            brandtempcontent = tempcontent3;
        }
        else if (paramTemplates[i].IsPartnerBranded) {

            tempcontent4 = "<p><label id=\"lblpartnerbrand\">" + sPartnerBrand + "</label></p>";
            brandtempcontent = tempcontent4;
        }
        var tempcontent5 = "<strong><label id=\"lblpaperoption\" class=\"paperoption\">" + paramTemplates[i].Detail + ":</label></strong></div>";

        var rightcontent = tempcontent1 + tempcontent2 + brandtempcontent + tempcontent5;

        var controlbtns;

        var button1 = "<div id=\"cntrlbuttons\" class=\"cntrlbuttons\">";

        var button2; var button3; var button4; var button5;

        if (paramTemplates[i].IsXeroxBranded && paramTemplates[i].IsPartnerBranded) {

            button3 = "<input id=\"btnedit\" type=\"button\" onclick=\"javascript:GoToEditDocument(" + paramTemplates[i].TemplateID + ", 'branded');\" class=\"templatebutton blue\" value=\"" + sBtnEdit + "\" />";
            controlbtns = button1 + button3 + "</div>";

            if (paramTemplates[i].Flag == 'Success') {

                button2 = "<input id=\"btnpreview\" type=\"button\" onclick=\"javascript:CallToPreviewDocument(" + paramTemplates[i].TemplateID + ", '" + paramTemplates[i].Name + "', 'branded');\" class=\"templatebutton blue\" value=\"" + sBtnPreview + "\" />";
                controlbtns = button1 + button2 + button3 + "</div>";

                //logic to check whether the template has been customized today or not
                for (j = 0; j < templatebutton.length; j++) {

                    if (paramTemplates[i].TemplateID == templatebutton[j].TemplateID &&
                            (templatebutton[j].IsCustomized) && (CompareTodaysDate(templatebutton[j].DateCustomized))) {

                        button4 = "<input id=\"btndownload\" type=\"button\" onclick=\"javascript:CallHandlerToPDF(" + paramTemplates[i].TemplateID + ", 'branded');\" class=\"templatebutton blue\" value=\"" + sBtnDownload + "\" />";
                        button5 = "<input id=\"btnorderprints\" type=\"button\" onclick=\"javascript:GoToOrderDocument(" + paramTemplates[i].TemplateID + ", 'branded');\" class=\"templatebutton orange\" value=\"" + sBtnOrderPrint + "\" />";
                        controlbtns = button1 + button2 + button3 + button4 + button5 + "</div>";
                    }
                }                
            }      
        }

        else if (paramTemplates[i].IsXeroxBranded) {

            button3 = "<input id=\"btnedit\" type=\"button\" onclick=\"javascript:GoToEditDocument(" + paramTemplates[i].TemplateID + ", 'xeroxbrand');\" class=\"templatebutton blue\" value=\"" + sBtnEdit + "\" />";
            controlbtns = button1 + button3 + "</div>";

            if (paramTemplates[i].Flag == 'Success') {

                button2 = "<input id=\"btnpreview\" type=\"button\" onclick=\"javascript:CallToPreviewDocument(" + paramTemplates[i].TemplateID + ", '" + paramTemplates[i].Name + "', 'xeroxbrand');\" class=\"templatebutton blue\" value=\"" + sBtnPreview + "\" />";
                controlbtns = button1 + button2 + button3 + "</div>";

                //logic to check whether the template has been customized today or not
                for (j = 0; j < templatebutton.length; j++) {

                    if (paramTemplates[i].TemplateID == templatebutton[j].TemplateID &&
                            (templatebutton[j].IsCustomized) && (CompareTodaysDate(templatebutton[j].DateCustomized))) {

                        button4 = "<input id=\"btndownload\" type=\"button\" onclick=\"javascript:CallHandlerToPDF(" + paramTemplates[i].TemplateID + ", 'xeroxbrand');\" class=\"templatebutton blue\" value=\"" + sBtnDownload + "\" />";
                        button5 = "<input id=\"btnorderprints\" type=\"button\" onclick=\"javascript:GoToOrderDocument(" + paramTemplates[i].TemplateID + ", 'xeroxbrand');\" class=\"templatebutton orange\" value=\"" + sBtnOrderPrint + "\" />";
                        controlbtns = button1 + button2 + button3 + button4 + button5 + "</div>";
                    }
                }                
            }
        }

        else if (paramTemplates[i].IsPartnerBranded) {

            button3 = "<input id=\"btnedit\" type=\"button\" onclick=\"javascript:GoToEditDocument(" + paramTemplates[i].TemplateID + ", 'partnerbrand');\" class=\"templatebutton blue\" value=\"" + sBtnEdit + "\" />";
            controlbtns = button1 + button3 + "</div>";

            if (paramTemplates[i].Flag == 'Success') {

                button2 = "<input id=\"btnpreview\" type=\"button\" onclick=\"javascript:CallToPreviewDocument(" + paramTemplates[i].TemplateID + ", '" + paramTemplates[i].Name + "', 'partnerbrand');\" class=\"templatebutton blue\" value=\"" + sBtnPreview + "\" />";
                controlbtns = button1 + button2 + button3 + "</div>";
                
                //logic to check whether the template has been customized today or not
                for (j = 0; j < templatebutton.length; j++) {

                    if (paramTemplates[i].TemplateID == templatebutton[j].TemplateID &&
                            (templatebutton[j].IsCustomized) && (CompareTodaysDate(templatebutton[j].DateCustomized))) {

                        button4 = "<input id=\"btndownload\" type=\"button\" onclick=\"javascript:CallHandlerToPDF(" + paramTemplates[i].TemplateID + ", 'partnerbrand');\" class=\"templatebutton blue\" value=\"" + sBtnDownload + "\" />";
                        button5 = "<input id=\"btnorderprints\" type=\"button\" onclick=\"javascript:GoToOrderDocument(" + paramTemplates[i].TemplateID + ", 'partnerbrand');\" class=\"templatebutton orange\" value=\"" + sBtnOrderPrint + "\" />";
                        controlbtns = button1 + button2 + button3 + button4 + button5 + "</div>";
                    }
                }                
            }
        }
      
        var bottomunderline = "<hr class=\"templateform\" />";
        jQuery('#templateform').append("<div id=\"template\" class=\"template\" templateid=\"" + paramTemplates[i].TemplateID + "\">" + leftimage + rightcontent + controlbtns + bottomunderline + "</div>");
        
    }
}

function GoToEditDocument(param_templateid, param_branded) {
    
    if (param_branded == 'branded') {
        //find which radio button is selected
        var brand = jQuery("#templateform").find("div[templateid=\"" + param_templateid + "\"]").find("div[id=\"tempcontentsection\"]").find("input[type='radio']:checked").attr('value');

        if (brand == 'xerox') {
            $('#templatebrand').val('xeroxbrand');
            window.location.href = sRootPath + "Edit.aspx?templateid=" + param_templateid + "&partnerbrand=false";
        }
        else {
            $('#templatebrand').val('partnerbrand');
            window.location.href = sRootPath + "Edit.aspx?templateid=" + param_templateid + "&partnerbrand=true";
            
        }
    }
    else if (param_branded == 'xeroxbrand') {
        $('#templatebrand').val('xeroxbrand');
        window.location.href = sRootPath + "Edit.aspx?templateid=" + param_templateid + "&partnerbrand=false";
    }
    else if (param_branded == 'partnerbrand') {
        $('#templatebrand').val('xeroxbrand');
        window.location.href = sRootPath + "Edit.aspx?templateid=" + param_templateid + "&partnerbrand=true";
    }
}

function GoToOrderDocument(param_templateid, param_branded) {

    if (param_branded == 'branded') {
        //find which radio button is selected
        var brand = jQuery("#templateform").find("div[templateid=\"" + param_templateid + "\"]").find("div[id=\"tempcontentsection\"]").find("input[type='radio']:checked").attr('value');

        if (brand == 'xerox')
            window.location.href = sRootPath + "OrderDocument.aspx?templateid=" + param_templateid + "&partnerbrand=false";
        else
            window.location.href = sRootPath + "OrderDocument.aspx?templateid=" + param_templateid + "&partnerbrand=true";
    }
    else if (param_branded == 'xeroxbrand')
        window.location.href = sRootPath + "OrderDocument.aspx?templateid=" + param_templateid + "&partnerbrand=false";
    else if (param_branded == 'partnerbrand')
        window.location.href = sRootPath + "OrderDocument.aspx?templateid=" + param_templateid + "&partnerbrand=true";
    
}

function ChangeBrand(param1, param2) {

    //change the corresponding template image url when the user clicked on partner brand
    if (param2 == 'partner') {
        jQuery("#templateform").find("div[templateid=\"" + param1 + "\"]").find("div[id=\"tempcontentsection\"]").find("input[id=\"rdbtn_xeroxbrand\"]").attr('checked', '');
        jQuery("#templateform").find("div[templateid=\"" + param1 + "\"]").find("div[id=\"tempcontentsection\"]").find("input[id=\"rdbtn_partnerbrand\"]").attr('checked', 'checked');
        jQuery("#templateform").find("div[templateid=\"" + param1 + "\"]").find("div[id=\"divimgtemplate\"]").find("img[id=\"imgtemplate\"]").attr('src', sRootPath + 'images/templates/partnerbrand/thumbnail_' + param1 + '.jpg');
    }
    //change the corresponding template image url when the user clicked on xerox brand
    else {
        jQuery("#templateform").find("div[templateid=\"" + param1 + "\"]").find("div[id=\"tempcontentsection\"]").find("input[id=\"rdbtn_partnerbrand\"]").attr('checked', '');
        jQuery("#templateform").find("div[templateid=\"" + param1 + "\"]").find("div[id=\"tempcontentsection\"]").find("input[id=\"rdbtn_xeroxbrand\"]").attr('checked', 'checked');
        jQuery("#templateform").find("div[templateid=\"" + param1 + "\"]").find("div[id=\"divimgtemplate\"]").find("img[id=\"imgtemplate\"]").attr('src', sRootPath + 'images/templates/xeroxbrand/thumbnail_' + param1 + '.jpg');
    }
}

function CallHandlerToPDF(param_templateid, param_branded) {

    //show ajax loader
    jQuery('.ajaxloader').dialog({
        autoOpen: true,
        closeOnEscape: false,
        position: [400, 300],
        width: 300,
        height: 150,
        modal: true,
        draggable: false,
        resizable: false
    });


    if (param_branded == 'branded') {
        //find which radio button is selected
        var brand = jQuery("#templateform").find("div[templateid=\"" + param_templateid + "\"]").find("div[id=\"tempcontentsection\"]").find("input[type='radio']:checked").attr('value');

        if (brand == 'xerox')
            window.location.href = sRootPath + 'Handlers/ReportHandler.ashx?mode=generatepdf&templateid=' + param_templateid + '&partnerbrand=false&channelpartnerid=' + sCPartnerId + '&channelpartnerloginid=' + sCLoginId + '&channelpartneremail=' + sCEmail + '&language=' + sLanguage;
        else
            window.location.href = sRootPath + 'Handlers/ReportHandler.ashx?mode=generatepdf&templateid=' + param_templateid + '&partnerbrand=true&channelpartnerid=' + sCPartnerId + '&channelpartnerloginid=' + sCLoginId + '&channelpartneremail=' + sCEmail + '&language=' + sLanguage;
    }
    else if (param_branded == 'xeroxbrand')
        window.location.href = sRootPath + 'Handlers/ReportHandler.ashx?mode=generatepdf&templateid=' + param_templateid + '&partnerbrand=false&channelpartnerid=' + sCPartnerId + '&channelpartnerloginid=' + sCLoginId + '&channelpartneremail=' + sCEmail + '&language=' + sLanguage;
    else if (param_branded == 'partnerbrand')
        window.location.href = sRootPath + 'Handlers/ReportHandler.ashx?mode=generatepdf&templateid=' + param_templateid + '&partnerbrand=true&channelpartnerid=' + sCPartnerId + '&channelpartnerloginid=' + sCLoginId + '&channelpartneremail=' + sCEmail + '&language=' + sLanguage;

        
    //call ajax to update the address fields in Document table
    jQuery.ajax({
        type: "POST",
        url: sRootPath + "services/CollateralHome.asmx/UpdateDocumentContactDetails",
        cache: false,
        data: JSON.stringify({ iTemplateID: param_templateid, bPartnerBrand: param_branded, sAttention: sjAttention, sCompanyName: sjCompanyName, sAddressLine1: sjAddressLine1, sAddressLine2: sjAddressLine2, sCity: sjCity, sState: sjState, sPostCode: sjPostCode, sFirstName: sjFirstName, sLastName: sjLastName, sPhone: sjPhone, sEmail: sjEmail }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response) {}
        },
        complete: function () {
            jQuery(".ajaxloader").dialog("close");
        }
    });
}

function CallToPreviewDocument(param_templateid, param_templatename, param_branded) {

    $('#templatenumber').val(param_templateid);
    
    //show ajax loader
    jQuery('.ajaxloader').dialog({
        autoOpen: true,
        closeOnEscape: false,
        position: [400, 300],
        width: 300,
        height: 150,
        modal: true,
        draggable: false,
        resizable: false
    });

    var bPXBrand;
    if (param_branded == 'branded') {
        
        //find which radio button is selected
        var brand = jQuery("#templateform").find("div[templateid=\"" + param_templateid + "\"]").find("div[id=\"tempcontentsection\"]").find("input[type='radio']:checked").attr('value');

        if (brand == 'xerox') {
            bPXBrand = false;
            $('#templatebrand').val('xeroxbrand');
        }
        else {
            bPXBrand = true;
            $('#templatebrand').val('partnerbrand');
        }
    }
    else if (param_branded == 'xeroxbrand') {
        bPXBrand = false;
        $('#templatebrand').val('xeroxbrand');
    }
    else if (param_branded == 'partnerbrand') {
        bPXBrand = true;
        $('#templatebrand').val('partnerbrand');
    }
    
    jQuery.ajax({

        type: "POST",
        url: sRootPath + "services/CollateralHome.asmx/GenerateDocument",
        cache: false,
        data: JSON.stringify({ iTemplateID: param_templateid, bPartnerBrand: bPXBrand, sLanguage: sLanguage }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response.DocumentDetail) {
                
                if (response.DocumentDetail[0] != '' && globalflag != 'errorshown') {
                    globalflag = 'errorshown';                    
                }
                PreviewDocument(response.DocumentDetail[1], param_templatename);
            }
        },
        complete: function () {
            jQuery(".ajaxloader").dialog("close");
        }
    });
}

function CompareTodaysDate(jsondate) {

    jsondate = jsondate.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
    }
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }

    var date = new Date(parseInt(jsondate, 10));
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

    //get today's date
    var today = new Date();
    //compare today's date is equal with the Json date format
    if (today.getFullYear() == date.getFullYear())
        if (today.getMonth() == date.getMonth())
            if (today.getDate() == currentDate)
                return true;
            else
                return false;
}
