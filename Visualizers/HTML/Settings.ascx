<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Modules.Reports.Visualizers.Html.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/urlcontrol.ascx" %>
<table id="tblSettings" runat="server" cellspacing="0" cellpadding="2" border="0"
       style="width: 100%">
    <tr>
        <td>
            <div class="Subhead">
                <dnn:Label ID="lblTemplateFile" runat="server" ControlName="ctlTemplate" Suffix=":"/>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <dnn:Url ID="ctlTemplate" runat="server" Width="400px" ShowUrls="False" ShowTabs="False"
                     ShowLog="False" ShowTrack="False" Required="True" FileFilter="htm,html">
            </dnn:Url>
        </td>
    </tr>
</table>