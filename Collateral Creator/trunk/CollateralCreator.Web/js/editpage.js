var PdfSrc;

    jQuery(document).ready(function () {

        //Check Address, Custom, Logo Details
        if (sNoDetailsFlag == 'Error') {
            $('#litNoDetailMessage').html(sNoDetailMessage).text();
        }
        LoadCustomizeImages();

        $('.savingtext').hide();
        $('.savingtext1').hide();
        $('.savingtext2').hide();
        $('.savingtext3').hide();
        $('.savingtext4').hide();

        //    $('textarea[maxlength]').keyup(function () {
        //        //get the limit from maxlength attribute  
        //        var limit = parseInt($(this).attr('maxlength'));
        //        //get the current text inside the textarea  
        //        var text = $(this).val();
        //        //count the number of characters in the text  
        //        var chars = text.length;

        //        //check if there are more characters then allowed  
        //        if (chars > limit) {
        //            //and if there are use substr to get the text before the limit  
        //            var new_text = text.substr(0, limit);

        //            //and change the current text with the new text  
        //            $(this).val(new_text);
        //        }
        //    });

        jQuery("#currenttemplatename").val(customizingTemplate);
        //load the pdf
        PdfSrc = sRootPath + 'temp/document' + fileDocumentId + '.pdf?v=' + new Date().getTime() + '#toolbar=0&statusbar=0&navpanes=0';
        jQuery("#iframePdfPreview").attr('src', PdfSrc);

        if ($('#textarea1').length != 0)
            TextArea1_Logic();
        if ($('#textarea2').length != 0)
            TextArea2_Logic();
        if ($('#textarea3').length != 0)
            TextArea3_Logic();
        //check to see if smart centre exists in the query string ~MPE 23/04/2013
        var field = 'Type=SmartCentre';
        var url = window.location.href;
       
        if (url.indexOf('?' + field) != -1) {
            document.getElementById('inputcollateralmode').value = '1';   
        }
        else if (url.indexOf('&' + field) != -1) {
            document.getElementById('inputcollateralmode').value = '1';
        }
  
    });

    function LoadCustomizeImages() {

        $('#btnUploadLogo').text(sbtnUploadText);
     
        if (sTemplateId != 0 && sTemplateId != undefined) {
            //load the default image
            jQuery.ajax({
                type: "POST",
                url: sRootPath + "services/CollateralHome.asmx/GetDefaultLogoImage",
                cache: false,
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    if (response.Image == 'default')
                        $('.customizeimages').find("img").attr('src', sRootPath + 'images/logos/XCT_YourLogo.jpg');
                    else {
                        $('.customizeimages').find("img").attr('src', 'Handlers/ImageHandler.ashx?v=' + new Date().getTime() + '&mode=defaultimage&loginid=' + response.Image[0]);

                        //update the document images
                        var ImageIDs = $('#hiddenImageID').val();
                        //logic to get imageids for all logos
                        var imagearray = ImageIDs.split(',');
                        var imageUrlid = '';
                        for (i = 0; i < imagearray.length; i++) {
                            imageUrlid += imagearray[i] + ',';
                        }

                        jQuery.ajax({
                            type: "POST",
                            url: sRootPath + "services/CollateralHome.asmx/UpdateDocumentImage",
                            cache: false,
                            data: JSON.stringify({ sImageAreaID: imageUrlid }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                                if (response) { }
                            }
                        });
                    }
                }
            });
        }
        //else if (sType == 'SmartCentre')
        //{
        //    $('.customizeimages').find("img").attr('src', sRootPath + 'images/logos/Box_logo_keyline.jpg');
        //}
        else {
            //load the custom images
            jQuery.ajax({
                type: "POST",
                url: sRootPath + "services/CollateralHome.asmx/GetEditPageLogoImage",
                cache: false,
                data: JSON.stringify({ iDocumentID: fileDocumentId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    if (response.Image) {
                        BuildImages(response.Image);
                    }
                }
            });
        }

    }

    function BuildImages(paramImage) {

        //logic to get imageids for all logos
        var imageUrl; var imageUrlid = '';
        imageUrl = 'Handlers/ImageHandler.ashx?mode=documentimage';

        for (i = 0; i < paramImage.length; i++) {
            imageUrlid += '&imageid' + i + '=' + paramImage[i];
        }

        $('.customizeimages').find("img").attr('src', imageUrl + imageUrlid);       
    }

    $('#currenttemplatename').live('focusout', function () {

        var documentname = $('#currenttemplatename').val();
        $('.savingtext').show();
        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/Document_UpdateName",
            cache: false,
            data: JSON.stringify({ iDocumentId: fileDocumentId, sDocumentName: documentname }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {
                    $('.savingtext').delay(1500).hide('slow');
                }
            }
        });
    });

    ///Sc2 text area update function - attached to multiple text boxes
    $('#textareasc1, #textareasc2, #textareasc3, #textareasc4, #textareasc5, #textareasc6, #textareasc7, #textareasc8, #textareasc9, #textareasc10, #textareasc11, #textareasc12, #textareasc13, #textareasc14, #textareasc15, #textareasc16, #textareasc17, #textareasc18, #textareasc19, #textareasc20, #textareasc21, #textareasc22, #textareasc23, #textareasc24, #textareasc25, #textareasc26, #textareasc27, #textareasc28, #textareasc29, #textareasc30').focusout(function (e) {
        
        var areaTextElementId = "hiddentextareaId1";
        var areaElementId = "hiddentextarea1";
        var textelementname = '#' + e.target.id;
        var text = new String($(textelementname).val());
        
        switch (textelementname) {
            case "#textareasc1":
                areaElementId = "hiddentextarea1";
                areaTextElementId = "hiddentextareaId1";
                break;
            case "#textareasc2":
                areaElementId = "hiddentextarea2";
                areaTextElementId = "hiddentextareaId2";
                break;
            case "#textareasc3":
                areaElementId = "hiddentextarea3";
                areaTextElementId = "hiddentextareaId3";
                break;
            case "#textareasc4":
                areaElementId = "hiddentextarea4";
                areaTextElementId = "hiddentextareaId4";
                break;
            case "#textareasc5":
                areaElementId = "hiddentextarea5";
                areaTextElementId = "hiddentextareaId5";
                break;
            case "#textareasc6":
                areaElementId = "hiddentextarea6";
                areaTextElementId = "hiddentextareaId6";
                break;
            case "#textareasc7":
                areaElementId = "hiddentextarea7";
                areaTextElementId = "hiddentextareaId7";
                break;
            case "#textareasc8":
                areaElementId = "hiddentextarea8";
                areaTextElementId = "hiddentextareaId8";
                break;
            case "#textareasc9":
                areaElementId = "hiddentextarea9";
                areaTextElementId = "hiddentextareaId9";
                break;
            case "#textareasc10":
                areaElementId = "hiddentextarea10";
                areaTextElementId = "hiddentextareaId10";
                break;
            case "#textareasc11":
                areaElementId = "hiddentextarea11";
                areaTextElementId = "hiddentextareaId11";
                break;
            case "#textareasc12":
                areaElementId = "hiddentextarea12";
                areaTextElementId = "hiddentextareaId12";
                break;
            case "#textareasc13":
                areaElementId = "hiddentextarea13";
                areaTextElementId = "hiddentextareaId13";
                break;
            case "#textareasc14":
                areaElementId = "hiddentextarea14";
                areaTextElementId = "hiddentextareaId14";
                break;
            case "#textareasc15":
                areaElementId = "hiddentextarea15";
                areaTextElementId = "hiddentextareaId15";
                break;
            case "#textareasc16":
                areaElementId = "hiddentextarea16";
                areaTextElementId = "hiddentextareaId16";
                break;
            case "#textareasc17":
                areaElementId = "hiddentextarea17";
                areaTextElementId = "hiddentextareaId17";
                break;
            case "#textareasc18":
                areaElementId = "hiddentextarea18";
                areaTextElementId = "hiddentextareaId18";
                break;
            case "#textareasc19":
                areaElementId = "hiddentextarea19";
                areaTextElementId = "hiddentextareaId19";
                break;
            case "#textareasc20":
                areaElementId = "hiddentextarea20";
                areaTextElementId = "hiddentextareaId20";
                break;
            case "#textareasc21":
                areaElementId = "hiddentextarea21";
                areaTextElementId = "hiddentextareaId21";
                break;
            case "#textareasc22":
                areaElementId = "hiddentextarea22";
                areaTextElementId = "hiddentextareaId22";
                break;
            case "#textareasc23":
                areaElementId = "hiddentextarea23";
                areaTextElementId = "hiddentextareaId23";
                break;
            case "#textareasc24":
                areaElementId = "hiddentextarea24";
                areaTextElementId = "hiddentextareaId24";
                break;
            case "#textareasc25":
                areaElementId = "hiddentextarea25";
                areaTextElementId = "hiddentextareaId25";
                break;
            case "#textareasc26":
                areaElementId = "hiddentextarea26";
                areaTextElementId = "hiddentextareaId26";
                break;
            case "#textareasc27":
                areaElementId = "hiddentextarea27";
                areaTextElementId = "hiddentextareaId27";
                break;
            case "#textareasc28":
                areaElementId = "hiddentextarea28";
                areaTextElementId = "hiddentextareaId28";
                break;
            case "#textareasc29":
                areaElementId = "hiddentextarea29";
                areaTextElementId = "hiddentextareaId29";
                break;
            case "#textareasc30":
                areaElementId = "hiddentextarea30";
                areaTextElementId = "hiddentextareaId30";
                break;
            }

        
       
        var areaId = $('#' + areaElementId).val();
        var textareaid = $('#' + areaTextElementId).val();

        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateDocumentTextArea",
            cache: false,
            data: JSON.stringify({ iAreaId: areaId, iTextAreaId: textareaid, iText: text }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {
                    $('.savingtext1').delay(1500).hide('slow');
                    if ($('#errormessage1').find("label[class=\"errortext\"]").text() == '')
                        $('#errormessage1').find('br').remove();

                    ajaxdownloadflag = 'updated';
                }
            }
        });
       
    });

    ///Alter the selected image to a blank - for sc2 logic when they have optional images
    function UploadBlankImage(imageId) {
       
        imageUrl = 'Handlers/SrcBlankImageHandler.ashx?imageid=' + imageId;
        jQuery.ajax({
            type: "POST",
            url: imageUrl,
            cache: false,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (data.error != '') {
                    alert(data.error);
                }
            }
        });
    }
    
    $('#textarea1').live('focusout', function () {

        var areaid = $('#textarea1').attr('areaid');
        var textareaid = $('#textarea1').attr('textareaid');
        var textcustomizearea = new String($('#textarea1').val());

        $('.savingtext1').show('fast');

        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateDocumentTextArea",
            cache: false,
            data: JSON.stringify({ iAreaId: areaid, iTextAreaId: textareaid, iText: textcustomizearea }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {
                    $('.savingtext1').delay(1500).hide('slow');
                    if ($('#errormessage1').find("label[class=\"errortext\"]").text() == '')
                        $('#errormessage1').find('br').remove();

                    ajaxdownloadflag = 'updated';
                }
            }
        });

    });

    $('#textarea1').keyup(function () {
      
        TextArea1_Logic();
    });

    function TextArea1_Logic() {

        var text = new String($('#textarea1').val());
        var textlines = text.split('\n');
        var linecount = 0; var charsperline = 0;
        var linemessage = 1;
        for (i = 0; i < textlines.length; i++) {

            var maxcharsperline = $('#textarea1').attr('charsperline');
            charsperline = textlines[i].length;

            LineCharMessages(1, linemessage, charsperline, maxcharsperline);

            if (textlines[i] != '')
                linecount++;

            linemessage++;
        }

        var maxlines = $('#textarea1').attr('rows');
        if (linecount > maxlines && $('#errormessage1').find('label[id=\"linecounterrortext\"]').text() == '')
            $('#errormessage1').append("<label id=\"linecounterrortext\" class=\"errortext\">" + sMaxLineErrorMsg + "</label><br/>");

    }

    $('#textarea1').keydown(function () {

        var text = new String($('#textarea1').val());
        var textlines = text.split('\n');
        var linecount = 0; var charsperline = 0;
        var linemessage = 1;

        for (i = 0; i < textlines.length; i++) {

            var maxcharsperline = $('#textarea1').attr('charsperline');
            charsperline = textlines[i].length;

            RemoveLineCharMessages(1, linemessage, charsperline, maxcharsperline);

            if (textlines[i] != '')
                linecount++;

            linemessage++;
        }

        var maxlines = $('#textarea1').attr('rows');
        if (linecount <= maxlines && $('#errormessage1').find('label[id=\"linecounterrortext\"]').text() != '') {
            $('#errormessage1').find("label[id=\"linecounterrortext\"]").remove();
        }
    });

    $('#textarea2').live('focusout', function () {

        var areaid = $('#textarea2').attr('areaid');
        var textareaid = $('#textarea2').attr('textareaid');
        var textcustomizearea = new String($('#textarea2').val());

        $('.savingtext2').show('fast');

        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateDocumentTextArea",
            cache: false,
            data: JSON.stringify({ iAreaId: areaid, iTextAreaId: textareaid, iText: textcustomizearea }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {
                    $('.savingtext2').delay(1500).hide('slow');
                    if ($('#errormessage2').find("label[class=\"errortext\"]").text() == '')
                        $('#errormessage2').find('br').remove()

                    ajaxdownloadflag = 'updated';
                }
            }
        });
    });

    $('#textarea2').keyup(function () {

        TextArea2_Logic();

    });

    function TextArea2_Logic() {

        var text = new String($('#textarea2').val());
        var textlines = text.split('\n');
        var linecount = 0; var charsperline = 0; var linemessage = 1;
        for (i = 0; i < textlines.length; i++) {

            var maxcharsperline = $('#textarea2').attr('charsperline');
            charsperline = textlines[i].length;

            LineCharMessages(2, linemessage, charsperline, maxcharsperline);

            if (textlines[i] != '')
                linecount++;

            linemessage++;
        }

        var maxlines = $('#textarea2').attr('rows');
        if (linecount > maxlines && $('#errormessage2').find('label[id=\"linecounterrortext\"]').text() == '')
            $('#errormessage2').append("<label id=\"linecounterrortext\" class=\"errortext\">" + sMaxLineErrorMsg + "</label><br/>");

    }

    $('#textarea2').keydown(function () {

        var text = new String($('#textarea2').val());
        var textlines = text.split('\n');
        var linecount = 0; var charsperline = 0;
        var linemessage = 1;

        for (i = 0; i < textlines.length; i++) {

            var maxcharsperline = $('#textarea2').attr('charsperline');
            charsperline = textlines[i].length;

            RemoveLineCharMessages(2, linemessage, charsperline, maxcharsperline);

            if (textlines[i] != '')
                linecount++;

            linemessage++;
        }

        var maxlines = $('#textarea2').attr('rows');
        if (linecount <= maxlines && $('#errormessage2').find('label[id=\"linecounterrortext\"]').text() != '') {
            $('#errormessage2').find("label[id=\"linecounterrortext\"]").remove();
        }
    });

    $('#textarea3').live('focusout', function () {

        var areaid = $('#textarea3').attr('areaid');
        var textareaid = $('#textarea3').attr('textareaid');
        var textcustomizearea = new String($('#textarea3').val());

        $('.savingtext3').show('fast');

        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateDocumentTextArea",
            cache: false,
            data: JSON.stringify({ iAreaId: areaid, iTextAreaId: textareaid, iText: textcustomizearea }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {
                    $('.savingtext3').delay(1500).hide('slow');
                    if ($('#errormessage3').find("label[class=\"errortext\"]").text() == '')
                        $('#errormessage3').find('br').remove();

                    ajaxdownloadflag = 'updated';
                }
            }
        });

    });

    $('#textarea3').keyup(function () {

        TextArea3_Logic();

    });

    function TextArea3_Logic() {

        var text = new String($('#textarea3').val());
        var textlines = text.split('\n');
        var linecount = 0; var charsperline = 0; var linemessage = 1;
        for (i = 0; i < textlines.length; i++) {

            var maxcharsperline = $('#textarea3').attr('charsperline');
            charsperline = textlines[i].length;

            LineCharMessages(3, linemessage, charsperline, maxcharsperline);

            if (textlines[i] != '')
                linecount++;

            linemessage++;
        }

        var maxlines = $('#textarea3').attr('rows');
        if (linecount > maxlines && $('#errormessage3').find('label[id=\"linecounterrortext\"]').text() == '')
            $('#errormessage3').append("<label id=\"linecounterrortext\" class=\"errortext\">" + sMaxLineErrorMsg + "</label><br/>");

    }

    $('#textarea3').keydown(function () {

        var text = new String($('#textarea3').val());
        var textlines = text.split('\n');
        var linecount = 0; var charsperline = 0;
        var linemessage = 1;

        for (i = 0; i < textlines.length; i++) {

            var maxcharsperline = $('#textarea3').attr('charsperline');
            charsperline = textlines[i].length;

            RemoveLineCharMessages(3, linemessage, charsperline, maxcharsperline);

            if (textlines[i] != '')
                linecount++;

            linemessage++;
        }

        var maxlines = $('#textarea3').attr('rows');
        if (linecount <= maxlines && $('#errormessage3').find('label[id=\"linecounterrortext\"]').text() != '') {
            $('#errormessage3').find("label[id=\"linecounterrortext\"]").remove();
        }
    });

    $('#textarea4').live('focusout', function () {

        var areaid = $('#textarea4').attr('areaid');
        var textareaid = $('#textarea4').attr('textareaid');
        var textcustomizearea = new String($('#textarea4').val());

        $('.savingtext4').show('fast');

        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateDocumentTextArea",
            cache: false,
            data: JSON.stringify({ iAreaId: areaid, iTextAreaId: textareaid, iText: textcustomizearea }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {
                    $('.savingtext4').delay(1500).hide('slow');
                    if ($('#errormessage4').find("label[class=\"errortext\"]").text() == '')
                        $('#errormessage4').find('br').remove();

                    ajaxdownloadflag = 'updated';
                }
            }
        });

    });

    $('#textarea4').keyup(function () {

        TextArea4_Logic();

    });

    function TextArea4_Logic() {

        var text = new String($('#textarea4').val());
        var textlines = text.split('\n');
        var linecount = 0; var charsperline = 0; var linemessage = 1;
        for (i = 0; i < textlines.length; i++) {

            var maxcharsperline = $('#textarea4').attr('charsperline');
            charsperline = textlines[i].length;

            LineCharMessages(4, linemessage, charsperline, maxcharsperline);

            if (textlines[i] != '')
                linecount++;

            linemessage++;
        }

        var maxlines = $('#textarea3').attr('rows');
        if (linecount > maxlines && $('#errormessage4').find('label[id=\"linecounterrortext\"]').text() == '')
            $('#errormessage4').append("<label id=\"linecounterrortext\" class=\"errortext\">" + sMaxLineErrorMsg + "</label><br/>");

    }

    $('#textarea4').keydown(function () {

        var text = new String($('#textarea4').val());
        var textlines = text.split('\n');
        var linecount = 0; var charsperline = 0;
        var linemessage = 1;

        for (i = 0; i < textlines.length; i++) {

            var maxcharsperline = $('#textarea4').attr('charsperline');
            charsperline = textlines[i].length;

            RemoveLineCharMessages(4, linemessage, charsperline, maxcharsperline);

            if (textlines[i] != '')
                linecount++;

            linemessage++;
        }

        var maxlines = $('#textarea4').attr('rows');
        if (linecount <= maxlines && $('#errormessage4').find('label[id=\"linecounterrortext\"]').text() != '') {
            $('#errormessage4').find("label[id=\"linecounterrortext\"]").remove();
        }
    });

    var documentType;

    if (sType == 'SmartCentre') {
        documentType = "smartcentre";
    }
    else {
        documentType = "edit";
    }

    function UpdatePreviewDocument() {

        jQuery("#iframePdfPreview").removeAttr('src');

        //show ajax loader
        jQuery("#iframePdfPreview").hide();
        jQuery('.divajaxloader').show();
            
        jQuery("#iframePdfPreview").removeAttr("src");
        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateDocument",
            cache: false,
            data: JSON.stringify({ iDocumentId: fileDocumentId, documentType: documentType }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response.DocumentDetail) {

                    fileDocumentId = response.DocumentDetail[1];
                    PdfSrc = sRootPath + 'temp/document' + fileDocumentId + '.pdf?v=' + new Date().getTime() + '#toolbar=0&statusbar=0&navpanes=0&view=fitH,50&view=fitV,50&view=FitBH,50';
                    jQuery("#iframePdfPreview").attr('src', PdfSrc);

                }
            },
            complete: function () {
                //hide ajax loader
                jQuery('.divajaxloader').hide();
                jQuery("#iframePdfPreview").show();
            }
        });

    }

    function getInternetExplorerVersion() {
        var rv = -1; // Return value assumes failure.
        if (navigator.appName == 'Microsoft Internet Explorer') {
            var ua = navigator.userAgent;
            var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
            if (re.exec(ua) != null)
                rv = parseFloat(RegExp.$1);
        }
        return rv;
    }

    function MaximizePreviewDocument() {

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


        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateDocument",
            cache: false,
            data: JSON.stringify({ iDocumentId: fileDocumentId, documentType: documentType }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response.DocumentDetail) {

                    jQuery("#iframePdfDocument").removeAttr('src');
                    jQuery("#iframePdfDocument").attr('src', sRootPath + 'temp/document' + response.DocumentDetail[1] + '.pdf?v=' + new Date().getTime() + '#toolbar=0&statusbar=0&navpanes=0&view=fitH,50&view=fitV,50&view=FitBH,50');

                    fileDocumentId = response.DocumentDetail[1];
                    GetTemplateName(fileDocumentId);
                }
            },
            failure: function () {
                jQuery("#iframePdfDocument").attr('src', sRootPath + 'temp/document' + fileDocumentId + '.pdf?v=' + new Date().getTime() + '#toolbar=0&statusbar=0&navpanes=0&view=fitH,50&view=fitV,50&view=FitBH,50');
            },
            complete: function () {

                //var ver = getInternetExplorerVersion();
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    if (ver = 8.0) {

                        jQuery.ajax({
                            type: "POST",
                            url: sRootPath + "services/CollateralHome.asmx/UpdateDocument",
                            cache: false,
                            data: JSON.stringify({ iDocumentId: fileDocumentId, documentType: documentType }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                                if (response.DocumentDetail) {

                                    //close the ajax loader
                                    jQuery(".ajaxloader").dialog("close");

                                    jQuery("#iframePdfDocument").removeAttr('src');
                                    jQuery("#iframePdfDocument").attr('src', sRootPath + 'temp/document' + response.DocumentDetail[1] + '.pdf?v=' + new Date().getTime() + '#toolbar=0&statusbar=0&navpanes=0&view=fitH,50&view=fitV,50&view=FitBH,50');

                                    fileDocumentId = response.DocumentDetail[1];
                                    GetTemplateName(fileDocumentId);
                                }
                            }
                        });

                    }
                    else
                        //close the ajax loader
                        jQuery(".ajaxloader").dialog("close");
                }
                else
                    //close the ajax loader
                    jQuery(".ajaxloader").dialog("close");
            }
        });
    }

    function GetTemplateName(fileDocumentId) {

        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/DocumentGetTemplateName",
            cache: false,
            data: JSON.stringify({ iDocumentID: fileDocumentId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {
                    DisplayDialog(response.TemplateName);
                }
            }
        });
    }

    function DisplayDialog(param_templatename) {

        //fade out the web page when dialog box opened
        $("html").css("overflow", "hidden");
        $('#mastercontent').slideUp('slow');
        $('#iframePdfPreview').hide('slow');

        $('.dialogPreviewBox').attr("title", param_templatename);
        $(".dialogPreviewBox").dialog("option", "title", param_templatename);

        jQuery('.dialogPreviewBox').dialog({
            autoOpen: true,
            closeOnEscape: false,
            //position: top,
            width: 600,
            height: 750,
            modal: true,
            draggable: false,
            resizable: false
        });
    }

    function CancelPreviewDialog() {

        jQuery(".dialogPreviewBox").dialog("close");
        jQuery("#iframePdfDocument").removeAttr("src");

        $('#iframePdfPreview').show('slow');
        $("html").css("overflow", "auto");
        $('#mastercontent').slideDown('slow');

        //close the ajax loader
        jQuery(".ajaxloader").dialog("close");
    }

    var ajaxdownloadflag = '';
    function DownloadDocument() {

        window.location.href = sRootPath + 'Handlers/ReportHandler.ashx?v=' + new Date().getTime() + '&mode=downloadpdf&documentname=document' + fileDocumentId + '.pdf';

        if (ajaxdownloadflag == 'updated') {

            jQuery.ajax({
                type: "POST",
                url: sRootPath + "services/CollateralHome.asmx/Document_DownloadLog",
                cache: false,
                data: JSON.stringify({ iDocumentId: fileDocumentId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    if (response) { }
                }
            });
        }
    }

    function GotoOrderPrints() {

        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/UpdateDocument",
            cache: false,
            data: JSON.stringify({ iDocumentId: fileDocumentId, documentType: documentType }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response.DocumentDetail) {
                    fileDocumentId = response.DocumentDetail[1];
                    window.location.href = sRootPath + "OrderDocument.aspx?documentid=" + fileDocumentId;
                }
            }
        });
    }

    function GotoMainMenu() {
        var result = confirm(sConfirmMsg);
        if (result) {

            jQuery.ajax({
                type: "POST",
                url: sRootPath + "services/CollateralHome.asmx/UpdateTemplateButton",
                cache: false,
                data: JSON.stringify({ iTemplateID: sTemplateId, sChannelPartnerLoginID: sCLoginId, bCustomize: true, bPartnerBrand: sTemplateBrand }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                    if (response) {
                        window.location.href = sRootPath + "Home.aspx";
                    }
                }
            });
        }
    }

    function redirect() {
        window.location.href = sRootPath + "Home.aspx";
    }

    function RemoveLineCharMessages(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline) {
        if (paramlinecount == 1 && $('#errormessage' + paramtext).find('label[id=\"errortext1\"]').text() != '')
            RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 2 && $('#errormessage' + paramtext).find('label[id=\"errortext2\"]').text() != '')
            RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 3 && $('#errormessage' + paramtext).find('label[id=\"errortext3\"]').text() != '')
            RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 4 && $('#errormessage' + paramtext).find('label[id=\"errortext4\"]').text() != '')
            RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 5 && $('#errormessage' + paramtext).find('label[id=\"errortext5\"]').text() != '')
            RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 6 && $('#errormessage' + paramtext).find('label[id=\"errortext6\"]').text() != '')
            RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 7 && $('#errormessage' + paramtext).find('label[id=\"errortext7\"]').text() != '')
            RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 8 && $('#errormessage' + paramtext).find('label[id=\"errortext8\"]').text() != '')
            RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
    }

    function RemoveDisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline) {
        if (paramcharsperline <= parammaxcharsperline) {

            $('#errormessage' + paramtext).find("label[id=\"errortext" + paramlinecount + "\"]").remove();
            $('#errormessage1').next().find('br').remove();
        }
    }

    function LineCharMessages(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline) {

        if (paramlinecount == 1 && $('#errormessage' + paramtext).find('label[id=\"errortext1\"]').text() == '')
            DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 2 && $('#errormessage' + paramtext).find('label[id=\"errortext2\"]').text() == '')
            DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 3 && $('#errormessage' + paramtext).find('label[id=\"errortext3\"]').text() == '')
            DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 4 && $('#errormessage' + paramtext).find('label[id=\"errortext4\"]').text() == '')
            DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 5 && $('#errormessage' + paramtext).find('label[id=\"errortext5\"]').text() == '')
            DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 6 && $('#errormessage' + paramtext).find('label[id=\"errortext6\"]').text() == '')
            DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 7 && $('#errormessage' + paramtext).find('label[id=\"errortext7\"]').text() == '')
            DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
        else if (paramlinecount == 8 && $('#errormessage' + paramtext).find('label[id=\"errortext8\"]').text() == '')
            DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline);
    }

    function DisplayMessage(paramtext, paramlinecount, paramcharsperline, parammaxcharsperline) {

        if (paramcharsperline > parammaxcharsperline) {
            var smaxmessage = sMaxCharErrorMsg.replace('<linecount>', paramlinecount);
            $('#errormessage' + paramtext).append("<label id=\"errortext" + paramlinecount + "\" class=\"errortext\">" + smaxmessage + "</label><br/>");
        }
        else
            $('#errormessage' + paramtext).find("label[id=\"errortext" + paramlinecount + "\"]").remove();
    }
