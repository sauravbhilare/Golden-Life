<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminLogin.aspx.cs" Inherits="Admin_AdminLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="Start your development with a Dashboard for Bootstrap 4.">
    <meta name="author" content="Creative Tim">
    <title>Golder Life</title> 
    <!-- <link rel="icon" href="../Images/logo3.png" type="image/icon type"> -->
    <!-- Favicon -->
    <!-- Favicon -->
  <%--<link href="Admin/assets/img/brand/blue.png" rel="icon" type="image/png" />--%>
    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet">
    <!-- Icons -->
    <link href="logincss/assets/vendor/nucleo/css/nucleo.css" rel="stylesheet">
    <link href="logincss/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
    <!-- Argon CSS -->
    <link type="text/css" href="logincss/assets/css/argon.css?v=1.0.0" rel="stylesheet">
    <link href="logincss/assets/css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style>
.overlay {
  position: absolute;
  top: 0;
  left: 0;
  height: 100%;
  width: 100%;
  background-color: #ec1d23;
  opacity: 0.7;
  z-index: 1;
}

.masthead {
  position: relative;
  overflow: hidden;
  padding-bottom: 3rem;
  z-index: 2;
  position: absolute;
    top: 0;
    left: 0;
    height: 100%;
     width: 35%;
    z-index: 1;
}

.masthead .masthead-bg {
  position: absolute;
  top: 0;
  bottom: 0;
  right: 0;
  left: 0;
  width: 100%;
  min-height: 35rem;
  height: 100%;
  background-color: #fff;
 -webkit-transform: skewX(8deg);
       transform: skewX(8deg);
    -webkit-transform-origin: bottom left;
       transform-origin: bottom left;
}


.masthead .masthead-content p {
  font-size: 1.2rem;
}
.justify-content-center1
{
  margin-top: 10%;
}
.justify-content-center2
{
    margin: 0% 45%;
}





@media (min-width: 768px) {
  .masthead {
    height: 100%;
    min-height: 0;
  
    padding-bottom: 0;
  }
 
  .masthead .masthead-content p {
    font-size: 1.3rem;
  }
}

.social-icons 
{
     margin: 0;
  position: absolute;
 
   right: 2.5rem;
    bottom: 14rem;
 width:100%;
  z-index: 2;
}
.mobile-logo
{
   display:none;
}
 @media screen and (min-width: 320px) and (max-width: 768px) {
  .masthead
  {
      display:none;
  }
  .justify-content-center2 {
    margin: 0% 0%;
}
.mobile-logo
{
    display:block;
    background:white;
    padding:5% 0;
}
}
 @media screen  and (width: 1024px) {
  .justify-content-center1 {
    margin-top: 40%;
}
.masthead 
{
  width: 46%;
}
}
 @media screen  and (width: 768px) {
 .justify-content-center2 {
    margin: 0% 20%;
}

}
    </style>
</head>
<body><%--#1bbd36--%>
 <%--   background-image: url(Images/Loginbackgroungd1.jpg);--%>
