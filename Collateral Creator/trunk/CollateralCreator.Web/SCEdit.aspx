<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SCEdit.aspx.cs" Inherits="CollateralCreator.Web.SCEdit" MasterPageFile="~/Templates/XeroxPage.Master"  %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Literal ID="LblSc2Title" runat="server"></asp:Literal>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Javascript" runat="server">

    <script type='text/javascript'>

        var fileDocumentId="<%=DocumentId%>";
        var sTemplateId="<%=TemplateId%>"; 
        var sTemplateBrand='0';
        var customizingTemplate="<%=TemplateName%>";
        var sCLoginId='NA';
        var sType = 'SmartCentre';
        var sNoDetailsFlag ="";
        var PdfSrc;
        var documentType = 'smartcentre';

        var sContactTitle ="<%=GetResourcesString("CustomizeAreaTxt") %>";
        var sConfirmMsg ="<%=GetResourcesString("ConfirmBackText") %>"; 
        var sbtnUploadText ="<%=GetResourcesString("DialogUploadbtnText") %>"; 
       
        var sMaxLineErrorMsg ="<%=GetResourcesString("MaxLineErrorMessage") %>";  
        var sMaxCharErrorMsg ="<%=GetResourcesString("MaxCharErrorMessage") %>";  
        var sNoDetailMessage ="<%=GetResourcesString("NoDetailsLabel") %>";   

        //character count function
        function charcounter(textAreaId, maxchars, labelelement ){
            var len = document.getElementById(textAreaId).value.length;                    
            document.getElementById(labelelement).innerHTML = len;
            return;
        }


        $(document).ready(function () {
            jQuery("#currenttemplatename").val(customizingTemplate);
            PdfSrc = sRootPath + 'temp/document' + fileDocumentId + '.pdf?v=' + new Date().getTime() + '#toolbar=0&statusbar=0&navpanes=0';
            jQuery("#iframePdfPreview").attr('src', PdfSrc);
        });

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
        $('#textareasc1, #textareasc2, #textareasc3, #textareasc4, #textareasc5, #textareasc6, #textareasc7, #textareasc8, #textareasc9, #textareasc10, #textareasc11, #textareasc12, #textareasc13, #textareasc14, #textareasc15, #textareasc16, #textareasc17, #textareasc18, #textareasc19, #textareasc20').focusout(function (e) {

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



    </script>

    <asp:Literal ID="litJavaScript" runat="server"></asp:Literal> 
    <asp:Literal ID="LblSc2JavaScript" runat="server"></asp:Literal>
   <%-- <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/editpage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>    --%>
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/uploadlogopage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script> 
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/jqueryfileupload/ajaxfileupload.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>      
    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Metatags" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager id="ScriptManagerEdit" runat="server"></asp:ScriptManager>

    <div id="mastercontent">
       
        <div class="editdescription" style="margin-bottom:20px;">  
            <asp:Literal ID="litCustomizeMessage" runat="server"></asp:Literal>
            <hr/>
            
           
            <div id="divsmartcentreMessage" runat="server" class="NoDetails">
                <asp:Literal ID="litsmartcentreMessage" runat="server"></asp:Literal>
            </div>

            <h2><asp:Label ID="lblEditHeader" runat="server"></asp:Label></h2>
            <asp:Label ID="lblDocumentName" runat="server"></asp:Label> : 
            <input id="currenttemplatename" type="text" class="currentdocumentname" />
        </div>
        
      
        <div id="editContentsc">   
          
            <div id="customArea" runat="server">
                <asp:Literal ID="litCustomizableTextArea" runat="server"></asp:Literal><br/><br/>   
            </div>
            
        </div>       

        <div id="previewContentsc">
                
            <div id="previewWindowsc">
                <%-- structure for Ajax Loader --%>
                <div class="divajaxloader" style="display: none;">
                    <strong class="loading" id="StrongLoading" runat="server">We are preparing your report, please wait...</strong><br/><br/>
                    <img src="<%=VirtualPathUtility.ToAbsolute("~/images/navigation/loadingAnimation.gif")%>" alt="loadingimage" style="display:block; margin:auto;" />
                </div>
                <iframe id="iframePdfPreview" class="framepdfpreviewsc"></iframe>
            </div>                        
              
            <div id="btnControls">
                <div class="previewbuttons">                    
                    <a title="click to update changes in pdf preview"><asp:Button ID="btnUpdatePreview" runat="server" CssClass="editpagebutton blue" UseSubmitBehavior="false" OnClientClick="return UpdatePreviewDocument();" ></asp:Button></a>&nbsp;&nbsp;&nbsp;&nbsp;
                    
                </div>
                <div id="orderbuttons" class="orderbuttons" runat="server">
                    <a title="click update preview to view the changes"><asp:Button ID="btnDownload" runat="server" ClientIDMode="Static" CssClass="editpagebutton blue" UseSubmitBehavior="false" OnClientClick="return DownloadDocument();"></asp:Button></a>&nbsp;&nbsp;&nbsp;&nbsp;
                    
                </div>
              
             </div>           
        </div>
       
        <!--Smart Centre 2 custom image area -->
       
        <div style=" background-color: white; overflow:auto; height:auto; width: 100%; padding-top:10px; border-top:1px dotted #666">
                <asp:Panel ID="PnlImage1" runat="server" Visible="false"  >
                        <asp:Label ID="LabelImageArea1" runat="server" CssClass="lblCoverAddress"></asp:Label>
                        <asp:Label ID="SubLabelImage1" runat="server" CssClass="lblCoverAddress" style="font-size:10px"></asp:Label>
                        <br />
                         <span id="SpanImageRemoval1" runat="server" style="font-size:13px">    
                            <label for="ImageRemoval1">    
                                <input type="checkbox" id="ChkImageRemoval1" runat="server" value="true" />
                                 Remove image (click the update preview button to complete the process)
                            </label>
                       </span>
                        <br />
                        <br />
                        <img id="CustomImage1" runat="server" alt="Custom Image 1" src="" imageid="<%ImageAreaID1%>" clientidmode="Static" />
                        <br /><br /><br />
                        <input type="button" id="InputChangeLogo1" runat="server" class="changeTemplatelogo blue"  />
                       
                        <br />
                        <hr style="color:gray; height:1px;" />  
                        
                </asp:Panel>
                <asp:Panel ID="PnlImage2" runat="server" Visible="false" >
                        <asp:Label ID="LabelImageArea2" runat="server" CssClass="lblCoverAddress"></asp:Label>
                        <asp:Label ID="SubLabelImage2" runat="server"  CssClass="lblCoverAddress" style="font-size:10px"></asp:Label>
                        <br />
                        <span id="SpanImageRemoval2" runat="server" style="font-size:13px">    
                            <label for="ImageRemoval2">
                                   <input type="checkbox" id="ChkImageRemoval2" runat="server" value="true" />
                                    Remove image (click the update preview button to complete the process)
                            </label>
                         </span>
                        <br /> <br />
                        <img id="CustomImage2" runat="server" alt="Custom Image 2" src="" imageid="<%ImageAreaID2%>" clientidmode="Static" />
                         <br />
                        <input type="button" id="InputChangeLogo2" runat="server" class="changeTemplatelogo blue"/>
                      
                        <br />
                        <hr style="color:gray; height:1px;" /> 
                </asp:Panel>
                 <asp:Panel ID="PnlImage3" runat="server" Visible="false" >
                        <asp:Label ID="LabelImageArea3" runat="server" CssClass="lblCoverAddress"></asp:Label>
                        <asp:Label ID="SubLabelImage3" runat="server"  CssClass="lblCoverAddress" style="font-size:10px"></asp:Label>
                        <span id="SpanImageRemoval3" runat="server" style="font-size:13px">        
                            <label for="ImageRemoval3">
                                   <input type="checkbox" id="ChkImageRemoval3" runat="server" value="true"  />
                                      Remove image (click the update preview button to complete the process)
                            </label>
                         </span>
                        <br />
                        <img id="CustomImage3" runat="server" alt="Custom Image 3" src="" imageid="<%ImageAreaID3%>" clientidmode="Static" />
                       <br /><br /><br />
                        <input type="button" id="InputChangeLogo3" runat="server" class="changeTemplatelogo blue"/>
                        
                       
                        <br />
                        <hr style="color:gray; height:1px;" /> 
                </asp:Panel>
                <asp:Panel ID="PnlImage4" runat="server" Visible="false" >
                        <asp:Label ID="LabelImageArea4" runat="server" CssClass="lblCoverAddress"></asp:Label>
                        <asp:Label ID="SubLabelImage4" runat="server"  CssClass="lblCoverAddress" style="font-size:10px"></asp:Label>
                         <br /> 
                         <span id="SpanImageRemoval4" runat="server" style="font-size:13px">    
                            <label for="ImageRemoval4">
                                   <input type="checkbox" id="ChkImageRemoval4" runat="server" value="true"  />
                                    Remove image (click the update preview button to complete the process)
                            </label>
                         </span>
                        <br /> <br />
                        <img id="CustomImage4" runat="server" alt="Custom Image 4" src="" imageid="<%ImageAreaID4%>" clientidmode="Static" />
                        <br /><br /><br />
                        <input type="button" id="InputChangeLogo4" runat="server" class="changeTemplatelogo blue" />
                        
                  
                        <br />
                        <hr style="color:gray; height:1px;" /> 
                </asp:Panel>
                <asp:Panel ID="PnlImage5" runat="server" Visible="false" >
                        <asp:Label ID="LabelImageArea5" runat="server" CssClass="lblCoverAddress"></asp:Label>
                        <asp:Label ID="SubLabelImage5" runat="server"  CssClass="lblCoverAddress" style="font-size:10px"></asp:Label>
                         <br /> 
                         <span id="SpanImageRemoval5" runat="server" style="font-size:13px">    
                            <label for="ImageRemoval5">
                                   <input type="checkbox" id="ChkImageRemoval5" runat="server" value="true"  />
                                    Remove image (click the update preview button to complete the process)
                            </label>
                         </span>
                        <br /> <br />
                        <img id="CustomImage5" runat="server" alt="Custom Image 5" src="" imageid="<%ImageAreaID5%>" clientidmode="Static" />
                        <br /><br /><br />
                        <input type="button" id="InputChangeLogo5" runat="server" class="changeTemplatelogo blue"/>
                        
                  
                        <br />
                        <hr style="color:gray; height:1px;" /> 
                </asp:Panel>
                <asp:Panel ID="PnlImage6" runat="server" Visible="false" >
                        <asp:Label ID="LabelImageArea6" runat="server" CssClass="lblCoverAddress"></asp:Label>
                        <asp:Label ID="SubLabelImage6" runat="server"  CssClass="lblCoverAddress" style="font-size:10px"></asp:Label>
                         <br /> 
                         <span id="SpanImageRemoval6" runat="server" style="font-size:13px">    
                            <label for="ImageRemoval6">
                                   <input type="checkbox" id="ChkImageRemoval6" runat="server" value="true"  />
                                    Remove image (click the update preview button to complete the process)
                            </label>
                         </span>
                        <br /> <br />
                        <img id="CustomImage6" runat="server" alt="Custom Image 6" src="" imageid="<%ImageAreaID6%>" clientidmode="Static" />
                        <br /><br /><br />
                        <input type="button" id="InputChangeLogo6" runat="server" class="changeTemplatelogo blue" />
                        
                  
                        <br />
                        <hr style="color:gray; height:1px;" /> 
                </asp:Panel>

            </div> 
        <!-- End of SC2 image area -->

       <%-- structure for preview window --%>
        <div class="dialogPreviewBox" style="display: none;">
            <div id="divDocumentLoader" style="display: none;" >
                <img src="<%=VirtualPathUtility.ToAbsolute("~/images/navigation/loadingAnimation.gif")%>" alt="loadingimage" style="display:block; margin:auto;" />
            </div>
            <div>
                <iframe id="iframePdfDocument" class="framepdfdocument"></iframe>            
            </div>
            <div class="dialogPreviewBottom" style="display:block; margin:auto;">
                <hr class="userpanel"/>            
               <a id="DialogBoxCancelButton" class="dialogclosebutton" href="javascript:CancelPreviewDialog();"><%=GetResourcesString("DialogCancelbtnText") %></a>
            </div>  
        </div>
       
        <%-- structure for upload logo panel --%>             
        <div id="logo_dialogbox" runat="server" class="logo_dialogbox" style="display: none;">     
                            
            <input id="fileToUpload" type="file" size="25" ClientIDMode="Static" name="fileToUpload" class="input" /><br/><br/><br/>
           
            <img id="loading" src="<%=VirtualPathUtility.ToAbsolute("~/js/jqueryfileupload/loading.gif")%>" alt="loading..." style="display:none;" />
          
            <hr class="userpanel"/>   

         

            <input type="button" id="btnUploadLogo" value="<%=GetResourcesString("btnUploadLogo") %>" class="dialogsavebutton" onclick="return FileTypeValidate('edit');" visible="true" />
            <asp:Button ID="btnCancelLogo" runat="server" CssClass="dialogbutton" UseSubmitBehavior="false" OnClientClick="return CancelLogoDialog();"/>                        
        
            <input id="hiddenImageID" type="hidden" runat="server" ClientIDMode="Static" value="" />
            <input id="hiddenDocumentID" type="hidden" runat="server" ClientIDMode="Static" value="" />
            <input id="hiddenSc2ImageScr" type="hidden" runat="server" ClientIDMode="Static" value="" />
        </div>
        <%-- end of structure for upload logo panel --%>                  
    </div>
 
    <input type="hidden" value="0" id="inputcollateralmode" />
</asp:Content>


