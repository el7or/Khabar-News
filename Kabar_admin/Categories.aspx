<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="Kabar_admin.Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function showResult(msg) {
           
            alert(msg);
           // window.location.reload();
        }
        function error_image() {
            alert("امتداد الصوره غير مناسب مسموح فقط ب  (png,jpeg,jpg)");
        }
        function error_upload(msg) {
            alert(" لا يمكن رفع الصوره الي الموقع الان لوجد خلل ");
            alert(msg);
        } function error_delete() {
            alert("لا يمكن حدف الصوره الان لوجد خلل ");
        }
        window.onload = function scrolltoview() {
            var id = getParameterByName("id");
            if (id) {
                var catid = 'c' + id;
                var table = document.getElementById('catlist');
                var row = document.getElementById(catid)
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
      <%--  <ul class="nav nav-tabs">
        <li class="nav-item">
            <a class="nav-link active" href="#">Categories Page</a>
        </li>
     <li class="nav-item">
            <a class="nav-link" href="#">News Filter</a>
        </li>

    </ul>--%>
    <div class="row" style="height:10px"> </div>
    <div class="row" id="tab1">
        <%-- left panel news grid --%>
        <div class="col-md-4">
             <div class="row border border-primary card card-header-tabs bg-primary" style="height: 63px;">
                <table class=" table table-bordered table-hover border border-primary  ">
                    <thead>

                        <tr>

   <th class="text-center font-weight-bold text-white  "  style="width: 100%"><i class="fa fa-bookmark"></i> Category List</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="catlist" class="row  border  card card-header-tabs border-primary  bg-light "  style="min-height: 500px; height: 100%; max-height: 500px; overflow-y: auto;">

                <table class=" table table-bordered table-hover  border border-white  ">
                  
                    <tbody id="NewsGrid" style="margin-top:50px;"  >
                         <%string url="http://"+ Request.Url.Host+"/images/"; %>
                        <%foreach (Kabar_admin.tbl_category cat in context.tbl_category.ToList())
                            { %>

                        <tr id="c<%=cat.CatPK %>" onclick=" window.location='Categories.aspx?id=<%=cat.CatPK %>&#tab1';" class="<%=(((Request.QueryString["id"] != null) && (Request.QueryString["id"] == cat.CatPK.ToString())) ? "table-active":"") %>">
                           
                            <td class="text-center "><img src="<%=url+cat.icon_url %>" alt="" width="36" height="36" style="float:left; border-radius: 50%;">
                                   <%if (cat.isCountry)  { %><a class="fa fa-globe"  ></a>  <%} %> 
                                <%=cat.title %></td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="col-md-8">
            <div class="row card card-header-tabs bg-white border  border-primary"  style="min-height: 550px;overflow:auto;">
                <div class="col-md-12">
                   
                        <div class="row bg-primary" style="min-height: 50px;">
                          
                            <asp:Button ID="ButtonReset" runat="server" Text="&#xf0e2; Clear All fields "   Font-Names="FontAwesome"   CssClass="btn btn-outline-light m-1 rounded" OnClick="Buttonreset_Click" />
                            <asp:Button ID="ButtonAdd" runat="server" Text="&#xf0fe; Add Category "   Font-Names="FontAwesome"   ValidationGroup="u" CssClass="btn btn-outline-light m-1 rounded" OnClick="ButtonAdd_Click" />
                            <asp:Button ID="ButtonUpdate" runat="server" Text="&#xf044; Update Category "   Font-Names="FontAwesome"   ValidationGroup="u" CssClass="btn  btn-outline-light  m-1 rounded" OnClick="ButtonUpdate_Click"  Enabled="false"/>
                            <asp:Button ID="ButtonDelete" runat="server" Text="&#xf014;  Delete Category "   Font-Names="FontAwesome"   CssClass="btn  btn-outline-light  m-1 rounded" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;"  data-toggle="modal" data-target="#confirmmodal" Enabled="false"/>
                            <asp:Button ID="ButtonImage" runat="server" Text="&#xf03e; Category Icon "   Font-Names="FontAwesome"   CssClass="btn  btn-outline-light  m-1 rounded" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;"  data-toggle="modal" data-target="#myModal"  Enabled="false" />
                        </div>

                    <div class="row border border-light">
                        <table class=" table table-responsive-lg border border-white">
                            <asp:HiddenField ID="HiddenFieldCatID" runat="server" />  
                            <asp:HiddenField ID="HiddenFieldResult" runat="server" Value="" />
                           
                            <tbody>
                                
                                <tr>
                                    <td  class="text-center"  style="width: 20%;">
                                        <label for="sourTitle">Title</label>
                                    </td>
                                    <td  style="width: 60%;">
                                        <input type="text" class="form-control" id="catTitle" runat="server">
                                    </td>
                                    <td  style="width:  20%;">
                                        <asp:RequiredFieldValidator ID="labelreqTitle" runat="server" Text="Required"  ValidationGroup="u"  CssClass="text-danger" ControlToValidate="catTitle"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                 <tr>
                                    <td  class="text-center" >
                                        <label for="SelectKind">Is Country</label>
                                    </td>
                                    <td>
                                        <div class="form-control">
                                            <input type="checkbox"  id="CheckboxisCountry" runat="server" class="form-check-inline"/>
                                         </div>
                                    </td>
                                    <td ></td>
                                </tr>
                                <tr>
                                    <td  class="text-center" >
                                        <label for="sourContent">Info</label>
                                    </td>
                                    <td >
                                        <textarea rows="5" class="form-control" id="catinfo" runat="server"></textarea>
                                    </td>
                                    <td >
                                        <asp:RequiredFieldValidator ID="labelreqContent" runat="server" Text="Required" ValidationGroup="u" CssClass="text-danger" ControlToValidate="catinfo"></asp:RequiredFieldValidator></td>
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
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Upload Category Icon</h4>
                </div>
                <div class="modal-body">
                    <asp:FileUpload ID="FileUploadCatIcon" runat="server" AllowMultiple="false" />
                    <img id="CatIcon" runat="server" src="" style="max-width: 100px; max-height: 100px;" />
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Buttondeletephoto" runat="server" Text="Delete Icon" Visible="false" class="btn btn-default btn-primary" OnClick="buttonImagedelete_click" />
                    <asp:Button ID="Buttonsavephoto" runat="server" Text="Save Icon" Visible="false" class="btn btn-default btn-primary" OnClick="Buttonsavephoto_Click" />
                    <button type="button" class="btn btn-default btn-primary" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="confirmmodal" role="dialog" >
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Delete Confirmation </h4>
                </div>
                <div class="modal-body">
                    Are you sure?
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button1" runat="server" Text="&#xf00c; Yes,I am"  Font-Names="FontAwesome"  class="btn btn-default btn-primary" OnClick="ButtonCatsdelete_Click" />

                    <button type="button" class="btn btn-default btn-primary" data-dismiss="modal"><i class="fa fa-times"></i> No</button>
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>