<div class="overlay" style="background: linear-gradient(to right, #e33e3e, #fda603); background-repeat:no-repeat; background-size:cover; opacity:1;">
 <%--<img src="img/ecommerce2.jpg" width="100%" height="100%"/>--%>
      <%--<center class="mobile-logo">
    <img src="Admin/assets/img/brand/blue.png" width="150px" height="100px"/>         
          </center>--%>
       <div class="container  pb-5">
       
      <div class="row justify-content-center1">
        <div class="col-lg-5 col-md-7 justify-content-center2">
          <div class="card bg-secondary shadow border-0">
           
            <div class="card-body px-lg-5 py-lg-5">
              <div class="text-center text-muted mb-4">
              <h1 class="text-black">Golden Life Pratishthan</h1>
                      <small>sign in with credentials</small><br />
                  <b style="color:red;"><asp:Label ID="lblInValid" runat="server" Text="Invalid Credentials!!" Visible="false" ></asp:Label></b>
              </div>
           <form id="form1" runat="server" role="form">
                <div class="form-group mb-3">
                  <div class="input-group input-group-alternative">
                    <div class="input-group-prepend">
                      <span class="input-group-text"><i class="ni ni-email-83"></i></span>
                    </div>
                    <%--<input class="form-control" placeholder="Email" type="email">--%>
                    <asp:TextBox ID="txtUsername" cssClass="form-control" placeholder="Username" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter Username." CssClass="val-msg" ControlToValidate="txtUsername" ValidationGroup="login" ></asp:RequiredFieldValidator>
                  </div>
                </div>
                <div class="form-group">
                  <div class="input-group input-group-alternative">
                    <div class="input-group-prepend">
                      <span class="input-group-text"><i class="ni ni-lock-circle-open"></i></span>
                    </div>
                    <%--<input class="form-control" placeholder="Password" type="password">--%>
                    <asp:TextBox ID="txtPassword" cssClass="form-control" placeholder="Password" runat="server" TextMode="Password" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter Password." CssClass="val-msg" ControlToValidate="txtPassword" ValidationGroup="login" ></asp:RequiredFieldValidator>


                  </div>
                </div>
                <%--<div class="col-6">
                  <asp:LinkButton ID="LinkForgotPass" OnClick="LinkForgotPass_Click" CssClass="text-light" runat="server">Forgot Password ?</asp:LinkButton>
                </div>--%>
                <div class="text-center">
                  <asp:Button ID="BtnLogin" runat="server" Text="Login" CssClass="btn" style="border-radius:5px; background-color:#dc9544; color:white;font-size:16px;padding-top:5px;" 
                   onclick="btnSignIn_Click"  CausesValidation="true" ValidationGroup="login" />
                </div>
              </form>
            </div>
          </div>
          <div class="row mt-3">
            <div class="col-6">
              <%--<asp:LinkButton ID="LinkForgotPass" OnClick="LinkForgotPass_Click" CssClass="text-light" runat="server">Forgot Password ?</asp:LinkButton>--%>
            </div>
            <div class="col-6 text-right">
              <!--<a href="#" class="text-light"><small>Create new account</small></a>-->
            </div>
          </div>
        </div>
      </div>
    </div>
     </div>
   

    <div class="masthead">
      <div class="masthead-bg"></div>
      <div class="container h-100">
        <div class="row h-100">
          <div class="col-12 my-auto">
            <div class="masthead-content text-white py-5 py-md-0">
              <center>
    <%--<img src="Admin/assets/img/brand/blue.png" width="200px" height="150px"/>--%>
          <table >
                  <img src="Images/logo2.jpg" style="width:367px;" />
                  <tbody style="display:none">
                  <tr>
                  <td> <span class="h5 font-weight-bold mb-0 text-orange" style="color:#046791;font-weight:bold !important"><i class="fas fa-envelope" style="font-size: 20px;margin: 5px;"></i></span></td>
                  <td><span class="h5 font-weight-bold mb-0" style="color:#000;font-weight:bold !important"> </span></td>
                  <td><span class="h5 font-weight-bold mb-0" style="color:#000;font-weight:initial !important"> admin@hillsopt.in</span></td>
                 <%-- -<asp:Label ID="Label2" runat="server" Text="0" class="badge badge-pill badge-success"></asp:Label>--%>
                 
                  </tr>
                  <tr>
                   <td> <span class="h5 font-weight-bold mb-0 text-green" style="font-weight:bold !important"><i class="fab fa-whatsapp" style="font-size: 20px;margin: 5px;"></i></span></td>
                   <td><span class="h5 font-weight-bold mb-0" style="color:#000;font-weight:bold !important"> </span></td>
                   <td><span class="h5 font-weight-bold mb-0" style="color:#000;font-weight:initial !important"> +91 8082656586</span></td>
                <%--   -<asp:Label ID="Label4" runat="server" Text="0" class="badge badge-pill badge-info"></asp:Label>--%>
                  
                   </tr>
                  <tr>
                   <td> <span class="h5 font-weight-bold mb-0" style="color:#046791;font-weight:bold !important"><i class="fas fa-mobile-alt" style="font-size: 20px;margin: 5px;"></i></span></td>
                   <td><span class="h5 font-weight-bold mb-0" style="color:#000;font-weight:initial !important"> </span></td>
                   <td><span class="h5 font-weight-bold mb-0" style="color:#000;font-weight:initial !important"> +91 8082656586</span></td>
                  <%--  -<asp:Label ID="Label6" runat="server" Text="0" class="badge badge-pill badge-warning"></asp:Label>--%>
                  
                   </tr>
                 
                  </tbody>
                  </table>
                   </center>
     <%--  <span class="" style="color: #046791; font-weight:bold">Email</span>
        <span class="" style="color: #046791; font-weight:bold">What's App</span>
         <span class="" style="color: #046791; font-weight:bold">Call Us</span>--%>
            </div>
          </div>
        </div>
      </div>
    </div>
 

  <!-- Argon Scripts -->
  <!-- Core -->
  <script src="logincss/assets/vendor/jquery/dist/jquery.min.js"></script>
  <script src="logincss/assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
  <!-- Argon JS -->
  <script src="logincss/assets/js/argon.js?v=1.0.0"></script>
</body>
</html>

