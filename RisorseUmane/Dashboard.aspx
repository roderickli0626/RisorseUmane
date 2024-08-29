<%@ Page Title="" Language="C#" MasterPageFile="~/Page.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="RisorseUmane.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Content/CSS/jquery.datetimepicker.min.css" />
    <link rel="stylesheet" href="Content/CSS/toastr.css">
    <style>
        .custom-calendar td {
            border: 0.01px solid #2A2D35;
            text-align: center;
        }
        td {
            padding-left: 10px;
            padding-right: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="HfLoginRole" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HfRequestID" runat="server" ClientIDMode="Static" />
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
                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ComboType" CssClass="form-select form-select-lg" OnSelectedIndexChanged="ComboType_SelectedIndexChanged" ClientIDMode="Static"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ComboState" CssClass="form-select form-select-lg" OnSelectedIndexChanged="ComboState_SelectedIndexChanged" ClientIDMode="Static"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" AutoPostBack="false" ID="TxtDate" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control form-control-lg w-100" placeholder="DATA..."></asp:TextBox>
                        </div>
                        <div class="col-md-3" runat="server" id="SearchDiv">
                            <asp:TextBox runat="server" AutoPostBack="true" ID="TxtSearch" ClientIDMode="Static" AutoCompleteType="Disabled" CssClass="form-control form-control-lg w-100" OnTextChanged="TxtSearch_TextChanged" placeholder="CERCA..."></asp:TextBox>
                        </div>
                        <asp:Button runat="server" ID="BtnHiddenForDate" CssClass="d-none" OnClick="BtnHiddenForDate_Click" ClientIDMode="Static" />
                    </div>
                    <div>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel" ClientIDMode="Static" class="overflow-auto">
                            <ContentTemplate>
                                <asp:Calendar ID="Calendar1" ClientIDMode="Static" runat="server" CssClass="text-center m-auto custom-calendar" BackColor="#000000" BorderColor="#FFFFFF"
                                    BorderWidth="2px" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="12pt" OnSelectionChanged="Calendar1_SelectionChanged" 
                                    ForeColor="#FFFFFF" ShowGridLines="True" OnDayRender="Calendar1_DayRender" OnVisibleMonthChanged="Calendar1_VisibleMonthChanged">
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
                                <asp:AsyncPostBackTrigger ControlID="TxtSearch" />
                                <asp:AsyncPostBackTrigger ControlID="BtnHiddenForDate" />
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
    <div class="modal fade show" id="FerieModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="FERmodalTitle">Ferie</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanelFER" ClientIDMode="Static" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSummaryFER" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:CustomValidator ID="ServerValidatorFER" runat="server" ErrorMessage="Save Failed." Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="ReqValA" runat="server" ErrorMessage="Inserire un indirizzo A." Display="None"></asp:CustomValidator>
                            <div class="col-md-12">
                                <div class="border rounded p-4 pb-0 mb-4">
                                    <figure class="text-center">
                                        <blockquote class="blockquote">
                                            <h2>HOLIDAY REQUEST</h2>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="FERCreatedDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">From Date</label>
                                    <asp:TextBox runat="server" ID="TxtFERFrom" ReadOnly="true" style="background-color: #000" AutoCompleteType="Disabled" CssClass="form-control form-control-lg" ClientIDMode="Static" placeholder="From..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">To Date</label>
                                    <asp:TextBox runat="server" ID="TxtFERTo" ReadOnly="true" style="background-color: #000" AutoCompleteType="Disabled" CssClass="form-control form-control-lg" ClientIDMode="Static" placeholder="To..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Description</label>
                                    <asp:TextBox runat="server" ID="TxtFERDescription" ReadOnly="true" style="background-color: #000" ClientIDMode="Static" TextMode="MultiLine" Rows="2" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12 row justify-content-between pe-0" id="OSA">
                                <div class="mb-3 col-2">
                                    <label for="TxtName" class="form-label">O</label>
                                    <asp:TextBox runat="server" ID="TxtFERO" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                                <div class="mb-3 col-2">
                                    <label for="TxtName" class="form-label">S</label>
                                    <asp:TextBox runat="server" ID="TxtFERS" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                                <div class="mb-3 col-2">
                                    <label for="TxtName" class="form-label">A</label>
                                    <asp:TextBox runat="server" ID="TxtFERA" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                                <div class="mb-3 pe-0 col-6">
                                    <label for="TxtName" class="form-label">State</label>
                                    <asp:DropDownList runat="server" ID="ComboFERState" CssClass="form-select form-select-lg" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12" id="CheckData">
                                <div class="border rounded p-4 pb-0 mb-0">
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="FERSender"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="FERSendDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="FERSChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="FERSCheckDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="FERAChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="FERACheckDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSaveFER" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="BtnSaveFER" ClientIDMode="Static" CssClass="btn btn-primary" Text="Conferma" OnClick="BtnSaveFER_Click" />
                    <button type="button" class="btn btn-warning" data-bs-dismiss="modal">Chiudi</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade show" id="MalattiaModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="MALmodalTitle">Malattia</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanelMAL" ClientIDMode="Static" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSummaryMAL" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:CustomValidator ID="ServerValidatorMAL" runat="server" ErrorMessage="Save Failed." Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="ReqValAForMAL" runat="server" ErrorMessage="Inserire un indirizzo A." Display="None"></asp:CustomValidator>
                            <div class="col-md-12">
                                <div class="border rounded p-4 pb-0 mb-4">
                                    <figure class="text-center">
                                        <blockquote class="blockquote">
                                            <h2>ILLNESS REQUEST</h2>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="MALCreatedDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">From Date</label>
                                    <asp:TextBox runat="server" ID="TxtMALFrom" ReadOnly="true" style="background-color: #000" AutoCompleteType="Disabled" CssClass="form-control form-control-lg" ClientIDMode="Static" placeholder="From..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">To Date</label>
                                    <asp:TextBox runat="server" ID="TxtMALTo" ReadOnly="true" style="background-color: #000" AutoCompleteType="Disabled" CssClass="form-control form-control-lg" ClientIDMode="Static" placeholder="To..."></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Description</label>
                                    <asp:TextBox runat="server" ID="TxtMALDescription" ReadOnly="true" style="background-color: #000" ClientIDMode="Static" TextMode="MultiLine" Rows="2" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12 row justify-content-between pe-0" id="OSAForMAL">
                                <div class="mb-3 col-2">
                                    <label for="TxtName" class="form-label">O</label>
                                    <asp:TextBox runat="server" ID="TxtMALO" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                                <div class="mb-3 col-2">
                                    <label for="TxtName" class="form-label">S</label>
                                    <asp:TextBox runat="server" ID="TxtMALS" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                                <div class="mb-3 col-2">
                                    <label for="TxtName" class="form-label">A</label>
                                    <asp:TextBox runat="server" ID="TxtMALA" AutoCompleteType="Disabled" ClientIDMode="Static" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                                <div class="mb-3 pe-0 col-6">
                                    <label for="TxtName" class="form-label">State</label>
                                    <asp:DropDownList runat="server" ID="ComboMALState" CssClass="form-select form-select-lg" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12" id="CheckMALData">
                                <div class="border rounded p-4 pb-0 mb-0">
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="MALSender"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="MALSendDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="MALSChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="MALSCheckDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="MALAChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="MALACheckDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSaveMAL" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="BtnSaveMAL" ClientIDMode="Static" CssClass="btn btn-primary" Text="Conferma" OnClick="BtnSaveMAL_Click" />
                    <button type="button" class="btn btn-warning" data-bs-dismiss="modal">Chiudi</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade show" id="ManutenzioneModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="MANmodalTitle">Manutenzione</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanelMAN" ClientIDMode="Static" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSummaryMAN" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:CustomValidator ID="ServerValidatorMAN" runat="server" ErrorMessage="Save Failed." Display="None"></asp:CustomValidator>
                            <div class="col-md-12">
                                <div class="border rounded p-4 pb-0 mb-4">
                                    <figure class="text-center">
                                        <blockquote class="blockquote">
                                            <h2>MANUTENZIONE</h2>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="MANCreatedDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-3">
                                    <label for="TxtTitle" class="form-label">Description</label>
                                    <asp:TextBox runat="server" ID="TxtMANDescription" ReadOnly="true" style="background-color: #000" ClientIDMode="Static" TextMode="MultiLine" Rows="2" CssClass="form-control form-control-lg"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12 row justify-content-between pe-0" id="OSAForMAN">
                                <div class="mb-3 pe-0 col-12">
                                    <label for="TxtName" class="form-label">State</label>
                                    <asp:DropDownList runat="server" ID="ComboMANState" CssClass="form-select form-select-lg" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12" id="CheckMANData">
                                <div class="border rounded p-4 pb-0 mb-0">
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="MANSender"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="MANSendDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="MANSChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="MANSCheckDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="MANAChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="MANACheckDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSaveMAN" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="BtnSaveMAN" ClientIDMode="Static" CssClass="btn btn-primary" Text="Conferma" OnClick="BtnSaveMAN_Click" />
                    <button type="button" class="btn btn-warning" data-bs-dismiss="modal">Chiudi</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade show" id="DPIModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" data-bs-backdrop="static" aria-modal="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content bg-secondary">
                <div class="modal-header">
                    <h4 class="modal-title text-white" id="DPImodalTitle">DPI/Material</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanelDPI" ClientIDMode="Static" UpdateMode="Conditional" ChildrenAsTriggers="false" class="row gy-3">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSummaryDPI" runat="server" CssClass="mt-lg mb-lg text-left bg-gradient" ClientIDMode="Static" />
                            <asp:CustomValidator ID="ServerValidatorDPI" runat="server" ErrorMessage="Save Failed." Display="None"></asp:CustomValidator>
                            <div class="col-md-12">
                                <div class="border rounded p-4 pb-0 mb-4">
                                    <figure class="text-center">
                                        <blockquote class="blockquote">
                                            <h2>MATERIAL REQUEST/DPI</h2>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="DPICreatedDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <asp:Button runat="server" ID="BtnDPILoad" ClientIDMode="Static" CssClass="btn btn-primary btn-lg w-100 d-none" Text="Load DPI" OnClick="BtnDPILoad_Click" />
                                <div>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Repeater ID="DPIRepeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="table-responsive">
                                                        <table class="table table-striped text-center">
                                                            <thead>
                                                                <tr>
                                                                    <th>Descrizione</th>
                                                                    <th>Size</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("Description")%></td>
                                                            <td><%# Eval("Size")%></td>
                                                        </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                                                    </table>
                                                    </div>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnDPILoad" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-12 row justify-content-between pe-0" id="OSAForDPI">
                                <div class="mb-3 pe-0 col-12">
                                    <label for="TxtName" class="form-label">State</label>
                                    <asp:DropDownList runat="server" ID="ComboDPIState" CssClass="form-select form-select-lg" ClientIDMode="Static"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-12" id="CheckDPIData">
                                <div class="border rounded p-4 pb-0 mb-0">
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="DPISender"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="DPISendDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="DPISChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="DPISCheckDate">
                                        </figcaption>
                                    </figure>
                                    <figure class="text-end">
                                        <blockquote class="blockquote">
                                            <h5 id="DPIAChecker"></h5>
                                        </blockquote>
                                        <figcaption class="blockquote-footer" id="DPIACheckDate">
                                        </figcaption>
                                    </figure>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSaveDPI" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="BtnSaveDPI" ClientIDMode="Static" CssClass="btn btn-primary" Text="Conferma" OnClick="BtnSaveDPI_Click" />
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
                    $(".btnMAL").click(function () {
                        var request = $(this).data("content");

                        $("#MalattiaModal").modal('show');
                        $("#HfRequestID").val(request.Id);

                        var totalState = 3;
                        if (request.AState != 3) totalState = request.AState;
                        else totalState = request.SState;

                        if ($("#HfLoginRole").val() == "User" || ($("#HfLoginRole").val() == "Staff" && totalState != 3) || ($("#HfLoginRole").val() == "Admin" && request.AState != 3))
                            $("#BtnSaveMAL").addClass('d-none');
                        else $("#BtnSaveMAL").removeClass('d-none');

                        $("#ComboMALState").val(totalState);
                        $("#ValSummaryMAL").addClass("d-none");
                        $("#TxtMALFrom").val(request.From);
                        $("#TxtMALTo").val(request.To);
                        $("#TxtMALDescription").val(request.Description);
                        $("#MALCreatedDate").text(request.CreatedDate);
                        $("#TxtMALO").val(request.O);
                        $("#TxtMALS").val(request.S);
                        $("#TxtMALA").val(request.A);


                        $("#MALSChecker").empty();
                        $("#MALSCheckDate").empty();
                        $("#MALAChecker").empty();
                        $("#MALACheckDate").empty();
                        $("#MALSender").empty();
                        $("#MALSendDate").empty();

                        $("#MALSender").html("SENDER: " + request.Sender);
                        $("#MALSendDate").html(request.CreatedDate);
                        if (request.SChecker != "") {
                            $("#MALSChecker").html("STAFF " + request.SChecker + " " + GetState(request.SState));
                            $("#MALSCheckDate").html(request.SCheckDate);
                        }
                        if (request.AChecker != "") {
                            $("#MALAChecker").html(request.AChecker + " " + GetState(request.AState));
                            $("#MALACheckDate").html(request.ACheckDate);
                        }
                    });

                    $(".btnFER").click(function () {
                        var request = $(this).data("content");

                        $("#FerieModal").modal('show');
                        $("#HfRequestID").val(request.Id);

                        var totalState = 3;
                        if (request.AState != 3) totalState = request.AState;
                        else totalState = request.SState;

                        if ($("#HfLoginRole").val() == "User" || ($("#HfLoginRole").val() == "Staff" && totalState != 3) || ($("#HfLoginRole").val() == "Admin" && request.AState != 3))
                            $("#BtnSaveFER").addClass('d-none');
                        else $("#BtnSaveFER").removeClass('d-none');

                        $("#ComboFERState").val(totalState);
                        $("#ValSummaryFER").addClass("d-none");
                        $("#TxtFERFrom").val(request.From);
                        $("#TxtFERTo").val(request.To);
                        $("#TxtFERDescription").val(request.Description);
                        $("#FERCreatedDate").text(request.CreatedDate);
                        $("#TxtFERO").val(request.O);
                        $("#TxtFERS").val(request.S);
                        $("#TxtFERA").val(request.A);


                        $("#FERSChecker").empty();
                        $("#FERSCheckDate").empty();
                        $("#FERAChecker").empty();
                        $("#FERACheckDate").empty();
                        $("#FERSender").empty();
                        $("#FERSendDate").empty();

                        $("#FERSender").html("SENDER: " + request.Sender);
                        $("#FERSendDate").html(request.CreatedDate);
                        if (request.SChecker != "") {
                            $("#FERSChecker").html("STAFF " + request.SChecker + " " + GetState(request.SState));
                            $("#FERSCheckDate").html(request.SCheckDate);
                        }
                        if (request.AChecker != "") {
                            $("#FERAChecker").html(request.AChecker + " " + GetState(request.AState));
                            $("#FERACheckDate").html(request.ACheckDate);
                        }
                    });

                    $(".btnMAN").click(function () {
                        var request = $(this).data("content");

                        $("#ManutenzioneModal").modal('show');
                        $("#HfRequestID").val(request.Id);

                        var totalState = 3;
                        if (request.AState != 3) totalState = request.AState;
                        else totalState = request.SState;

                        if ($("#HfLoginRole").val() == "User" || ($("#HfLoginRole").val() == "Staff" && totalState != 3) || ($("#HfLoginRole").val() == "Admin" && request.AState != 3))
                            $("#BtnSaveMAN").addClass('d-none');
                        else $("#BtnSaveMAN").removeClass('d-none');

                        $("#ComboMANState").val(totalState);
                        $("#ValSummaryMAN").addClass("d-none");
                        $("#TxtMANDescription").val(request.Description);
                        $("#MANCreatedDate").text(request.CreatedDate);

                        $("#MANSChecker").empty();
                        $("#MANSCheckDate").empty();
                        $("#MANAChecker").empty();
                        $("#MANACheckDate").empty();
                        $("#MANSender").empty();
                        $("#MANSendDate").empty();

                        $("#MANSender").html("SENDER: " + request.Sender);
                        $("#MANSendDate").html(request.CreatedDate);
                        if (request.SChecker != "") {
                            $("#MANSChecker").html("STAFF " + request.SChecker + " " + GetState(request.SState));
                            $("#MANSCheckDate").html(request.SCheckDate);
                        }
                        if (request.AChecker != "") {
                            $("#MANAChecker").html(request.AChecker + " " + GetState(request.AState));
                            $("#MANACheckDate").html(request.ACheckDate);
                        }
                    });

                    $(".btnDPI").click(function () {
                        var request = $(this).data("content");

                        $("#DPIModal").modal('show');
                        $("#HfRequestID").val(request.Id);

                        var totalState = 3;
                        if (request.AState != 3) totalState = request.AState;
                        else totalState = request.SState;

                        if ($("#HfLoginRole").val() == "User" || ($("#HfLoginRole").val() == "Staff" && totalState != 3) || ($("#HfLoginRole").val() == "Admin" && request.AState != 3))
                            $("#BtnSaveDPI").addClass('d-none');
                        else $("#BtnSaveDPI").removeClass('d-none');

                        $("#ComboDPIState").val(totalState);
                        $("#ValSummaryDPI").addClass("d-none");
                        $("#DPICreatedDate").text(request.CreatedDate);

                        $("#DPISChecker").empty();
                        $("#DPISCheckDate").empty();
                        $("#DPIAChecker").empty();
                        $("#DPIACheckDate").empty();
                        $("#DPISender").empty();
                        $("#DPISendDate").empty();

                        $("#DPISender").html("SENDER: " + request.Sender);
                        $("#DPISendDate").html(request.CreatedDate);
                        if (request.SChecker != "") {
                            $("#DPISChecker").html("STAFF " + request.SChecker + " " + GetState(request.SState));
                            $("#DPISCheckDate").html(request.SCheckDate);
                        }
                        if (request.AChecker != "") {
                            $("#DPIAChecker").html(request.AChecker + " " + GetState(request.AState));
                            $("#DPIACheckDate").html(request.ACheckDate);
                        }

                        $("#BtnDPILoad").click();
                    });
                }
            }
        };
    </script>
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
        });

        $.datetimepicker.setLocale('it');

        $("#TxtDate").datetimepicker({
            format: "d/m/Y",
        }).on('change', function (e) {
            $(this).datetimepicker('hide');
            $("#BtnHiddenForDate").click();
            return false;
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
    <script>
        $(".btnMAL").click(function () {
            var request = $(this).data("content");

            $("#MalattiaModal").modal('show');
            $("#HfRequestID").val(request.Id);

            var totalState = 3;
            if (request.AState != 3) totalState = request.AState;
            else totalState = request.SState;

            if ($("#HfLoginRole").val() == "User" || ($("#HfLoginRole").val() == "Staff" && totalState != 3) || ($("#HfLoginRole").val() == "Admin" && request.AState != 3))
                $("#BtnSaveMAL").addClass('d-none');
            else $("#BtnSaveMAL").removeClass('d-none');

            $("#ComboMALState").val(totalState);
            $("#ValSummaryMAL").addClass("d-none");
            $("#TxtMALFrom").val(request.From);
            $("#TxtMALTo").val(request.To);
            $("#TxtMALDescription").val(request.Description);
            $("#MALCreatedDate").text(request.CreatedDate);
            $("#TxtMALO").val(request.O);
            $("#TxtMALS").val(request.S);
            $("#TxtMALA").val(request.A);


            $("#MALSChecker").empty();
            $("#MALSCheckDate").empty();
            $("#MALAChecker").empty();
            $("#MALACheckDate").empty();
            $("#MALSender").empty();
            $("#MALSendDate").empty();

            $("#MALSender").html("SENDER: " + request.Sender);
            $("#MALSendDate").html(request.CreatedDate);
            if (request.SChecker != "") {
                $("#MALSChecker").html("STAFF " + request.SChecker + " " + GetState(request.SState));
                $("#MALSCheckDate").html(request.SCheckDate);
            }
            if (request.AChecker != "") {
                $("#MALAChecker").html(request.AChecker + " " + GetState(request.AState));
                $("#MALACheckDate").html(request.ACheckDate);
            }
        });

        $(".btnFER").click(function () {
            var request = $(this).data("content");

            $("#FerieModal").modal('show');
            $("#HfRequestID").val(request.Id);

            var totalState = 3;
            if (request.AState != 3) totalState = request.AState;
            else totalState = request.SState;

            if ($("#HfLoginRole").val() == "User" || ($("#HfLoginRole").val() == "Staff" && totalState != 3) || ($("#HfLoginRole").val() == "Admin" && request.AState != 3))
                $("#BtnSaveFER").addClass('d-none');
            else $("#BtnSaveFER").removeClass('d-none');

            $("#ComboFERState").val(totalState);
            $("#ValSummaryFER").addClass("d-none");
            $("#TxtFERFrom").val(request.From);
            $("#TxtFERTo").val(request.To);
            $("#TxtFERDescription").val(request.Description);
            $("#FERCreatedDate").text(request.CreatedDate);
            $("#TxtFERO").val(request.O);
            $("#TxtFERS").val(request.S);
            $("#TxtFERA").val(request.A);


            $("#FERSChecker").empty();
            $("#FERSCheckDate").empty();
            $("#FERAChecker").empty();
            $("#FERACheckDate").empty();
            $("#FERSender").empty();
            $("#FERSendDate").empty();

            $("#FERSender").html ("SENDER: " + request.Sender);
            $("#FERSendDate").html(request.CreatedDate);
            if (request.SChecker != "") {
                $("#FERSChecker").html("STAFF " + request.SChecker + " " + GetState(request.SState));
                $("#FERSCheckDate").html(request.SCheckDate);
            }
            if (request.AChecker != "") {
                $("#FERAChecker").html(request.AChecker + " " + GetState(request.AState));
                $("#FERACheckDate").html(request.ACheckDate);
            }
        });

        $(".btnMAN").click(function () {
            var request = $(this).data("content");

            $("#ManutenzioneModal").modal('show');
            $("#HfRequestID").val(request.Id);

            var totalState = 3;
            if (request.AState != 3) totalState = request.AState;
            else totalState = request.SState;

            if ($("#HfLoginRole").val() == "User" || ($("#HfLoginRole").val() == "Staff" && totalState != 3) || ($("#HfLoginRole").val() == "Admin" && request.AState != 3))
                $("#BtnSaveMAN").addClass('d-none');
            else $("#BtnSaveMAN").removeClass('d-none');

            $("#ComboMANState").val(totalState);
            $("#ValSummaryMAN").addClass("d-none");
            $("#TxtMANDescription").val(request.Description);
            $("#MANCreatedDate").text(request.CreatedDate);

            $("#MANSChecker").empty();
            $("#MANSCheckDate").empty();
            $("#MANAChecker").empty();
            $("#MANACheckDate").empty();
            $("#MANSender").empty();
            $("#MANSendDate").empty();

            $("#MANSender").html("SENDER: " + request.Sender);
            $("#MANSendDate").html(request.CreatedDate);
            if (request.SChecker != "") {
                $("#MANSChecker").html("STAFF " + request.SChecker + " " + GetState(request.SState));
                $("#MANSCheckDate").html(request.SCheckDate);
            }
            if (request.AChecker != "") {
                $("#MANAChecker").html(request.AChecker + " " + GetState(request.AState));
                $("#MANACheckDate").html(request.ACheckDate);
            }
        });

        $(".btnDPI").click(function () {
            var request = $(this).data("content");

            $("#DPIModal").modal('show');
            $("#HfRequestID").val(request.Id);

            var totalState = 3;
            if (request.AState != 3) totalState = request.AState;
            else totalState = request.SState;

            if ($("#HfLoginRole").val() == "User" || ($("#HfLoginRole").val() == "Staff" && totalState != 3) || ($("#HfLoginRole").val() == "Admin" && request.AState != 3))
                $("#BtnSaveDPI").addClass('d-none');
            else $("#BtnSaveDPI").removeClass('d-none');

            $("#ComboDPIState").val(totalState);
            $("#ValSummaryDPI").addClass("d-none");
            $("#DPICreatedDate").text(request.CreatedDate);

            $("#DPISChecker").empty();
            $("#DPISCheckDate").empty();
            $("#DPIAChecker").empty();
            $("#DPIACheckDate").empty();
            $("#DPISender").empty();
            $("#DPISendDate").empty();

            $("#DPISender").html("SENDER: " + request.Sender);
            $("#DPISendDate").html(request.CreatedDate);
            if (request.SChecker != "") {
                $("#DPISChecker").html("STAFF " + request.SChecker + " " + GetState(request.SState));
                $("#DPISCheckDate").html(request.SCheckDate);
            }
            if (request.AChecker != "") {
                $("#DPIAChecker").html(request.AChecker + " " + GetState(request.AState));
                $("#DPIACheckDate").html(request.ACheckDate);
            }

            $("#BtnDPILoad").click();
        });

        function GetState(state) {
            if (state == 1) return '<span class="bg-success">ACCEPTED</span>';
            else if (state == 2) return '<span class="bg-danger">REJECTED</span>';
            else return '<span class="bg-info">PROGRESS</span>';
        }
    </script>
</asp:Content>
