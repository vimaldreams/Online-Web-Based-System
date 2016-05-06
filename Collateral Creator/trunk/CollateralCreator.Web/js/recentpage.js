jQuery(document).ready(function () {

    //Check Address, Custom, Logo Details
    if (sNoDetailsFlag == 'Error') {
        $('#litNoDetailMessage').html(sNoDetailMessage).text();
    }
    
    //set the document div's height initially to auto 
    jQuery('.recentdocumentsection').css('height', 'auto');
    jQuery('.recentactivity').css('height', 'auto');

    //set the print activity div's height initially to auto 
    jQuery('.printdocumentsection').css('height', 'auto');
    jQuery('.printhouseactivity').css('height', 'auto');
    
    LoadDocuments();
    LoadPrintHouseDocuments();
});

function LoadDocuments() {

    jQuery.ajax({
        type: "POST",
        url: sRootPath + "services/CollateralHome.asmx/GetLatestDocuments",
        cache: false,
        data: JSON.stringify({ sCountry : sCountry }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response) {
                if (response.LatestDocuments.length == 0) {
                    BuildDocumentSection(response.LatestDocuments);
                    $('.norecentdocumentlabel').text('No documents have been created recently.');
                    $('.view-more-link').hide();
                } else {
                    $('.norecentdocumentlabel').text('');
                    BuildDocumentSection(response.LatestDocuments);
                }
            }
        }
    });
}

function ExpandDiv() {

    var img = jQuery('.view-more-link').find("img").attr('src');

    if (jQuery(".viewmorelesslink[viewmore]").length > 0) { //viewmore state so the actions is to open the div
        jQuery(".viewmorelesslink").find("div[viewmore]").hide();
        jQuery(".viewmorelesslink").find("div[viewless]").show();
        jQuery("#tbdocumentactions").find("tr[id=\"trouterdocumentaction\"]").show('slow');
        if (img == sRootPath + "images/portal/icons/plus.gif")
            jQuery('.view-more-link').find("img").attr('src', sRootPath + 'images/portal/icons/minus.gif');

        if (today == 0 && lastweek == 0) {

            jQuery('.recentdocumentsection').css('height', '200px');
            jQuery('.recentdocumentsection').css('overflow', 'auto');
        }
        else {
            jQuery('.recentdocumentsection').css('height', recentdocumentdivheight + 'px');
            jQuery('.recentdocumentsection').css('overflow', 'auto');
        }

        jQuery(".viewmorelesslink").removeAttr("viewmore").attr("viewless", "viewless");
    }
    else { //viewless state so the actions is to hide the div
        jQuery(".viewmorelesslink").find("div[viewmore]").show();
        jQuery(".viewmorelesslink").find("div[viewless]").hide();
        jQuery("#tbdocumentactions").find("tr[id=\"trouterdocumentaction\"]").hide();

        if (img == sRootPath + "images/portal/icons/minus.gif")
            jQuery('.view-more-link').find("img").attr('src', sRootPath + 'images/portal/icons/plus.gif');
        jQuery(".viewmorelesslink").removeAttr("viewless").attr("viewmore", "viewmore");

        jQuery('.recentdocumentsection').css('height', 'auto');
        jQuery('.recentdocumentsection').css('overflow', 'hidden');
        jQuery('.recentactivity').css('height', 'auto');
    }
}

var lastweek = 0; var today = 0; var older = 0; var recentdocumentdivheight;

