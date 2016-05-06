<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/XeroxPage.Master" CodeBehind="RecentActivity.aspx.cs" Inherits="CollateralCreator.Web.RecentActivity" %>
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
            <h2><asp:Label ID="lblRecentActicityHeader" runat="server"></asp:Label></h2>
        </div>
        
        <div class="NoDetails">
             <label id="litNoDetailMessage"></label>
        </div>

        <div id="leftContent">
            <strong>
                <a href="Home.aspx" class="lnkcreatecollateral">
                    <img src="<%=VirtualPathUtility.ToAbsolute("~/images/navigation/link_pointer_13px.gif")%>" alt="arrowpointer"/>
                    <asp:Label ID="lblcreatenewCollateral" runat="server"></asp:Label>
                </a>
            </strong><br/><br/>
            <div class="recentactivity">
                <strong>
                    <asp:Label ID="lblmyrecentactivity" runat="server"></asp:Label>
                </strong><br/><br/>
                <label class="norecentdocumentlabel"></label> 
                <div id="recentdocumentsection" class="recentdocumentsection">
                  <%--structure to build the document with link button actions--%>                
                  <table id="tbdocumentactions" class="tbmentiondocument">
                   
                  </table>

                  <div class="view-more-link">
                    <a id="amorelnk" href="javascript:ExpandDiv();" class="pageLinkIndented viewmorelesslink" viewmore="viewmore" >
                        <img src="<%=VirtualPathUtility.ToAbsolute("~/images/portal/icons/plus.gif")%>" alt="expandcollapse" />
                        <div id="divviewmore" viewmore="viewmore" ><asp:Label ID="lblviewmore" runat="server" CssClass="pageLink3" ></asp:Label></div>
                        <div id="divviewless" viewless="viewless" style="display: none;"><asp:Label ID="lblviewless" runat="server" CssClass="pageLink3"></asp:Label></div>
                    </a> 
                  </div> 
                </div>                
            </div>

            <div class="printhouseactivity">
                <strong>
                    <asp:Label ID="lblmyprinthouse" runat="server"></asp:Label>
                </strong><br/><br/>
                <label class="norecentdocumentlabel"></label> 
                <div id="printdocumentsection" class="printdocumentsection">
                    <%--structure to build the print house document with link button actions--%>                    
                    <table id="tbprintdocument" class="tbprintdocument">
                      
                    </table>

                    <div class="pview-more-link">
                        <a id="pamorelnk" href="javascript:PrintExpandDiv();" class="pviewmorelesslink" pviewmore="pviewmore" >
                            <img src="<%=VirtualPathUtility.ToAbsolute("~/images/portal/icons/plus.gif")%>" alt="expandcollapse" />
                            <div id="divpviewmore" pviewmore="pviewmore" ><asp:Label ID="lblpviewmore" runat="server" CssClass="pageLink3" ></asp:Label></div>
                            <div id="divpviewless" pviewless="pviewless" style="display: none;"><asp:Label ID="lblpviewless" runat="server" CssClass="pageLink3"></asp:Label></div>
                        </a> 
                  </div> 
                </div>
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

        <div id="rightContent">
             <div class="recentupdateDetails">             
                <%--user control to update address  --%>   
                <user:AddressControl ID="uAddressControl" runat="server" />

                <%--user control to update address  --%>   
                <user:LogoControl ID="LogoControl" runat="server" />
            </div> 
        </div>   
    </div>    
    <iframe src="AutoLogin.aspx" class="hide"></iframe>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Javascript" runat="server">
    <asp:Literal ID="litJavaScript" runat="server"></asp:Literal>
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/recentpage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script> 
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/updatedefault.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/uploadlogopage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script> 

    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/jqueryfileupload/ajaxfileupload.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>      
</asp:Content>

