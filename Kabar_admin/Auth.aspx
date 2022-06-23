<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Auth.aspx.cs" Inherits="Kabar_admin.Auth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
      <link rel="icon" href="Content/a.png">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Authentication</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Auth.css" rel="stylesheet" />

</head>
<body>
   <form class="form-signin" runat="server">
      <div class="text-center mb-4">
        <img class="mb-4" src="Content/a.png" alt="" width="72" height="72">
        <h1 class="h3 mb-3 font-weight-normal text-danger">khabar App</h1>
        <p class="h6 text-primary">Identify yourself</p>
      </div>

      <div class="form-label-group">
         
        <input type="email" id="inputEmail" class="form-control" placeholder="Email address" runat="server" required autofocus="autofocus" >
        <label for="inputEmail">Email address</label>
      </div>

      <div class="form-label-group">
         
        <input type="password" id="inputPassword" class="form-control" placeholder="Password"  runat="server" required autofocus="autofocus"  >
        <label for="inputPassword">Password</label>
      </div>
        <div class="form-label-group">
            <asp:Label ID="error" runat="server" Text="Please Check Email and Password." CssClass="text-danger" Visible="false"></asp:Label>
            </div>
      <div class="checkbox mb-3">
        <label>
          <input type="checkbox" value="remember-me"> Remember me
        </label>
      </div>
       <asp:Button  ID="loginbtn" runat="server" CssClass="btn btn-lg btn-primary btn-block" Text="Sign in" OnClick="loginbtn_Click"/>
      
      <p class="mt-5 mb-3 text-muted text-center">&copy; 2017-2018</p>
    </form>
</body>
</html>
