<%@ Page Language="C#" AutoEventWireup="true" CodeFile="change_hours.aspx.cs" Inherits="dialog_change_hours" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Change Chat Availability</title>
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <!-- JQuery UI -->
  <link href="../App_Include/bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />

  <!-- Bootstrap Core CSS -->
  <link rel="stylesheet" href="../App_Include/jquery/jquery-ui-1.11.2/jquery-ui.min.css" />
  <link href="../App_Include/css/ebridge.css" rel="stylesheet" />
</head>
<body>
  <div class="container">
    <div class="row top-buffer">
      <div class="col-md-6 col-md-offset-3">
        <div class="well well-sm">
          <form id="form1" class="form-horizontal" runat="server" method="post" action="action.aspx?p=change_hours">
            <fieldset>
              <asp:ListView ID="ListView1" runat="server" GroupItemCount="1">
                <LayoutTemplate>
                  <legend class="text-center">Change Online Chat Availability</legend>
                  <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>

                  <div class="form-group">
                    <div class="col-md-6 col-md-offset-3 text-right">
                      <asp:Button ID="Button1" PostBackUrl="action.aspx?p=change_hours" Text="Submit" class="btn btn-primary btn-sm" runat="server"></asp:Button>
                    </div>
                  </div>
                </LayoutTemplate>

                <GroupTemplate>
                  <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                </GroupTemplate>

                <ItemTemplate>

                  <div class="form-group">
                    <label class="col-md-3 control-label" for="<%# Eval("Weekday") %>"><%# Eval("Weekday") %> </label>
                    <div class="col-md-6">
                      <input name="<%# Eval("Weekday") %>" type="text" value="<%# Eval("Value") %>" class="form-control">
                    </div>
                    <div class="col-md-3">
                    </div>
                  </div>
                </ItemTemplate>

              </asp:ListView>

            </fieldset>
          </form>
        </div>
      </div>
    </div>
  </div>
    <!-- Jquery -->
  <script src="../App_Include/jquery/jquery-1.11.0.min.js"></script>
  <script src="../App_Include/jquery/jquery-ui-1.11.2/jquery-ui.min.js"></script>
  <script src="../App_Include/bootstrap/js/bootstrap.min.js"></script>
  <script src="../App_Include/javascript/ebridge.js"></script>
</body>
</html>
