﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="EvaluationSectionResult.aspx.cs" Inherits="AMBER.Pages.EvaluationResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row mb-3">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray"><i class="fa-solid fa-clipboard-user"></i>
                                Student Evaluation Report</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <%--<asp:PlaceHolder ID="plcData" runat="server">--%>
        <div class="row">
            <div class="col">
                <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" EnablePartialRendering="true" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="row">
                                <div class="col">
                                    <asp:DropDownList ID="ddlDepartment" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col">
                                    <asp:DropDownList ID="ddlSection" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" runat="server"></asp:DropDownList>
                                </div>
                                <div class="col">
                                    <asp:DropDownList ID="ddlStatus" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select" runat="server">
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                        <asp:ListItem Selected="True">-Evaluated?-</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:PlaceHolder ID="plcData" runat="server">
                                <asp:GridView ID="GridView1" CssClass="table table-bordered" BorderColor="Transparent" runat="server" AutoGenerateColumns="False" DataKeyNames="section_id" OnRowDataBound="GridView1_RowDataBound" ShowHeader="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SECTION" SortExpression="section_name">
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" Text='<%# Bind("section_name") %>' ID="TextBox1"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("section_name") %>' ID="Label2" Font-Size="X-Large" Font-Bold="true" Style="text-align: center;"></asp:Label>
                                                <asp:GridView ID="GridviewSchedule" GridLines="Horizontal" CssClass="table table-hover table-bordered" AutoGenerateColumns="false" runat="server" DataKeyNames="sched_Id" OnRowCommand="GridviewSchedule_RowCommand">
                                                    <HeaderStyle BackColor="Orange" BorderColor="Transparent" />
                                                    <Columns>
                                                        <asp:BoundField DataField="INSTRUCTOR" HeaderText="INSTRUCTOR" SortExpression="INSTRUCTOR"></asp:BoundField>
                                                        <asp:BoundField DataField="SUBJECT" HeaderText="SUBJECT" ItemStyle-Width="40%" SortExpression="SUBJECT"></asp:BoundField>
                                                        <asp:BoundField DataField="time" HeaderText="time" SortExpression="time"></asp:BoundField>
                                                        <asp:BoundField DataField="day" HeaderText="day" SortExpression="day"></asp:BoundField>
                                                        <asp:BoundField DataField="isEvaluated" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Evaluated?" SortExpression="isEvaluated"></asp:BoundField>
                                                        <asp:BoundField DataField="COUNT" HeaderText="Total Evaluator" SortExpression="COUNT"></asp:BoundField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnRESULT" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" CommandName="selectSched" runat="server" Text="VIEW RESULTS" />
                                                            </ItemTemplate>

                                                            <ControlStyle CssClass="btn btn-outline-primary"></ControlStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="plcNoData" runat="server">
                                <div class="row mt-5">
                                    <div class="col">
                                        <div class="card shadow">
                                            <div class="card-body">
                                                <center>
                                                    <p>
                                                        <img src="../../Pictures/nonodata.png" style="height: 350px; width: 350px;" />
                                                    </p>
                                                    <h1>No Record Found</h1>
                                                </center>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>
    <script type="text/javascript">
        function customOpen(url) {
            var w = window.open(url, '', 'width=1000,height=600,toolbar=0,status=0,location=0,menubar=0,directories=0,resizable=1,scrollbars=1');
            w.focus();

        }
    </script>
</asp:Content>