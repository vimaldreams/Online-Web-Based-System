<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/templates/XeroxPage.Master"  CodeBehind="BuiltDocument.aspx.cs" Inherits="CollateralCreator.Web.BuiltDocument" %>
<%@ Register TagPrefix="user" TagName="PreviewDocument" Src="~/usercontrol/PreviewDocument.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Metatags" runat="server" >

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div id="mastercontent">
        <div class="sitemap">
            <asp:Label ID="lblbreadcrumbdesc" runat="server"></asp:Label><asp:Label ID="SiteHierarchy" runat="server"></asp:Label>
        </div>

        <div>        
            <h2><asp:Label ID="lblHeaderText" runat="server"></asp:Label></h2><br/>

            <asp:Literal ID="litThanksText" runat="server"></asp:Literal><br/><br/>

            <asp:Label ID="lblDeliveryText" runat="server"></asp:Label><br/><br/>

            <img id="ImgDocument" runat="server" class="imgbuiltdocument" alt="BuiltCollateral" src=""/><br/><br/>

            <%--<asp:Button ID="btnBackToCollateralCreator" runat="server" useSubmitBehavior="false" OnClick="btnBackToCollateralCreator_Click" CssClass="backhomebutton blue"/>--%>
            <asp:HyperLink ID="btnBackToCollateralCreator" runat="server" useSubmitBehavior="false" ForeColor="White" NavigateUrl="~/Home.aspx" OnClick="btnBackToCollateralCreator_Click" CssClass="backhomebutton blue"/>
        </div>
    </div>
    <iframe src="AutoLogin.aspx" class="hide"></iframe>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Javascript" runat="server">

</asp:Content>