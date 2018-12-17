using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace WorkTimeCalculator
{
	public class TatarstanWorkDayCalendar : IWorkDayCalendar
	{
		public TatarstanWorkDayCalendar(TimeSpan workDayStart, TimeSpan workDayEnd)
		{
			WorkDayStart = workDayStart;
			WorkDayEnd = workDayEnd;
		}

		public TimeSpan WorkDayStart { get; }
		public TimeSpan WorkDayEnd { get; }

		private struct Month
		{
			public string Name;
			public int[] Holidays;
			public int[] BeforeHolidays;
		}

		public readonly static string[] MonthNames = new[]
		{
			"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь",
			"Декабрь"
		};

		private readonly Dictionary<int, Month[]> _monthsByYear = new Dictionary<int, Month[]>();
		private bool _isInitialied;

		public async Task InitializeAsync(int year)
		{
			var httpClient = new HttpClient();
			HtmlDocument document = new HtmlDocument();
			var response = await httpClient.GetAsync($"https://assistentus.ru/proizvodstvennyj-kalendar-{year}/tatarstan/");
			var content = await response.Content.ReadAsStringAsync();
			document.LoadHtml(content);
			var calendarNode = document.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[1]/article/div/div");

			var monthTables = calendarNode.SelectNodes("table")
				.Where(e => new[] {"month", "month m3"}.Contains(e.GetAttributeValue("class", null))).ToArray();

			var months = monthTables.Select(m =>
			{
				var cells = m.Descendants("td").ToArray();
				var monthNameCell = cells.FirstOrDefault(e => e.GetAttributeValue("class", null) == "tbbg");
				var holidayCells = cells.Where(e =>
					new[] { "holiday", "holiday tb" }.Contains(e.GetAttributeValue("class", null)));
				var beforeHolidayCells = cells.Where(e =>
					new[] { "before_holiday" }.Contains(e.GetAttributeValue("class", null)));
				return new Month
				{
					Name = monthNameCell.InnerText,
					Holidays = holidayCells.Select(e => e.InnerText).Select(int.Parse).ToArray(),
					BeforeHolidays = beforeHolidayCells.Select(e => e.InnerText.Replace("*", "")).Select(int.Parse).ToArray()
				};
			}).ToArray();
			_monthsByYear.Add(year, months);
		}

		public async Task EnsureInitializedAsync()
		{
			if (!_isInitialied)
			{
				await InitializeAsync(2018);
				await InitializeAsync(2019);
				_isInitialied = true;
			}
		}

		public TimePeriod GetWorkDay(DateTime date)
		{
			var year = date.Year;
			var months = _monthsByYear[year];
			var monthName = MonthNames[date.Month - 1];
			var month = months.FirstOrDefault(m => m.Name == monthName);
			if (month.BeforeHolidays.Contains(date.Day))
			{
				return new TimePeriod {Start = date.Date.Add(WorkDayStart), End = date.Date.Add(WorkDayEnd).AddHours(-1)};
			}

			if (month.Holidays.Contains(date.Day))
			{
				return new TimePeriod {Start = date.Date.Add(WorkDayStart), End = date.Date.Add(WorkDayStart)};
			}

			return new TimePeriod { Start = date.Date.Add(WorkDayStart), End = date.Date.Add(WorkDayEnd) };
		}
	}
}