<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestRssFeed.aspx.cs" Inherits="Kabar_admin.TestRssFeed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-control">
        <label for="urlinput" class="col-8">RSS Feed Url</label>
        <input type="url" id="urlinput"  runat="server" class="col-12 form-control" required/>
    </div>
    <div class="form-control">
        <asp:Button ID="Button1" runat="server" Text="Test Source" OnClick="Button1_Click" CssClass="btn btn-danger" />
</div>
     <div id="Rssfeed" runat="server"></div>
</asp:Content>
