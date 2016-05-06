jQuery(document).ready(function() {

    //load default logo
    LoadDefaultLogoImage();
    
});

function CancelLogoDialog() {
    jQuery(".logo_dialogbox").dialog("close");
    //jQuery("#previewContent").show();
    return false;
}

//For Home and Recent Page
function UpdateLogoDialog() {

    $('.logo_dialogbox').attr("title", "File Upload");
    jQuery('.logo_dialogbox').dialog({
        autoOpen: true,
        closeOnEscape: false,
        width: 300,
        height: 150,
        modal: true,
        draggable: false,
        resizable: false,
        create: function (event, ui) {
            dlg = jQuery("div.ui-dialog").detach();
            jQuery("form").append(dlg);
        }
    });
}

//var imgWidth; var imgHeight; 
var Dialog_div;
function ChangeLogoDialog(ImageIDs, DocumentID, srcObjName) {

    $('.logo_dialogbox').attr("title", "File Upload");
//    $('.logo_dialogbox').find("input[id=\"hiddenImageWidth\"]").val(ImageWidth);
//    $('.logo_dialogbox').find("input[id=\"hiddenImageHeight\"]").val(ImageHeight);
    $('.logo_dialogbox').find("input[id=\"hiddenImageID\"]").val(ImageIDs);
    $('.logo_dialogbox').find("input[id=\"hiddenDocumentID\"]").val(DocumentID);
    $('.logo_dialogbox').find("input[id=\"hiddenSc2ImageScr\"]").val(srcObjName); //~Src2 image object name
   

    $('.logo_dialogbox').dialog({
        autoOpen: true,
        closeOnEscape: false,
        position: [300, 400],
        width: 300,
        height: 150,
        modal: true,
        draggable: false,
        resizable: false,
        create: function (event, ui) {
            dlg = jQuery("div.ui-dialog").detach();
            jQuery("form").append(dlg);
        }
    });
}

function LoadDefaultLogoImage() {
    $('#btnUploadDefaultLogo').text(sbtnUploadText);
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
                $('#ImageLogo').attr('src', sRootPath + 'images/logos/XCT_YourLogo.jpg');
            else
                $('#ImageLogo').attr('src', sRootPath + 'Handlers/ImageHandler.ashx?v=' + new Date().getTime() + '&mode=defaultimage&loginid=' + response.Image[0]);
        }
    });

}


var selectedImage = 'RdBtnMultiImage1';

$("input[type=radio]").click(function () {

    if (document.getElementById('RdBtnMultiImage1').checked) {
        selectedImage = 'RdBtnMultiImage1';
    } else if (document.getElementById('RdBtnMultiImage2').checked) {
        selectedImage = 'RdBtnMultiImage2';
    } else if (document.getElementById('RdBtnMultiImage3').checked) {
        selectedImage = 'RdBtnMultiImage3';
    } else if (document.getElementById('RdBtnMultiImage4').checked) {
        selectedImage = 'RdBtnMultiImage4';
    } else if (document.getElementById('RdBtnMultiImage5').checked) {
        selectedImage = 'RdBtnMultiImage5';
    } else if (document.getElementById('RdBtnMultiImage6').checked) {
        selectedImage = 'RdBtnMultiImage6';
    }
});


function UploadMultiSelectImage(imageAreaID) {   

    var multiImageUrl;

    if (selectedImage == 'RdBtnMultiImage1') {
        multiImageUrl = multiImageUrls[0];
    } else if (selectedImage == 'RdBtnMultiImage2') {
        multiImageUrl = multiImageUrls[1];
    } else if (selectedImage == 'RdBtnMultiImage3') {
        multiImageUrl = multiImageUrls[2];
    } else if (selectedImage == 'RdBtnMultiImage4') {
        multiImageUrl = multiImageUrls[3];
    } else if (selectedImage == 'RdBtnMultiImage5') {
        multiImageUrl = multiImageUrls[4];
    } else if (selectedImage == 'RdBtnMultiImage6') {
        multiImageUrl = multiImageUrls[5];
    }
    
    $.ajax({
        url: sRootPath + "Handlers/WebImageHandler.ashx?imageAreaID=" + imageAreaID + "&imageUrl=" + multiImageUrl,
        success: function (data) { alert(data); },
        error: function (data) { alert(data); }
    });
    
}


