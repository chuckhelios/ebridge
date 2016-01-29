<%@ Control Language="C#" AutoEventWireup="true" CodeFile="display_links.ascx.cs" Inherits="dialog_change_links" %>


<asp:ObjectDataSource ID="dsLink" runat="server" InsertMethod="createLink" DeleteMethod="deleteLink" SelectMethod="getAllLinks" TypeName="RESOURCE_LINK" UpdateMethod ="updateLink">
  <DeleteParameters>
    <asp:Parameter Name ="RID" Type="Int32" />
  </DeleteParameters>
  <UpdateParameters>
    <asp:Parameter Name="RID" Type="Int32" />
    <asp:Parameter Name="URL_TEXT" Type="String" />
    <asp:Parameter Name="URL" Type="String" />
    <asp:Parameter Name="ADDTL_TEXT" Type="String" />
    <asp:Parameter Name="RANK" Type="Int32" />
    <asp:Parameter Name="ACTIVE" Type="String" />
  </UpdateParameters>
</asp:ObjectDataSource>
<asp:GridView ID="gvLink" runat="server" AllowPaging ="True" AutoGenerateColumns="false" DataSourceID="dsLink" DataKeyNames="RID">
  <Columns>
    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
    <asp:BoundField DataField="RID" HeaderText="RID" SortExpression="RID" />
    <asp:BoundField DataField="URL_TEXT" HeaderText="Display Text" />
    <asp:BoundField DataField="URL" HeaderText="Url" />
    <asp:BoundField DataField="ADDTL_TEXT" HeaderText="Additional Information" />
    <asp:BoundField DataField="RANK" HeaderText="Rank" />
    <asp:BoundField DataField="ACTIVE" HeaderText="Active" />
  </Columns>
</asp:GridView>
<asp:Button ID="AddLink" runat="server" Text ="Add" OnClick="addLink" />

