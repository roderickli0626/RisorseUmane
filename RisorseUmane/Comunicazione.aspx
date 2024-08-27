<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Comunicazione.aspx.cs" Inherits="RisorseUmane.Comunicazione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ui-autocomplete {  
            z-index: 10000; 
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HfComunicazioneID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HfLoginUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HfReceiverID" runat="server" ClientIDMode="Static" />
    <div class="container-fluid pt-4 px-4">
        <div class="row g-4">
            <div class="col-12" style="min-height: 100vh;">
                <div class="bg-secondary rounded h-100 p-5">
                    <h1 class="mb-4 text-center">COMUNICAZIONE</h1>
                    <div class="mt-5 row">
                        <div class="col-md-4">
                            <button class="btn btn-lg btn-primary w-100 mb-2 btn-add" runat="server" id="BtnForStaff">+ AGG. COMUNICAZIONE</button>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="ComboType" CssClass="form-select form-select-lg" ClientIDMode="Static"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="TxtSearch" ClientIDMode="Static" CssClass="form-control form-control-lg w-100" placeholder="CERCA..."></asp:TextBox>
                        </div>
                    </div>
                    <table class="table table-striped text-center mt-4" id="comunicazione-table">
                        <thead>
                            <tr>
                                <th scope="col">Nr</th>
                                <th scope="col">Subject</th>
                                <th scope="col">Create Date</th>
                                <th scope="col">Description</th>
                                <th scope="col">Sender</th>
                                <th scope="col">Receiver</th>
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
    <div class="modal fade show" id="ComunicazioneModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="modalTitle">Comunicazione</h4>
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
                                            <h2>COMUNICAZIONE</h2>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="CreatedDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Description</label>
                                    <asp:TextBox runat="server" ID="TxtDescription" ClientIDMode="Static" TextMode="MultiLine" Rows="2" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12" id="FromToData">
                                <div class="border rounded p-4 pb-0 mb-0">
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="From">From ME</h5>
                                        </blockquote>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="To">To STAFF</h5>
                                        </blockquote>
                                    </figure>
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

    <div class="modal fade show" id="AddComunicazioneModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="AddmodalTitle">Comunicazione</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" ClientIDMode="Static" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:CustomValidator ID="CustomValidator" runat="server" ErrorMessage="Save Failed." Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidatorForDescription" runat="server" ErrorMessage="Inserire un indirizzo Description." Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidatorForReceiver" runat="server" ErrorMessage="Inserire un indirizzo Receiver." Display="None"></asp:CustomValidator>
                            <div class="col-md-12">
                                <div class="border rounded p-4 pb-0 mb-4">
                                    <figure class="text-center">
                                        <blockquote class="blockquote">
                                            <h2>COMUNICAZIONE</h2>
                                        </blockquote>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-12" runat="server" id="ForStaff" visible="false">
                                <label for="TxtTitle" class="form-label">Send To</label>
                                <div class="border rounded p-4 pb-3 mb-4">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-check mb-4">
                                                <input class="form-check-input" type="radio" name="inlineRadioOptions" runat="server" id="FromStaffToCEO" value="option1">
                                                <label class="form-check-label" for="inlineRadio1">CEO</label>
                                            </div>
                                            <div class="form-check">
                                                <input class="form-check-input" type="radio" name="inlineRadioOptions" runat="server" id="FromStaffToAdmin" value="option2">
                                                <label class="form-check-label" for="inlineRadio2">ADMIN</label>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-check mb-4">
                                                <input class="form-check-input" type="radio" name="inlineRadioOptions" runat="server" id="FromStaffToEmployer" value="option3">
                                                <label class="form-check-label" for="inlineRadio3">EMPLOYERS</label>
                                            </div>
                                            <div class="form-check">
                                                <input class="form-check-input" type="radio" name="inlineRadioOptions" runat="server" id="FromStaffToLogistic" value="option4">
                                                <label class="form-check-label" for="inlineRadio3">LOGISTICS</label>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-check mb-3">
                                                <input class="form-check-input" type="radio" name="inlineRadioOptions" runat="server" id="FromStaffToIndividual" clientIDMode="static" value="option5">
                                                <label class="form-check-label" for="inlineRadio3">INDIVIDUAL</label>
                                            </div>
                                            <div class="form-check ps-0">
                                                <input class="form-control receiver_input" placeholder="CERCA..."></input>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" runat="server" id="ForAdmin" visible="false">
                                <label for="TxtTitle" class="form-label">Send To</label>
                                <div class="border rounded p-4 pb-3 mb-4">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-check mb-4">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromAdminToCEO" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">CEO</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromAdminToStaff" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">STAFFS</label>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-check mb-4">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromAdminToEmployer" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">EMPLOYERS</label>
                                            </div>
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromAdminToLogistic" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">LOGISTICS</label>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-check mb-3">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromAdminToIndividual" clientIDMode="static" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">INDIVIDUAL</label>
                                            </div>
                                            <div class="form-check ps-0">
                                                <input class="form-control receiver_input d-none" placeholder="CERCA..."></input>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12" runat="server" id="ForCEO" visible="false">
                                <label for="TxtTitle" class="form-label">Send To</label>
                                <div class="border rounded p-4 pb-3 mb-4">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-check mb-4">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromCEOToAdmin" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">ADMIN</label>
                                            </div>
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromCEOToStaff" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">STAFFS</label>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-check mb-4">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromCEOToEmployer" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">EMPLOYERS</label>
                                            </div>
                                            <div class="form-check">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromCEOToLogistic" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">LOGISTICS</label>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-check mb-3">
                                                <input class="form-check-input" type="checkbox" runat="server" id="FromCEOToIndividual" clientIDMode="static" value="option1">
                                                <label class="form-check-label" for="inlineCheckbox1">INDIVIDUAL</label>
                                            </div>
                                            <div class="form-check ps-0">
                                                <input class="form-control receiver_input d-none" placeholder="CERCA..."></input>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Description</label>
                                    <asp:TextBox runat="server" ID="TxtAddDescription" ClientIDMode="Static" TextMode="MultiLine" Rows="2" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnAddSave" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="BtnAddSave" ClientIDMode="Static" CssClass="btn btn-primary" Text="Conferma" OnClick="BtnAddSave_Click" />
                    <button type="button" class="btn btn-warning" data-bs-dismiss="modal">Chiudi</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoadedHandler);

        function pageLoadedHandler(sender, args) {
            // This function will be called after each UpdatePanel postback
            var updatedPanels = args.get_panelsUpdated();

            for (var i = 0; i < updatedPanels.length; i++) {
                var updatePanelID = updatedPanels[i].id;

                if (updatePanelID === "UpdatePanel1") {
                    $(".receiver_input").autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                url: 'DataService.asmx/FindAllUsers',
                                dataType: "json",
                                data: { term: request.term },
                                success: function (data) {
                                    response(data.map(item => ({ label: item.Name, id: item.Id, value: item.Name })));
                                }
                            });
                        },
                        minLength: 0,
                        select: function (event, ui) {
                            var receiverID = ui.item.id;
                            $("#HfReceiverID").val(receiverID);
                        }
                    });

                    $("#FromCEOToIndividual, #FromAdminToIndividual, #FromStaffToIndividual").prop('checked', false);

                    $("#FromCEOToIndividual, #FromAdminToIndividual").on('change', function () {
                        if ($(this).is(':checked')) $(".receiver_input").removeClass('d-none');
                        else $(".receiver_input").addClass('d-none');
                    });
                }
            }
        };
    </script>
    <script>
        $("#TxtSearch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: 'DataService.asmx/FindAllComunicaziones',
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

        $(".receiver_input").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: 'DataService.asmx/FindAllUsers',
                    dataType: "json",
                    data: { term: request.term },
                    success: function (data) {
                        response(data.map(item => ({ label: item.Name, id: item.Id, value: item.Name })));
                    }
                });
            },
            minLength: 0,
            select: function (event, ui) {
                var receiverID = ui.item.id;
                $("#HfReceiverID").val(receiverID);
            }
        });

        $("#FromCEOToIndividual, #FromAdminToIndividual, #FromStaffToIndividual").on('change', function () {
            if ($(this).is(':checked')) $(".receiver_input").removeClass('d-none');
            else $(".receiver_input").addClass('d-none');
        });

    </script>
    <script>
        $(".btn-add").click(function () {
            $("#AddComunicazioneModal").modal('show');
            $("#AddmodalTitle").text("AGG. COMUNICAZIONE");
            $("#HfComunicazioneID").val("");
            $("#HfReceiverID").val("");
            $("#ValidationSummary1").addClass("d-none");
            $("#TxtAddDescription").val("");

            return false;
        });

        var datatable = $('#comunicazione-table').dataTable({
            "serverSide": true,
            "ajax": 'DataService.asmx/FindComunicaziones',
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
                "data": "Subject",
            }, {
                "data": "CreatedDate",
            }, {
                "data": "Description",
            }, {
                "data": "Sender",
            }, {
                "data": "Receiver",
            }, {
                "data": null,
                "render": function (data, type, row, meta) {
                    var loginUserID = $("#HfLoginUserID").val();
                    return (loginUserID == row.SenderID) ? '<div class="justify-content-center">' +
                        '<i class="fa fa-edit mt-1 btn-edit" style="font-size:20px; padding-right: 10px; color:greenyellow"></i>' +
                        '<i class="fa fa-trash mt-1 btn-delete" style="font-size:20px; padding-right: 10px; color:red"></i>' +
                        '</div > ' : '<div class="justify-content-center">' +
                        '<i class="fa fa-eye mt-1 btn-show" style="font-size:20px; color:white"></i>' +
                        '</div > ';
                }
            }],

            "fnServerParams": function (aoData) {
                aoData.searchVal = $('#TxtSearch').val();
                aoData.type = $("#ComboType").val() || 0;
            },

            "rowCallback": function (row, data, index) {
                $(row).find('td').css({ 'vertical-align': 'middle' });
                $("#comunicazione-table_wrapper").css('width', '100%');
            }
        });

        $('#TxtSearch').on('input', function () {
            datatable.fnDraw();
        });

        $('#ComboType').change(function () {
            datatable.fnDraw();
        });

        datatable.on('click', '.btn-edit', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            $("#ComunicazioneModal").modal('show');
            $("#modalTitle").text("AGGIORNA COMUNICAZIONE");
            $("#HfComunicazioneID").val(row.Id);
            $("#ValSummary").addClass("d-none");
            $("#TxtDescription").val(row.Description);
            $("#CreatedDate").text(row.CreatedDate);

            $("#From").text("From " + row.Sender);
            $("#To").text("To " + row.Receiver);

            $("#BtnSave").show();
        });

        datatable.on('click', '.btn-show', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            $("#ComunicazioneModal").modal('show');
            $("#modalTitle").text("AGGIORNA COMUNICAZIONE");
            $("#HfComunicazioneID").val(row.Id);
            $("#ValSummary").addClass("d-none");
            $("#TxtDescription").val(row.Description);
            $("#CreatedDate").text(row.CreatedDate);

            $("#From").text("From " + row.Sender);
            $("#To").text("To " + row.Receiver);

            $("#BtnSave").hide();
        });

        datatable.on('click', '.btn-delete', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            if (!confirm("Click OK per cancellare."))
                return;

            $.ajax({
                type: "POST",
                url: 'DataService.asmx/DeleteComunicazione',
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
    </script>
</asp:Content>
