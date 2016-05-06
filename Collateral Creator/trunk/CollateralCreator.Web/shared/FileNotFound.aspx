<%@ Page Title="" Language="C#" MasterPageFile="~/templates/XeroxPage.Master" AutoEventWireup="true" CodeBehind="FileNotFound.aspx.cs" Inherits="CollateralCreator.Web.FileNotFound" %>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <div id="left">
		
	</div>
	<div id="right" style="min-height:400px;height:400px">
		<div class="content">
            <img id="ThemeBanner" src="/images/en/xerox-event-large.jpg" alt="Image Placeholder" runat="server"  style="display:none;"/>
            <h1>404 - File not found</h1>
            <p class="error">The Template you requested does not exist.</p>
            <p class="error">Please contact your&nbsp;Xerox representative for the correct&nbsp;Template.&nbsp;</p>
            <p class="error">Visit <a href="http://www.xerox.com" class="required">www.xerox.com</a>&nbsp;for more information on Xerox Products and Services.</p>
		</div>
	</div>
	<br classs="clear" />
							
</asp:Content>
