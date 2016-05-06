<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/templates/XeroxPage.Master"  CodeBehind="OrderSummary.aspx.cs" Inherits="CollateralCreator.Web.OrderSummary" %>
<%@ Register TagPrefix="user" TagName="PreviewDocument" Src="~/usercontrol/PreviewDocument.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Metatags" runat="server" >
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div id="mastercontent">
        <div class="sitemap">
           <asp:Label ID="lblbreadcrumbdesc" runat="server"></asp:Label><asp:Label ID="SiteHierarchy" runat="server"></asp:Label>
        </div>

        <div class="leftorderform">
             <h2><asp:Label ID="lblHeaderText" runat="server"></asp:Label></h2><br/>
             <strong><asp:Label ID="lblSubHeaderText" runat="server"></asp:Label></strong><br/><br/>        

             <table id="ordersummaryForm" class="ordersummaryForm">

                <tr><td><strong><asp:Label ID="lblSelectProduct" runat="server" CssClass="headerText"></asp:Label></strong></td></tr>   

                <tr><td class="tdsummarytext"><asp:Label ID="lblSelectedProductText" runat="server"></asp:Label></td></tr>   

                <asp:Literal ID="litCustomizeDocumentSection" runat="server"></asp:Literal>
               
                <tr><td><strong><asp:Label ID="lblPrintOptions" runat="server" CssClass="headerText"></asp:Label></strong></td></tr>   

                <asp:Literal ID="litPrintOptionsText" runat="server"></asp:Literal>  

                <tr><td><strong><asp:Label ID="lblDeliveryAddress" runat="server" CssClass="headerText"></asp:Label></strong></td></tr>   

                <asp:Literal ID="litDeliveryAddressText" runat="server"></asp:Literal>   

                <tr><td><strong><asp:Label ID="lblOrderContactInfo" runat="server" CssClass="headerText"></asp:Label></strong></td></tr>   

                <asp:Literal ID="litOrderContactText" runat="server"></asp:Literal> 
                   
             </table>
        </div>

        <div class="rightpreviewdocument">
            <%--user control to preview the document --%>   
            <user:PreviewDocument ID="uPreviewDocument" runat="server" />

            <div class="previeworderbutton">
                <asp:Button ID="btnSubmitOrder" runat="server" useSubmitBehavior="false" OnClick="btnSubmitOrder_Click" CssClass="button"></asp:Button>
                <asp:Button ID="btnCancel" runat="server" useSubmitBehavior="false" OnClick="btnCancel_Click" CssClass="button"></asp:Button>
            </div>

        </div>
    </div>
    <iframe src="AutoLogin.aspx" class="hide"></iframe>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Javascript" runat="server">
    <asp:Literal ID="litJavaScript" runat="server"></asp:Literal>      
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/previewpage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script> 
</asp:Content>



