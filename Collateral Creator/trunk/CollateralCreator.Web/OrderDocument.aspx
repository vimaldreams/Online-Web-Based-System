<%@ Page Title="" Language="C#" MasterPageFile="~/templates/XeroxPage.Master" AutoEventWireup="true" CodeBehind="OrderDocument.aspx.cs" Inherits="CollateralCreator.Web.OrderDocument" %>
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
         <strong><asp:Label ID="lblSubHeaderText1" runat="server"></asp:Label></strong><br/><br/>
         <p><asp:Label ID="lblSubHeaderText2" runat="server"></asp:Label></p>                           
        
         <table id="orderForm" class="orderForm">
            <tr><td><asp:Label ID="lblSubHeaderText3" runat="server"></asp:Label></td></tr>
            <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText1" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:DropDownList ID="ddQuantity" runat="server" CssClass="textboxstyle" ClientIDMode="Static">
                        <%--<asp:ListItem runat="server" InitialValue="0">Select</asp:ListItem>
                        <asp:ListItem runat="server">100</asp:ListItem>
                        <asp:ListItem runat="server">250</asp:ListItem>
                        <asp:ListItem runat="server">500</asp:ListItem>
                        <asp:ListItem runat="server">750</asp:ListItem>
                        <asp:ListItem runat="server">1000</asp:ListItem>
                        <asp:ListItem runat="server">1500</asp:ListItem>
                        <asp:ListItem runat="server">2000</asp:ListItem>--%>
                    </asp:DropDownList>      
                    <asp:CompareValidator ID="QtyCompareValidator" runat="server" CssClass="QuantityError" Text="*" Display="Dynamic"
                         ValueToCompare="0" Operator="GreaterThan" ControlToValidate="ddQuantity" Type="Integer" ValidationGroup="orderDocument"></asp:CompareValidator>           
                </td>
            </tr>          
            <tr class="fieldformat">
                <td class="labelformat" style="width:90px;"><asp:Label ID="lblFieldText2" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:Label ID="lblPrintJobText" runat="server"></asp:Label>                   
                </td>
            </tr>
            <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText3" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:CheckBox ID="chkPrintJob" runat="server" Checked="True"/>
                    <asp:Label ID="lblPrintPaperText" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="fieldformat">
                <td class="labelformat" style="width:95px;"><asp:Label ID="lblFieldText4" runat="server"></asp:Label></td>
                <td class="textformat"><asp:Label ID="txtBoxInput4" runat="server"></asp:Label></td>
            </tr>
            <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText5" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:Label ID="txtBoxInput5" runat="server"></asp:Label>             
                </td>
            </tr>
            <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText6" runat="server"></asp:Label></td>
                <td class="textformat"><asp:Label ID="txtBoxInput6" runat="server"></asp:Label></td>
            </tr>
            <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText7" runat="server"></asp:Label></td>
                <td class="textformat"><asp:Label ID="txtBoxInput7" runat="server"></asp:Label></td>
            </tr>
            <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText8" runat="server"></asp:Label></td>
                <td class="textformat"><asp:Label ID="txtBoxInput8" runat="server"></asp:Label></td>
            </tr>     
            
            <tr>
               <asp:ValidationSummary ID="ValidationSummary" runat="server" EnableClientScript="true" ValidationGroup="orderDocument" 
                    CssClass="QuantityError" ForeColor="red"/>
            </tr>

            <tr><td><asp:Label ID="lblSubHeaderText4" runat="server"></asp:Label></td></tr>
             <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText9" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput9" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorAttention" ControlToValidate="txtBoxInput9" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                </td>
            </tr> 
             <tr class="fieldformat">
                <td class="labelformat" style="width:80px;"><asp:Label ID="lblFieldText10" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput10" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorCompanyName" ControlToValidate="txtBoxInput10" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                </td>
            </tr> 
             <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText11" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput11" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorAddressLine1" ControlToValidate="txtBoxInput11" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                </td>
            </tr> 
             <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText12" runat="server"></asp:Label></td>
                <td class="textformat"><asp:TextBox ID="txtBoxInput12" runat="server" CssClass="textboxstyle"></asp:TextBox></td>
            </tr> 
            <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText13" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput13" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorCity" ControlToValidate="txtBoxInput13" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                 </td>
            </tr> 
             <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText13a" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput13a" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorState" ControlToValidate="txtBoxInput13a" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                 </td>
            </tr>
             <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText14" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput14" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorPostCode" ControlToValidate="txtBoxInput14" CssClass="QuantityError" 
                         Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                 </td>
            </tr>  
            
             <tr><td style="width:150px"><asp:Label ID="lblSubHeaderText5" runat="server"></asp:Label></td></tr>
             <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText15" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput15" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorFirstName" ControlToValidate="txtBoxInput15" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                 </td>
             </tr> 
             <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText16" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput16" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorLastName" ControlToValidate="txtBoxInput16" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                </td>
            </tr> 
             <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText17" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput17" runat="server" CssClass="textboxstyle"></asp:TextBox>
                     <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorPhone" ControlToValidate="txtBoxInput17" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                </td>
            </tr> 
            <tr class="fieldformat">
                <td class="labelformat"><asp:Label ID="lblFieldText18" runat="server"></asp:Label></td>
                <td class="textformat">
                    <asp:TextBox ID="txtBoxInput18" runat="server" CssClass="textboxstyle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidatorEmail" ControlToValidate="txtBoxInput18" CssClass="QuantityError" 
                        Display="Dynamic" Text="*" ValidationGroup="orderDocument" ForeColor="red"/>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator" runat="server" Display="Static" ForeColor="red" ControlToValidate="txtBoxInput18" EnableClientScript="true"
                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"/>    
                </td>
            </tr>             
            <tr class="fieldformat">
                <td></td>
                <td>
                    <asp:Button ID="btnCancel" runat="server" useSubmitBehavior="false"  OnClick="btnCancel_Click" CssClass="buttonstyle blue"/>                  
                    <asp:Button ID="btnSubmit" runat="server" useSubmitBehavior="false"  OnClick="btnSubmit_Click" CssClass="buttonstyle blue"
                        ValidationGroup="orderDocument" CausesValidation="true"/>
                </td>
            </tr> 
         </table>    
         
    </div>

    <div class="rightpreviewdocument">
        <%--user control to preview the document --%>   
        <user:PreviewDocument ID="uPreviewDocument" runat="server" />
    </div>
</div>
<iframe src="AutoLogin.aspx" class="hide"></iframe>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Javascript" runat="server">
    <asp:Literal ID="litJavaScript" runat="server"></asp:Literal>    
    <script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/js/previewpage.js")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>"></script>   
</asp:Content>

