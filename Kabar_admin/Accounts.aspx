<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="Kabar_admin.Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function showResult(msg) {

            alert(msg);
            // window.location.reload();
        }
        function validatePass(source, arguments)
        {
           
            var inputpassword = document.getElementById('<%=TextBoxpassword.ClientID%>');
            //alert(inputpassword);
            var inputconfirmpassword = document.getElementById('<%=TextBoxconfirmpassword.ClientID%>');
            //alert(inputconfirmpassword);
            //alert(inputpassword.value);
            //alert(inputconfirmpassword.value);
           
            if (inputpassword.value == inputconfirmpassword.value) {
               
                arguments.IsValid = true;
            } else {
                arguments.IsValid = false;
            }
           
        }
        function hideEmail()
        {
            var LabelEmailused = document.getElementById('<%=LabelEmailused.ClientID%>');
            LabelEmailused.style.display = 'none';
        }
        window.onload = function scrolltoview() {
            var id = getParameterByName("id");
            if (id) {
                var accountid = 'a' + id;
                var table = document.getElementById('accountlist');
                var row = document.getElementById(accountid)
                if (row) {
                    table.scrollTop = (row.offsetTop);
                }
            }
        };
       
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }
    </script>
   <%-- <ul class="nav nav-tabs">
        <li class="nav-item">
            <a class="nav-link active" href="#">Accounts Page</a>
        </li>
          <li class="nav-item">
            <a class="nav-link" href="#">News Filter</a>
        </li>
    </ul>--%>
    <div class="row" style="height:10px"> </div>
    <div class="row " id="tab1">
        <%-- left panel news grid --%>
        <div class="col-lg-4">
             <div class="row border border-primary card card-header-tabs bg-primary" style="height: 63px;">
                <table class=" table table-bordered table-hover border border-primary  ">
                    <thead>

                        <tr>

                           <th class="text-center font-weight-bold text-white  " style="width: 100%"><span class="fa fa-users"></span> Account List</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="accountlist" class="row  border  card card-header-tabs border-primary  bg-light " style="min-height: 500px; height: 100%; max-height: 500px; overflow-y: auto;">

                <table class=" table table-bordered table-hover  border border-white  ">
                  

                    <tbody id="NewsGrid" style="margin-top: 50px;">

                        <%foreach (Kabar_admin.tbl_frontend_users user in context.tbl_frontend_users.ToList())
                            { %>

                        <tr id="a<%=user.FUserPK%>" onclick=" window.location='Accounts.aspx?id=<%=user.FUserPK%>&#tab1';" class="<%=(((Request.QueryString["id"] != null) && (Request.QueryString["id"] == user.FUserPK.ToString())) ? "table-active":"") %>">

                            <td class="text-center "><%=user.displayname %></td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="col-lg-8">
            <div class="row card card-header-tabs bg-white border  border-primary" style="min-height: 550px;overflow:auto;">
                <div class="col-12">
                  
                        <div class="row bg-primary" style="min-height: 50px;">
                           
                            <asp:Button ID="ButtonReset" runat="server" Text="&#xf0e2; Clear All fields " CssClass="btn btn-outline-light m-1 rounded"   Font-Names="FontAwesome"   OnClick="Buttonreset_Click" />
                            <asp:Button ID="ButtonAdd" runat="server" Text="&#xf0fe; Add User " ValidationGroup="u"  Font-Names="FontAwesome"  CssClass="btn btn-outline-light m-1 rounded" OnClick="ButtonAdd_Click" />
                            <asp:Button ID="ButtonUpdate" runat="server" Text="&#xf044; Update User " ValidationGroup="u"   Font-Names="FontAwesome"  CssClass="btn  btn-outline-light  m-1 rounded" OnClick="ButtonUpdate_Click" Enabled="false" />
                            <asp:Button ID="ButtonDelete" runat="server" Text="&#xf014; Delete User " Font-Names="FontAwesome" CssClass="btn  btn-outline-light  m-1 rounded" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;" data-toggle="modal" data-target="#confirmmodal" Enabled="false" />
                             <asp:Button ID="Buttonchangepassword" runat="server" Text="&#xf023; Change Password" CssClass="btn  btn-outline-light  m-1 rounded"   Font-Names="FontAwesome"   UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;"  data-toggle="modal" data-target="#myModal"  Enabled="false"  />
                        </div>

                    <div class="row border border-light">
                        <table class=" table  border border-white">
                            <asp:HiddenField ID="HiddenFieldUserID" runat="server" />
                            <asp:HiddenField ID="HiddenFieldResult" runat="server" Value="" />

                            <tbody>

                                <tr>
                                    <td class="text-center" style="width: 20%;">
                                        <label for="sourTitle">Displayname</label>
                                    </td>
                                    <td style="width: 60%;">
                                        <input type="text" class="form-control" id="inputDisplayname" runat="server">
                                    </td>
                                    <td style="width: 20%;">
                                        <asp:RequiredFieldValidator ID="labelreqdisplayname" runat="server" Text="Required" ValidationGroup="u" CssClass="text-danger" ControlToValidate="inputDisplayname"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourContent">Email</label>
                                    </td>
                                    <td>
                                        <input id="inuputEmail" type="email" class="form-control" runat="server" onkeypress="hideEmail()"  />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="labelreqEmail" runat="server"  Text="Required" ValidationGroup="u" CssClass="text-danger" ControlToValidate="inuputEmail"></asp:RequiredFieldValidator>
                                        <asp:Label ID="LabelEmailused" runat="server" Text="Exist" CssClass="text-danger" style="display:none;"></asp:Label>
                                    </td>
                                </tr>
                                 <tr>
                                    <td  class="text-center" >
                                        <label for="usr">Kind</label>
                                    </td>
                                    <td  >
                                         <select class="form-control" id="SelectKind"    runat="server"> 
                                             <option value="1">Normal</option>
                                             <option value="2">Super User</option>
                                             <option value="3">Golden</option>
                                        </select>
                                    </td>
                                    <td  class="col-2"></td>
                                </tr>
                                 <tr>
                                    <td  class="text-center" >
                                        <label for="usr">Admin</label>
                                    </td>
                                    <td  >
                                          <div class="form-check-inline">
                                            <div class="form-check-input">
                                                <asp:CheckBox ID="CheckBoxisAdmin" runat="server" CssClass="form-check" />
                                            </div>
                                              </div>
                                    </td>
                                    <td  class="col-2"></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourContent">News Rights</label>
                                    </td>
                                   <td>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">View</div>
                                            <div class="form-check-input">
                                                <asp:CheckBox ID="CheckBoxNewsView" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>


                                        <div class="form-check-inline">
                                            <div class="form-check-label">Add</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxNewsAdd" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">Update</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxNewsUpdate" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">Delete</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxNewsDelete" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourContent">Sources Rights</label>
                                    </td>
                                    <td>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">View</div>
                                            <div class="form-check-input">
                                                <asp:CheckBox ID="CheckBoxSourceView" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>


                                        <div class="form-check-inline">
                                            <div class="form-check-label">Add</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxSourceAdd" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">Update</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxSourceUpdate" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">Delete</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxSourceDelete" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourContent">Categories Rights</label>
                                    </td>
                                   <td>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">View</div>
                                            <div class="form-check-input">
                                                <asp:CheckBox ID="CheckBoxCategoriesView" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>


                                        <div class="form-check-inline">
                                            <div class="form-check-label">Add</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxCategoriesAdd" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">Update</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxCategoriesUpdate" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">Delete</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxCategoriesDelete" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourContent">Account Rights</label>
                                    </td>
                                    <td>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">View</div>
                                            <div class="form-check-input">
                                                <asp:CheckBox ID="CheckBoxAccountView" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>


                                        <div class="form-check-inline">
                                            <div class="form-check-label">Add</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxAccountAdd" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">Update</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxAccountUpdate" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                        <div class="form-check-inline">
                                            <div class="form-check-label">Delete</div>
                                            <div class="form-check-inline">
                                                <asp:CheckBox ID="CheckBoxAccountDelete" runat="server" CssClass="form-check" />
                                            </div>
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>

                            </tbody>
                        </table>

                    </div>
            </div>
        </div>
    </div>
    
        </div>
       
      <!-- Modal -->
    <div class="modal fade" id="myModal" role="dialog" >
        <div class="modal-dialog">
            <div class="modal-content">
               
                <div class="modal-header">
                    <h4 class="modal-title">Change Password</h4>
                </div>
                <div class="modal-body">
                  
                        <div class="form-group">
                            <label for="password">Password <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" CssClass=" text-danger" ControlToValidate="TextBoxpassword" ValidationGroup="p" ></asp:RequiredFieldValidator></label>  
                            <asp:TextBox ID="TextBoxpassword" runat="server" TextMode="Password" CssClass="form-control form-inline" ></asp:TextBox>
                          
                        </div>
                         <div class="form-group">
                            <label for="confirmpassword">Confirm Password <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"  CssClass="text-danger"  ControlToValidate="TextBoxconfirmpassword"  ValidationGroup="p"></asp:RequiredFieldValidator></label>
                             <asp:TextBox ID="TextBoxconfirmpassword" runat="server"  TextMode="Password"   CssClass="form-control"></asp:TextBox>
                             
                        </div>
                   <div class="form-group"> <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Not Matched"  CssClass="text-danger" ValidationGroup="p" ClientValidationFunction="validatePass"></asp:CustomValidator></div>
                   
                </div>
                <div class="modal-footer">
                   
                    <asp:Button ID="ButtonUpdatePassword" runat="server" Text="Update" class="btn btn-default btn-primary" ValidationGroup="p"  CausesValidation="true"  OnClick="ButtonupdatePassword_Click"  />
                    <button type="button" class="btn btn-default btn-primary" data-dismiss="modal">Cancel</button>
                </div>
                   
            </div>
        </div>
    </div>
    <div class="modal fade" id="confirmmodal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Delete Confirmation </h4>
                </div>
                <div class="modal-body">
                    Are you sure?
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button1" runat="server" Text="&#xf00c; Yes,I am"  Font-Names="FontAwesome"  class="btn btn-default btn-primary" OnClick="ButtonAccountdelete_Click" />

                    <button type="button" class="btn btn-default btn-primary" data-dismiss="modal"><i class="fa fa-times"></i> No</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
