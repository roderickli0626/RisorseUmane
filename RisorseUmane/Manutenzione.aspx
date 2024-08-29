﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Manutenzione.aspx.cs" Inherits="RisorseUmane.Manutenzione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HfManutenzioneID" runat="server" ClientIDMode="Static" />
    <div class="container-fluid pt-4 px-4">
        <div class="row g-4">
            <div class="col-12" style="min-height: 100vh;">
                <div class="bg-secondary rounded h-100 p-5">
                    <h1 class="mb-4 text-center">MANUTENZIONE</h1>
                    <div class="mt-5 row">
                        <div class="col-md-4">
                            <button class="btn btn-lg btn-primary w-100 mb-2 btn-add">+ AGG. MANUTENZIONE</button>
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList runat="server" ID="ComboType" CssClass="form-select form-select-lg" ClientIDMode="Static"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox runat="server" ID="TxtSearch" ClientIDMode="Static" CssClass="form-control form-control-lg w-100" placeholder="CERCA..."></asp:TextBox>
                        </div>
                    </div>
                    <table class="table table-striped text-center mt-4" id="manutenzione-table">
                        <thead>
                            <tr>
                                <th scope="col">Nr</th>
                                <th scope="col">Subject</th>
                                <th scope="col">Create Date</th>
                                <th scope="col">Description</th>
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
    <div class="modal fade show" id="ManutenzioneModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="modalTitle">Manutenzione</h4>
                </div>
                <div class="modal-body">
                    <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel" ClientIDMode="Static" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSummary" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="ReqValDescription" runat="server" ErrorMessage="Inserire un indirizzo Description." CssClass="text-bg-danger" ControlToValidate="TxtDescription" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="ServerValidator" runat="server" ErrorMessage="Save Failed." Display="None"></asp:CustomValidator>
                            <div class="col-md-12">
                                <div class="border rounded p-4 pb-0 mb-4">
                                    <figure class="text-center">
                                        <blockquote class="blockquote">
                                            <h2>MANUTENZIONE</h2>
                                        </blockquote>
                                        <figcaption class="blockquote-footer d-none" id="CreatedDate">
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
                            <div class="col-md-12 d-none" id="CheckData">
                                <div class="border rounded p-4 pb-0 mb-0">
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="SChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="SCheckDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="AChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="ACheckDate">
                                        </figcaption>
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterPlaceHolder" runat="server">
    <script>
        $("#TxtSearch").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: 'DataService.asmx/FindAllManutenziones',
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
    </script>
    <script>
        $(".btn-add").click(function () {
            $("#ManutenzioneModal").modal('show');
            $(".modal-title").text("AGG. MANUTENZIONE");
            $("#HfManutenzioneID").val("");
            $("#ValSummary").addClass("d-none");
            $("#TxtDescription").val("");

            $("#CreatedDate").addClass("d-none");
            $("#CheckData").addClass("d-none");
            $("#BtnSave").show();

            return false;
        });

        var datatable = $('#manutenzione-table').dataTable({
            "serverSide": true,
            "ajax": 'DataService.asmx/FindManutenziones',
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
                "data": null,
                "render": function (data, type, row, meta) {
                    switch (row.SState) {
                        case 1: {
                            if (row.AState == 2) {
                                return '<p class="text-danger">REJECTED</p>';
                            }
                            else return '<p class="text-success">ACCEPTED</p>'; 
                            break;
                        } 
                        case 2: {
                            if (row.AState == 1) {
                                return '<p class="text-success">ACCEPTED</p>'; 
                            }
                            else return '<p class="text-danger">REJECTED</p>';
                            break;
                        }
                        case 3: {
                            if (row.AState == 1) {
                                return '<p class="text-success">ACCEPTED</p>';
                            }
                            else if (row.AState == 2) {
                                return '<p class="text-danger">REJECTED</p>';
                            }
                            else return '<p class="text-white">PROGRESS</p>';
                            break;
                        }
                    }
                }
            }, {
                "data": null,
                "render": function (data, type, row, meta) {
                    return (row.SState == 3 && row.AState == 3) ? '<div class="justify-content-center">' +
                        '<i class="fa fa-edit mt-1 btn-edit" style="font-size:20px; padding-right: 10px; color:greenyellow"></i>' +
                        '<i class="fa fa-trash mt-1 btn-delete" style="font-size:20px; padding-right: 10px; color:red"></i>' +
                        '</div > ' : '<div class="justify-content-center">' +
                        '<i class="fa fa-eye mt-1 btn-show" style="font-size:20px; color:white"></i>' +
                        '</div > ';
                }
            }],

            "fnServerParams": function (aoData) {
                aoData.searchVal = $('#TxtSearch').val();
                aoData.type = $("#ComboType").val();
            },

            "rowCallback": function (row, data, index) {
                $(row).find('td').css({ 'vertical-align': 'middle' });
                $("#manutenzione-table_wrapper").css('width', '100%');
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

            $("#ManutenzioneModal").modal('show');
            $(".modal-title").text("AGGIORNA MANUTENZIONE");
            $("#HfManutenzioneID").val(row.Id);
            $("#ValSummary").addClass("d-none");
            $("#TxtDescription").val(row.Description);
            $("#CreatedDate").text(row.CreatedDate);

            $("#CreatedDate").removeClass("d-none");
            $("#CheckData").addClass("d-none");
            $("#BtnSave").show();
        });

        datatable.on('click', '.btn-show', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            $("#ManutenzioneModal").modal('show');
            $(".modal-title").text("AGGIORNA MANUTENZIONE");
            $("#HfManutenzioneID").val(row.Id);
            $("#ValSummary").addClass("d-none");
            $("#TxtDescription").val(row.Description);
            $("#CreatedDate").text(row.CreatedDate);

            $("#SChecker").empty();
            $("#SCheckDate").empty();
            $("#AChecker").empty();
            $("#ACheckDate").empty();
            if (row.SChecker != "") {
                $("#SChecker").html("STAFF " + row.SChecker + " " + GetState(row.SState));
                $("#SCheckDate").html(row.SCheckDate);
            }
            if (row.AChecker != "") {
                $("#AChecker").html(row.AChecker + " " + GetState(row.AState));
                $("#ACheckDate").html(row.ACheckDate);
            }

            $("#CreatedDate").removeClass("d-none");
            $("#CheckData").removeClass("d-none");
            $("#BtnSave").hide();
        });

        datatable.on('click', '.btn-delete', function (e) {
            e.preventDefault();

            var row = datatable.fnGetData($(this).closest('tr'));

            if (!confirm("Click OK per cancellare."))
                return;

            $.ajax({
                type: "POST",
                url: 'DataService.asmx/DeleteManutenzione',
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
            if (state == 1) return '<span class="bg-success">ACCEPTED</span>';
            else if (state == 2) return '<span class="bg-danger">REJECTED</span>';
            else return '<span class="bg-info">PROGRESS</span>';
        }
    </script>
</asp:Content>
