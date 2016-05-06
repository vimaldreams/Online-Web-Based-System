<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="CollateralCreator.Web.Templates.Header" %>
<%@ Import Namespace="Xerox.SSOComponents.Models" %>

   <div id="navlinks">     
        <ul>
           <%-- <li>
                <a id="lnkAppHome" runat="server" />
            </li>
            <li>|</li>--%>
            <%--<li>
                <a id="lnkAdmin" runat="server" Visible="True">Admin</a>
            </li>  --%>
        </ul>            
    </div>

    <div id="banner" style="display:none;">
        <asp:Literal id="litThemeImage" runat="server" />
    </div>
    <asp:Panel ID="pnlTabs" runat="server">
    <div id="tabs">
        <asp:Literal id="litTabs" runat="server" />
    </div>
    </asp:Panel>
