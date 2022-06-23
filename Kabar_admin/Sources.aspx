<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sources.aspx.cs" Inherits="Kabar_admin.Sources" %>

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
        function a()
        {
            var text =document.getElementById('<%=sourfeedurl.ClientID%>').value;
        $("#testRSS").attr("href", "testrssfeed/?url="+text);
        }
        window.onload = function scrolltoview() {
            var id = getParameterByName("id");
            if (id) {
                var sourid = 's' + id;
                var table = document.getElementById('sourlist');
                var row = document.getElementById(sourid)
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
            <a class="nav-link active" href="#">Sources Page</a>
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

                            <th class="text-center font-weight-bold text-white "   style="width: 100%"><i class="fa fa-rss-square"  style="margin-right:1px;"> </i>    <span> Source List  </span> (<%=context.tbl_source.Count(s=>!s.bdeleted) %> )    <a href="Sources" class=" btn btn-outline-light  fa fa-repeat"></a></th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="sourlist" class="row  border  card card-header-tabs border-primary  bg-light "  style="min-height: 500px; height: 100%; max-height: 500px; overflow-y: auto;">

                <table class=" table table-bordered table-hover  border border-white  ">

                    <tbody id="NewsGrid" style="margin-top: 50px;">
                        <%//string url="http://"+ Request.Url.Host+"/images/"; %>
                        <%foreach (Kabar_admin.tbl_source sour in  context.tbl_source.Where(s=>s.bdeleted==false).ToList())
                            { %>

                        <tr  id="s<%=sour.SourcePK %>" onclick=" window.location='Sources.aspx?id=<%=sour.SourcePK %>&#tab1';" class="<%=(((Request.QueryString["id"] != null) && (Request.QueryString["id"] == sour.SourcePK.ToString())) ? "table-active":"") %>">

                            <td class="text-center "> <img src="<%=sour.icon_url %>" alt="" width="36" height="36" style="float:left; border-radius: 50%;">
                                <%if (sour.isArgent)  { %><a class="fa fa-bell"  ></a><%} %> 
                                <%=sour.title %>

                            </td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="col-md-8">
            <div class="row card card-header-tabs bg-white border  border-primary"  style="min-height: 550px;max-height: 550px;overflow:auto;">
                <div class="col-md-12">
                    
                        <div class="row bg-primary " style="min-height: 50px;">
                          
                            <asp:Button ID="ButtonReset" runat="server" Text="&#xf0e2; Clear All fields "  Font-Names="FontAwesome"   CssClass="btn btn-outline-light m-1 rounded" OnClick="Buttonreset_Click" />
                            <asp:Button ID="ButtonAdd" runat="server" Text="&#xf0fe; Add Source "  Font-Names="FontAwesome" ValidationGroup="u" CssClass="btn btn-outline-light m-1 rounded" OnClick="ButtonAdd_Click" />
                            <asp:Button ID="ButtonUpdate" runat="server" Text="&#xf044; Update Source "  Font-Names="FontAwesome" ValidationGroup="u" CssClass="btn  btn-outline-light  m-1 rounded" OnClick="ButtonUpdate_Click" Enabled="false" />
                            <asp:Button ID="ButtonDelete" runat="server" Text="&#xf014; Delete Source "  Font-Names="FontAwesome" CssClass="btn  btn-outline-light  m-1 rounded" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;" data-toggle="modal" data-target="#confirmmodal" Enabled="false" />
                             <asp:Button ID="Buttonfetch" runat="server" Text="&#xf044; Fetch Source " Font-Names="FontAwesome" ValidationGroup="u" CssClass="btn btn-outline-light m-1 rounded" OnClick="Buttonfetch_Click" />
                           
                            <asp:Button ID="ButtonImage" runat="server" Text="&#xf03e; Icon "  Font-Names="FontAwesome" CssClass="btn  btn-outline-light  m-1 rounded" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;" data-toggle="modal" data-target="#myModal" Enabled="false" />
                           
                        </div>

                    <div class="row border border-light" style="min-height: 490px;max-height: 490px;overflow:auto;">
                        <table class=" table  border border-white">
                            <asp:HiddenField ID="HiddenFieldsourID" runat="server" />
                            <asp:HiddenField ID="HiddenFieldResult" runat="server" Value="" />
                          
                            <tbody>
                                
                                <tr>
                                    <td  class="text-center" >
                                        <label for="sourTitle">Title</label>
                                    </td>
                                    <td >
                                        <input type="text" class="form-control" id="sourTitle" runat="server">
                                    </td>
                                    <td >
                                        <asp:RequiredFieldValidator ID="labelreqTitle" runat="server" Text="Required" ValidationGroup="u" CssClass="text-danger" ControlToValidate="sourTitle"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td  class="text-center" >
                                        <label for="sourinfo">Info</label>
                                    </td>
                                    <td >
                                        <textarea rows="1" class="form-control" id="sourinfo" runat="server"></textarea>
                                    </td>
                                    <td >
                                        <asp:RequiredFieldValidator ID="labelreqContent" runat="server" Text="Required" ValidationGroup="u" CssClass="text-danger" ControlToValidate="sourinfo"></asp:RequiredFieldValidator></td>
                                </tr>
                                
                                  <tr>
                                    <td  class="text-center" >
                                        <label for="SelectKind">Manual Source</label>
                                    </td>
                                    <td>
                                        <div class="form-control">
                                            <input type="checkbox"  id="CheckboxisManual" runat="server" class="form-check-inline"/>
                                         </div>
                                    </td>
                                    <td ></td>
                                </tr>
                                  <tr>
                                    <td  class="text-center" >
                                        <label for="SelectKind">Is Urgent</label>
                                    </td>
                                    <td>
                                        <div class="form-control">
                                            <input type="checkbox"  id="CheckboxisArgent" runat="server" class="form-check-inline"/>
                                         </div>
                                    </td>
                                    <td ></td>
                                </tr>
                                <tr>
                                    <td  class="text-center" >
                                        <label for="sourfeedurl">Rss Feed Url </label>
                                    </td>
                                    <td >
                                        <a id="testRSS" href="" onclick="a();" target="_blank">Test Rss Feed  <i class="fa fa-external-link"></i></a>
                                        <textarea rows="1" class="form-control" id="sourfeedurl" runat="server"></textarea>
                                    </td>
                                    <td >
                                       <%-- <asp:RequiredFieldValidator ID="labelreqfeedurl" runat="server" Text="Required" ValidationGroup="u" CssClass="text-danger" ControlToValidate="sourfeedurl"></asp:RequiredFieldValidator>--%></td>
                                </tr>
                                <tr>
                                    <td  class="text-center" >
                                        <label for="SelectKind">Kind</label>
                                    </td>
                                    <td >
                                        <div class="form-control">
                                      <%-- <div class="form-check-inline">Normal</div><input type="checkbox"  id="checkNormal" runat="server" class="form-check-inline"/>
                                            <div class="form-check-inline">Super User</div><input type="checkbox"   id="checksuperuser" runat="server"  class="form-check-inline"/>
                                            <div class="form-check-inline">Golden</div><input type="checkbox"   id="checkgolden" runat="server"   class="form-check-inline"/>
                                              <div class="form-check-inline">Argent</div><input type="checkbox"   id="CheckboxisArgent" runat="server"   class="form-check-inline"/>--%>
                                             <div class="container form-check-inline">
                                            <asp:CheckBoxList ID="CheckBoxListkinds" runat="server" RepeatColumns="4" RepeatLayout="Table" CellPadding="10" CellSpacing="10">
                                            </asp:CheckBoxList>

                                        </div>
                                        </div>
                                    </td>
                                    <td ></td>
                                </tr>
                                  <tr>
                                    <td  class="text-center"  style="width: 20%;">
                                        <label for="usr">Categories</label>
                                    </td>
                                    <td style="width: 60%;">
                                     <%--   <select class="form-control" id="CategoriesCombo"  accesskey="s"  runat="server"> 
                                        </select>--%>
                                         <div class="container form-control form-check-inline">
                                            <asp:CheckBoxList ID="CheckBoxListcategories" runat="server" RepeatColumns="3" RepeatLayout="Table" CellPadding="10" CellSpacing="10">
                                            </asp:CheckBoxList>

                                        </div>
                                    </td>
                                    <td  style="width: 20%;"></td>
                                </tr>

                            </tbody>
                        </table>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Upload Source Icon</h4>
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
                    <asp:Button ID="Button1" runat="server" Text="&#xf00c; Yes,I am"  Font-Names="FontAwesome" class="btn btn-default btn-primary" OnClick="ButtonSourcesdelete_Click" />
                    <button type="button" class="btn btn-default btn-primary" data-dismiss="modal"><i class="fa fa-times"></i> No</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
