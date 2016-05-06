<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PortalHeader.ascx.cs" Inherits="CollateralCreator.Web.Templates.PortalHeader" %>
<%@ Import Namespace="Xerox.SSOComponents.Models" %>
    
    <div id="navlinks">     
        <ul>
            <li>
                <a id="lnkHome" runat="server" />
            </li>            
            <li>|</li>            
            <li>
                <a id="lnkAppHome" runat="server" />
            </li>
            <li>|</li>
            <%--<li>
                <a id="lnkAdmin" runat="server" Visible="True">Admin</a>
            </li>  
            <li>
                <a id="lnkAdminline" runat="server" Visible="False">|</a>
            </li> --%>
            <li>
                <a id="lnkProfile" runat="server" />
            </li>
            <li>|</li>
            <li>
                 <a id="lnkTutorial" runat="server" />
            </li>
            <li>|</li>
            <li>
                <a id="lnkHelp" runat="server" />
            </li> 
            <li>|</li>                                                  
           <li class="last" id="qlcontainer">
                <a tabindex="0" href="#news-items" class="fg-button fg-button-icon-right ui-widget ui-state-default ui-corner-all" id="hierarchy">
                <span class="ui-icon ui-icon-triangle-1-s"></span>Quick Links</a>
                <div id="news-items" class="hidden">                    
                     <ul id="xrx_bnr_partner_utilitynav">
	                    <li class="xrx_bnr_first"><a href="#">Projects</a>
                        <ul>
                            <% if (this.Reports == null || this.Reports.Count() == 0) { %>
                                <li>You have no projects to view.</li>
                            <% } else { %>

                                <% foreach (string item in this.Reports as IEnumerable<string>) { %>
                                <li><a href="#"><%= item %></a></li>
                                <% } %>

                            <% } %>
                        </ul>
                        </li>
	                    <li><a href="#">Tools</a>
	                    <ul>
                            <% if (this.Applications == null || this.Applications.Count() == 0){ %>
                                    <li>Unable to load tools.</li>
                             <% } else { %>
                                 <% foreach (Application item in this.Applications as IEnumerable<Application>) { %>
                                    <% if (!string.IsNullOrEmpty(item.EntrypointUrl)) { %>
                                    <li><a href="<%=PortalUrl%>Home/ChangeApp?appTag=<%=item.Id%>"><%=item.Title %></a></li>
                                    <% } %>                            
                                <% } %>
                             <% } %>
	                    </ul>
	                    </li>
	                    <li><a href="#">Reports</a>
                            <ul>
                            <% if (this.Reports == null || this.Reports.Count() == 0) { %>
                                <li>You have no reports to view.</li>
                            <% } else { %>

                                <% foreach (string item in this.Reports as IEnumerable<string>) { %>
                                <li><a href="#"><%= item %></a></li>
                                <% } %>

                            <% } %>
                            </ul>
	                    </li>
	                </ul>
                </div>
            </li>
        </ul>            
    </div>

    <div id="banner" style="display:none;">
        <asp:Literal id="litThemeImage" runat="server" />
    </div>
    <asp:Panel ID="pnlTabs" runat="server" Visible="false">
    <div id="tabs">
        <asp:Literal id="litTabs" runat="server" />
    </div>
    </asp:Panel>
    