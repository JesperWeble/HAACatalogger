using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OfficeOpenXml;

namespace HAACatalogger
{
    public class WriteToExcel
    {
        public void ImportToExcel(List<Card> cards)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Describes that this is a non-commercal use of the EPPlus license - this is required for the program to function.


            //string currentFilepath = Path.GetFullPath(typeof(WriteToExcel).Assembly.Location); //Finds the current filepath of this file
            //currentFilepath = currentFilepath.Replace("\\bin\\Debug\\net8.0\\HAACatalogger.dll", ""); // Ensures that it finds the correct filepath even when this is debugging through visual studio.

            //using (var package = new ExcelPackage(new FileInfo($"{currentFilepath}/Excel/Test.xlsx"))) // "using" to ensure that it is properly cleaned up after it is done.


            foreach (Card card in cards)
            {
                using (var package = new ExcelPackage(new FileInfo("../../Excel/HAA2/Test.xlsx"))) // "using" to ensure that it is properly cleaned up after it is done.
                {
                    var workbook = package.Workbook;
                    workbook.Worksheets["Test"].Cells["A1"].Value = card.name;

                    package.Save();


                }
            }
            
        }
    }
}
