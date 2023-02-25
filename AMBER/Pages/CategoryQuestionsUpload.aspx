﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/AmberMP.Master" AutoEventWireup="true" CodeBehind="CategoryQuestionsUpload.aspx.cs" Inherits="AMBER.Pages.CategoryQuestionsUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="container p-4 p-md-5 pt-5">
        <div class="row">
            <div class="col">
                <div class="card shadow-lg" style="background-image: url('../../Pictures/former.png'); background-size: cover; background-repeat: no-repeat;">
                    <div class="card-body">
                        <center>
                            <h1 style="color: darkslategray">
                                <i class="fa-solid fa-file"></i> Category & Questions Upload</h1>
                        </center>
                    </div>
                </div>
            </div>
        </div>
        <br /><br />
        <div class="row my-3">
            <div class="col-7 mx-auto shadow-lg">
                <br />
                <center>
                    <label for="body_FileUpload2"><i class="fa-solid fa-file-csv text-warning" style="font-size: 250px; cursor: pointer;"></i></label>
                    <asp:FileUpload ID="FileUpload2" onchange="getInfo(this.value);" Style="display: none; visibility: hidden;" accept=".csv" CssClass="form-control" runat="server" />
                    <div id="displayInformation" class="p-2">Browse CSV file</div>
                    <asp:Button ID="btnconsupload" OnClick="btnconsupload_Click" CssClass="btn btn-secondary" runat="server" Text="Upload CSV File" />
                </center>
                <br />
            </div>
                        <div class="col-5">
                <div class="card mb-2 border-0 bg-transparent">
                    <div class="card-body">
                        <div class="row">
                            <div class="col">
                                <center>
                                    <h5>Click to Download Sample Template File for Categories & Questions Upload</h5>
                                </center>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <p class="text-justify">
                                    <b>Note</b>:  Use the sample template file for your reference on how to insert necessary information for the uploading of Categories and Questions for the Evaluation. And Please don't forget to save the file as <b>.csv</b> format after inserting all of the information.
                                </p>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <asp:GridView ID="GridView1" CssClass="table table-bordered table-striped" ShowHeader="false" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" OnClick="lnkDownload_Click" runat="server" Text='<%# Bind("file_name") %>'
                                                    CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function getInfo(filename) {
            var info = filename.replace(/^.*\\/, "");
            $('#displayInformation').html(info);
            document.getElementById("body_btnconsupload").className = 'btn btn-success';
        }
    </script>
</asp:Content>
