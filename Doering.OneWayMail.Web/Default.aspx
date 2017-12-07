<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Doering.OneWayMail.Web._Default" %>
<%@ Import Namespace="Doering.OneWayMail.Web" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>
            OneWayMail
            <i class="fa fa-paper-plane-o" aria-hidden="true"></i>
        </h1>
        <p class="lead">Temporäre E-Mail-Adressen erstellen mit Weiterleitung an meine eigene E-Mail-Adresse.</p>
    </div>
    
    <div class="row" style="max-width: 600px">
        <div class="col-md-12">
            <div class="form">
                <h3>Alle E-Mails an</h3>
                <div class="form-group">
                    <div class="input-group">
                        <asp:TextBox runat="server" ID="txtEmail"
                            CssClass="form-control" placeholder="jane.doe"></asp:TextBox>
                        <div class="input-group-addon">@<%= Settings.MailDomain %></div>
                    </div>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                        SetFocusOnError="True" ValidationGroup="SubscriptionForm" />
                </div>
                <h3>weiterleiten an</h3>
                <div class="form-group">
                    <asp:TextBox runat="server" ID="txtForwardTo" TextMode="Email"
                        CssClass="form-control" placeholder="john.doe@example.com"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtForwardTo"
                        SetFocusOnError="True" ValidationGroup="SubscriptionForm" />
                </div>
                <asp:Button runat="server" ID="btnAdd" CssClass="btn btn-lg btn-primary"
                    Text="Weiterleitung einrichten" style="margin: 15px 0 5px 0"
                    OnClick="btnAdd_OnClick" ValidationGroup="SubscriptionForm" />
                <p>Sie haben später die Möglichkeit die Weiterleitung wieder abzubestellen.</p>
            </div>
        </div>
    </div>
    
    <div class="alert alert-success" runat="server" ID="alertSuccess" Visible="False">
        <i class="fa fa-check" aria-hidden="true"></i>
        <asp:Literal runat="server" ID="txtAlertSuccess" />
    </div>
    
    <div class="alert alert-danger" runat="server" ID="alertError" Visible="False">
        <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>
        <asp:Literal runat="server" ID="txtAlertError" />
    </div>
     
    <div class="row" runat="server" ID="panelSubscriptions">
        <hr/>
        <div class="col-md-12">
            <h2>Meine Weiterleitungen</h2>
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>E-Mail-Adresse</th>
                        <th>Weiterleitung an</th>
                        <th>Gültig bis</th>
                        <th>Empfangene E-Mails</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="repeaterSubscriptions" ItemType="Doering.OneWayMail.Model.Subscription">
                        <ItemTemplate>
                            <tr>
                                <td><%# Item.EmailAddress %></td>
                                <td><%# Item.ForwardTo %></td>
                                <td><%# Item.ValidUntil.ToString("g") %> Uhr</td>
                                <td><%# Item.Usages.Count %></td>
                                <td>
                                    <a href="Remove.aspx?id=<%# Item.Id %>" class="btn btn-primary btn-xs">
                                        <i class="fa fa-times" aria-hidden="true"></i>
                                        Weiterleitung entfernen
                                    </a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>

</asp:Content>
