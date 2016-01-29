<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chat_input.aspx.cs" Inherits="chat_chat_input" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Chat With Counselor</title>
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
      <div class="col-md-8 col-md-offset-2">
        <div class="well well-sm">
          <form runat="server">

            <asp:ScriptManager ID="ScriptManager1" runat="server" />

            <asp:Timer runat="server" ID="UpdateTimer" Interval="2000" OnTick="UpdateTimer_Tick" />

            <div class="panel panel-primary">
              <div class="panel-heading">
                <span class="glyphicon glyphicon-comment"></span>&nbsp; Chat
                    <div class="btn-group pull-right">
                      <button type="button" class="btn btn-default btn-xs dropdown-toggle" data-toggle="dropdown">
                        <span class="glyphicon glyphicon-chevron-down"></span>
                      </button>
                      <ul class="dropdown-menu slidedown">
                        <li><a href="/"><span class="glyphicon glyphicon-refresh"></span>Refresh</a></li>
                        <li><a href="/"><span class="glyphicon glyphicon-time"></span>
                          Away</a></li>
                        <li class="divider"></li>
                        <li><a href="/"><span class="glyphicon glyphicon-off"></span>
                          Sign Out</a></li>
                      </ul>
                    </div>
              </div>

              <div class="panel-body">
                <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                  <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
                    <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="UpdateButton1" EventName="Click" />
                  </Triggers>
                  <ContentTemplate>
                    <asp:Repeater ID="Repeater1" runat="server">
                      <HeaderTemplate>
                        <ul class="chat">
                      </HeaderTemplate>
                      <ItemTemplate>
                        <li class="<%# Eval("Position") %> clearfix">
                          <span class="pull-<%# Eval("Position") %>">
                            <img src="../App_Include/image/<%# Eval("Role") %>.gif" alt="User Avatar" class="img-circle" />
                          </span>
                          <div class="chat-body clearfix">
                            <div class="header">
                              <strong class="primary-font"><%# Eval("Name") %> </strong><small class="pull-right text-muted">
                                <span class="glyphicon glyphicon-time"></span><%# ((DateTime)Eval("Time")).ToShortTimeString() %></small>
                            </div>
                            <p><%# Eval("Message") %></p>
                          </div>
                          <span></span>
                        </li>
                      </ItemTemplate>
                      <FooterTemplate>
                        </ul>
                      </FooterTemplate>
                    </asp:Repeater>
                  </ContentTemplate>
                </asp:UpdatePanel>
              </div>

              <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
                  <asp:AsyncPostBackTrigger ControlID="UpdateButton1" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                  <asp:Label runat="server" ID="DateTimeLabel1" Text="Click to Update" CssClass="form-control" />
                  <asp:Button runat="server" ID="UpdateButton1" CssClass="btn btn-primary btn-sm" OnClick="UpdateButton_Click" Text="Update"></asp:Button>
                </ContentTemplate>
              </asp:UpdatePanel>

              <div class="row">
                <div class="col-md-12">
                  <CKEditor:CKEditorControl ID="CKEditor1" CssClass="form-control" runat="server"></CKEditor:CKEditorControl>
                </div>
              </div>
            </div>

            <div class="row">
              <div class=" form-group">
                <div class="col-md-10">
                  <asp:UpdateProgress runat="server" ID="PageUpdateProgress">
                    <ProgressTemplate>
                      <!--<div class="progress progress-striped active">
                        <div class="progress-bar" style="width: 100%">
                        </div>
                      </div>-->
                    </ProgressTemplate>
                  </asp:UpdateProgress>
                </div>

                <div class="col-md-2 text-right">
                  <asp:Button ID="Button1" CssClass="btn btn-primary btn-sm" OnClick="Button1_Click" Text="Send" runat="server"></asp:Button>
                </div>
              </div>
            </div>

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
