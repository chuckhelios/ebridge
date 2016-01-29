<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLg.master" AutoEventWireup="true" CodeFile="add_link.aspx.cs" Inherits="dialog_add_link" %>

<asp:Content ID="Content1" ContentPlaceHolderID="LeftContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
  <div>
    <asp:ObjectDataSource ID="dsLink" runat="server" InsertMethod="createLink" SelectMethod="getAllLinks" TypeName="RESOURCE_LINK">

    </asp:ObjectDataSource>
    <asp:FormView ID="fvLink" runat="server" DataSourceID="dsLink" DefaultMode="Insert" OnItemInserted="fvLink_ItemInserted" >
      <InsertItemTemplate>
        Display Text:
        <br />
        <asp:TextBox ID="LinkNameTextBox" runat="server" Text='<%#: Bind("URL_TEXT") %>' />
        <asp:RequiredFieldValidator ID="rfLinkName" runat="server" ControlToValidate="LinkNameTextBox" ErrorMessage="Enter Link Name">
        </asp:RequiredFieldValidator>
        <br />
        Url:
        <br />
        <asp:TextBox ID="TextBox1" runat="server" Text='<%#: Bind("URL") %>' />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage="Enter Link Name">
        </asp:RequiredFieldValidator>
        <br />
        Additional Information:
        <br />
        <asp:TextBox ID="TextBox2" runat="server" Text='<%#: Bind("ADDTL_TEXT") %>' />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox2" ErrorMessage="Enter Link Name">
        </asp:RequiredFieldValidator>
        <br />
        Display Order:
        <br />
        <asp:TextBox ID="TextBox3" runat="server" Text='<%#: Bind("RANK") %>' />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox3" ErrorMessage="Enter Link Name">
        </asp:RequiredFieldValidator>
        <br />
        Active:
        <br />
        <asp:CheckBox ID="CheckBox1" runat="server" Checked='true' />
        <br />
        <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="true" CommandName="Insert" Text="Insert" />
        &nbsp;
        <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="false" CommandName="Cancel" Text="Cancel" OnClick="fvLink_ItemInserted"/>
      </InsertItemTemplate>

    </asp:FormView>
  </div>
</asp:Content>

