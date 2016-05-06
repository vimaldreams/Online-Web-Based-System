<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/XeroxPage.Master"  AutoEventWireup="true" EnableViewState="true" CodeBehind="Edit.aspx.cs" Inherits="CollateralCreator.Web.Edit" %>


<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <asp:Literal ID="LblSc2Title" runat="server"></asp:Literal>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Metatags" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>

    <div id="mastercontent">
        <div class="sitemap">
            <asp:Label ID="lblbreadcrumbdesc" runat="server"></asp:Label><asp:Label ID="SiteHierarchy" runat="server"></asp:Label>
        </div>

        <div class="editdescription">  
            <asp:Literal ID="litCustomizeMessage" runat="server"></asp:Literal>
            <hr/>
            
            <div id="divNoDetailMessage" runat="server" class="NoDetails">
                 <label id="litNoDetailMessage"></label>
            </div>
            <div id="divsmartcentreMessage" runat="server" class="NoDetails">
                <asp:Literal ID="litsmartcentreMessage" runat="server"></asp:Literal>
            </div>

            <h2><asp:Label ID="lblEditHeader" runat="server"></asp:Label></h2>
            <asp:Label ID="lblDocumentName" runat="server"></asp:Label> : 
            <input id="currenttemplatename" type="text" class="currentdocumentname" />
        </div>
        
        <div class="divsavingtext" style="margin-bottom:10px">
            <span class="savingtext" id="SpanSaving" runat="server"> We are preparing your report, please wait...</span>
        </div>
        
        <div id="editContent">   
            <div>               
                <asp:Literal ID="litHiddenFieldValues" runat="server"></asp:Literal>              
            </div>
            <div id="customArea" runat="server">
                <asp:Literal ID="litCustomizableTextArea" runat="server"></asp:Literal><br/><br/>   
            </div>
            <strong><asp:Label ID="lblCustomizeImages" runat="server" Visible="false" CssClass="lblCoverAddress"></asp:Label></strong><br/>        
            
            <asp:Literal ID="litCustomizeImageArea" runat="server"></asp:Literal>            
        </div>       

        <div id="previewContent">
                
            <div id="previewWindow" runat="server">
                <%-- structure for Ajax Loader --%>
                <div class="divajaxloader" style="display: none;">
                    <strong class="loading" id="StrongLoading" runat="server">We are preparing your report, please wait...</strong><br/><br/>
                    <img src="<%=VirtualPathUtility.ToAbsolute("~/images/navigation/loadingAnimation.gif")%>" alt="loadingimage" style="display:block; margin:auto;" />
                </div>
                <iframe id="iframePdfPreview" class="framepdfpreview" runat="server" clientidmode="Static"></iframe>
            </div>                        
              
            <div id="btnControls">
                <div id="previewbuttons" class="previewbuttons" runat="server">                    
                    <a title="click to update changes in pdf preview"><asp:Button ID="btnUpdatePreview" runat="server" CssClass="editpagebutton blue" UseSubmitBehavior="false" OnClientClick="return UpdatePreviewDocument();" ></asp:Button></a>&nbsp;&nbsp;&nbsp;&nbsp;
                    <a title="click to maximize pdf preview"><asp:Button ID="btnMaximisePreview" runat="server" ClientIDMode="Static" CssClass="editpagebutton blue" UseSubmitBehavior="false" OnClientClick=" return MaximizePreviewDocument();"></asp:Button></a>
                </div>
                <div id="orderbuttons" class="orderbuttons" runat="server">
                    <a title="click update preview to view the changes"><asp:Button ID="btnDownload" runat="server" ClientIDMode="Static" CssClass="editpagebutton blue" UseSubmitBehavior="false" OnClientClick="return DownloadDocument();"></asp:Button></a>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnOrderPrints" runat="server" ClientIDMode="Static" CssClass="editpagebutton orange" UseSubmitBehavior="false" OnClientClick="return GotoOrderPrints();"></asp:Button>
                </div>
                <div class="mainmenubutton">
                    <asp:Button ID="btnBackToMenu" runat="server" CssClass="editpagebutton blue" UseSubmitBehavior="false" OnClientClick="return GotoMainMenu();"></asp:Button>
                </div>  
             </div>           
        </div>
        
        <!--Smart Centre 2 custom image area -->
            <div style=" background-color: white; overflow:auto; height:auto; width: 100%;">
                                
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
                        <img id="CustomImage1" class="" runat="server" alt="Custom Image 1" src="" imageid="<%ImageAreaID1%>" clientidmode="Static" />
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
                
                <asp:Panel ID="PnlMultiImageSelect" runat="server" Visible="false">
                    
                   <asp:Label ID="LabelMultiImageHeader" runat="server" CssClass="lblCoverAddress"></asp:Label> <br />                 
                   <span id="SpanMultiImageRemoval" runat="server" style="font-size:13px">    
                       <label for="MultiImageRemoval">
                                <input type="checkbox" id="ChkMultiImageRemoval" runat="server" value="true"  />
                                Remove image (click the update preview button to complete the process)
                       </label>
                   </span>

                   <table>
                       <tr>
                           <td>
                                <asp:Image ID="ImgMultiImage1" runat="server" style="max-height:150px;max-width:150px;" ImageUrl="" Visible="false" />
                           </td>
                           <td>
                                <asp:RadioButton ID="RdBtnMultiImage1" runat="server" GroupName="MultiImageRadioBtn" Visible="false" Checked="true" ClientIDMode="Static" />
                           </td>
                       </tr>
                       <tr>
                           <td>
                                <asp:Image ID="ImgMultiImage2" runat="server" style="max-height:150px;max-width:150px;" ImageUrl="" Visible="false"/>
                            </td>
                            <td>
                                <asp:RadioButton ID="RdBtnMultiImage2" runat="server" GroupName="MultiImageRadioBtn" Visible="false" ClientIDMode="Static"/>
                            </td> 
                       </tr>
                       <tr>
                            <td>
                                <asp:Image ID="ImgMultiImage3" runat="server" style="max-height:150px;max-width:150px;" ImageUrl="" Visible="false"/>
                            </td>
                            <td>
                                <asp:RadioButton ID="RdBtnMultiImage3" runat="server" GroupName="MultiImageRadioBtn" Visible="false" ClientIDMode="Static"/>
                            </td>
                       </tr>
                       <tr>
                            <td>
                                <asp:Image ID="ImgMultiImage4" runat="server" style="max-height:150px;max-width:150px;" ImageUrl="" Visible="false"/>
                            </td>
                            <td>
                                <asp:RadioButton ID="RdBtnMultiImage4" runat="server" GroupName="MultiImageRadioBtn" Visible="false" ClientIDMode="Static"/>
                            </td>
                       </tr>
                       <tr>
                            <td>
                                <asp:Image ID="ImgMultiImage5" runat="server" style="max-height:150px;max-width:150px;" ImageUrl="" Visible="false"/>
                            </td>
                            <td>
                                <asp:RadioButton ID="RdBtnMultiImage5" runat="server" GroupName="MultiImageRadioBtn" Visible="false" ClientIDMode="Static"/>
                            </td>
                       </tr>
                       <tr>
                           <td>
                                <asp:Image ID="ImgMultiImage6" runat="server" style="max-height:150px;max-width:150px;" ImageUrl="" Visible="false" />                    
                           </td>
                           <td>
                                <asp:RadioButton ID="RdBtnMultiImage6" runat="server" GroupName="MultiImageRadioBtn" Visible="false" ClientIDMode="Static"/>
                           </td>
                       </tr>
                   </table>
                   
                   <input type="button" id="InputMultiImageChangeLogo" runat="server" class="changeTemplatelogo blue"/>
                              
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
                <asp:Literal ID="litDialogBoxPreviewCancelButton" runat="server"></asp:Literal>
            </div>  
        </div>
       
        <%-- structure for upload logo --%>             
        <div id="logo_dialogbox" runat="server" class="logo_dialogbox" style="display: none;">     
                            
            <input id="fileToUpload" type="file" size="25" ClientIDMode="Static" name="fileToUpload" class="input" /><br/><br/><br/>
            
            <img id="loading" src="<%=VirtualPathUtility.ToAbsolute("~/js/jqueryfileupload/loading.gif")%>" alt="loading..." style="display:none;" />
          
            <hr class="userpanel"/>   
            
        
            <input id="hiddenImageID" type="hidden" runat="server" ClientIDMode="Static" value="" />
            <input id="hiddenDocumentID" type="hidden" runat="server" ClientIDMode="Static" value="" />
            <input id="hiddenSc2ImageScr" type="hidden" runat="server" ClientIDMode="Static" value="" />

            <button id="btnUploadLogo" value="" class="dialogsavebutton" onclick="return FileTypeValidate('edit');" visible="true"></button>
            
            <asp:Button ID="btnCancelLogo" runat="server" CssClass="dialogbutton" UseSubmitBehavior="false" OnClientClick="return CancelLogoDialog();"/>                        
        </div>      
    </div>
    <iframe src="AutoLogin.aspx" class="hide"></iframe>
    <input type="hidden" value="0" id="inputcollateralmode" />
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Javascript" runat="server">
    <asp:Literal ID="litJavaScript" runat="server"></asp:Literal> 
    <asp:Literal ID="LblSc2JavaScript" runat="server"></asp:Literal>
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/editpage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>    
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/uploadlogopage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script> 
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/jqueryfileupload/ajaxfileupload.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>      
    
</asp:Content>
