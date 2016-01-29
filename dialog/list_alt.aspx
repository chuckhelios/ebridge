<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLg.master" AutoEventWireup="true" CodeFile="list_alt.aspx.cs" Inherits="dialog_list_alt" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Modal" runat="server">
  <div class="modal fade" id="modal2" tabindex="-1" role="dialog" aria-labelledby="modal1label" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" id="button-close2" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
          <h4 class="modal-title" id="exampleModalLabe2">Send Email to Participant</h4>
        </div>
        <div class="modal-body">
          <div class="input-group">
            <label for="recipient-name" class="control-label">Recipient:</label>
            <input type="text" class="form-control" id="recipient-name" disabled>
          </div>
          <div class="form-group">
              <label for="subject-text" class="control-label">Subject:</label>
              <input type="text" class="form-control" id="subject-text">
          </div>
          <div>
              <label for="message-text" class="control-label">Message:</label>
              <textarea class="form-control" id="message-text"></textarea>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
          <button type="button" class="btn btn-primary">Send message</button>
          <div class="progress progress-striped active" id="progressouter2">
          <div class="bar" id="progress2"></div>
        </div>
      </div>
    </div>
  </div>
  </div>
  <div class="modal fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="modal1label" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" id="button-close" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
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
          <asp:Button runat="server" ID="ButtonClose" data-dismiss="modal" CssClass="btn btn-default" OnClientClick="return false;" Text="Close" />
          <asp:Button runat="server" ID="ButtonSend" CssClass="btn btn-primary" OnClientClick="return false;" Text="Submit" />
          <div class="progress progress-striped active" id="progressouter1">
          <div class="bar" id="progress1"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Navigation" runat="server">
  <input type="hidden" id="participant-id" value="#" />
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
  <asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
      <div class="row" id="left-content">
        <div class="col-md-12">
          <div class="list-group">
            <a href="#" class="list-group-item active">Participant Summary</a>
            <ul class="list-group">
    </HeaderTemplate>

    <ItemTemplate>
      <li class="list-group-item">
        <span class="badge"><%# Eval("Value") %></span>
        <%# Eval("Description") %>
      </li>
    </ItemTemplate>

    <FooterTemplate>
      </ul>
      </div>
      </div>
      </div>
    </FooterTemplate>

  </asp:Repeater>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
  <div class="row">
    <div class="col-md-12">
      <asp:Label ID="Label1" runat="server"></asp:Label>

      <asp:Repeater ID="Repeater2" runat="server">
        <HeaderTemplate>
          <table class="table table-striped table-hover">
            <thead id="listheader">
              <tr>
                <th>ID</th>
                <th>Assigned To</th>
                <th>Last Student Post</th>
                <th>Student Post #</th>
                <th>Counselor Post #</th>
                <th>Survey Completed On</th>
                <th>Last Counselor Post</th>
              </tr>
            </thead>
        </HeaderTemplate>

        <ItemTemplate>
          <tr>
            <td><a href="dialog_c.aspx?p=counselor&id=<%# Eval("Id") %>"><span class="<%# (bool)Eval("NeedAttn") ? "label label-warning" : "label label-success" %>"><%# Eval("Id") %></span></a></td>
            <td><%# Eval("AssignedTo") %></td>
            <td>
              <small><span class="glyphicon glyphicon-time"></span><%#Eval("LastPost") != null ? ((DateTime)Eval("LastPost")).ToString("MM/dd/yy hh:mm tt") : "--" %></small>
              <p data-toggle="tooltip" data-placement="top" data-original-title="<%# Eval("LastPostContent") %>"><%# Eval("LastPostContent") != null ? abbrString((string)Eval("LastPostContent")) : "--" %></p>
            </td>
            <td><%# Eval("StudentPostCnt").ToString() %></td>
            <td><%# Eval("CounselorPostCnt").ToString() %></td>
            <td>
              <small><span class="glyphicon glyphicon-time"></span><%# ((DateTime)Eval("SurveyCompleteDate")).ToString("MM/dd/yy hh:mm tt") %></small>
            </td>
            <td>
              <small><span class="glyphicon glyphicon-time"></span><%# ((DateTime)Eval("LastCounselorPost")).ToString("MM/dd/yy hh:mm tt") %></small>
              <p data-toggle="tooltip" data-placement="top" data-original-title="<%# Eval("LastCounselorPostContent") %>"><%# Eval("LastCounselorPostContent") != null ? abbrString((string)Eval("LastCounselorPostContent")) : "--" %></p>
            </td>
          </tr>
        </ItemTemplate>

        <FooterTemplate>
          </table>
        </FooterTemplate>

      </asp:Repeater>
    </div>
  </div>
</asp:Content>

