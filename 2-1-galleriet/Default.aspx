<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_2_1_galleriet.Default"
    ViewStateMode="Disabled" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Galleriet</title>
    <%: Styles.Render("~/Content") %>
    <%: Scripts.Render("~/bundles/modernizr") %>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Galleriet</h1>
            <%--Rättmeddelande--%>
            <asp:Panel runat="server" ID="SuccessMessagePanel" Visible="false" CssClass="icon-ok">
                <asp:Literal runat="server" ID="SuccessMessageLiteral" />
            </asp:Panel>
            <%--Felmeddelanden--%>
            <asp:ValidationSummary runat="server" CssClass="validation-summary-errors"
                HeaderText="Ett fel inträffade. Korrigera felet och gör ett nytt försök." />
            <%--Image-kontroll som visar den stora bilden--%>
            <asp:Panel runat="server" ID="BigImagePanel" Visible="false">
                <asp:Image ID="BigImage" runat="server" Width="518px" Height="300px" />
            </asp:Panel>
            <%--Repeater som visar tumnaglarna--%>
            <asp:Repeater ID="ThumbRepeater" runat="server" ItemType="_2_1_galleriet.Model.ThumbPicture"
                SelectMethod="ThumbRepeater_GetData" OnItemDataBound="ThumbRepeater_ItemDataBound">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <%-- http://www.aspsnippets.com/Green/Articles/Bind-Single-or-Multiple-QueryString-Parameters-to-NavigateUrl-of-HyperLink-using-Eval-function-inside-ASPNet-GridView.aspx --%>
                    <asp:HyperLink ID="ThumbHyperLink" runat="server"
                        ImageUrl='<%# Eval("Name", "~/Images/Thumbnails/{0}") %>'
                        NavigateUrl='<%# Eval("Name", "~/Default.aspx?name={0}") %>'></asp:HyperLink>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
            </asp:Repeater>
            <%--FileUpload--%>
            <div id="fileupload">
                <asp:FileUpload ID="FileUploader" runat="server" />
                <asp:Button ID="UploadButton" runat="server" Text="Ladda upp" OnClick="UploadButton_Click" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="En fil måste väljas."
                    Display="Dynamic" ControlToValidate="FileUploader" CssClass="field-validation-error">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Endast filer av typerna gif, jpeg eller png är tillåtna."
                    ControlToValidate="FileUploader" Display="Dynamic" ValidationExpression=".*\.(gif|jpg|png)"
                    CssClass="field-validation-error">*</asp:RegularExpressionValidator>
            </div>
        </div>
    </form>
    <%: Scripts.Render("~/bundles/app") %>
</body>
</html>
