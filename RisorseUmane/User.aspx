﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="RisorseUmane.User1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HfUserID" runat="server" ClientIDMode="Static" />
    <div class="container-fluid pt-4 px-4">
        <div class="row g-4">
            <div class="col-12" style="min-height: 100vh;">
                <div class="bg-secondary rounded h-100 p-5">
                    <h1 class="mb-4 text-center">Lista Operatori</h1>
                    <div class="mt-5 row">
                        <div class="col-md-4">
                            <button class="btn btn-lg btn-primary w-100 mb-2 btn-add">+ Agg. User</button>
                        </div>
                        <div class="col-md-4 ms-auto">
                            <asp:TextBox runat="server" ID="TxtSearch" ClientIDMode="Static" CssClass="form-control form-control-lg w-100" placeholder="CERCA..."></asp:TextBox>
                        </div>
                    </div>
                    <table class="table table-striped text-center mt-4" id="user-table">
                        <thead>
                            <tr>
                                <th scope="col">Nr</th>
                                <th scope="col">Nome</th>
                                <th scope="col">Email</th>
                                <th scope="col">Surname</th>
                                <th scope="col">Mobile</th>
                                <th scope="col">Role</th>
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
    <div class="modal fade show" id="UserModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="modalTitle">User</h4>
                </div>
                <div class="modal-body">
                    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel" ClientIDMode="Static" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSummary" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="ReqValEmail" runat="server" ErrorMessage="Inserire un indirizzo Email." CssClass="text-bg-danger" ControlToValidate="TxtEmail" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="PasswordValidator" runat="server" ErrorMessage="Le Password non corrispondono." Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="EmailValidator" runat="server" ErrorMessage="Email non è corretta." Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="ServerValidator" runat="server" ErrorMessage="Questo indirizzo Email è già registrato." Display="None"></asp:CustomValidator>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Nome</label>
                                    <asp:TextBox runat="server" ID="TxtName" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Email</label>
                                    <asp:TextBox runat="server" ID="TxtEmail" ClientIDMode="Static" AutoCompleteType="Disabled" TextMode="Email" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Surname</label>
                                    <asp:TextBox runat="server" ID="TxtSurname" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Role</label>
                                    <asp:DropDownList runat="server" ID="ComboType1" CssClass="form-select form-select-lg" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Mobile</label>
                                    <asp:TextBox runat="server" ID="TxtMobile" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Password</label>
                                    <asp:TextBox runat="server" ID="TxtPassword" ClientIDMode="Static" AutoCompleteType="Disabled" TextMode="Password" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Conferma Password</label>
                                    <asp:TextBox runat="server" ID="TxtPasswordRepeat" ClientIDMode="Static" AutoCompleteType="Disabled" TextMode="Password" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSave" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer modal--footer">
                    <asp:Button runat="server" ID="BtnSave" CssClass="btn btn-primary" Text="Conferma" OnClick="BtnSave_Click" />
                    <button type="button" class="btn btn-warning" data-bs-dismiss="modal">Chiudi</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script>
        $("#TxtSearch").autocomplete({
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
                //var selectedModelId = ui.item.id;
                datatable.fnDraw();
            }
        });
    </script>
    <script>
    $(".btn-add").click(function () {
        $("#UserModal").modal('show');
        $(".modal-title").text("AGG. USER");
        $("#HfUserID").val("");
        $("#ValSummary").addClass("d-none");
        $("#TxtName").val("");
        $("#TxtEmail").val("");
        $("#ComboType1").val("");
        $("#TxtSurname").val("");
        $("#TxtMobile").val("");
        $("#TxtPassword").val("");
        $("#TxtPasswordRepeat").val("");

        return false;
    });

    var datatable = $('#user-table').dataTable({
        "serverSide": true,
        "ajax": 'DataService.asmx/FindUsers',
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
            "data": "Email",
        }, {
            "data": "Surname",
        }, {
            "data": "Mobile",
        }, {
            "data": "Role",
            "render": function (data, type, row, meta) {
                if (data == 3) return '<p class="text-success">STAFF</p>';
                else if (data == 4) return '<p class="text-danger">LOGISTIC</p>';
                else if (data == 5) return '<p class="text-warning">EMPLOYER</p>';
                else return "";
            }
        },  {
            "data": null,
            "render": function (data, type, row, meta) {
                return '<div class="justify-content-center">' +
                    '<i class="fa fa-edit mt-1 btn-edit" style="font-size:20px; padding-right: 10px; color:greenyellow"></i>' +
                    '<i class="fa fa-trash mt-1 btn-delete" style="font-size:20px; padding-right: 10px; color:red"></i>' +
                    '</div > ';
            }
        }],

        "fnServerParams": function (aoData) {
            aoData.searchVal = $('#TxtSearch').val();
        },

        "rowCallback": function (row, data, index) {
            $(row).find('td').css({ 'vertical-align': 'middle' });
            $("#user-table_wrapper").css('width', '100%');
        }
    });

    $('#TxtSearch').on('input', function () {
        datatable.fnDraw();
    });

    datatable.on('click', '.btn-edit', function (e) {
        e.preventDefault();

        var row = datatable.fnGetData($(this).closest('tr'));

        $("#UserModal").modal('show');
        $(".modal-title").text("AGGIORNA USER");
        $("#HfUserID").val(row.Id);
        $("#ValSummary").addClass("d-none");
        $("#TxtName").val(row.Name);
        $("#TxtEmail").val(row.Email);
        $("#ComboType1").val(row.Role);
        $("#TxtSurname").val(row.Surname);
        $("#TxtMobile").val(row.Mobile);
        $("#TxtPassword").val("");
        $("#TxtPasswordRepeat").val("");
    });

    datatable.on('click', '.btn-delete', function (e) {
        e.preventDefault();

        var row = datatable.fnGetData($(this).closest('tr'));

        if (!confirm("Click OK per cancellare."))
            return;

        $.ajax({
            type: "POST",
            url: 'DataService.asmx/DeleteUser',
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
            alert("Fallito!");
        }
    };
    </script>
</asp:Content>
