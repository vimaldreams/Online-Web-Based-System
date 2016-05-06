<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Templates/XeroxPage.Master" CodeBehind="AdminOrderScreen.aspx.cs" Inherits="CollateralCreator.Web.AdminOrderScreen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Metatags" runat="server">
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <div id="adminmastercontent">

        <div id="headercontent">
            <h2><asp:Label ID="lblAdminHeader" runat="server"></asp:Label></h2>
            <br/>
            <h4><asp:Label ID="lblAdminQueue" runat="server"></asp:Label></h4>
        </div>
        
        <div class="btnarchivedownload"><asp:Button ID="btnArchive" runat="server" UseSubmitBehavior="false" OnClick="btndownload_ArchiveReport" Text="Download Archive Report" CssClass="buttondownload blue"/></div><br/>

        <asp:Panel ID="pnlTable" runat="server" Visible="true">
        <div class="admintool">
            <div class="btndownload"><asp:Button ID="btndownlaod" runat="server" UseSubmitBehavior="false" OnClick="btndownload_UsageReport" CssClass="buttondownload blue"/></div><br/>
            <div class="admindata">
            <asp:GridView ID="GridView_AdminArea" runat="server"  DataKeyNames="DocumentID" AutoGenerateColumns="False"
            OnPageIndexChanging = "GridView_AdminArea_PageIndexChanging" AllowPaging="true" PageSize="10" ClientIDMode="Static" 
            OnRowUpdating="GridView_AdminArea_RowUpdating" OnRowDataBound="GridView_AdminArea_RowDataBound" OnRowCommand="GridView_AdminArea_RowCommand"
            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
            GridLines="Vertical">
            
                <Columns>

                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/> 
                        <ItemTemplate>
                            <div style="width: 170px; padding-left:10px;">
                                <asp:Label ID="lbldocumentame" runat="server" Text ='<%# Eval("Name")%>' CssClass=""></asp:Label>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>
                    
                     <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/>
                        <ItemTemplate>
                            <div style="width: 65px; padding-left:5px;">
                                <asp:Label ID="lbldocumentpartnumber" runat="server" Text ='<%# Eval("PartNumber")%>' CssClass=""></asp:Label>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>

                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/>
                        <ItemTemplate>
                            <div style="width: 67px; padding-left:5px;">
                                <asp:Label ID="lbldocumentcountry" runat="server" Text ='<%# Eval("Country")%>' CssClass=""></asp:Label>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>

                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/>
                        <ItemTemplate>
                            <div style="width: 35px; padding-left:10px;">
                              <a href="javascript:SummaryOrder('<%# Eval("DocumentID")%>');">
                                <asp:Label ID="lbldocumentid" runat="server" Text ='<%# Eval("DocumentID")%>' CssClass=""></asp:Label>
                              </a>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>

                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/> 
                        <ItemTemplate>
                            <div style="width: 65px; padding-left:10px;">
                                <asp:Label ID="lblcontact" runat="server" Text ='<%# Eval("FirstName")%>' CssClass=""></asp:Label>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>

                     <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/>                        
                        <ItemTemplate>
                            <div style="width: 55px; padding-left:10px;">
                                <asp:Label ID="lblcreateddate" runat="server" Text ='<%# Eval("CreatedDate")%>' CssClass=""></asp:Label>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>                   
                    
                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/> 
                         <HeaderTemplate>
                            <div style="width: 65px; padding-left:10px;">                            
                                <asp:Label ID="lblDocumentSelect" runat="server" CssClass=""></asp:Label><br/>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkHeaderDocument" runat="server" Checked="false" AutoPostBack="true" CssClass="" OnCheckedChanged="GridView_AdminArea_BatchSelect" ClientIDMode="Static"></asp:CheckBox>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="width: 60px; padding-left:30px;">
                                <asp:CheckBox ID="chkDocument" runat="server" Checked="false" CssClass="" ClientIDMode="Static"></asp:CheckBox>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>

                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/> 
                        <HeaderTemplate>                            
                            <div style="width: 140px;">
                                <asp:Label ID="lblDocumentStatus" runat="server" CssClass="lblstatus"></asp:Label><br/>&nbsp;&nbsp;
                                <asp:DropDownlist ID="ddHeaderDocumentStatus" runat="server" CssClass="" ClientIDMode="Static"></asp:DropDownlist>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="width: 110px; padding-left:20px;">
                                <asp:DropDownlist ID="ddDocumentStatus" runat="server" CssClass="" ClientIDMode="Static">
                                </asp:DropDownlist>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>

                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/> 
                        <ItemTemplate>
                            <div style="width: 140px; padding-left:10px;">
                                <asp:Textbox ID="txttrackingnumber" runat="server" Text ='<%# Eval("TrackingNumber")%>' CssClass="trackingnumber"  ClientIDMode="Static"></asp:Textbox>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>
                    
                     <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/>                        
                        <ItemTemplate>
                            <div style="width: 50px; padding-left:10px;">
                                <asp:Label ID="lblmodifieddate" runat="server" Text ='<%# Eval("ModifiedDate")%>' CssClass=""></asp:Label>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>  

                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/> 
                        <ItemTemplate>
                            <div style="width: 60px; padding-left:10px;">
                                <asp:DropDownlist ID="ddDocumentCarrier" runat="server" CssClass="" ClientIDMode="Static"></asp:DropDownlist>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>

                    <asp:TemplateField>                      
                        <ItemStyle CssClass = "documentitem"/>
                        <HeaderStyle CssClass = ""/> 
                        <HeaderTemplate>
                            <div style="width: 140px; padding-left:10px;">                            
                                <asp:Label ID="lblBatchSubmit" runat="server" CssClass=""></asp:Label><br/>&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="lnkbatchSubmitStatus" runat="server" UseSubmitBehavior="false" CssClass="btnsubmit blue" CommandName="BatchUpdate"></asp:Button>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="width: 140px; padding-left:10px;">
                               <asp:Button ID="lnkSubmitStatus" runat="server" UseSubmitBehavior="false" CommandName="Update" CssClass="btnsubmit blue"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
                               <asp:Button ID="lnkDownloadDocument" runat="server" UseSubmitBehavior="false" CssClass="btnsubmit blue" ></asp:Button>
                                <%--OnClientClick='<%# "javascript:return DownloadDocument(" + Eval("DocumentID") + ");" %>'--%>
                            </div>
                        </ItemTemplate>  
                    </asp:TemplateField>

                    <asp:BoundField  runat="server" Visible="false" DataField="DocumentStateID" ></asp:BoundField>
                     
                </Columns>

                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="" Font-Bold="True" ForeColor="White" CssClass="grdview_header"/>
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <PagerStyle BackColor="#efefef" ForeColor="#008CC9" HorizontalAlign="Center" Font-Bold="True" CssClass="grdview_pager" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
                <PagerSettings  Mode="NextPreviousFirstLast" Position="Bottom" PreviousPageText="Previous" NextPageText="Next" />
            </asp:GridView>
            </div>
        </div>       
        </asp:Panel>

        <asp:Panel ID="pnlAdmin" runat="server" Visible="false">
            <h3><asp:Label ID="lblAdminEmptyMessage" runat="server"></asp:Label></h3>
        </asp:Panel>
    </div>
    
    <div class="summaryorder" style="display: none;" >
        
        <h2>Job Summary:</h2>
        <a class="summaryclose" href="javascript:window.print();"><span style="color: color: #008CC9;">Print</span></a>
        <br/><br/>    
        <table id="summarypage" class="summarypage">
            <tr>
                <td><h4>DocumentID</h4></td>
                <td><label id="lblDocumentID"></label></td>
            </tr>   
            <tr>
                <td><h4>Name</h4></td>
                <td><label id="lblName"></label></td>
            </tr>  
            <tr>
                <td><h4>CompanyName</h4></td>
                <td><label id="lblCompanyName"></label></td>
            </tr>  
            <tr>
                <td><h4>CompanyID</h4></td>
                <td><label id="lblCompanyID"></label></td>
            </tr>  
            <tr>
                <td><h4>ResellerID</h4></td>
                <td><label id="lblResellerID"></label></td>
            </tr>  
            <tr>
                <td><h4>FirstName</h4></td>
                <td><label id="lblFirstName"></label></td>
            </tr>  
            <tr>
                <td><h4>LastName</h4></td>
                <td><label id="lblLastName"></label></td>
            </tr>  
            <tr>
                <td><h4>Phone</h4></td>
                <td><label id="lblPhone"></label></td>
            </tr>  
            <tr>
                <td><h4>Email</h4></td>
                <td><label id="lblEmail"></label></td>
            </tr>  
            <tr>
                <td><h4>Origination Date</h4></td>
                <td><label id="lblOriginationDate"></label></td>
            </tr>  
            <tr>
                <td><h4>Quantity</h4></td>
                <td><label id="lblQuantity"></label></td>
            </tr>  
            <tr>
                <td><h4>DeliveryTo</h4></td>
                <td><label id="lblDeliveryTo"></label></td>
            </tr>  
            <tr>
                <td><h4>DeliveryAddress1</h4></td>
                <td><label id="lblDeliveryAddress1"></label></td>
            </tr>  
            <tr>
                <td><h4>DeliveryAddress2</h4></td>
                <td><label id="lblDeliveryAddress2"></label></td>
            </tr>  
            <tr>
                <td><h4>City</h4></td>
                <td><label id="lblCity"></label></td>
            </tr>  
            <tr>
                <td><h4>State</h4></td>
                <td><label id="lblState"></label></td>
            </tr>  
            <tr>
                <td><h4>PostCode</h4></td>
                <td><label id="lblPostCode"></label></td>
            </tr>  
             <tr>
                <td><h4>Country</h4></td>
                <td><label id="lblCountry"></label></td>
            </tr>  
            <tr>
                <td><h4>Delivery/AttentionTo</h4></td>
                <td><label id="lblAttentionTo"></label></td>
            </tr>  
            <tr>
                <td><h4>Delivery Phone</h4></td>
                <td><label id="lblDeliveryPhone"></label></td>
            </tr>  
            <tr>
                <td><h4>PartNumber</h4></td>
                <td><label id="lblPartNumber"></label></td>
            </tr>  
        </table>
        
        <br/><hr/>
        <a class="summaryclose" href="javascript:CancelSummaryDialog();"><span style="color: #008CC9;">Close</span></a>

    </div>

