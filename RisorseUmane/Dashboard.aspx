<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="RisorseUmane.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Content/CSS/jquery.datetimepicker.min.css" />
    <link rel="stylesheet" href="Content/CSS/toastr.css">
    <style>
        .custom-calendar td {
            border: 0.01px solid #2A2D35;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid pt-4 px-4">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <div class="row g-4" style="height: 100vh;">
            <div class="col-sm-12 col-md-10 d-flex flex-column justify-content-between" style="height: 98%">
                <div class="bg-secondary text-center rounded p-4 mb-4 overflow-auto" style="height: 80%;">
                    <div class="d-flex align-items-center justify-content-between mb-4">
                        <h6 class="mb-0">Richieste</h6>
                    </div>
                    <div class="row mb-3 justify-content-between">
                        <div class="col-md-3">
                            <asp:DropDownList runat="server" ID="ComboType" CssClass="form-select form-select-lg" OnSelectedIndexChanged="ComboType_SelectedIndexChanged" ClientIDMode="Static"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList runat="server" ID="ComboState" CssClass="form-select form-select-lg" OnSelectedIndexChanged="ComboState_SelectedIndexChanged" ClientIDMode="Static"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="TxtDate" ClientIDMode="Static" CssClass="form-control form-control-lg w-100" OnTextChanged="TxtDate_TextChanged" placeholder="DATA..."></asp:TextBox>
                        </div>
                        <div class="col-md-3" runat="server" id="SearchDiv">
                            <asp:TextBox runat="server" ID="TxtSearch" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control form-control-lg w-100" OnTextChanged="TxtSearch_TextChanged" placeholder="CERCA..."></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel" class="overflow-auto">
                            <ContentTemplate>
                                <asp:Calendar ID="Calendar1" runat="server" CssClass="text-center m-auto custom-calendar" BackColor="#000000" BorderColor="#FFFFFF"
                                    BorderWidth="2px" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt"
                                    ForeColor="#FFFFFF" ShowGridLines="True">
                                    <SelectedDayStyle BackColor="#3B3E46" Font-Bold="True" />
                                    <SelectorStyle BackColor="#FFFFFF" />
                                    <TodayDayStyle BackColor="#4C4F57" ForeColor="White" />
                                    <OtherMonthDayStyle ForeColor="White" BackColor="#191C24" />
                                    <NextPrevStyle Font-Size="12pt" ForeColor="#FFFFFF" BackColor="#EB1616" CssClass="text-center" />
                                    <DayHeaderStyle BackColor="#EB1616" Font-Bold="True" Height="1px" />
                                    <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                                </asp:Calendar>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ComboType" />
                                <asp:AsyncPostBackTrigger ControlID="ComboState" />
                                <asp:AsyncPostBackTrigger ControlID="TxtDate" />
                                <asp:AsyncPostBackTrigger ControlID="TxtSearch" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="bg-secondary text-center rounded p-4 overflow-auto" style="height: 20%;">
                    <div class="d-flex align-items-center justify-content-between mb-4">
                        <h6 class="mb-0">Scadenza Datas</h6>
                    </div>
                    <div class="row">
                        <asp:Repeater ID="ExpireDateRepeater" runat="server">
                            <ItemTemplate>
                                <div class="col-md-4 col-sm-12 d-flex align-items-center border-bottom py-2">
                                    <input class="form-check-input m-0" type="checkbox">
                                    <div class="w-100 ms-3">
                                        <div class="d-flex w-100 align-items-center justify-content-between">
                                            <h6 style="margin-bottom: 0px;"><%# Eval("Remember")%></h6>
                                            <span><%# Eval("Description")%></span>
                                            <h6 style="margin-bottom: 0px;"><%# Eval("UserName")%></h6>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-md-2" style="height: 98%">
                <div class="bg-secondary text-center rounded p-4 h-100 overflow-auto">
                    <div class="align-items-center justify-content-between">
                        <h6 class="mb-0">Datore di lavoro / Logistica</h6>
                        <asp:TextBox runat="server" ID="userSearch" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control form-control-sm mt-3" placeholder="CERCA..."></asp:TextBox>
                    </div>
                    <table class="table table-striped text-center mt-2" id="user-table">
                        <tbody>
                        </tbody>
                    </table>
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
        });

        var datatable = $('#user-table').dataTable({
            "serverSide": true,
            "ajax": 'DataService.asmx/FindDashboardUsers',
            "dom": '<"table-responsive"t>',
            "autoWidth": false,
            "pageLength": 10000,
            "processing": true,
            "ordering": false,
            "columns": [{
                "data": "Presence",
                "render": function (data, type, row, meta) {
                    var bg = data ? "bg-success" : "bg-danger";
                    return `<div class="position-relative">
                                <img class="rounded-circle" src="Content/Images/user-default.jpg" alt="" style="width: 40px; height: 40px;">
                                <div class="${bg} rounded-circle border border-2 border-white position-absolute end-0 bottom-0 p-1"></div>
                            </div>`;
                }
            }, {
                "data": "Name",
                "render": function (data, type, row, meta) {
                    return `<h6 class="mb-0" style="text-align: left">${data}</h6>`;
                }
            }],

            "fnServerParams": function (aoData) {
                aoData.searchVal = $('#userSearch').val();
            },

            "rowCallback": function (row, data, index) {
                $(row).find('td').css({ 'vertical-align': 'middle' });
                $("#user-table_wrapper").css('width', '100%');
            }
        });

        $("#userSearch").on('input', function () {
            datatable.fnDraw();
        });
    </script>
</asp:Content>
