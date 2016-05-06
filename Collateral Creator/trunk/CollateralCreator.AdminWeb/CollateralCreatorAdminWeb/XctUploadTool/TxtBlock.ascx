<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TxtBlock.ascx.cs" Inherits="XCTUploadToolWeb.TxtBlock" %>
 <div style="width:100%; position:relative; display:block; float:left; margin-top:5px; margin-bottom:5px; ">
     <div style="width:100%; position:relative; display:block; ">    
            <h3>Enter x/y coordinates and width and height for text block <asp:Label ID="LblBlockID" runat="server"></asp:Label></h3>
     </div>

     <div style="width:100%; position:relative; display:block; float:left ;margin-bottom:10px;">  
        <div style="width:150px;  position:relative; display:block; float:left;  margin-right:5px; ">    
                Page No:<asp:TextBox ID="TxtPageNo" runat="server"  Width="50px">1</asp:TextBox>
        </div>
        <div style="width:100px;  position:relative; display:block;float:left; margin-right:5px; ">  
            X:<asp:TextBox ID="TxtX" runat="server" Width="50px"></asp:TextBox>
        </div>
        <div style="width:100px; position:relative; display:block; float:left; margin-right:5px; ">   
            Y:<asp:TextBox ID="TxtY" runat="server" Width="50px"></asp:TextBox>
        </div>
         <div style="width:100px; position:relative; display:block; float:left; margin-right:5px; ">  
            W:<asp:TextBox ID="TxtWidth" runat="server" Width="50px"></asp:TextBox>
        </div>
        <div style="width:100px;  position:relative; display:block; float:left; margin-right:5px; ">   
            H:<asp:TextBox ID="TxtHeight" runat="server" Width="50px"></asp:TextBox>
        </div>
        <div style="width:150px;  position:relative; display:block; float:left;  margin-right:5px; ">    
                Rotation:<asp:TextBox ID="TxtRotation" runat="server" Width="50px">0</asp:TextBox>
        </div>
       

     </div>
    <div style="width:100%; position:relative; display:block; float:left ;margin-bottom:10px;">  
        <div style="width:220px;  position:relative; display:block; float:left ">  
            Font type:<asp:DropDownList ID="DrpFont" runat="server">
                            <asp:ListItem Text="XeroxSans" Value="XeroxSans" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="XeroxSans-Light" Value="XeroxSans-Light"></asp:ListItem>
                            <asp:ListItem Text="XeroxSans-Bold" Value="XeroxSans-Bold"></asp:ListItem>
                            <asp:ListItem Text="HelveticaNeue" Value="HelveticaNeue"></asp:ListItem>
                            <asp:ListItem Text="HelveticaNeue-Bold" Value="HelveticaNeue-Bold"></asp:ListItem>
                      </asp:DropDownList>
         </div>
         <div style="width:150px;  position:relative; display:block; float:left;  margin-right:5px; ">    
            Font size:<asp:TextBox ID="TxtFontSize" runat="server" Width="50px"></asp:TextBox>
         </div>
          <div style="width:200px;  position:relative; display:block; float:left;  margin-right:5px; ">  
               Font colour:<asp:DropDownList ID="DrpFontColour" runat="server">
                           
                            <asp:ListItem Text="Black" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Gray" Value="8224125"></asp:ListItem>
                            <asp:ListItem Text="Dark Gray" Value="5855577"></asp:ListItem>
                            <asp:ListItem Text="Red" Value="14230065"></asp:ListItem>
                            <asp:ListItem Text="Green" Value="7188285"></asp:ListItem>
                            <asp:ListItem Text="Blue" Value="2659797"></asp:ListItem>
                            <asp:ListItem Text="Orange" Value="15103488"></asp:ListItem>
                            <asp:ListItem Text="Turquosie" Value="3456186"></asp:ListItem>
                            <asp:ListItem Text="Violet" Value="10167683"></asp:ListItem>
                            <asp:ListItem Text="White" Value="16777215"></asp:ListItem>
                      </asp:DropDownList>
        </div>
    </div>
    
    <div id="DivRGB" runat="server" visible="false"  style="width:100%;  position:relative; display:block; float:left; margin-bottom:10px; float:left; border-bottom:1px dotted grey; padding-bottom:5px; ">
        <div style="width:350px;  position:relative; display:block; float:left  ">  
            <span style="vertical-align:top;">
                If colour doesn't appear add RGB settings <asp:CheckBox ID="ChkUseRGB" runat="server" />
            </span>
        </div>
         <div style="width:250px;  position:relative; display:block; float:left  ">  
             R:<asp:TextBox ID="TxtR" runat="server" Width="50px"></asp:TextBox>
             G:<asp:TextBox ID="TxtG" runat="server" Width="50px"></asp:TextBox>
             B:<asp:TextBox ID="TxtB" runat="server" Width="50px"></asp:TextBox>
          </div>
        
     </div>
    
    <div style="width:100%;  position:relative; display:block; float:left; margin-bottom:10px; float:left; border-bottom:1px dotted grey; padding-bottom:5px; ">
    </div>
    
 </div>
