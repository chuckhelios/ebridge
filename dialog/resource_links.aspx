<%@ Page Title="" Language="C#" MasterPageFile="~/MasterLg.master" AutoEventWireup="true" CodeFile="resource_links.aspx.cs" Inherits="dialog_resource_links" %>

<%@ Register Src="~/dialog/display_links.ascx" TagName="displayLinks" TagPrefix="uc" %>
<%@ Register Src="~/dialog/show_resources.ascx" TagName="showResources" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="LeftContent" runat="Server">
  <uc:displayLinks ID="displayLinks1" runat="server"></uc:displayLinks>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterContent" runat="server">
  <uc:showResources ID="showResources1" runat="server" />
</asp:Content>