function BuildDocumentSection(paramdocuments) {

    if (paramdocuments.length == 0)
        $('.view-more-link').hide();
    else
        $('.view-more-link').show();

    //remove the documents before building 
    jQuery(".tbmentiondocument tr").remove(); //find("tr[class=\"trdocumentaction\"]")

    for (i = 0; i < paramdocuments.length; i++) {

        var dateheader;
        //compare todays date with Json date - append if it matches with today's date as today
        if (CompareTodaysDate(paramdocuments[i].ModifiedDate)) {

            if (today == 0) {
                dateheader = "<tr><td><strong><label id=\"lbldateheader\" class=\"dateheader\">" + sTodaysDateHeader + ":</label></strong></td></tr>";
                today++;
            }
            else { dateheader = ""; }

            var buildrow1 = "<tr id=\"trinnerdocumentaction\" class=\"trdocumentaction\">";
            var buildrow2;
            if (paramdocuments[i].PartnerBranded)
                buildrow2 = "<td class=\"tddocumentname\"><label id=\"lbldocumentname\">" + paramdocuments[i].Name + " - " + sPartner + "</label></td>";
            else
                buildrow2 = "<td class=\"tddocumentname\"><label id=\"lbldocumentname\">" + paramdocuments[i].Name + " - " + sXerox + "</label></td>";

            var buildrow3 = "<td class=\"tdlinkactionname\">";
            var buildrow4 = "<a id=\"lnkview\" onclick=\"PreviewDocument(" + paramdocuments[i].DocumentID + ");\" class=\"lnkdocumentaction\"><label id=\"lblViewButton\">" + sBtnView + "</label></a> | ";

            //check whether the document has sent to print - correspondingly 1-New 2-Submitted 3-Received 4-Dispatched
            var buildrow5;
            if (paramdocuments[i].DocumentStateID == 3)
                buildrow5 = "<label id=\"lblEditButton\">" + sDocumentStatus + "</label> | ";
            else
                buildrow5 = "<a id=\"lnkEdit\" onclick=\"GotoEditPage(" + paramdocuments[i].DocumentID + ", 'edit');\" class=\"lnkdocumentaction\"><label id=\"lblEditButton\">" + sBtnEdit + "</label></a> | ";

            var buildrow6 = "<a id=\"lnkclone\" onclick=\"GotoEditPage(" + paramdocuments[i].DocumentID + ", 'clone');\" class=\"lnkdocumentaction\"><label id=\"lblCloneButton\">" + sBtnClone + "</label></a> | ";
            var buildrow7 = "<a id=\"lnkdelete\" onclick=\"return DeleteDocument(" + paramdocuments[i].DocumentID + ");\" class=\"lnkdocumentaction\"><label id=\"lblDeleteButton\">" + sBtnDelete + "</label></a> ";
            var buildrow8 = "</td>";
            var buildrow9 = "</tr>";
            var documentrow = dateheader + buildrow1 + buildrow2 + buildrow3 + buildrow4 + buildrow5 + buildrow6 + buildrow7 + buildrow8 + buildrow9;
            jQuery('#tbdocumentactions').append(documentrow);
        }
        //compare todays date with Json date - append if it matches with last 6 day's date as lastweek
        else if (CompareLastWeekDate(paramdocuments[i].ModifiedDate)) {

            if (lastweek == 0) {
                dateheader = "<tr><td><strong><label id=\"lbldateheader\" class=\"dateheader\">" + sLastWeekDateHeader + ":</label></strong></td></tr>";
                lastweek++;
            }
            else { dateheader = ""; }

            var buildrow1 = "<tr id=\"trinnerdocumentaction\" class=\"trdocumentaction\">";

            var buildrow2;
            if (paramdocuments[i].PartnerBranded)
                buildrow2 = "<td class=\"tddocumentname\"><label id=\"lbldocumentname\">" + paramdocuments[i].Name + " - " + sPartner + "</label></td>";
            else
                buildrow2 = "<td class=\"tddocumentname\"><label id=\"lbldocumentname\">" + paramdocuments[i].Name + " - " + sXerox + "</label></td>";

            var buildrow3 = "<td class=\"tdlinkactionname\">";
            var buildrow4 = "<a id=\"lnkview\" onclick=\"PreviewDocument(" + paramdocuments[i].DocumentID + ");\" class=\"lnkdocumentaction\"><label id=\"lblViewButton\">" + sBtnView + "</label></a> | ";

            //check whether the document has sent to print - correspondingly 1-New 2-Submitted 3-Received 4-Dispatched
            var buildrow5;
            if (paramdocuments[i].DocumentStateID == 2)
                buildrow5 = "<label id=\"lblEditButton\">" + sDocumentStatus + "</label> | ";
            else
                buildrow5 = "<a id=\"lnkEdit\" onclick=\"GotoEditPage(" + paramdocuments[i].DocumentID + ", 'edit');\" class=\"lnkdocumentaction\"><label id=\"lblEditButton\">" + sBtnEdit + "</label></a> | ";

            var buildrow6 = "<a id=\"lnkclone\" onclick=\"GotoEditPage(" + paramdocuments[i].DocumentID + ", 'clone');\" class=\"lnkdocumentaction\"><label id=\"lblCloneButton\">" + sBtnClone + "</label></a> | ";
            var buildrow7 = "<a id=\"lnkdelete\" onclick=\"return DeleteDocument(" + paramdocuments[i].DocumentID + ");\" class=\"lnkdocumentaction\"><label id=\"lblDeleteButton\">" + sBtnDelete + "</label></a> ";
            var buildrow8 = "</td>";
            var buildrow9 = "</tr>";
            var documentrow = dateheader + buildrow1 + buildrow2 + buildrow3 + buildrow4 + buildrow5 + buildrow6 + buildrow7 + buildrow8 + buildrow9;
            jQuery('#tbdocumentactions').append(documentrow);
        }
        // append if it doesn't matches with the above dates as older
        else {
            if (older == 0) {
                dateheader = "<tr id=\"trouterdocumentaction\"><td><strong><label id=\"lbldateheader\" class=\"dateheader\">" + sPreviousDateHeader + ":</label></strong></td></tr>";
                older++;
            }
            else { dateheader = ""; }
            var buildrow1 = "<tr id=\"trouterdocumentaction\" class=\"trdocumentaction\">";

            var buildrow2;
            if (paramdocuments[i].PartnerBranded)
                buildrow2 = "<td class=\"tddocumentname\"><label id=\"lbldocumentname\">" + paramdocuments[i].Name + " - " + sPartner + "</label></td>";
            else
                buildrow2 = "<td class=\"tddocumentname\"><label id=\"lbldocumentname\">" + paramdocuments[i].Name + " - " + sXerox + "</label></td>";

            var buildrow3 = "<td class=\"tdlinkactionname\">";
            var buildrow4 = "<a id=\"lnkview\" onclick=\"PreviewDocument(" + paramdocuments[i].DocumentID + ");\" class=\"lnkdocumentaction\"><label id=\"lblViewButton\">" + sBtnView + "</label></a> | ";

            //check whether the document has sent to print - correspondingly 1-New 2-Submitted 3-Received 4-Dispatched
            var buildrow5;
            if (paramdocuments[i].DocumentStateID == 2)
                buildrow5 = "<label id=\"lblEditButton\">" + sDocumentStatus + "</label> | ";
            else
                buildrow5 = "<a id=\"lnkEdit\" onclick=\"GotoEditPage(" + paramdocuments[i].DocumentID + ", 'edit');\" class=\"lnkdocumentaction\"><label id=\"lblEditButton\">" + sBtnEdit + "</label></a> | ";

            var buildrow6 = "<a id=\"lnkclone\" onclick=\"GotoEditPage(" + paramdocuments[i].DocumentID + ", 'clone');\" class=\"lnkdocumentaction\"><label id=\"lblCloneButton\">" + sBtnClone + "</label></a> | ";
            var buildrow7 = "<a id=\"lnkdelete\" onclick=\"return DeleteDocument(" + paramdocuments[i].DocumentID + ");\" class=\"lnkdocumentaction\"><label id=\"lblDeleteButton\">" + sBtnDelete + "</label></a> ";
            var buildrow8 = "</td>";
            var buildrow9 = "</tr>";
            var documentrow = dateheader + buildrow1 + buildrow2 + buildrow3 + buildrow4 + buildrow5 + buildrow6 + buildrow7 + buildrow8 + buildrow9;
            jQuery('#tbdocumentactions').append(documentrow);
        }
    }

    //hide the outerdocument row initially 
    jQuery("#tbdocumentactions").find("tr[id=\"trouterdocumentaction\"]").hide();
    recentdocumentdivheight = $('.recentdocumentsection').height();

}

