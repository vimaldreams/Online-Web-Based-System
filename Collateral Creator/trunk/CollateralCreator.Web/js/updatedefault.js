var ChannelPartnerID;
var LoginID;
var Addressline1;
var Addressline2;
var Addressline3;
var Town;
var State;
var Country;
var PostCode;
var Type;

var ChannelPartnerInfoIDs;
var CompanyName;
var WebPage;
var PhoneNumber;

function UpdateAddressDialog() {

    $('#tbContactDetails').find("input[id=\"txtAddress1\"]").val($('#hiddentxtAddressline1').val());
    $('#tbContactDetails').find("input[id=\"txtAddress2\"]").val($('#hiddentxtAddressline2').val());
    $('#tbContactDetails').find("input[id=\"txtAddress3\"]").val($('#hiddentxtAddressline3').val());
    $('#tbContactDetails').find("input[id=\"txtTown\"]").val($('#hiddentxtTown').val());
    $('#tbContactDetails').find("input[id=\"txtState\"]").val($('#hiddentxtState').val());
    $('#tbContactDetails').find("input[id=\"txtCountry\"]").val($('#hiddentxtCountry').val());
    $('#tbContactDetails').find("input[id=\"txtPostcode\"]").val($('#hiddentxtPostcode').val());

    $('.address_dialogbox').attr("title", sContactTitle);

    jQuery(".address_dialogbox").dialog({
        autoOpen: true,
        closeOnEscape: false,
        width: 400,
        height: 320,
        modal: true,
        draggable: false,
        resizable: false
    });
}

function UpdateCustomDialog() {

    $('#tbCustomDetails').find("input[id=\"txtCompanyname\"]").val($('#hiddenCompanyname').val());
    $('#tbCustomDetails').find("input[id=\"txtWebUrl\"]").val($('#hiddenWebUrl').val());
    $('#tbCustomDetails').find("input[id=\"txtPhonenumber\"]").val($('#hiddenPhonenumber').val());

    $('.custom_dialogbox').attr("title", sCustomTitle);

    jQuery(".custom_dialogbox").dialog({
        autoOpen: true,
        closeOnEscape: false,
        width: 400,
        height: 200,
        modal: true,
        draggable: false,
        resizable: false
    });

}

function SaveAddressDialog() {
    
    Addressline1 = $('#txtAddress1').val();
    Addressline2 = $('#txtAddress2').val();
    Addressline3 = $('#txtAddress3').val();
    Town = $('#txtTown').val();
    State = $('#txtState').val();
    Country = $('#txtCountry').val();
    PostCode = $('#txtPostcode').val();

    jQuery.ajax({
        type: "POST",
        url:  sRootPath + "services/CollateralHome.asmx/UpdateContactDetails",
        cache: false,
        data: JSON.stringify({sAddressLine1: Addressline1, sAddressLine2: Addressline2, sAddressLine3: Addressline3, sTown: Town, sState: State, sCountry: Country, sPostCode: PostCode}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response) {
                jQuery(".address_dialogbox").dialog("close");
                alert(sContactSuccessMsg);
                RebuildContactDetails(response.Address);
            }
        }
    });

}

function SaveCustomDialog() {

    ChannelPartnerInfoIDs = $('#hiddenChannelPartnerInfoID').val();
    LoginID = $('#hiddentxtLoginID').val();
    CompanyName = $('#txtCompanyname').val();
    WebPage = $('#txtWebUrl').val();
    PhoneNumber = $('#txtPhonenumber').val();

    jQuery.ajax({
        type: "POST",
        url: sRootPath + "services/CollateralHome.asmx/UpdateCustomInfoDetails",
        cache: false,
        data: JSON.stringify({ sChannelPartnerInfoID: ChannelPartnerInfoIDs, sLoginID: LoginID, sCompanyName: CompanyName, sWebPage: WebPage, sPhoneNumber: PhoneNumber }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
            if (response) {
                jQuery(".custom_dialogbox").dialog("close");
                alert(sContactSuccessMsg);
                RebuildCustomInfoDetails();
            }
        }
    });
}

function CancelAddressDialog() {
    jQuery(".address_dialogbox").dialog("close");
}

function CancelCustomDialog() {
    jQuery(".custom_dialogbox").dialog("close");
}

