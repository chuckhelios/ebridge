<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLg.master" AutoEventWireup="true" CodeFile="dialog_c.aspx.cs" Inherits="dialog_dialog_c" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="uc" TagName="SurveyStatus" Src="~/dialog/survey_status.ascx" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Modal" runat="server">
  <div class="modal fade" id="modal2" tabindex="-1" role="dialog" aria-labelledby="modal1label" aria-hidden="true">
    <div class ="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" id="button-close2" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <h4 class="modal-title" id="exampleModalLabe2">Send Email to Participant</h4>
        </div>
        <div class="modal-body">
          <div class="form-group">
            <label for="recipient-name" class="control-label">Recipient:</label>
            <input type="text" class="form-control" id="recipient-name">
          </div>
          <div class="form-group">
            <label for="message-text" class="control-label">Message:</label>
            <textarea class="form-control" id="message-text"></textarea>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
          <button type="button" class="btn btn-primary">Send message</button>
        </div>
        </div>
      </div>
    </div>
  <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="modal1label" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" id="button-close1" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <h4 class="modal-title" id="exampleModalLabel">Change Online Chat Availability</h4>
        </div>
        <div class="modal-body">
          <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
            <Triggers>
              <asp:AsyncPostBackTrigger ControlID="ButtonSend" EventName="Click" />
            </Triggers>
            <ContentTemplate>
              <asp:Repeater runat="server" ID="RepeaterHours">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                  <div class="form-group">
                    <label for="<%#: ((WeekdayHours)Container.DataItem).Weekday %>" class="control-label"></label>
                    <input type="text" value="<%#: ((WeekdayHours)Container.DataItem).Hour %>" class="form-control" />
                  </div>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
              </asp:Repeater>
            </ContentTemplate>
          </asp:UpdatePanel>
        </div>
        <div class="modal-footer">
          <asp:Button runat="server" ID="ButtonClose" CssClass="btn btn-default" Text="Close" />
          <asp:Button runat="server" ID="ButtonSend" CssClass="btn btn-primary" OnClientClick="return false;" Text="Submit" />
        </div>
      </div>
    </div>
  </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Navigation" runat="server">
  <ul class="nav nav-pills nav-stacked">
    <asp:Repeater ID="RepeaterNav" runat="server" OnItemDataBound="getLinkAttr">
      <ItemTemplate>
        <li>
          <asp:HyperLink runat="server" ID="HyperLink1">
          </asp:HyperLink>
        </li>
      </ItemTemplate>
    </asp:Repeater>
  </ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftContent" runat="Server">

  <div class="row">
    <div class="col-md-12">
      <div class="list-group">
        <a href="#" class="list-group-item active">Participant Summary
        </a>
        <ul class="list-group nav" id="summary1">

          <uc:SurveyStatus runat="server" ID="SurveyStatus1" />

        </ul>
      </div>
    </div>
  </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
  <div class="panel col-md-10">
    <asp:FormView ID="FormView1" runat="server" ItemType="PARTICIPANT" RenderOuterTable="false">
      <ItemTemplate>
        <div class="panel-heading col-md-12">
          <span class="glyphicon glyphicon-user"></span>&nbsp <%#: Item.ID %>
          <div class="btn-group pull-right">
            <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
              <span class="glyphicon glyphicon-chevron-down"></span>
            </button>
            <ul class="dropdown-menu slidedown">
              <li>&nbsp Name:  <%#: Item.NAME_FIRST %></li>
              <li>&nbsp Year:  <%#: Item.YEAR %></li>
              <li class="divider"></li>
              <li><a href="email.aspx?id=<%#: Item.ID %>"><span class="glyphicon glyphicon-envelope"></span><%#: Item.EMAIL %></a></li>
            </ul>
          </div>
        </div>
      </ItemTemplate>
    </asp:FormView>

    <div class="row">
      <div class="col-md-12">
        <CKEditor:CKEditorControl ID="CKEditor1" CssClass="form-control" runat="server"></CKEditor:CKEditorControl>
      </div>
    </div>
    <div class="row">
      <div class="col-md-12 text-right">
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
          <asp:Button runat="server" ID="Button1" CssClass="btn btn-primary btn-sm" OnClick="SendMessage" Text="Send" />

        </asp:PlaceHolder>

      </div>
    </div>
  </div>
  <div class="row">
  </div>
  <div class="col-md-10">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
      <Triggers>
        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
      </Triggers>
      <ContentTemplate>
        <asp:Repeater ID="Repeater2" ItemType="MESSAGE_ALT" runat="server">
          <HeaderTemplate>
            <div class="panel">
              <div class="panel-heading">
              </div>
              <ul class="chat">
          </HeaderTemplate>
          <ItemTemplate>
            <li class="<%#: Item.Position %> clearfix">
              <span class="pull-<%#: Item.Position %>">
                <img src="../App_Include/image/<%#: convertUser(Item.FROM_ID, Item.TO_ID) %>.gif" alt="User Avatar" class="img-circle" />
              </span>
              <div class="chat-body clearfix">
                <div class="header">
                  <strong class="primary-font"><%#: getParticipantId(Item.TO_ID, Item.FROM_ID) %> </strong><small class="pull-right text-muted">
                    <span class="glyphicon glyphicon-time"></span><%#: Item.DATE_TIME.ToShortTimeString() %></small>
                </div>
                <p><%# HttpUtility.HtmlDecode((string) Eval("MESSAGE_BODY")) %></p>
              </div>
              <span></span>
            </li>
          </ItemTemplate>
          <FooterTemplate>
            </ul>
              </div>
          </FooterTemplate>
        </asp:Repeater>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>