function DeleteDocument(param_documentid) {
    var answer = confirm(sConfirmDeleteMsg)
    if (answer) {

        jQuery.ajax({
            type: "POST",
            url: sRootPath + "services/CollateralHome.asmx/DeleteDocumentByID",
            cache: false,
            data: JSON.stringify({ iDocumentID: param_documentid }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                if (response) {

                    //set the document div's height initially to auto 
                    jQuery('.recentdocumentsection').css('height', 'auto');
                    jQuery('.recentactivity').css('height', 'auto');

                    //Reload the page
                    lastweek = 0; today = 0; older = 0;
                    recentdocumentdivheight; dateheader = "";
                    
                    jQuery('.recentdocumentsection').css('height', 'auto');
                    jQuery('.recentactivity').css('height', 'auto');
                    LoadDocuments();
                    countname = 0;
                    LoadPrintHouseDocuments();
                }
            }
        });
    }
    else
        return false;
}

function GotoEditPage(param_documentid, param_action) {

    window.location.href = sRootPath + "Edit.aspx?documentid=" + param_documentid + "&action=" + param_action;
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

function CompareLastWeekDate(jsondate) {

    jsondate = jsondate.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
    }
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }

    var date = new Date(parseInt(jsondate, 10));
    var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

    //get today's date
    var today = new Date();
    var todaymonth = today.getMonth() + 1 < 10 ? "0" + (today.getMonth() + 1) : today.getMonth() + 1;
    var todaycurrentDate = today.getDate() < 10 ? "0" + today.getDate() : today.getDate();

    var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    var documentdate = new Date(date.getFullYear(), month, currentDate);
    var todaydate = new Date(today.getFullYear(), todaymonth, todaycurrentDate);

    var diffDays = Math.abs((documentdate.getTime() - todaydate.getTime()) / (oneDay));

    //compare today's date is less than a week with the Json date format
    if (diffDays <= 6)
        return true;
    else
        return false;

}

