<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressControl.ascx.cs" Inherits="CollateralCreator.Web.AddressControl" %>

<div class="updateContact">
    <strong><asp:Label ID="lblContactdetails" runat="server"></asp:Label></strong><br/>
    <p><asp:Label ID="lblContactDesc" runat="server" Width="350"></asp:Label></p>
    <textarea id="txtContactDetails" runat="server" readonly="readonly" rows="10" cols="10" class="txthomecontactstyle" ClientIDMode="Static"></textarea>
                   
    <input id="hiddentxtAddressline1" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddentxtAddressline2" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddentxtAddressline3" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddentxtTown" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddentxtState" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddentxtCountry" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddentxtPostcode" runat="server" type="hidden" ClientIDMode="Static"/>
    <asp:Literal ID="litButtonUpdateContactDetails" runat="server"></asp:Literal>
</div>

<div class="updateContact">
    <strong><asp:Label ID="lblCustomdetails" runat="server"></asp:Label></strong><br/>
    <p><asp:Label ID="lblCustomdescription" runat="server" Width="350"></asp:Label></p>

    <%--<div class="customlabels">--%>
       <textarea id="txtCustomDetails" runat="server" readonly="readonly" rows="10" cols="10" class="txthomecontactstyle" ClientIDMode="Static"></textarea>
       <%-- <asp:Label ID="txtCompanyName" runat="server"></asp:Label><br/>
        <asp:Label ID="txtWebUrl" runat="server"></asp:Label><br/>
        <asp:Label ID="txtPhonenumber" runat="server"></asp:Label><br/>  --%>            
    <%--</div>--%>
    <input id="hiddentxtLoginID" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddenChannelPartnerInfoID" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddenCompanyname" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddenWebUrl" runat="server" type="hidden" ClientIDMode="Static"/>
    <input id="hiddenPhonenumber" runat="server" type="hidden" ClientIDMode="Static"/> 
    <asp:Literal ID="litButtonUpdateCustomDetails" runat="server"></asp:Literal>
</div>


<div id="address_dialogbox" runat="server" style="display:none" class="address_dialogbox">
        <table id="tbContactDetails" class="tbContactDetails">
            <tr>
                <td><asp:Label ID="lblAddress1" runat="server"></asp:Label></td>
                <td><input id="txtAddress1" type="text"/></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblAddress2" runat="server"></asp:Label></td>
                <td><input id="txtAddress2" type="text"/></td>
            </tr>                       
            <tr>
                <td><asp:Label ID="lblAddress3" runat="server"></asp:Label></td>
                <td><input id="txtAddress3" type="text"/></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblTown" runat="server"></asp:Label></td>
                <td><input id="txtTown" type="text"/></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblState" runat="server"></asp:Label></td>
                <td><input id="txtState" type="text"/></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblCountry" runat="server"></asp:Label></td>
                <td><input id="txtCountry" type="text"/></td>
            </tr>
                <tr>
                <td><asp:Label ID="lblPostcode" runat="server"></asp:Label></td>
                <td><input id="txtPostcode" type="text"/></td>
            </tr>
        </table>
        <hr class="userpanel"/>
        <input id="DialogBoxAddressSaveButton" runat="server" type="button" class="dialogsavebutton" onclick="javascript:SaveAddressDialog();" />
        <%-- <asp:Literal ID="litDialogBoxAddressSaveButton" runat="server"></asp:Literal>--%>
        <asp:Literal ID="litDialogBoxAddressCancelButton" runat="server"></asp:Literal>
</div>


<div id="custom_dialogbox" runat="server" style="display:none" class="custom_dialogbox">
        <table id="tbCustomDetails" class="tbCustomDetails">
            <tr>
                <td><asp:Label ID="tblblCompanyname" runat="server"></asp:Label></td>
                <td><input id="txtCompanyname" type="text"/></td>
            </tr>
            <tr>
                <td><asp:Label ID="tblblWebUrl" runat="server"></asp:Label></td>
                <td><input id="txtWebUrl" type="text"/></td>
            </tr>                       
            <tr>
                <td><asp:Label ID="tblblPhonenumber" runat="server"></asp:Label></td>
                <td><input id="txtPhonenumber" type="text"/></td>
            </tr>           
        </table>
        <hr class="userpanel"/>
        <input id="DialogBoxCustomSaveButton" runat="server" type="button" class="dialogsavebutton" onclick="javascript:SaveCustomDialog();" />
        <%--<asp:Literal ID="litDialogBoxCustomSaveButton" runat="server"></asp:Literal>--%>
        <asp:Literal ID="litDialogBoxCustomCancelButton" runat="server"></asp:Literal>
       
</div>

<asp:Literal ID="litJavaScript" runat="server"></asp:Literal>

<script type="text/javascript">

</script>