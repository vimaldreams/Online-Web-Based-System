<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/XeroxPage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CollateralCreator.Web.Home" %>
<%@ Register TagPrefix="user" TagName="AddressControl" Src="~/usercontrol/AddressControl.ascx" %>
<%@ Register TagPrefix="user" TagName="LogoControl" Src="~/usercontrol/LogoControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Metatags" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">  
      
<div id="mastercontent">
    <div class="sitemap">
        <asp:Label ID="lblbreadcrumbdesc" runat="server"></asp:Label><asp:Label ID="SiteHierarchy" runat="server"></asp:Label>
    </div>

    <div id="headercontent">
        <h2><asp:Label ID="lblWelcomeMsg" runat="server" meta:resourcekey="WelcomeMsg"></asp:Label></h2>
        <p><asp:Label ID="lblWelcomeDesc" runat="server" Width="925"  meta:resourcekey="WelcomeDesc" ></asp:Label></p>
    </div>
    
    <div class="NoDetails">
        <a id="NoDetail" href="javascript:void();">
            <label id="litNoDetailMessage"></label>
        </a>
    </div>

    <div id="maincontent">
        <strong>
            <a href="RecentActivity.aspx" class="lnkrecentactivity">
                <img src="<%=VirtualPathUtility.ToAbsolute("~/images/navigation/link_pointer_13px.gif")%>" alt="arrowpointer"/>
                <asp:Label ID="lblRecentActivity" runat="server" meta:resourcekey="RecentActivity"  ></asp:Label>
            </a>
        </strong>
      
        <div id="leftPanel">
            <div class="treeview">
                <div class="selectlabel">
                    <strong><asp:Label ID="lblSelectProduct" runat="server"  meta:resourcekey="SelectProduct"></asp:Label></strong>
                </div>
                <asp:Literal ID="litSelectProducts" runat="server"></asp:Literal>                
            </div>
        </div>

        <div id="rightPanel">
            <div class="templateview">
                <h3><asp:Label ID="lblrightpanelHeader" runat="server" meta:resourcekey="RightpanelHeader"></asp:Label></h3>
                <p class="selectedprodText"><asp:Label ID="lblselectedText" runat="server" meta:resourcekey="SelectedText"></asp:Label> :<strong><label id="lblselecteditem"></label></strong></p>
                <hr class="templateform" />

                <div id="tempsection" class="templatesection">
                    <div id="templateform" class="maintemplateform"></div>
                </div>

            </div>
        </div>

        <div id="bottomPanel">
            <div class="updateDetails">
                <%--user control to update address  --%>   
                <user:AddressControl ID="uAddressControl" runat="server" />
                
                <%--user control to update address  --%>               
                <user:LogoControl ID="uLogoControl" runat="server" />                  
            </div>           
        </div>
    </div>
    
    <%-- structure for preview window --%>
    <div class="dialogPreviewBox" style="display: none;">
        <div id="divDocumentLoader" style="display: none;" >
            <img src="<%=VirtualPathUtility.ToAbsolute("~/images/navigation/loadingAnimation.gif")%>" style="display:block; margin:auto;" />
        </div>
        <div>
            <iframe id="iframePdfDocument" type="application/pdf" class="framepdfdocument"></iframe>            
        </div>
        <div class="dialogPreviewBottom" style="display:block; margin:auto;">
            <hr class="userpanel"/>            
            <asp:Literal ID="litDialogBoxCancelButton" runat="server"></asp:Literal>
        </div>  
        <input type="hidden" id="templatenumber" />
        <input type="hidden" id="templatebrand" />
    </div>

</div> 
<iframe src="~/AutoLogin.aspx" width="1" height="1" frameborder="0" class="hide"></iframe>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Javascript" runat="server">    

    <asp:Literal ID="litJavaScript" runat="server"></asp:Literal>  
   <%-- <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/jquery/jsort/jquery.jsort.0.4.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>--%>
    <%-- <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/jquery/jsort/jquery.jsort.0.4.min.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>--%>
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/homepage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/previewpage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script> 
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/updatedefault.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/uploadlogopage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script> 

    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/jqueryfileupload/ajaxfileupload.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>      
</asp:Content>
