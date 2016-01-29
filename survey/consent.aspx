<%@ Page Language="C#" MasterPageFile="master/survey.master" AutoEventWireup="true" CodeFile="consent.aspx.cs" Inherits="survey_consent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <asp:Literal id="HeadPlace" runat="server"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <asp:Label runat="server" id="ConsentLabel" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="foot" Runat="Server" >
    <asp:Literal id="FootPlace" runat="server"/>
</asp:Content>