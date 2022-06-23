<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="Kabar_admin.MyAccount" %>
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
    </script>
    <ul class="nav nav-tabs">
        <li class="nav-item">
            <a class="nav-link active" href="#">Accounts Page</a>
        </li>
        <%--  <li class="nav-item">
            <a class="nav-link" href="#">News Filter</a>
        </li>--%>
    </ul>
    <div class="row">
        <div class="col-md-12">
            <div class="row bg-white border  border-white">
                <div class="col-md-12">
                    <div class="row badge-dark">
                        <div class="btn-group " style="min-height: 50px;">
                            <div class="btn btn-dark mt-1 " style="width: 150px;">    </div>
                          
                            <asp:Button ID="ButtonUpdate" runat="server" Text="Update User " ValidationGroup="u" CssClass="btn  btn-outline-light  m-1 rounded" OnClick="ButtonUpdate_Click" Enabled="false" />
                             <asp:Button ID="Buttonchangepassword" runat="server" Text="Change Password" CssClass="btn  btn-outline-light  m-1 rounded" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;"  data-toggle="modal" data-target="#myModal"  Enabled="false"  />
                        </div>

                    </div>
                    <div class="row border border-light disabled">
                        <table class=" table table-responsive-lg border border-white">
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
                   
                    <asp:Button ID="ButtonUpdatePassword" runat="server" Text="Update" class="btn btn-default" ValidationGroup="p"  CausesValidation="true"  OnClick="ButtonupdatePassword_Click"  />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
                   
            </div>
        </div>
    </div>
    
</asp:Content>
