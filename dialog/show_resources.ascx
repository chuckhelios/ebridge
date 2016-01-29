<%@ Control Language="C#" AutoEventWireup="true" CodeFile="show_resources.ascx.cs" Inherits="dialog_show_resources" %>

<asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="columnDataSource">

  <HeaderTemplate>
    <table style='line-height: 15px; font-size: 14px; padding-top: 20px'>
      <thead>
        <tr>
          <td>
            <br />
            <u>Resources at <%#: SchoolInfo["name"] %></u>
          </td>
        </tr>
        <tr>
          <td>&nbsp  
          </td>
        </tr>
      </thead>
      <tbody>
        <tr>
  </HeaderTemplate>
  <ItemTemplate>
    <td style="padding-left: 5px; vertical-align: top; text-align: left">
      <asp:Repeater ID="Repeater2" ItemType="RESOURCE_LINK" runat="server">
        <ItemTemplate>
          <p>
            <a href='/ebridge/redirect.aspx?action=resources&url=s<%#: Item.RID %>' target='_blank'>
              <%#: Item.URL_TEXT %>
            </a>
          </p>
          <p>
            <%#: Item.ADDTL_TEXT %>
          </p>
          <br />
        </ItemTemplate>
      </asp:Repeater>
    </td>
  </ItemTemplate>
  <FooterTemplate>
    </tr>
    </tbody>
    </table>
  </FooterTemplate>
</asp:Repeater>
