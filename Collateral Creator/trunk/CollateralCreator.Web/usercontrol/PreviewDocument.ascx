<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PreviewDocument.ascx.cs" Inherits="CollateralCreator.Web.PreviewDocument" %>

<div class="previewdocument">
    <div class="userdocument">
        <asp:Literal ID="litPreviewImage" runat="server"></asp:Literal>
    </div>

    <div class="divproofbutton">
        <asp:Literal ID="litPreviewButton" runat="server"></asp:Literal>
    </div>

    <div class="divproofdesc">
        <p>
            <strong><asp:Label ID="lblproofheader" runat="server"></asp:Label></strong>
            <asp:Label ID="lblproofdescription" runat="server"></asp:Label>
        </p>

        <p>
            <strong><asp:Label ID="lbladobeheader" runat="server"></asp:Label></strong>
            <asp:Label ID="lbladobedescription" runat="server"></asp:Label>
        </p>
    </div>

    <%-- structure for preview window --%>
    <div class="dialogPreviewBox" style="display: none;">
        <div id="divDocumentLoader" style="display: none;" >
            <img src="<%=VirtualPathUtility.ToAbsolute("~/images/navigation/loadingAnimation.gif")%>" style="display:block; margin:auto;" />
        </div>
        <div>
            <iframe id="iframePdfDocument" class="framepdfdocument"></iframe>            
        </div>
        <div class="dialogPreviewBottom" style="display:block; margin:auto;">
            <hr class="userpanel"/>            
            <asp:Literal ID="litDialogBoxCancelButton" runat="server"></asp:Literal>
        </div>  
    </div>

</div>

<asp:Literal ID="litJavaScript" runat="server"></asp:Literal>

<script language="javascript" type="text/javascript">
    
</script>