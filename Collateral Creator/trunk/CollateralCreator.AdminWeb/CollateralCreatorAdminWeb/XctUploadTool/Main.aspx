<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="XCTUploadToolWeb.Main" %>
<%@ Reference Control="TxtBlock.ascx" %>
<%@ Register TagPrefix="uc" TagName="BlockControl"  Src="TxtBlock.ascx" %>
<%@ Import Namespace="System.Globalization" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script src="Script/jquery-1.7.1.js" type="text/javascript"></script>
     <script src="Script/Mask.js" type="text/javascript"></script>

     <!-- Stylesheet Includes -->  
     <link rel="stylesheet" type="text/css" href="<%=VirtualPathUtility.ToAbsolute("~/Content/corp_740px_bundle.embed.css")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>" />
     <link rel="stylesheet" type="text/css" href="<%=VirtualPathUtility.ToAbsolute("~/Content/site.css")%>?v=<%=DateTime.Now.ToString("yyyyMMdd")%>" />
    
    <script type="text/javascript">
       </script>
    
    <style>
        .text-label {
            color: #cdcdcd;
            font-weight: bold;
        }
    </style>
</head>

<body>
    <header>
        <div id="xrx_bnr_hdr" class="xrx_bnr_partner xrx_bnr_partner_nonav">
            <div id="xrx_bnr_hdr_position">
                <a id="xrx_bnr_hdr_lnk_logo" href="http://www.xerox.com/" name="&lid=hdr-logo-home"><span>Xerox.com</span></a> 
                <span class="title" id="xrx_bnr_partner_title">IMPACT Marketplace - Collateral Creator Admin</span>                       
                <div class="portalheader">
                
                </div>
            </div>            
        </div>
    </header>

    <div id="xrx_pg_legacy_pagewidth">
	  <div class="xrx_pg_legacy_clearfix" id="xrx_pg_legacy_wrapper">
		<div id="xrx_pg_legacy_twocols" class="full-width"> 
			<div id="xrx_pg_legacy_maincol" style="width:1000px !important;">			            
				<div id="xrx_pg_legacy_wrapper">
                    <form id="form1" runat="server">

                        <h2>SmartCentre Collateral Upload</h2>
                        <a href="../Home/Index">Home</a><br /><br /><br />
                        
                        <div>
                            <h3 style="color:red !important"><asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label></h3>
                            <h3 style="color:green !important"><asp:Label ID="LblMessage" runat="server" ForeColor="Green"></asp:Label></h3>
                           <h3>
                               Enter document ID: <asp:TextBox ID="TxtDocID" runat="server"></asp:TextBox>
                           </h3>
                        </div>
                        <div>
                            <h3>
                                Enter part number:  <asp:TextBox ID="TxtPartNo" runat="server"></asp:TextBox>
                            </h3>
                        </div>
        
                        <div>
                            <h3>
                                Enter number of pages in pdf:  <asp:TextBox ID="TxtNoPages" runat="server">1</asp:TextBox>

                            </h3>
                        </div>
                         <div>
                            <h3>
                               Landscape <asp:CheckBox ID="ChkLandscape" runat="server" />

                            </h3>
                        </div>
                        <div>
                            <h3>Number of text blocks

                                 <asp:DropDownList ID="DrpTextBlocks" runat="server" OnSelectedIndexChanged="DrpTextBlocks_SelectedIndexChanged" AutoPostBack="true">
                                 <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                 <asp:ListItem Text="1" Value="1" ></asp:ListItem>
                                 <asp:ListItem Text="2" Value="2" ></asp:ListItem>
                                 <asp:ListItem Text="3" Value="3" ></asp:ListItem>
                                 <asp:ListItem Text="4" Value="4" ></asp:ListItem>
                                 <asp:ListItem Text="5" Value="5" ></asp:ListItem>
                                 <asp:ListItem Text="6" Value="6" ></asp:ListItem>
                                 <asp:ListItem Text="7" Value="7" ></asp:ListItem>
                                 <asp:ListItem Text="8" Value="8" ></asp:ListItem>
                                 <asp:ListItem Text="9" Value="9" ></asp:ListItem>
                                 <asp:ListItem Text="10" Value="10" ></asp:ListItem>
                                 <asp:ListItem Text="11" Value="11" ></asp:ListItem>
                                 <asp:ListItem Text="12" Value="12" ></asp:ListItem>
                                 <asp:ListItem Text="13" Value="13" ></asp:ListItem>
                                 <asp:ListItem Text="14" Value="14" ></asp:ListItem>
                                 <asp:ListItem Text="15" Value="15" ></asp:ListItem>
                                 <asp:ListItem Text="16" Value="16" ></asp:ListItem>
                                 <asp:ListItem Text="17" Value="17" ></asp:ListItem>
                                 <asp:ListItem Text="18" Value="18" ></asp:ListItem>
                                 <asp:ListItem Text="19" Value="19" ></asp:ListItem>
                                 <asp:ListItem Text="20" Value="20" ></asp:ListItem>
                                 <asp:ListItem Text="21" Value="21" ></asp:ListItem>
                                 <asp:ListItem Text="22" Value="22" ></asp:ListItem>
                                 <asp:ListItem Text="23" Value="23" ></asp:ListItem>
                                 <asp:ListItem Text="24" Value="24" ></asp:ListItem>
                                 <asp:ListItem Text="25" Value="25" ></asp:ListItem>
                                 <asp:ListItem Text="26" Value="26" ></asp:ListItem>
                                 <asp:ListItem Text="27" Value="27" ></asp:ListItem>
                                 <asp:ListItem Text="28" Value="28" ></asp:ListItem>
                                 <asp:ListItem Text="29" Value="29" ></asp:ListItem>
                                 <asp:ListItem Text="30" Value="30" ></asp:ListItem>
                            </asp:DropDownList>

                            </h3>
                        </div>
                        <div>
                            <uc:BlockControl id="BlockControl1" runat="server"  Visible="false" BlockID="1"  />
                            <uc:BlockControl id="BlockControl2" runat="server"  Visible="false" BlockID="2"  />
                            <uc:BlockControl id="BlockControl3" runat="server"  Visible="false" BlockID="3"  />
                            <uc:BlockControl id="BlockControl4" runat="server"  Visible="false" BlockID="4"  />
                            <uc:BlockControl id="BlockControl5" runat="server"  Visible="false" BlockID="5"  />
                            <uc:BlockControl id="BlockControl6" runat="server"  Visible="false" BlockID="6"  />
                            <uc:BlockControl id="BlockControl7" runat="server"  Visible="false" BlockID="7"  />
                            <uc:BlockControl id="BlockControl8" runat="server"  Visible="false" BlockID="8" />
                            <uc:BlockControl id="BlockControl9" runat="server"  Visible="false" BlockID="9"  />
                            <uc:BlockControl id="BlockControl10" runat="server"  Visible="false" BlockID="10"  />
                            <uc:BlockControl id="BlockControl11" runat="server"  Visible="false" BlockID="11"  />
                            <uc:BlockControl id="BlockControl12" runat="server"  Visible="false" BlockID="12"  />
                            <uc:BlockControl id="BlockControl13" runat="server"  Visible="false" BlockID="13"  />
                            <uc:BlockControl id="BlockControl14" runat="server"  Visible="false" BlockID="14"  />
                            <uc:BlockControl id="BlockControl15" runat="server"  Visible="false" BlockID="15"  />
                            <uc:BlockControl id="BlockControl16" runat="server"  Visible="false" BlockID="16"  />
                            <uc:BlockControl id="BlockControl17" runat="server"  Visible="false" BlockID="17"  />
                            <uc:BlockControl id="BlockControl18" runat="server"  Visible="false" BlockID="18"  />
                            <uc:BlockControl id="BlockControl19" runat="server"  Visible="false" BlockID="19"  />
                            <uc:BlockControl id="BlockControl20" runat="server"  Visible="false" BlockID="20"  />
                            <uc:BlockControl id="BlockControl21" runat="server"  Visible="false" BlockID="21"  />
                            <uc:BlockControl id="BlockControl22" runat="server"  Visible="false" BlockID="22"  />
                            <uc:BlockControl id="BlockControl23" runat="server"  Visible="false" BlockID="23"  />
                            <uc:BlockControl id="BlockControl24" runat="server"  Visible="false" BlockID="24"  />
                            <uc:BlockControl id="BlockControl25" runat="server"  Visible="false" BlockID="25"  />
                            <uc:BlockControl id="BlockControl26" runat="server"  Visible="false" BlockID="26"  />
                            <uc:BlockControl id="BlockControl27" runat="server"  Visible="false" BlockID="27"  />
                            <uc:BlockControl id="BlockControl28" runat="server"  Visible="false" BlockID="28"  />
                            <uc:BlockControl id="BlockControl29" runat="server"  Visible="false" BlockID="29"  />
                            <uc:BlockControl id="BlockControl30" runat="server"  Visible="false" BlockID="30"  />
                        </div>      
                        <div>
                            <h3>Enter x/y coordinates and width and height for image

                                [Use Image Block 1: <asp:CheckBox ID="ChkUseImage1" runat="server" />]
                            </h3>
                            X:<asp:TextBox ID="TxtImg1X" runat="server"></asp:TextBox>
                            Y:<asp:TextBox ID="TxtImg1Y" runat="server"></asp:TextBox>
                            W:<asp:TextBox ID="TxtImgWidth1" runat="server"></asp:TextBox>
                            H:<asp:TextBox ID="TxtImgHeight1" runat="server"></asp:TextBox>
                            Rotation:<asp:TextBox ID="TxtImageRotation1" runat="server" Width="60px"></asp:TextBox>
                            Page No:<asp:TextBox ID="TxtImagePageNo1" runat="server" Width="60px"></asp:TextBox>
                            <input type="file" id="FileImage1" runat="server" title="Upload Image 1" />
                        </div>
                         <div>
              
                              <h3>Enter x/y coordinates and width and height for image
                                  [Use Image Block 2: <asp:CheckBox ID="ChkUseImage2" runat="server" />]
                              </h3>
                              X:<asp:TextBox ID="TxtImg2X" runat="server"></asp:TextBox>
                              Y:<asp:TextBox ID="TxtImg2Y" runat="server"></asp:TextBox>
                              W:<asp:TextBox ID="TxtImgWidth2" runat="server"></asp:TextBox>
                              H:<asp:TextBox ID="TxtImgHeight2" runat="server"></asp:TextBox>
                              Rotation:<asp:TextBox ID="TxtImageRotation2" runat="server" Width="60px"></asp:TextBox>
                              Page No:<asp:TextBox ID="TxtImagePageNo2" runat="server" Width="60px"></asp:TextBox>
                              <input type="file" id="FileImage2" runat="server" title="Upload Image 2" />
                        </div>
                         <div>              
                              <h3>Enter x/y coordinates and width and height for image
                                  [Use Image Block 3: <asp:CheckBox ID="ChkUseImage3" runat="server" />]
                              </h3>
                              X:<asp:TextBox ID="TxtImg3X" runat="server"></asp:TextBox>
                              Y:<asp:TextBox ID="TxtImg3Y" runat="server"></asp:TextBox>
                              W:<asp:TextBox ID="TxtImgWidth3" runat="server"></asp:TextBox>
                              H:<asp:TextBox ID="TxtImgHeight3" runat="server"></asp:TextBox>
                              Rotation:<asp:TextBox ID="TxtImageRotation3" runat="server" Width="60px"></asp:TextBox>
                              Page No:<asp:TextBox ID="TxtImagePageNo3" runat="server" Width="60px"></asp:TextBox>
                              <input type="file" id="FileImage3" runat="server" title="Upload Image 3" />
                        </div>
                         <div>              
                              <h3>Enter x/y coordinates and width and height for image
                                  [Use Image Block 4: <asp:CheckBox ID="ChkUseImage4" runat="server" />]
                              </h3>
                              X:<asp:TextBox ID="TxtImg4X" runat="server"></asp:TextBox>
                              Y:<asp:TextBox ID="TxtImg4Y" runat="server"></asp:TextBox>
                              W:<asp:TextBox ID="TxtImgWidth4" runat="server"></asp:TextBox>
                              H:<asp:TextBox ID="TxtImgHeight4" runat="server"></asp:TextBox>
                              Rotation:<asp:TextBox ID="TxtImageRotation4" runat="server" Width="60px"></asp:TextBox>
                              Page No:<asp:TextBox ID="TxtImagePageNo4" runat="server" Width="60px"></asp:TextBox>
                              <input type="file" id="FileImage4" runat="server" title="Upload Image 4" />
                        </div>
                         <div>              
                              <h3>Enter x/y coordinates and width and height for image
                                  [Use Image Block 5: <asp:CheckBox ID="ChkUseImage5" runat="server" />]
                              </h3>
                              X:<asp:TextBox ID="TxtImg5X" runat="server"></asp:TextBox>
                              Y:<asp:TextBox ID="TxtImg5Y" runat="server"></asp:TextBox>
                              W:<asp:TextBox ID="TxtImgWidth5" runat="server"></asp:TextBox>
                              H:<asp:TextBox ID="TxtImgHeight5" runat="server"></asp:TextBox>
                              Rotation:<asp:TextBox ID="TxtImageRotation5" runat="server" Width="60px"></asp:TextBox>
                              Page No:<asp:TextBox ID="TxtImagePageNo5" runat="server" Width="60px"></asp:TextBox>
                              <input type="file" id="FileImage5" runat="server" title="Upload Image 5" />
                        </div>
                           <div>              
                              <h3>Enter x/y coordinates and width and height for image
                                  [Use Image Block 6: <asp:CheckBox ID="ChkUseImage6" runat="server" />]
                              </h3>
                              X:<asp:TextBox ID="TxtImg6X" runat="server"></asp:TextBox>
                              Y:<asp:TextBox ID="TxtImg6Y" runat="server"></asp:TextBox>
                              W:<asp:TextBox ID="TxtImgWidth6" runat="server"></asp:TextBox>
                              H:<asp:TextBox ID="TxtImgHeight6" runat="server"></asp:TextBox>
                              Rotation:<asp:TextBox ID="TxtImageRotation6" runat="server" Width="60px"></asp:TextBox>
                              Page No:<asp:TextBox ID="TxtImagePageNo6" runat="server" Width="60px"></asp:TextBox>
                              <input type="file" id="FileImage6" runat="server" title="Upload Image 6" />
                        </div>
                         <div>
                            <h3>
                               Select Xml document:
                            </h3>
           
                            <input type="file" id="UploadXml" runat="server" title="Upload Xml" />
                        </div>
                        <div>
                            <h3>
                               Select pdf document:
                            </h3>
           
                            <input type="file" id="UploadPdf" runat="server" title="Upload Pdf" />
                        </div>
      
                        <div style="margin-top:10px;">
                            <asp:Button ID="CmdConvert" runat="server" Text="Convert" OnClick="CmdConvert_Click" />

                        </div>
                    </form>
                </div>
			 </div>
		 </div> 		  
	  </div>
    </div>

    <footer>
        <div id="xrx_bnr_ftr">
	  	    <div id="xrx_bnr_ftr_inner">
	            <ul class="xrx_bnr_third_party_ftr_nav">
                    <li class="xrx_bnr_first"><a name="&amp;lid=cell-" href="#"></a></li>
                </ul>
	              <p class="xrx_bnr_third_party_ftr_copyright">	            
			        &copy; 1999-<%=DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)%> XEROX CORPORATION. All rights reserved.	
	            </p>
	        </div>
		</div> 
    </footer>
</body>
</html>
