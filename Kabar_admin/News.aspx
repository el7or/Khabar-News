<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="Kabar_admin.News" ValidateRequest="false" %>

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
        function reflect() {
            alert();

        }
        window.onload=function scrolltoview()
        {
            var id = getParameterByName("id");
            if (id) {
                var newsid = 'n' + id;
                var table = document.getElementById('newslist');
                var row = document.getElementById(newsid)
                if (row) {
                   table.scrollTop = (row.offsetTop );
                }
            }
        };
        //window.onload = function go() {
        //    var ex = getParameterByName("ex");
        //    if (ex == "1") {
        //        window.open('ExportNews', '_blank');
        //        window.history.pushState({}, document.title, "News");
        //    }
        //};
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }
        function selectday()
        {
            var selectedday = document.getElementById('<%=Selectoneday.ClientID%>').value;
            if(selectedday!=-1)
            {
                document.getElementById('<%=Selectdateinterval.ClientID%>').value = -1;
            }

        }
        function selectinterval()
        {
            var selectedinterval = document.getElementById('<%=Selectdateinterval.ClientID%>').value;
            if (selectedinterval != -1)
            {
                document.getElementById('<%=Selectoneday.ClientID%>').value = -1;
            }

        }
        //selectcreator  SelectCreatorFilter  selectsource  combosourceFilter
        function selectcreator()
        {
            var selectedcreator = document.getElementById('<%=SelectCreatorFilter.ClientID%>').value;
            if (selectedcreator != -1)
            {
                document.getElementById('<%=combosourceFilter.ClientID%>').value = -1;
            }

        }
        function selectsource()
        {
            var selectedsource = document.getElementById('<%=combosourceFilter.ClientID%>').value;
            if (selectedsource != -1)
            {
                document.getElementById('<%=SelectCreatorFilter.ClientID%>').value =0;
            }

        }
    </script>
    <%--  <ul class="nav nav-tabs">
        <li class="nav-item">
            <a class="nav-link active" href="#">News Page</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" href="#">News Filter</a>
        </li>

    </ul>--%>
    <div class="row" style="height: 10px"></div>
    <div class="row" id="tab1">
        <%-- left panel news grid --%>
        <%
            int pgsize = 15;
            int i = filterlist.Count();
            int temp = 0;
            int pgnum = (Request.QueryString["pgnum"] != null && int.TryParse(Request.QueryString["pgnum"], out temp)&&temp>0) ? temp : 1;
            int iskip =(pgnum-1)*pgsize;
            int pglast = ((int)i / pgsize);
             %>
        <div class="col-lg-4">
            <div id="newscontainer" class="row border border-primary card card-header-tabs bg-primary" style="height: 63px;">
                <table class=" table table-bordered table-hover border border-primary  ">
                    <thead>

                        <tr>

                            <th class="text-center font-weight-bold  text-white " style="width: 100%">
                                <i class="fa fa-newspaper-o mr-1"></i>News List  (<%=i %>)  
                                 <a href="News" class=" btn btn-outline-light  fa fa-repeat"></a>
                                <a href="#" data-toggle="modal" data-target="#filtermodal"  class=" btn btn-outline-light  fa fa-filter"></a>
                                 <a href="ExportNews" target="_blank" class=" btn btn-outline-light  fa fa-eye"></a>
                                <a href="ExportNews?download=T" target="_blank" class=" btn btn-outline-light  fa fa-download"></a>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="row border border-primary card card-header-tabs bg-primary" style="height: 63px;">
                <table class=" table table-bordered table-hover border border-primary  ">
                    <thead>

                        <tr>
                            <th class="text-center font-weight-bold text-white " style="width: 30%"><i class="fa fa-rss-square mr-1"></i>Source</th>
                            <th class="text-center font-weight-bold  text-white " style="width: 70%"><i class="fa fa-newspaper-o mr-1"></i>News Title</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="row  border  card card-header-tabs border-primary  bg-light ">
            <div id="newslist"  style="min-height: 1015px; height: 100%; max-height: 1015px; overflow-y: auto;">
               
                <table id="tblNews" class=" table table-bordered table-hover border border-white  ">


                    <tbody id="NewsGrid" style="margin-top: 50px;">
                    
                        <%foreach (Kabar_admin.tbl_today_news tnews in filterlist.OrderByDescending(n=>n.NewsPK).Skip(iskip).Take(pgsize).ToList())
                                { %>

                        <tr id="n<%=tnews.NewsPK %>" onclick="window.location='News.aspx?id=<%=tnews.NewsPK %>&pgnum=<%=pgnum %>&#tab1';" class="<%=(((Request.QueryString["id"] != null) && (Request.QueryString["id"] == tnews.NewsPK.ToString())) ? "table-active" : "") %>">
                            <td class="text-center " style="height: 33px;width: 30%"><%=tnews.tbl_source.title %></td>

                            <td class="text-center " style="width: 67%"><%=tnews.title %></td>
                        </tr>
                        <%}
                           %>
                    </tbody>
                </table>
                
            </div>
                <%if (i > pgsize)
                        {%>
           <nav aria-label="..." style="position:sticky;bottom:0px;margin-right:auto;margin-left:auto;" >
                    <ul class="pagination">
                        <%if (pgnum == 1)
                        { %>
                        <li class="page-item disabled">
                            <span class="page-link">First</span>
                        </li>
                        <li class="page-item disabled">
                            <span class="page-link">Previous</span>
                        </li>
                       <%}
                        else
                        { %>
                         <li class="page-item ">
                          <a class="page-link" href="News?pgnum=1&#tab1">First</a>
                        </li>
                        <li class="page-item ">
                           <a class="page-link" href="News?pgnum=<%=pgnum - 1 %>&#tab1">Previous</a>
                        </li>
                        <%} %>
                          <li class="page-item ">
                            <span class="page-link"><%=pgnum %></span>
                        </li>
                        <%if (pgnum != pglast)
                        {%>
                        <li class="page-item">
                            <a class="page-link" href="News?pgnum=<%=pgnum + 1 %>&#tab1">Next</a>
                        </li>
                         <li class="page-item">
                            <a class="page-link" href="News?pgnum=<%=pglast%>&#tab1">Last</a>
                        </li>
                        <%}
                        else
                        { %>
                         <li class="page-item disabled">
                            <span class="page-link">Next</span>
                        </li>
                        <li class="page-item disabled">
                            <span class="page-link">Last</span>
                        </li>
                        <%} %>

                    </ul>
                </nav>
                <%} %>
                </div>
            
        </div>

        <div class="col-lg-8">
            <div id="newscontent" class="row card card-header-tabs bg-white border  border-primary" style="min-height: 1118px; overflow: auto;">
                <div class="col-md-12">

                    <div class="row  btn-primary " style="min-height: 50px;">

                        <asp:Button ID="ButtonReset" runat="server" Text="&#xf0e2; Clear All fields" Font-Names="FontAwesome" CssClass="btn btn-outline-light m-1 rounded" OnClick="Buttonreset_Click" />
                        <asp:Button ID="ButtonAdd" runat="server" Text="&#xf0fe; Add News " Font-Names="FontAwesome" CssClass="btn btn-outline-light m-1 rounded" OnClick="ButtonAdd_Click" />
                        <asp:Button ID="ButtonUpdate" runat="server" Text="&#xf044; Update News " Font-Names="FontAwesome" CssClass="btn  btn-outline-light  m-1 rounded" OnClick="ButtonUpdate_Click" Enabled="false" />
                        <asp:Button ID="ButtonDelete" runat="server" Text="&#xf014; Delete News " Font-Names="FontAwesome" CssClass="btn  btn-outline-light  m-1 rounded" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;" data-toggle="modal" data-target="#confirmmodal" Enabled="false" />
                        <asp:Button ID="ButtonImage" runat="server" Text="&#xf03e; News Image " Font-Names="FontAwesome" CssClass="btn  btn-outline-light  m-1 rounded" UseSubmitBehavior="false" CausesValidation="false" OnClientClick="return false;" data-toggle="modal" data-target="#myModal" />
                       <a id="openurl" runat="server" href="" target="_blank" class="btn  btn-outline-light  m-1 rounded disabled ">Open <i class="fa fa-external-link"></i></a>
                    </div>


                    <div class="row border border-light">
                        <table class=" table table-responsive-lg border border-white">
                            <asp:HiddenField ID="HiddenFieldnewsID" runat="server" />
                            <asp:HiddenField ID="HiddenFieldResult" runat="server" Value="" />

                            <tbody>
                                <tr>
                                    <td class="text-center" style="width: 20%;">
                                        <label for="usr">Source</label>
                                    </td>
                                    <td style="width: 60%;">
                                        <select class="form-control" id="SourcesCombo" accesskey="s" runat="server">
                                        </select>
                                    </td>
                                    <td style="width: 20%;"></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourTitle">Title</label>
                                    </td>
                                    <td>
                                        <input type="text" class="form-control" id="sourTitle" runat="server">
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="labelreqTitle" runat="server" Text="Required" CssClass="text-danger" ControlToValidate="sourTitle"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourTitle">Pub Date</label>
                                    </td>
                                    <td>
                                        <input type="text" class="form-control" id="Textdate" runat="server" disabled="disabled">
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourContent">Content</label>
                                    </td>
                                    <td>
                                        <textarea rows="2" class="form-control" id="sourContent" runat="server"></textarea>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="labelreqContent" runat="server" Text="Required" CssClass="text-danger" ControlToValidate="sourContent"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="sourContent">Content Html</label>
                                    </td>
                                    <td>
                                        <div class="form-control disabled" style="overflow: auto; max-width: 500px; max-height: 500px;" id="contenthtml" runat="server"></div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="usr">Kind</label>
                                    </td>
                                    <td>
                                        <div class="form-control">
                                            <%--  <select class="form-control" id="SelectKind" runat="server">
                                            <option value="-1">Unknown</option>
                                            <option value="0">Not Published</option>
                                           
                                        </select>--%>
                                            <div class="container form-check-inline">
                                                <asp:CheckBoxList ID="CheckBoxListkinds" runat="server" RepeatColumns="4" RepeatLayout="Table" CellPadding="10" CellSpacing="10">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="col-2"></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="SourVideoUrl">Video Url</label>
                                          <a id="SourVideoUrl1" runat="server" href="" target="_blank" class=" "> <i class="fa fa-external-link"></i></a>
                                    </td>
                                    <td>
                                        <input type="text" class="form-control" id="SourVideoUrl" runat="server">
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="SourExternalUrl">External Url</label>
                                        <a id="externalurl1" runat="server" href="" target="_blank" class=" "> <i class="fa fa-external-link"></i></a>
                                    </td>
                                    <td>
                                        
                                        <input type="text" class="form-control" id="SourExternalUrl" runat="server">
                                         
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td class="text-center">
                                        <label for="CheckBoxListcategories">Categories</label>
                                    </td>
                                    <td>
                                        <div class="container form-control form-check-inline">
                                            <asp:CheckBoxList ID="CheckBoxListcategories" runat="server" RepeatColumns="3" RepeatLayout="Table" CellPadding="10" CellSpacing="10">
                                            </asp:CheckBoxList>

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
    <div class="modal fade" id="filtermodal" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">News Filter</h4>
                </div>
                <div class="modal-body">
                    <div class="container-fluid">
                        <div class="row">


                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group" style="display: block;">
                                            <label class="control-label control-label-left col-sm12" for="field1">Title Contains</label>
                                            <div class="controls col-sm-12">

                                                <input id="inputtitleFilter" runat="server" class="form-control " placeholder="Any word" type="text">
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group" style="display: block;">
                                            <label class="control-label control-label-left col-sm12" for="field1">Body Contains</label>
                                            <div class="controls col-sm-12">

                                                <input id="inputbodyFilter" runat="server" class="form-control " placeholder="Any word" type="text">
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                     
                                          <div class="form-group" style="display: block;">
                                            <label class="control-label control-label-left col-sm-12" for="field1">News Creator</label>
                                            <div class="controls col-sm-12">
                                                  <select id="SelectCreatorFilter" runat="server" class="form-control" onchange="selectcreator()">
                                                         <option value="0" selected >Rss Feed</option>
                                                    <option value="-1" >All Users</option> 
                                                    
                                                </select>
                                               
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <label class="control-label control-label-left col-sm-12" for="field3">Source</label>
                                            <div class="controls col-sm-12">

                                                <select id="combosourceFilter" runat="server" class="form-control"  onchange="selectsource()">
                                                    <option value="-1" selected >All Sources</option>
                                                </select>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group" style="display: block;">
                                            <label class="control-label control-label-left col-sm-12" for="field14">Just</label>
                                            <div class="controls col-sm-12">
                                                <select id="Selectoneday"  runat="server"    class="form-control" onchange="selectday()">
                                                      <option value="-1" >All Time</option>
                                                    <option value="0" selected>Today</option>
                                                    <option value="1">Yestarday</option>
                                                    <option value="2">Two days ago</option>
                                                    <option value="3">Three days ago</option>
                                                    <option value="4">Four days ago</option>
                                                    <option value="5">Five days ago</option>
                                                </select>
                                            </div>

                                        </div>
                                        <div class="form-group" style="display: block;">
                                            <label class="control-label control-label-left col-sm-12" for="field15">From</label>
                                            <div class="controls col-sm-12">
                                                <select id="Selectdateinterval" runat="server"  class="form-control"  onchange="selectinterval()">
                                                    <option value="-1" selected >The beginning</option>
                                                    <option value="1" >Yestarday</option>
                                                    <option value="2" >Two days ago</option>
                                                    <option value="3" >Three days ago</option>
                                                    <option value="4" >Four days ago</option>
                                                    <option value="5" >Five days ago</option>
                                                    <option value="7" >Last week</option>
                                                    <option value="14" >Two weeks ago</option>
                                                    <option value="21" >Three weeks ago</option>
                                                    <option value="30" >Month ago</option>
                                                </select>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label control-label-left col-sm-12">Image</label>
                                    <div class=" col-sm-12">
                                        <div class="form-control ">
                                            <label class="in" for="radio26">
                                                <input value="1" id="hasimagecheck" runat="server" name="field25"  type="radio">
                                                Has Image

                                            </label>
                                            <label class="radio-inline" for="radio27">
                                                <input value="0" id="noimagecheck" runat="server" name="field25" type="radio">
                                                No Image</label>
                                            <label class="radio-inline" for="radio28">
                                                <input value="-1" id="allcheck" runat="server" name="field25" checked type="radio">
                                                All</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label control-label-left col-sm-12" for="field3">Category</label>
                                    <div class="controls col-sm-12">

                                        <select id="catcombofilter" runat="server" class="form-control">
                                            <option value="-1" selected>All Categories</option>
                                        </select>
                                    </div>

                                </div>

                            </div>




                        </div>
                    </div>
                </div>
                <div class="modal-footer"> 
                   <%-- <asp:Button ID="Buttondownload" runat="server" Text="Preview"  class="btn btn-default btn-primary" OnClick="ButtondownloadClick"  CausesValidation="false"  />--%>
                     <asp:Button ID="ButtonApplyFilter" runat="server" Text="&#xf0b0; Apply filter" Font-Names="FontAwesome"  class="btn btn-default btn-primary" OnClick="ButtonApplyFilterClick"  CausesValidation="false"  />
                     <asp:Button ID="ButtonCancelFilter" runat="server" Text="Clear filter"  class="btn btn-default btn-primary" OnClick="ButtonCancelFilterClick"  CausesValidation="false"  />
                    <button type="button" class="btn btn-default btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Upload News Image</h4>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="HiddenFieldimagename" runat="server" />
                    <asp:FileUpload ID="FileUploadNewsphoto" runat="server" AllowMultiple="false" />
                    <img id="NewsImage" runat="server" src="" style="max-width: 100%; max-height: 400px;" />
                </div>
                <div class="modal-footer">
                    <asp:Button ID="Buttondeletephoto" runat="server" Text="Delete Image" Visible="false" class="btn btn-default btn-primary" OnClick="buttonImagedelete_click" CausesValidation="false" />
                    <asp:Button ID="Buttonsavephoto" runat="server" Text="Save Image" Visible="false" class="btn btn-default btn-primary" OnClick="Buttonsavephoto_Click"  CausesValidation="true"  />
                    <button type="button" class="btn btn-default btn-primary" data-dismiss="modal"  CausesValidation="false" >Cancel</button>
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
                    <asp:Button ID="Button1" runat="server" Text="&#xf00c; Yes,I am" Font-Names="FontAwesome" class="btn btn-default btn-primary" OnClick="ButtonNewsdelete_Click"  CausesValidation="false"  />

                    <button type="button" class="btn btn-default btn-primary" data-dismiss="modal"><i class="fa fa-times"></i>No</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
