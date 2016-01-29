<%@ Page Language="C#" AutoEventWireup="true" CodeFile="screen_alt.aspx.cs" Inherits="survey3_screen_alt" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <!-- JQuery UI -->
  <link href="../App_Include/bootstrap/css/bootstrap.min.css" rel="stylesheet" media="screen" />

  <!-- Bootstrap Core CSS -->
  <link rel="stylesheet" href="../App_Include/jquery/jquery-ui-1.11.2/jquery-ui.min.css" />
  <link href="../App_Include/css/ebridge.css" rel="stylesheet" />
</head>
<body>
  <form id="form1" runat="server">
    <asp:HiddenField ID="Hidden1" runat="server" />
    <div class="container">
      <div class="row top-buffer">
        <div class="col-md-8 col-md-offset-2">
          <div class="well well-sm">

            <%--image banner--%>
            <div class="row">
              <asp:Image ID="Image1" runat="server" />
            </div>

            <%--debug info--%>
            <div class="row">
              <div class="col-md-12">
                DEBUG (same as participant info module)
                <br />
                <br />
                <br />
              </div>
            </div>

            <%--prog bar and question image--%>
            <div class="row">
              <div class="col-md-6">
                <asp:Image ID="Image2" runat="server" />
              </div>
              <div class="col-md-6">
                <p>Question Progress</p>
                <div class="progress">
                  <div id="progress_bar" class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 60%;" runat="server">
                    0/0
                  </div>
                </div>

              </div>
            </div>

            <%--survey--%>
            <div class="row">
              <div class="col-md-12">
                <asp:ListView ID="ListView1" runat="server" GroupItemCount="1">
                  <LayoutTemplate>
                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                    <asp:Button ID="Button1" PostBackUrl="action.aspx?p=data" Text="Submit" class="btn btn-primary btn-sm" runat="server"></asp:Button>
                  </LayoutTemplate>

                  <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                  </GroupTemplate>

                  <ItemTemplate>

                    <div class="row">
                      <div class="col-md-2"><%# Eval("Value") %>_<%# Eval("Order") %>:</div>
                      <div class="col-md-8"> <%# Eval("Content") %> </div>
                    </div>
                    <div class="row">
                      <div class="col-md-8 col-md-offset-1">
                        <p>
                          <label for="value_<%# Eval("Order") %>">Answer:</label>
                          <input type="text" name="<%# Eval("Value") %>_<%# Eval("Order") %>" readonly style="border: 0; color: #f6931f; font-weight: bold;">
                        </p>
                        <div name="<%# Eval("Value") %>_<%# Eval("Order") %>" class="slider"></div>
                        <%--<input name="<%# Eval("Order") %>" type="text" value="<%# Eval("Value") %>" class="form-control">--%>

                        <%--<div class="bfh-slider" data-name="<%# Eval("Order") %>" data-min="0" data-max="<%# Eval("Scale") %>">--%>
                      </div>
                    </div>
                  </ItemTemplate>

                </asp:ListView>
              </div>
            </div>


          </div>
        </div>
      </div>
    </div>
  </form>
  <!-- Jquery -->
  <script src="../App_Include/jquery/jquery-1.11.0.min.js"></script>
  <script src="../App_Include/jquery/jquery-ui-1.11.2/jquery-ui.min.js"></script>
  <script src="../App_Include/bootstrap/js/bootstrap.min.js"></script>
  <script src="../App_Include/javascript/ebridge.js"></script>
</body>
</html>
