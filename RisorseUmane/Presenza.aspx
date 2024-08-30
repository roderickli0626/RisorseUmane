<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Presenza.aspx.cs" Inherits="RisorseUmane.Presenza" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Content/CSS/jquery.datetimepicker.min.css" />
    <link rel="stylesheet" href="Content/CSS/toastr.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HfPresenceID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HfUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HfDate" runat="server" ClientIDMode="Static" />
    <div class="container-fluid pt-4 px-4">
        <div class="row g-4">
            <div class="col-12" style="min-height: 100vh;">
                <div class="bg-secondary rounded h-100 p-5">
                    <h1 class="mb-4 text-center">PRESENZA</h1>
                    <div class="mt-5 row justify-content-end">
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="TxtDate" AutoCompleteType="Disabled" CssClass="form-control form-control-lg" ClientIDMode="Static" placeholder="DATA..."></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="TxtSearch" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg w-100" placeholder="CERCA..."></asp:TextBox>
                        </div>
                    </div>
                    <table class="table table-striped text-center mt-4" id="presence-table">
                        <thead>
                            <tr>
                                <th scope="col">Nr.</th>
                                <th scope="col">Name</th>
                                <th scope="col">Data</th>
                                <th scope="col">TurnOnA</th>
                                <th scope="col">TurnOffA</th>
                                <th scope="col">TurnOnB</th>
                                <th scope="col">TurnOffB</th>
                                <th scope="col">O</th>
                                <th scope="col">S</th>
                                <th scope="col">A</th>
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

    <div class="modal fade show" id="PresenceModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="modalTitle">PRESENZA</h4>
                </div>
                <div class="modal-body">
                    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel" ClientIDMode="Static" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSummary" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:CustomValidator ID="ServerValidator" runat="server" ErrorMessage="Save Failed." Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="ServerValidatorForOSA" runat="server" ErrorMessage="Inserire un indirizzo O, S, A." Display="None"></asp:CustomValidator>
                            <div class="col-md-12 row justify-content-between pe-0" id="OSA">
                                <div class="mb-3 col-3">
                                    <label for="TxtName" class="form-label">O</label>
                                    <asp:TextBox runat="server" ID="TxtO" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                                <div class="mb-3 col-3">
                                    <label for="TxtName" class="form-label">S</label>
                                    <asp:TextBox runat="server" ID="TxtS" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                                <div class="mb-3 col-3">
                                    <label for="TxtName" class="form-label">A</label>
                                    <asp:TextBox runat="server" ID="TxtA" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
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
        $.datetimepicker.setLocale('it');

        $("#TxtDate").datetimepicker({
            format: "d/m/Y",
            value: new Date().toISOString().slice(0, 10).replace("T", " ")
        });

        var datatable = $('#presence-table').dataTable({
            "serverSide": true,
            "ajax": 'DataService.asmx/FindPresences',
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
                "data": "Name",
            }, {
                "data": "Date",
            }, {
                "render": function (data, type, row, meta) {
                    return "08.00";
            }
            }, {
                "render": function (data, type, row, meta) {
                    return "12.00";
                }
            }, {
                "render": function (data, type, row, meta) {
                    return "14.00";
                }
            }, {
                "render": function (data, type, row, meta) {
                    return "18.00";
                }
            }, {
                "data": "O",
            }, {
                "data": "S",
            }, {
                "data": "A",
            }, {
                "data": null,
                "render": function (data, type, row, meta) {
                    return '<i class="fa fa-edit mt-1 btn-edit" style="font-size:20px; color:greenyellow"></i>';
                }
            }],

            "fnServerParams": function (aoData) {
                aoData.searchVal = $('#TxtSearch').val();
                aoData.searchDate = $('#TxtDate').val();
            },

            "rowCallback": function (row, data, index) {
                $(row).find('td').css({ 'vertical-align': 'middle' });
                $("#presence-table_wrapper").css('width', '100%');
            }
        });

        $('#TxtSearch').on('input', function () {
            datatable.fnDraw();
        });

        $('#TxtDate').change(function () {
            datatable.fnDraw();
        });

        datatable.on('click', '.btn-edit', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            $("#PresenceModal").modal('show');
            $("#modalTitle").text("PRESENZA");
            $("#HfPresenceID").val(row.Id);
            $("#HfUserID").val(row.UserId);
            $("#HfDate").val(row.Date);
            $("#ValSummary").addClass("d-none");
            $("#TxtO").val(row.O);
            $("#TxtS").val(row.S);
            $("#TxtA").val(row.A);
        });
    </script>
</asp:Content>
