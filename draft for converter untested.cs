using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

        }
        
           static void CovertExcelToCsv(string excelFilePath, string csvOutputFile, int worksheetNumber = 1)
        {
           
            // connection string
            var cnnStr = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilePath + ";Extended Properties=Excel 12.0;");
            var cnn = new OleDbConnection(cnnStr);

            // get schema, then data
            var dt = new DataTable();
            try
            {
                cnn.Open();
                var schemaTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTable.Rows.Count < worksheetNumber) throw new ArgumentException("The worksheet number provided cannot be found in the spreadsheet");
                string worksheet = schemaTable.Rows[worksheetNumber - 1]["table_name"].ToString().Replace("'", "");
                string sql = String.Format("select * from [{0}]", worksheet);
                var da = new OleDbDataAdapter(sql, cnn);
                da.Fill(dt);
            }
            catch (Exception e)
            {
                // ???
                throw e;
            }
            finally
            {
                // free resources
                cnn.Close();
            }

            // write out CSV data
            using (var wtr = new StreamWriter(csvOutputFile))
            { 
                foreach (DataColumn col in dt.Columns)
                      {
                        wtr.Write(col.ColumnName);
                      wtr.Write(";");
                        }
                    wtr.WriteLine("");

                foreach (DataRow row in dt.Rows)
                {
                    bool firstLine = true;

                    foreach (DataColumn col in dt.Columns)
                    {
                        if (!firstLine) 
                        { 
                            wtr.Write(";"); 
                         } 
                        
                        else { firstLine = false; }
                        var data = row[col.ColumnName].ToString().Replace("\"", "\"\"");
                        wtr.Write(String.Format("\"{0}\"", data));
                    }
                    wtr.WriteLine();
                }
            }
        }


    

        private void button2_Click(object sender, EventArgs e)
        {
            //здесь запускается скрипт на R через System
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //здесь запускается скрипт на R через System
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sourceFile;
            int worksheetName;
            string targetFile;
            sourceFile = "C:\\J\\Learnings\\Test.xlsx";
            //sourceFile = "Test.xlsx";
            worksheetName = 1;
            targetFile = "C:\\J\\Learnings\\Target.csv";
            //targetFile = "Target.csv";
            //CovertExcelToCsv(sourceFile, worksheetName, targetFile);
            CovertExcelToCsv(sourceFile, targetFile, worksheetName);
        }
    }
}