</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="Javascript" runat="server">

  <asp:Literal ID="litJavaScript" runat="server"></asp:Literal>  
  <script type="text/javascript">

      function DownloadDocument(paramDocumentID) {

          jQuery.ajax({
              type: "POST",
              url: sRootPath + "services/CollateralHome.asmx/Document_DownloadLog",
              cache: false,
              data: JSON.stringify({ iDocumentId: paramDocumentID }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (response) {
                  response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                  if (response) {}
              }
          });

          window.location.href = sRootPath + 'Handlers/ReportHandler.ashx?v=' + new Date().getTime() + '&mode=downloadpdf&documentname=document' + paramDocumentID + '.pdf';
      }   
      
      function SummaryOrder(paramDocumentID) {

          jQuery.ajax({
              type: "POST",
              url: sRootPath + "services/CollateralHome.asmx/GetDocumentSummary",
              cache: false,
              data: JSON.stringify({ iDocumentID: paramDocumentID }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (response) {
                  response = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                  if (response) {

                      $('#lblDocumentID').text(response.DocumentSummary.DocumentID);
                      $('#lblName').text(response.DocumentSummary.Name);
                      $('#lblCompanyName').text(response.DocumentSummary.CompanyName); //sChannelPartnerCompany
                      $('#lblCompanyID').text(response.DocumentSummary.CompanyID);
                      $('#lblResellerID').text(response.DocumentSummary.ChannelPartnerLoginID);
                      $('#lblFirstName').text(response.DocumentSummary.FirstName);
                      $('#lblLastName').text(response.DocumentSummary.LastName);
                      $('#lblPhone').text(response.DocumentSummary.ChannelPartnerPhone); //sChannelPartnerPhone
                      $('#lblEmail').text(response.DocumentSummary.Email);
                      $('#lblOriginationDate').text(GetFormatdate(response.DocumentSummary.CreatedDate));
                      $('#lblQuantity').text(response.DocumentSummary.Quantity);
                      $('#lblDeliveryTo').text(response.DocumentSummary.CompanyName);
                      $('#lblDeliveryAddress1').text(response.DocumentSummary.AddressLine1);
                      $('#lblDeliveryAddress2').text(response.DocumentSummary.AddressLine2);
                      $('#lblCity').text(response.DocumentSummary.City);
                      $('#lblState').text(response.DocumentSummary.State);
                      $('#lblPostCode').text(response.DocumentSummary.PostCode);
                      $('#lblCountry').text(response.DocumentSummary.Country);
                      $('#lblAttentionTo').text(response.DocumentSummary.Attention);
                      $('#lblDeliveryPhone').text(response.DocumentSummary.Phone);
                      $('#lblPartNumber').text(response.DocumentSummary.PartNumber.substring(0,9));

                      //open dialog box
                      $('#xrx_bnr_ftr').hide();
                      $("html").css("overflow", "hidden");
                      $('#adminmastercontent').slideUp('slow');
                      
                      $('.summaryorder').attr("title", "OrderSummary");

                      jQuery(".summaryorder").dialog({
                          autoOpen: true,
                          closeOnEscape: false,
                          width: 600,
                          height: 850,
                          modal: true,
                          draggable: false,
                          resizable: false
                      });
                  }
              }
          });
      }

      function GetFormatdate(jsondate) {

          jsondate = jsondate.replace("/Date(", "").replace(")/", "");
          if (jsondate.indexOf("+") > 0) {
              jsondate = jsondate.substring(0, jsondate.indexOf("+"));
          }
          else if (jsondate.indexOf("-") > 0) {
              jsondate = jsondate.substring(0, jsondate.indexOf("-"));
          }

          var date = new Date(parseInt(jsondate, 10));
          var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
          var curretnYear = date.getFullYear();

          return date.getMonth() + '/' + currentDate + '/' + curretnYear.toString().slice(2);
      }

      function CancelSummaryDialog() {
          $('#xrx_bnr_ftr').show();
          jQuery(".summaryorder").dialog("close");
          $('#adminmastercontent').slideDown('slow');
      }
      
  </script>
</asp:Content>