$("#btnUploadLogo").click(function (event) {

    if ($('#fileToUpload').val().length > 0) {

        $('#loading').show();

        var ImageIDs = $('#hiddenImageID').val();
        var DocumentId = $('#hiddenDocumentID').val();
        //~MPE 24/04/2013 new scr2 variables
        var mode = $('#inputcollateralmode').val();
        var srcObjName = $('#hiddenSc2ImageScr').val();
      
        //logic to get imageids for all logos
        var imagearray = ImageIDs.split(',');
        var imageUrl;
        var imageUrlid = '';

        imageUrl = sRootPath + 'Handlers/FileUploadHandler.ashx?mode=documentimage';
        for (i = 0; i < imagearray.length; i++) {
            imageUrlid += '&imageid' + i + '=' + imagearray[i];
        }

        $.ajaxFileUpload({
            url: imageUrl + imageUrlid + '&documentid=' + DocumentId + '&end',
            secureuri: false,
            fileElementId: 'fileToUpload',
            dataType: 'json',
            data: {},
            success: function (data, status) {

                if (typeof (data.error) != 'undefined') {

                    if (data.error != '') {
                        alert(data.error);
                    } else {
                        if (mode == 1) { //scr2 mode
                            
                            srcObjName = '#' + srcObjName;
                            $(srcObjName).attr('src', '');           
                            imageUrl = sRootPath + 'Handlers/ScImageHandler.ashx?v=' + new Date().getTime() + '&imageid=' + ImageIDs;
                           
                            $(srcObjName).attr('src', imageUrl);
                            jQuery(".logo_dialogbox").dialog("close");

                            ajaxdownloadflag = 'updated';
                        }
                        else {
                            //update the new logo
                            $('.customizeimages').find("img").attr('src', ''); //[imageid=\"" + ImageID + "\"]

                            //logic to get imageids for all logos
                            var imagearray = ImageIDs.split(',');
                            var imageUrl;
                            var imageUrlid = '';

                            imageUrl = sRootPath + 'Handlers/ImageHandler.ashx?v=' + new Date().getTime() + '&mode=documentimage';
                            for (i = 0; i < imagearray.length; i++) {
                                imageUrlid += '&imageid' + i + '=' + imagearray[i];
                            }

                            $('.customizeimages').find("img").attr('src', imageUrl + imageUrlid);
                            
                            jQuery(".logo_dialogbox").dialog("close");

                            ajaxdownloadflag = 'updated';
                        }
                    }
                }
            },
            complete: function (data, status) {
                $('#loading').hide();
            },
            error: function (data, status, e) {
                alert(e);
            }
        })
        event.preventDefault();
    } 
})

$("#btnUploadDefaultLogo").click(function (event) {

    if ($('#fileLogoUrl').val().length > 0) {
        
        $('#loading').show();

        var LoginId = $('#hiddenLoginID').val();

        $.ajaxFileUpload({
            url: sRootPath + 'Handlers/FileUploadHandler.ashx?mode=defaultimage&loginid=' + LoginId + '&end',
            secureuri: false,
            fileElementId: 'fileLogoUrl',
            dataType: 'json',
            data: { },
            success: function(data, status) {
                if (typeof(data.error) != 'undefined') {
                    if (data.error != '') {
                        alert(data.error);
                    } else {
                        //update the new logo
                        $('#ImageLogo').attr('src', '');
                        $('#ImageLogo').attr('src', sRootPath + 'Handlers/ImageHandler.ashx?v=' + new Date().getTime() + '&mode=defaultimage&loginid=' + LoginId);
                        jQuery(".logo_dialogbox").dialog("close");
                        
                         if ($('#hiddentxtAddressline1').val() != '' && $('#hiddentxtAddressline2').val() != '' && $('#hiddentxtAddressline3').val() != ''
                            && $('#hiddentxtTown').val() != '' && $('#hiddentxtState').val() != '' && $('#hiddentxtCountry').val() != '' && $('#hiddentxtPostcode').val() != ''
                            && $('#txtCompanyname').val() != '' && $('#txtPhonenumber').val() != '' && $('#txtWebUrl').val() != '')
                             $('#litNoDetailMessage').text('');
                    }
                }
            },
            complete: function(data, status) {
                $('#loading').hide();
            },
            error: function(data, status, e) {
                alert(e);
            }
        })
        event.preventDefault();
    }
})

//function GetImageSize() {
//    
//    imgWidth = $('#hiddenImageWidth').val();
//    imgHeight = $('#hiddenImageHeight').val();
//    var img = new Image(document.getElementById('fileLogoChangeUrl'));
//    var width = img.clientWidth;
//    var height = img.clientHeight;
//    
//    if (imgWidth == width && imgHeight == height)
//        return true;
//    else {
//        alert("Invalid Logo Size !! Please upload logo with image size Width: " + imgWidth + " and " + "Height: " + imgHeight);
//        $('#fileLogoChangeUrl').replaceWith('<input id="fileLogoChangeUrl" runat="server" type="file" class="dialogbutton" onchange="return GetImageSize();" />');
//        CancelLogoDialog();
//        }
//}

function FileTypeValidate(param) {
    
    var fileUpload;
    if (param == 'edit')
        fileUpload = $('#fileToUpload').val();
    else if(param == 'home')
        fileUpload = $('#fileLogoUrl').val();

    var extension = fileUpload.substring(fileUpload.lastIndexOf('.')).toLowerCase();
    var ValidFileType = ".jpg , .jpeg, .png , .bmp";

    if (fileUpload.length > 0) {

        if (ValidFileType.toLowerCase().indexOf(extension) < 0) {
            alert("Invalid file type!!");
        }
        else
            return true;
    }
    else {
        alert("please upload file using browse button!!");
    }
    //jQuery('#previewContent').show();
    return false;
}