function LoadPrintHouseDocuments() {

    jQuery.ajax({
        type: "POST",
        url: sRootPath + "services/CollateralHome.asmx/GetPrintHouseActivityDocuments",
        cache: false,
        data: JSON.stringify({ sCountry: sCountry }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response) {
                if (response.PrintHouseDocuments.length == 0) {
                    BuildPrintHouseDocuments(response.PrintHouseDocuments);
                    $('.noprinthouselabel').text('No documents have been sent to print recently.');
                    $('.pview-more-link').hide();
                } else {
                    $('.noprinthouselabel').text('');
                    BuildPrintHouseDocuments(response.PrintHouseDocuments);
                }
            }
        }
    });
}

var documentid; var countname = 0; var docname1; var docname2; var displaycount = 1; var printdocumentdivheight;

function BuildPrintHouseDocuments(paramdocuments) {

    if (paramdocuments.length == 0)
        $('.pview-more-link').hide();
    else
        $('.pview-more-link').show();

    //remove the documents before building 
    jQuery(".tbprintdocument tr").remove(); 

    for (i = 0; i < paramdocuments.length; i++) {

        //logic to find the unique document id to display in the format mentioned
        if (i == 0)
            documentid = paramdocuments[i].DocumentID;
        if (documentid != paramdocuments[i].DocumentID) {
            countname = 0;
            displaycount++;
            documentid = paramdocuments[i].DocumentID;
        }

        if (displaycount <= 2) {
            if (documentid == paramdocuments[i].DocumentID) {
                if (countname == 0) {
                    if (paramdocuments[i].PartnerBranded)
                        docname1 = "<tr id=\"trinnerprintdocument\"><td class=\"tdprintdocumentname\"><label>" + paramdocuments[i].Name + " - " + sPartner + "</label></td>";
                    else
                        docname1 = "<tr id=\"trinnerprintdocument\"><td class=\"tdprintdocumentname\"><label>" + paramdocuments[i].Name + " - " + sXerox + "</label></td>";

                    docname2 = "<td class=\"tdprintdocumentview\"><a id=\"lnkview\" onclick=\"PreviewDocument(" + paramdocuments[i].DocumentID + ", 'view');\"><label>" + sBtnView + "</label></a></td></tr>";
                    countname++;
                }
                else {
                    docname1 = ""; docname2 = "";
                }
                var printhistory1 = "<tr id=\"trinnerprintdocument\" class=\"trprinthistory\"><td>";
                //date stamp should be in the format like 2/Feb/2011
                var printhistory2 = "<label>" + GetPrintFormatdate(paramdocuments[i].datestamp) + "</label> : <label>" + paramdocuments[i].DocumentStateName + "</label></td></tr>";

                var printdocumentrow = docname1 + docname2 + printhistory1 + printhistory2;
                jQuery('#tbprintdocument').append(printdocumentrow);
            }
        }
        else {
            if (documentid == paramdocuments[i].DocumentID) {
                if (countname == 0) {
                    if (paramdocuments[i].PartnerBranded)
                        docname1 = "<tr id=\"trouterprintdocument\"><td class=\"tdprintdocumentname\"><label>" + paramdocuments[i].Name + " - " + sPartner + "</label></td>";
                    else
                        docname1 = "<tr id=\"trouterprintdocument\"><td class=\"tdprintdocumentname\"><label>" + paramdocuments[i].Name + " - " + sXerox + "</label></td>";

                    docname2 = "<td><a id=\"lnkview\" onclick=\"PreviewDocument(" + paramdocuments[i].DocumentID + ", 'view');\"><label>" + sBtnView + "</label></a></td></tr>";
                    countname++;
                }
                else {
                    docname1 = ""; docname2 = "";
                }
                var printhistory1 = "<tr id=\"trouterprintdocument\" class=\"trprinthistory\"><td>";
                //date stamp should be in the format like 2/Feb/2011
                var printhistory2 = "<label>" + GetPrintFormatdate(paramdocuments[i].datestamp) + "</label> : <label>" + paramdocuments[i].DocumentStateName + "</label></td></tr>";

                var printdocumentrow = docname1 + docname2 + printhistory1 + printhistory2;
                jQuery('#tbprintdocument').append(printdocumentrow);
            }
        }
    }

    //hide the outerdocument row initially 
    jQuery("#tbprintdocument").find("tr[id=\"trouterprintdocument\"]").hide();
    printdocumentdivheight = $('.printdocumentsection').height();
}

