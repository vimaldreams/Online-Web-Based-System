<%@ Page Title="" Language="C#" MasterPageFile="~/templates/XeroxPage.Master" AutoEventWireup="true" CodeBehind="NotAuthenticated.aspx.cs" Inherits="CollateralCreator.Web.NotAuthenticated" %>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <div id="left">
		
	</div>
	<div id="right" style="min-height:400px;height:400px">
		<div class="content">
                       <img id="ThemeBanner" src="/images/en/xerox-event-large.jpg" alt="Image Placeholder" runat="server"  style="display:none;"/>
            <h1>401 - Not Authenticated</h1>
            <p class="error">You have been directed here for one of the following reasons:</p>
            <ul class="errors">
                <li>Your session timed out</li>
                <li>Your details could not be verified</li>
                <li>You do not have permission to access the requested page.</li>
            </ul>
            <p class="error">Please try again or contact your&nbsp;Xerox representative.</p>
            <p class="error">Visit <a href="http://www.xerox.com" class="required">www.xerox.com</a>&nbsp;for more information on Xerox Products and Services.</p>
		</div>
	</div>
	<br classs="clear" />
							
</asp:Content>
