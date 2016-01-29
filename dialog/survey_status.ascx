<%@ Control Language="C#" AutoEventWireup="true" CodeFile="survey_status.ascx.cs" Inherits="dialog_survey_status" %>

<asp:Repeater ID="Repeater1" runat="server" ItemType="PAGE_REF" OnItemDataBound="Repeater1_ItemDataBound">
  <ItemTemplate>
    <li class="list-group-item">
      <a href="#qstn_<%#: Item.pid %>" data-toggle="collapse"><span class="pull-right badge">page <%#: Item.pid %></span><%#: Item.value %></a>
      <ul id="qstn_<%#: Item.pid %>" class="collapse">
        <asp:Repeater ID="Subrepeater1" ItemType="QUESTION_REF" runat="server">
          <ItemTemplate>
            <li>
              <p data-toggle="tooltip" data-placement="top" data-original-title="<%#: Helper.SanitizeHtml(HttpUtility.HtmlDecode(Item.content)) %>">
                <%#: Item.PAGE_REF.value + " " + Item.qid %>
              </p>
              <%# Item.DisplayResponses %>
            </li>
          </ItemTemplate>
        </asp:Repeater>
      </ul>
    </li>
  </ItemTemplate>

</asp:Repeater>
