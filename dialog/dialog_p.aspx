<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLg.master" AutoEventWireup="true" CodeFile="dialog_p.aspx.cs" Inherits="dialog_dialog_p" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="uc" TagName="ShowResources" Src="~/dialog/show_resources.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Modal" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Navigation" runat="server">
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

<asp:Content ID="Content3" ContentPlaceHolderID="LeftContent" runat="Server">
  <div class="row">
    <div class="col-md-12">
      <div class="list-group">
        <a href="#" class="list-group-item active">Additional Resources
        </a>
        <ul class="list-group nav" id="summary1">

          <uc:ShowResources runat="server" ID="ShowResources1" />

        </ul>
      </div>
    </div>
  </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">
  <div class="panel col-md-10">
    <asp:FormView ID="FormView1" runat="server" ItemType="PARTICIPANT" EnableViewState="true">
      <ItemTemplate>
        <div class="panel-heading">
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
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>

