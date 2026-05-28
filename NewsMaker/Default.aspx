using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Spreadsheet;
using System.IO;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }

    protected void ASPxButton1_Click(object sender, EventArgs e) {
        Workbook wb = new Workbook();
        wb.Worksheets[0]["A1"].Value = "HELLO WORLD";
        MemoryStream memoryStream = new MemoryStream();
        wb.SaveDocument(memoryStream, DocumentFormat.OpenXml);
        ExportStreamToResponse(memoryStream, "test", "xlsx", false);
    }

    public void ExportStreamToResponse(MemoryStream memoryStream, string fileName, string fileType, bool inline) {
        Response.Clear();
        Response.ContentType = "application/" + fileType;
        Response.AddHeader("Content-Disposition", string.Format("{0}; filename={1}.{2}", inline ? "Inline" : "Attachment", fileName, fileType));
        Response.AddHeader("Content-Length", memoryStream.Length.ToString());
        Response.BinaryWrite(memoryStream.ToArray());
        Response.Flush();
        Response.Close();
        Response.End();
    }
}