function GetPrintFormatdate(jsondate) {

    jsondate = jsondate.replace("/Date(", "").replace(")/", "");
    if (jsondate.indexOf("+") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
    }
    else if (jsondate.indexOf("-") > 0) {
        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
    }

    var date = new Date(parseInt(jsondate, 10));
    //var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();

    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"];

    return currentDate + '/' + monthNames[date.getMonth()] + '/' + date.getFullYear();
}

function PrintExpandDiv() {

    var img = jQuery('.pview-more-link').find("img").attr('src');

    if (jQuery(".pviewmorelesslink[pviewmore]").length > 0) { //viewmore state so the actions is to open the div
        jQuery(".pviewmorelesslink").find("div[pviewmore]").hide();
        jQuery(".pviewmorelesslink").find("div[pviewless]").show();

        jQuery("#tbprintdocument").find("tr[id=\"trouterprintdocument\"]").show('slow');
        if (img == sRootPath + "images/portal/icons/plus.gif")
            jQuery('.pview-more-link').find("img").attr('src', sRootPath + 'images/portal/icons/minus.gif');

        //var divheight = $('.printdocumentsection').height();
        jQuery('.printdocumentsection').css('height', printdocumentdivheight + 'px');
        jQuery('.printdocumentsection').css('overflow', 'auto');

        jQuery(".pviewmorelesslink").removeAttr("pviewmore").attr("pviewless", "pviewless");
    }
    else { //viewless state so the actions is to hide the div
        jQuery(".pviewmorelesslink").find("div[pviewmore]").show();
        jQuery(".pviewmorelesslink").find("div[pviewless]").hide();

        jQuery("#tbprintdocument").find("tr[id=\"trouterprintdocument\"]").hide();
        if (img == sRootPath + "images/portal/icons/minus.gif")
            jQuery('.pview-more-link').find("img").attr('src', sRootPath + 'images/portal/icons/plus.gif');

        jQuery(".pviewmorelesslink").removeAttr("pviewless").attr("pviewmore", "pviewmore");

        jQuery('.printdocumentsection').css('height', 'auto');
        jQuery('.printdocumentsection').css('overflow', 'hidden');
        jQuery('.printhouseactivity').css('height', 'auto');
    }

}

function PreviewDocument(param_documentid) {

    jQuery('.ajaxloader').dialog({
        autoOpen: true,
        closeOnEscape: false,
        position: 'center', //[300, 50],
        width: 500,
        height: 500,
        modal: true,
        draggable: false,
        resizable: false
    });
    
    //    jQuery("#iframePdfDocument").hide();
    //    jQuery(".dialogPreviewBottom").hide();
    //    jQuery("#divDocumentLoader").show();

    var PdfSrc = sRootPath + 'temp/document' + param_documentid + '.pdf#toolbar=0&statusbar=0&navpanes=0&view=fitH,50&view=fitV,50&view=FitBH,50';
    jQuery("#iframePdfDocument").attr('src', PdfSrc);

    //jQuery("#iframePdfDocument").load(function () {
    //    jQuery("#divDocumentLoader").hide();
    //    jQuery("#iframePdfDocument").show();
    //    jQuery(".dialogPreviewBottom").show();
    //});

    //fade out the web page when dialog box opened
    //$("html").css("overflow", "hidden");
    //$('body').fadeOut('slow');

    jQuery.ajax({
        type: "POST",
        url: sRootPath + "services/CollateralHome.asmx/DocumentGetTemplateName",
        cache: false,
        data: JSON.stringify({ iDocumentID: param_documentid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response) {

                $('.dialogPreviewBox').attr("title", response.TemplateName);
                $(".dialogPreviewBox").dialog("option", "title", response.TemplateName);

                jQuery('.dialogPreviewBox').dialog({
                    autoOpen: true,
                    closeOnEscape: false,
                    //position: [300, 50],
                    width: 600, //$(window).width() * 0.48, 
                    height: 750, //$(window).height() * 0.83, 
                    modal: true,
                    draggable: false,
                    resizable: false
                });
            }
        },
        complete: function () {
            jQuery(".ajaxloader").dialog("close");
        }    
    });
}

function CancelPreviewDialog() {

    jQuery(".dialogPreviewBox").dialog("close");
    jQuery("#iframePdfDocument").removeAttr("src");

    //fade out the web page when dialog box opened
    //$("html").css("overflow", "auto");
    //$('body').fadeIn('fast');
}
