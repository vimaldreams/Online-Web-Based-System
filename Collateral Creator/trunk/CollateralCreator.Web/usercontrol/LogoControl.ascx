<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogoControl.ascx.cs" Inherits="CollateralCreator.Web.LogoControl" %>

 <div class="updateLogo">
    <strong><asp:Label ID="lblLogo" runat="server"></asp:Label></strong><br/>
    <p><asp:Label ID="lblLogoDesc" runat="server" Width="350"></asp:Label></p>   
    <div class="LogoImageDiv">        
        <img id="ImageLogo" Width="125px" Height="75px" src="" alt="defaultlogo"/>
        <asp:Label ID="lblImageLogo" runat="server" Visible="false"></asp:Label>
    </div>
    <p><asp:Label ID="lblLogoSizeDesc" runat="server" Width="350"></asp:Label></p>  
    <asp:Literal ID="litButtonUpdateLogoDetails" runat="server"></asp:Literal>
</div>
  

<div id="logo_dialogbox" runat="server" style="display:none" class="logo_dialogbox">
    <%--<input id="fileLogoUrl" runat="server" type="file" ClientIDMode="Static" class="dialogbutton" /><br/>--%>

    <input id="fileLogoUrl" type="file" size="25" ClientIDMode="Static" name="fileToUpload" class="input" /><br/><br/><br/>           
    <img id="loading" src="<%=VirtualPathUtility.ToAbsolute("~/js/jqueryfileupload/loading.gif")%>" alt="loading..." style="display:none;" />
    <%--<asp:RegularExpressionValidator ID="restrictUploadFile" ControlToValidate="fileLogoUrl" CssClass="errormsg"
            ValidationExpression="^.+(.jpg|.jpeg|.png|.bmp)$" ErrorMessage="Invalid file type !!" runat="server">
    </asp:RegularExpressionValidator>--%>
    <input id="hiddenLoginID" type="hidden" runat="server" ClientIDMode="Static" value="" />

    <hr class="userpanel"/>  
    <button id="btnUploadDefaultLogo" value="" class="dialogsavebutton"  onclick="return FileTypeValidate('home');"></button>    
    <%--<asp:Button ID="btnUploadLogo" runat="server" CssClass="dialogsavebutton" UseSubmitBehavior="false" OnClick="btnclick_uploadlogo" />--%>
    <asp:Literal ID="litDialogBoxCancelButton" runat="server"></asp:Literal>
</div>

<asp:Literal ID="litJavaScript" runat="server"></asp:Literal> 