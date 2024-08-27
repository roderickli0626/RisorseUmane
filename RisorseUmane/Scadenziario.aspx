<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Scadenziario.aspx.cs" Inherits="RisorseUmane.Scadenziario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Content/CSS/jquery.datetimepicker.min.css" />
    <link rel="stylesheet" href="Content/CSS/toastr.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HfRememberID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HfUserLogin" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HfLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HfCheck" runat="server" ClientIDMode="Static" />
    <div class="container-fluid pt-4 px-4">
        <div class="row g-4">
            <div class="col-12" style="min-height: 100vh;">
                <div class="bg-secondary rounded h-100 p-5">
                    <h1 class="mb-4 text-center">DATE DA RICORDARE</h1>
                    <div class="mt-5 row">
                        <div class="col-md-3">
                            <button class="btn btn-lg btn-primary w-100 mb-2 btn-add" runat="server">+ AGG. Data</button>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="TxtFrom" AutoCompleteType="Disabled" CssClass="form-control form-control-lg" ClientIDMode="Static" placeholder="DAL ..."></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="TxtTo" AutoCompleteType="Disabled" CssClass="form-control form-control-lg" ClientIDMode="Static" placeholder="AL ..."></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="TxtSearch" ClientIDMode="Static" CssClass="form-control form-control-lg w-100" placeholder="CERCA..."></asp:TextBox>
                        </div>
                    </div>
                    <table class="table table-striped text-center mt-4" id="remember-table">
                        <thead>
                            <tr>
                                <th scope="col">Nr.</th>
                                <th scope="col">Remember Date</th>
                                <th scope="col">Descrizione</th>
                                <th scope="col">User</th>
                                <th scope="col">Created Date</th>
                                <th scope="col">State</th>
                                <th scope="col">Azione</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade show" id="RememberModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="modalTitle">DATE DA RICORDARE</h4>
                </div>
                <div class="modal-body">
                    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel" ClientIDMode="Static" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSummary" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:CustomValidator ID="ServerValidator" runat="server" ErrorMessage="Save Failed." Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="ServerValidatorForDescription" runat="server" ErrorMessage="Inserire un indirizzo Description." Display="None"></asp:CustomValidator>
                            <div class="col-md-12">
                                <div class="border rounded p-4 pb-0 mb-4">
                                    <figure class="text-center">
                                        <blockquote class="blockquote">
                                            <h2>SCADENZIARIO</h2>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="CreatedDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-8">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Date Da Ricordare</label>
                                    <asp:TextBox runat="server" ID="TxtRememberDate" AutoCompleteType="Disabled" CssClass="form-control form-control-lg" ClientIDMode="Static" placeholder="RICOR ..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4 d-none" id="StateDiv">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">State</label>
                                    <asp:DropDownList runat="server" ID="ComboState" CssClass="form-select form-select-lg" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Description</label>
                                    <asp:TextBox runat="server" ID="TxtDescription" AutoCompleteType="Disabled" ClientIDMode="Static" TextMode="MultiLine" Rows="2" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSave" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="BtnSave" ClientIDMode="Static" CssClass="btn btn-primary" Text="Conferma" OnClick="BtnSave_Click" />
                    <button type="button" class="btn btn-warning" data-bs-dismiss="modal">Chiudi</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script src="Scripts/JS/jquery.datetimepicker.full.min.js"></script>
    <script src="Scripts/JS/toastr.min.js"></script>
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoadedHandler);

        function pageLoadedHandler(sender, args) {
            // This function will be called after each UpdatePanel postback
            var updatedPanels = args.get_panelsUpdated();

            for (var i = 0; i < updatedPanels.length; i++) {
                var updatePanelID = updatedPanels[i].id;

                if (updatePanelID === "UpdatePanel") {
                    $("#TxtRememberDate").datetimepicker({
                        format: "d/m/Y",
                    });
                }
            }
        };
    </script>
    <script>
        $("#TxtSearch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: 'DataService.asmx/FindAllRemembers',
                    dataType: "json",
                    data: { term: request.term },
                    success: function (data) {
                        response(data.map(item => ({ label: item.Description, id: item.Id, value: item.Description })));
                    }
                });
            },
            minLength: 0,
            select: function (event, ui) {
                //var selectedModelId = ui.item.id;
                datatable.fnDraw();
            }
        });

        $.datetimepicker.setLocale('it');

        $("#TxtFrom").datetimepicker({
            format: "d/m/Y",
        });

        $("#TxtTo").datetimepicker({
            format: "d/m/Y",
        });

        $("#TxtRememberDate").datetimepicker({
            format: "d/m/Y",
        });

        $(".btn-add").click(function () {
            $("#RememberModal").modal('show');
            $("#modalTitle").text("AGG. SCADENZIARIO");
            $("#HfRememberID").val("");
            $("#HfCheck").val("");
            $("#ValSummary").addClass("d-none");
            $("#TxtDescription").val("");
            $("#TxtRememberDate").val("");
            $("#StateDiv").addClass("d-none");

            $("#CreatedDate").addClass("d-none");
            $("#BtnSave").show();

            return false;
        });

        var datatable = $('#remember-table').dataTable({
            "serverSide": true,
            "ajax": 'DataService.asmx/FindRemembers',
            "dom": '<"table-responsive"t>pr',
            "autoWidth": false,
            "pageLength": 20,
            "processing": true,
            "ordering": false,
            "columns": [{
                "render": function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            }, {
                "data": "Remember",
            }, {
                "data": "Description",
            }, {
                "data": "UserName",
            }, {
                "data": "CreatedDate",
            }, {
                "data": "State",
                "render": function (data, type, row, meta) {
                    return GetState(data);
                }
            }, {
                "data": null,
                "render": function (data, type, row, meta) {
                    var IsUser = $("#HfUserLogin").val();
                    return ((IsUser == "User" && row.State == 3) || IsUser != "User") ? '<div class="justify-content-center">' +
                        '<i class="fa fa-edit mt-1 btn-edit" style="font-size:20px; padding-right: 10px; color:greenyellow"></i>' +
                        '<i class="fa fa-trash mt-1 btn-delete" style="font-size:20px; padding-right: 10px; color:red"></i>' +
                        '</div > ' : '<div class="justify-content-center">' +
                        '<i class="fa fa-eye mt-1 btn-show" style="font-size:20px; padding-right: 10px; color:white"></i>' +
                        '<i class="fa fa-trash mt-1 btn-delete" style="font-size:20px; padding-right: 10px; color:red"></i>' +
                        '</div > ';
                }
            }],

            "fnServerParams": function (aoData) {
                aoData.searchVal = $('#TxtSearch').val();
                aoData.searchFrom = $('#TxtFrom').val();
                aoData.searchTo = $('#TxtTo').val();
            },

            "rowCallback": function (row, data, index) {
                $(row).find('td').css({ 'vertical-align': 'middle' });
                $("#remember-table_wrapper").css('width', '100%');
            }
        });

        $('#TxtSearch').on('input', function () {
            datatable.fnDraw();
        });

        $('#TxtFrom, #TxtTo').change(function () {
            datatable.fnDraw();
        });

        datatable.on('click', '.btn-edit', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            $("#RememberModal").modal('show');
            $("#modalTitle").text("AGGIORNA SCADENZIARIO");
            $("#HfRememberID").val(row.Id);
            $("#ValSummary").addClass("d-none");
            $("#TxtRememberDate").val(row.Remember);
            $("#TxtDescription").val(row.Description);
            $("#ComboState").val(row.State);
            $("#CreatedDate").text(row.CreatedDate);

            if ($("#HfUserLogin").val() == "Staff" && row.UserID != $("#HfLoginUserID").val()) {
                $("#StateDiv").removeClass("d-none");
                $("#HfCheck").val("Check");
            }
            else {
                $("#StateDiv").addClass("d-none");
                $("#HfCheck").val("");
            }

            $("#CreatedDate").removeClass("d-none");
            $("#BtnSave").show();
        });

        datatable.on('click', '.btn-show', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            $("#RememberModal").modal('show');
            $("#modalTitle").text("VIEW SCADENZIARIO");
            $("#HfRememberID").val(row.Id);
            $("#ValSummary").addClass("d-none");
            $("#TxtRememberDate").val(row.Remember);
            $("#TxtDescription").val(row.Description);
            $("#ComboState").val(row.State);
            $("#CreatedDate").text(row.CreatedDate);

            $("#StateDiv").removeClass("d-none");

            $("#CreatedDate").removeClass("d-none");
            $("#BtnSave").hide();
        });

        datatable.on('click', '.btn-delete', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            if (!confirm("Click OK per cancellare."))
                return;

            $.ajax({
                type: "POST",
                url: 'DataService.asmx/DeleteRemember',
                data: {
                    id: row.Id
                },
                success: function () {
                    onSuccess({ success: true });
                },
                error: function () {
                    onSuccess({ success: false });
                }
            });
        });

        var onSuccess = function (data) {
            if (data.success) {

                datatable.fnDraw();

            } else {
                alert("Failed!");
            }
        };

        function GetState(state) {
            if (state == 1) return '<span class="text-success">ACCEPTED</span>';
            else if (state == 2) return '<span class="text-danger">REJECTED</span>';
            else return '<span class="text-white">PROGRESS</span>';
        }
    </script>
</asp:Content>