function RebuildCustomInfoDetails() {

    var custominfotdetails = new String();

    if ($('#txtCompanyname').val() != '')
        $('#hiddenCompanyname').val($('#txtCompanyname').val());
    if ($('#txtWebUrl').val() != '')
        $('#hiddenWebUrl').val($('#txtWebUrl').val());
    if ($('#txtPhonenumber').val() != '')
        $('#hiddenPhonenumber').val($('#txtPhonenumber').val());

    if ($('#txtCompanyname').val() != '')
        custominfotdetails += $('#txtCompanyname').val();
    if ($('#txtPhonenumber').val() != '')
        custominfotdetails += "\n" + $('#txtPhonenumber').val();
    if ($('#txtWebUrl').val() != '')
        custominfotdetails += "\n" + $('#txtWebUrl').val();

    $('#txtCustomDetails').val(custominfotdetails);

    LoadHomePage();

    if ($('#hiddentxtAddressline1').val() != '' && $('#hiddentxtAddressline2').val() != '' && $('#hiddentxtAddressline3').val() != ''
            && $('#hiddentxtTown').val() != '' && $('#hiddentxtState').val() != '' && $('#hiddentxtCountry').val() != '' && $('#hiddentxtPostcode').val() != ''
            && $('#txtCompanyname').val() != '' && $('#txtPhonenumber').val() != '' && $('#txtWebUrl').val() != '')
        $('#litNoDetailMessage').text('');
}

function RebuildContactDetails(paramAddress) {

    if (paramAddress != null) {
        if (paramAddress.AddressLine1 != '')
            $('#hiddentxtAddressline1').val(paramAddress.AddressLine1);
        if (paramAddress.AddressLine2 != '')
            $('#hiddentxtAddressline2').val(paramAddress.AddressLine2);
        if (paramAddress.AddressLine3 != '')
            $('#hiddentxtAddressline3').val(paramAddress.AddressLine3);
        if (paramAddress.Town != '')
            $('#hiddentxtTown').val(paramAddress.Town);
        if (paramAddress.State != '')
            $('#hiddentxtState').val(paramAddress.State);
        if (paramAddress.Country != '')
            $('#hiddentxtCountry').val(paramAddress.Country);
        if (paramAddress.PostCode != '')
            $('#hiddentxtPostcode').val(paramAddress.PostCode);

        var contactdetails = new String();

        if (paramAddress.AddressLine1 != '')
            contactdetails += paramAddress.AddressLine1;
        if (paramAddress.AddressLine2 == '' && paramAddress.AddressLine3 == '' && paramAddress.Town.length == 2)
            contactdetails += ', ' + paramAddress.Town;
        if (paramAddress.AddressLine2 != '')
            contactdetails += "\n" + paramAddress.AddressLine2;
        if (paramAddress.AddressLine3 == '' && paramAddress.Town.length == 2)
            contactdetails += ', ' + paramAddress.Town;
        if (paramAddress.AddressLine3 != '')
            contactdetails += "\n" + paramAddress.AddressLine3;
        if (paramAddress.Town != '' && paramAddress.Town.length > 2)
            contactdetails += "\n" + paramAddress.Town;
        if (paramAddress.State != '' && paramAddress.State.length == 2)
            contactdetails += ', ' + paramAddress.State;
        if (paramAddress.State != '' && paramAddress.State.length > 2)
            contactdetails += "\n" + paramAddress.State;
        if (paramAddress.Country != '' && paramAddress.Country.length == 2)
            contactdetails += ', ' + paramAddress.Country;
        if (paramAddress.Country != '' && paramAddress.Country.length > 2)
            contactdetails += "\n" + paramAddress.Country;
        if (paramAddress.PostCode != '')
            contactdetails += "\n" + paramAddress.PostCode;

        $('#txtContactDetails').val(contactdetails);
        
        LoadHomePage();       

        if ($('#hiddentxtAddressline1').val() != '' && $('#hiddentxtAddressline2').val() != '' && $('#hiddentxtAddressline3').val() != ''
            && $('#hiddentxtTown').val() != '' && $('#hiddentxtState').val() != '' && $('#hiddentxtCountry').val() != '' && $('#hiddentxtPostcode').val() != '' 
            && $('#txtCompanyname').val() != '' && $('#txtPhonenumber').val() != '' && $('#txtWebUrl').val() != '')
            $('#litNoDetailMessage').text('');
    }    
}

function LoadHomePage() {

    $("#productoptions").find("div[class=\"product-category\"]").find("span[class=\"producttext enableitemtext\"]").removeClass('enableitemtext');
   
    var nodeID;
    //load menu tree templates after successfully updated the contact details
    $("#productoptions").find("div[class=\"product-category\"] > span").each(function () {

        nodeID = $("#productoptions").find("div[class=\"product-category\"] > span").attr("nodeid");

        $("#productoptions").find("div[class=\"product-category\"]").find("span[nodeid=\"" + nodeID + "\"]").addClass('enableitemtext');

        //default selected product
        $("#lblselecteditem").text($("#productoptions").find("div[class=\"product-category\"]").find("span[nodeid=\"" + nodeID + "\"]").text());

        return;
    });

    LoadMenuTreeTemplates(nodeID);

    return;
}