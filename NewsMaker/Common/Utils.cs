using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
namespace NewsMaker
{
    public static class Utils
    {
        //public static void SetupLabels(BaseItemCollection group)
        //{
        //    foreach (var item in group)
        //    {
        //        var layout = item as LayoutControlItem;
        //        if (layout != null)
        //        {
        //            layout.Text = layout.Text + ":";
        //            switch (layout.TextLocation)
        //            {
        //                case Locations.Top:
        //                case Locations.Bottom:
        //                    continue;
        //                default:
        //                    break;
        //            }
        //            layout.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
        //            continue;
        //        }
        //        var layoutGroup = item as LayoutGroup;
        //        if (layoutGroup != null)
        //        {
        //            SetupLabels(layoutGroup.Items);
        //            continue;
        //        }
        //        var tabbed = item as TabbedControlGroup;
        //        if (tabbed != null) SetupLabels(tabbed.TabPages);
        //    }
        //}
        //public static string CalculateProcessing(int processed, int total)
        //{
        //    return string.Format("{0} из {1}", processed, total);
        //}
        //public static string CalculateRemaining(DateTime processStarted, int totalElements, int processedElements)
        //{
        //    var secondsRemaining = 0;
        //    var totalSecond = (int) (DateTime.Now - processStarted).TotalSeconds;
        //    if (totalSecond > 0)
        //    {
        //        var itemsPerSecond = processedElements / totalSecond;
        //        if (itemsPerSecond > 0) secondsRemaining = (totalElements - processedElements) / itemsPerSecond;
        //    }
        //    return new TimeSpan(0, 0, secondsRemaining).ToString(@"hh\:mm\:ss");
        //}
        //public static string CalculateDuration(DateTime processStarted)
        //{
        //    return TimeSpan.FromTicks(DateTime.Now.Subtract(processStarted).Ticks).ToString(@"hh\:mm\:ss");
        //}
        //public static bool EmailIsValid(string email)
        //{
        //    var pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
        //    var isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
        //    if (isMatch.Success)
        //        return true;
        //    return false;
        //}
        //[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        //public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //[DllImport("USER32.DLL")]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);
        //public static async void WaitAndKeysendWindowAsinc(string win_title, string sent_text)
        //{
        //    var wait_step = 0;
        //    while (wait_step < 1000)
        //    {
        //        await Task.Delay(50);
        //        var DialogHandle = FindWindow(null, win_title);
        //        if (DialogHandle != IntPtr.Zero)
        //        {
        //            SetForegroundWindow(DialogHandle);
        //            if (sent_text == "")
        //            {
        //                SendKeys.Send("{ENTER}");
        //            }
        //            else
        //            {
        //                await Task.Delay(100);
        //                SendKeys.Send(sent_text + "{ENTER}");
        //            }
        //            break;
        //        }
        //        wait_step++;
        //    }
        //}
        //public static void SaveTextToFile(string text, string filePath, bool openDir = false)
        //{
        //    var dir = Application.StartupPath + @"\output\";
        //    filePath = dir + Path.GetFileNameWithoutExtension(filePath) + ".txt";
        //    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        //    File.WriteAllText(filePath, text, Encoding.GetEncoding(1251));
        //    if (!File.Exists(filePath) || !openDir) return;
        //    var PrFolder = new Process();
        //    var psi = new ProcessStartInfo();
        //    psi.CreateNoWindow = false;
        //    psi.WindowStyle = ProcessWindowStyle.Normal;
        //    psi.FileName = "explorer";
        //    psi.Arguments = @"/n, /select, """ + filePath + @"""";
        //    PrFolder.StartInfo = psi;
        //    PrFolder.Start();
        //}
    }
    //public static class ExcelExporter
    //{
    //    private const string ExcelOleDbConnectionStringTemplate =
    //        "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES\";";
    //    /// <summary>
    //    ///     Creates the Excel file from items in DataTable and writes them to specified output file.
    //    /// </summary>
    //    public static void CreateXlsFromDataTable(DataTable dataTable, string fullFilePath)
    //    {
    //        var createTableWithHeaderScript = GenerateCreateTableCommand(dataTable);
    //        using (var conn = new OleDbConnection(string.Format(ExcelOleDbConnectionStringTemplate, fullFilePath)))
    //        {
    //            if (conn.State != ConnectionState.Open) conn.Open();
    //            var cmd = new OleDbCommand(createTableWithHeaderScript, conn);
    //            cmd.ExecuteNonQuery();
    //            foreach (DataRow dataExportRow in dataTable.Rows) AddNewRow(conn, dataExportRow);
    //        }
    //    }
    //    private static void AddNewRow(OleDbConnection conn, DataRow dataRow)
    //    {
    //        var insertCmd = GenerateInsertRowCommand(dataRow);
    //        using (var cmd = new OleDbCommand(insertCmd, conn))
    //        {
    //            AddParametersWithValue(cmd, dataRow);
    //            cmd.ExecuteNonQuery();
    //        }
    //    }
    //    /// <summary>
    //    ///     Generates the insert row command.
    //    /// </summary>
    //    private static string GenerateInsertRowCommand(DataRow dataRow)
    //    {
    //        var stringBuilder = new StringBuilder();
    //        var columns = dataRow.Table.Columns.Cast<DataColumn>().ToList();
    //        var columnNamesCommaSeparated = string.Join(",", columns.Select(x => x.Caption));
    //        var questionmarkCommaSeparated = string.Join(",", columns.Select(x => "?"));
    //        stringBuilder.AppendFormat("INSERT INTO [{0}] (", dataRow.Table.TableName);
    //        stringBuilder.Append(columnNamesCommaSeparated);
    //        stringBuilder.Append(") VALUES(");
    //        stringBuilder.Append(questionmarkCommaSeparated);
    //        stringBuilder.Append(")");
    //        return stringBuilder.ToString();
    //    }
    //    /// <summary>
    //    ///     Adds the parameters with value.
    //    /// </summary>
    //    private static void AddParametersWithValue(OleDbCommand cmd, DataRow dataRow)
    //    {
    //        var paramNumber = 1;
    //        for (var i = 0; i <= dataRow.Table.Columns.Count - 1; i++)
    //        {
    //            if (!ReferenceEquals(dataRow.Table.Columns[i].DataType, typeof(int)) &&
    //                !ReferenceEquals(dataRow.Table.Columns[i].DataType, typeof(decimal)))
    //            {
    //                cmd.Parameters.AddWithValue("@p" + paramNumber, dataRow[i].ToString().Replace("'", "''"));
    //            }
    //            else
    //            {
    //                var value = GetParameterValue(dataRow[i]);
    //                var parameter = cmd.Parameters.AddWithValue("@p" + paramNumber, value);
    //                if (value is decimal) parameter.OleDbType = OleDbType.Currency;
    //            }
    //            paramNumber = paramNumber + 1;
    //        }
    //    }
    //    /// <summary>
    //    ///     Gets the formatted value for the OleDbParameter.
    //    /// </summary>
    //    private static object GetParameterValue(object value)
    //    {
    //        if (value is string) return value.ToString().Replace("'", "''");
    //        return value;
    //    }
    //    private static string GenerateCreateTableCommand(DataTable tableDefination)
    //    {
    //        var stringBuilder = new StringBuilder();
    //        var firstcol = true;
    //        stringBuilder.AppendFormat("CREATE TABLE [{0}] (", tableDefination.TableName);
    //        foreach (DataColumn tableColumn in tableDefination.Columns)
    //        {
    //            if (!firstcol) stringBuilder.Append(", ");
    //            firstcol = false;
    //            var columnDataType = "CHAR(255)";
    //            switch (tableColumn.DataType.Name)
    //            {
    //                case "String":
    //                    columnDataType = "CHAR(255)";
    //                    break;
    //                case "Int32":
    //                    columnDataType = "INTEGER";
    //                    break;
    //                case "Decimal":
    //                    // Use currency instead of decimal because of bug described at 
    //                    // http://social.msdn.microsoft.com/Forums/vstudio/en-US/5d6248a5-ef00-4f46-be9d-853207656bcc/localization-trouble-with-oledbparameter-and-decimal?forum=csharpgeneral
    //                    columnDataType = "CURRENCY";
    //                    break;
    //            }
    //            stringBuilder.AppendFormat("{0} {1}", tableColumn.ColumnName, columnDataType);
    //        }
    //        stringBuilder.Append(")");
    //        return stringBuilder.ToString();
    //    }
    //}
}
