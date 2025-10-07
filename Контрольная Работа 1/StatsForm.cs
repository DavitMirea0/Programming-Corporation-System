using CalendarApp.Models;
using CalendarApp.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CalendarApp
{
    public partial class StatsForm : Form
    {
        private readonly CalendarService _calendarService;

        public StatsForm(CalendarService calendarService)
        {
            _calendarService = calendarService;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "📊 Статистика событий";
            Size = new Size(500, 400);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Padding = new Padding(20);

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };

            var statsLabel = new Label
            {
                Text = "Статистика событий",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 130, 180),
                Size = new Size(400, 40),
                Location = new Point(0, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            mainPanel.Controls.Add(statsLabel);

            // Здесь можно добавить различную статистику
            var statsText = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Location = new Point(20, 60),
                Size = new Size(420, 250),
                Font = new Font("Consolas", 9),
                BackColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical
            };

            // Генерация статистики
            statsText.Text = GenerateStatistics();

            mainPanel.Controls.Add(statsText);
            Controls.Add(mainPanel);
        }

        private string GenerateStatistics()
        {
            var allEvents = _calendarService.GetAllEvents();
            var now = DateTime.Now;

            var totalEvents = allEvents.Count;
            var todayEvents = allEvents.Count(e => e.Date.Date == now.Date);
            var upcomingEvents = allEvents.Count(e => e.Date >= now.Date);

            // Статистика по категориям
            var categoryStats = allEvents
                .GroupBy(e => e.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var statsText = $"Статистика событий\n\n";
            statsText += $"• Всего событий: {totalEvents}\n";
            statsText += $"• Событий на сегодня: {todayEvents}\n";
            statsText += $"• Предстоящих событий: {upcomingEvents}\n\n";
            statsText += $"• События по категориям:\n";

            foreach (var stat in categoryStats)
            {
                var categoryName = GetCategoryDisplayName(stat.Category);
                statsText += $"  - {categoryName}: {stat.Count}\n";
            }

            // Статистика по напоминаниям
            var reminderStats = allEvents
                .GroupBy(e => e.Reminder)
                .Select(g => new { Reminder = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            statsText += $"\n• Напоминания:\n";
            foreach (var stat in reminderStats)
            {
                var reminderName = GetReminderDisplayName(stat.Reminder);
                statsText += $"  - {reminderName}: {stat.Count}\n";
            }

            return statsText;
        }

        private string GetCategoryDisplayName(EventCategory category)
        {
            return category switch
            {
                EventCategory.Work => "💼 Работа",
                EventCategory.Personal => "👤 Личное",
                EventCategory.Holiday => "🎉 Праздник",
                EventCategory.Birthday => "🎂 День рождения",
                EventCategory.Meeting => "🤝 Встреча",
                EventCategory.Appointment => "📅 Встреча",
                EventCategory.Other => "📌 Другое",
                _ => category.ToString()
            };
        }

        private string GetReminderDisplayName(ReminderType reminder)
        {
            return reminder switch
            {
                ReminderType.None => "Без напоминания",
                ReminderType.FiveMinutes => "За 5 минут",
                ReminderType.FifteenMinutes => "За 15 минут",
                ReminderType.ThirtyMinutes => "За 30 минут",
                ReminderType.OneHour => "За 1 час",
                ReminderType.OneDay => "За 1 день",
                _ => reminder.ToString()
            };
        }
    }
}