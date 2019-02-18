using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace ExcelScheduleGenerator
{
	using WorkTimeCalculator;

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
				var tat = new TatarstanWorkDayCalendar(TimeSpan.FromHours(10),
					TimeSpan.FromHours(18));
				tat.EnsureInitializedAsync().Wait();
				var workTimeCalculator =
					new WorkTimeCalculator(tat);
				Worksheet scheduleSheet = scheduleWorkbook.ActiveSheet;
				PrintDateRow(scheduleSheet, startDate, endDate);
				var schedule = workTimeCalculator.GetSchedule(startDate, new WorkTime(20, 0, 0));
				PrintSchedule(scheduleSheet, 2, schedule);
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

		static void PrintSchedule(Worksheet sheet, int row, IEnumerable<TimePeriod> schedule)
		{
			var index = 1;
			foreach (var timePeriod in schedule)
			{
				Range cell = sheet.Cells[row, index];
				if (timePeriod.Length > TimeSpan.Zero)
				{
					cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
				}
				index++;
			}
		}
	}
}
