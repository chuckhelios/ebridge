<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLg.master" AutoEventWireup="true" CodeFile="login_alt.aspx.cs" Inherits="dialog_login_alt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Modal" runat="Server">

  <div class="modal fade" id="login-modal" tabindex="-1" role="dialog" aria-labelledby="loginmodallabel" aria-hidden="true">
    <input type="hidden" id="from-url" value="<%= this.fromUrl  %>" />
    <input type="hidden" id="user-type" value="<%= this.UserType %>" />
    <div class="modal-dialog">
      <div class="modal-content">
        <!-- /.modal-header -->
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
          <h4 class="modal-title" id="login-modal-title">Log in</h4>
        </div>
          <div class="modal-body">
            <asp:Repeater runat="server" ID="Repeater1" ItemType="LoginUser" OnItemDataBound="bindLogins">
              <ItemTemplate>
                <div class="form-group">
                  <div class="input-group">
                    <%# RenderControlToHtml(Item.UserName) %>
                    <label for="uLogin" class="input-group-addon glyphicon glyphicon-user"></label>
                  </div>
                </div>

                <div class="form-group">
                  <div class="input-group">
                    <input type="password" class="form-control" id="uPassword" placeholder="Password">
                    <label for="uPassword" class="input-group-addon glyphicon glyphicon-lock"></label>
                  </div>
                  <!-- /.input-group -->
                </div>
                <!-- /.form-group -->

                <%# RenderControlToHtml(Item.CheckBox) %>
                <!-- /.checkbox -->
              </ItemTemplate>
            </asp:Repeater>
          </div>
          <!-- /.modal-body -->

          <div class="modal-footer">
            <div class ="col-md-6">
            <asp:Button runat="server" ID="ButtonClose" CssClass="form-control btn btn-default" Text="Close" />
              </div>
            <div class ="col-md-6">
            <!-- default behavior of the modal's button is to cause postback - OnClientClick='return false;' is added to prevent this" -->
            <asp:Button runat="server" ID="ButtonSend" CssClass="form-control btn btn-primary" OnClientClick="return false;" Text="Login" />
              </div>
          </div>
          <!-- /.modal-footer -->
        </div>
        <!-- /.modal-content -->
      </div>
    <!-- /.modal-dialog -->
  </div>
  <!-- /.modal -->
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="Server">
  <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="Server">
</asp:Content>

