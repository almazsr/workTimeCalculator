using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace ExcelScheduleGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			Application application = new Application();
			application.DisplayAlerts = false;
			Workbook scheduleWorkbook = null;
			var startDate = DateTime.Now.Date;
			var endDate = startDate.AddMonths(1);
			try
			{
				scheduleWorkbook = application.Workbooks.Add();
				Worksheet scheduleSheet = scheduleWorkbook.ActiveSheet;
				PrintDateRow(scheduleSheet, startDate, endDate);
				scheduleWorkbook.SaveAs(@"\C:\Users\almaz\OneDrive\Документы\test.xlsx");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			finally
			{
				scheduleWorkbook?.Close(true);
				application.Quit();
				Marshal.ReleaseComObject(application);
			}
		}

		static void PrintDateRow(Worksheet sheet, DateTime startDate, DateTime endDate)
		{
			var date = startDate;
			var index = 1;
			while (date <= endDate)
			{
				Range cell = sheet.Cells[1, index];
				cell.Value = date.ToShortDateString();
				index++;
				date = date.AddDays(1);
			}
		}
	}
}
