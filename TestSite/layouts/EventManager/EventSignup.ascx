<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventSignup.ascx.cs" Inherits="TestSite.layouts.EventManager.EventSignup" %>
<asp:Label runat="server" AssociatedControlID="Name" Text="Name"/>
<asp:TextBox runat="server" ID="Name" />
<br />

<asp:Label runat="server" AssociatedControlID="Email" Text="Email"/>

<asp:TextBox runat="server" ID="Email"/>
<br />

<asp:Button runat="server" OnClick="Signup"/>