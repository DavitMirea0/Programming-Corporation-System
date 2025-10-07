using CalendarApp.Models;
using CalendarApp.Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CalendarApp;

public partial class WeekViewForm : Form
{
    private readonly DateTime _currentWeekStart;
    private readonly CalendarService _calendarService;

    public WeekViewForm(DateTime date, CalendarService calendarService)
    {
        _calendarService = calendarService;

        // Находим начало недели (понедельник)
        int daysFromMonday = ((int)date.DayOfWeek + 6) % 7;
        _currentWeekStart = date.AddDays(-daysFromMonday).Date;

        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = $"Неделя с {_currentWeekStart:dd.MM.yyyy} по {_currentWeekStart.AddDays(6):dd.MM.yyyy}";
        Size = new Size(1000, 700);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.White;
        Padding = new Padding(10);

        var mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };

        var weekPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 7,
            RowCount = 2,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            BackColor = Color.White
        };

        // Настройка размеров
        for (int i = 0; i < 7; i++)
        {
            weekPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28f));
        }
        weekPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
        weekPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        // Заголовки дней недели
        string[] dayNames = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };

        var weekEvents = _calendarService.GetEventsForWeek(_currentWeekStart);

        for (int i = 0; i < 7; i++)
        {
            var dayDate = _currentWeekStart.AddDays(i);
            var isToday = dayDate.Date == DateTime.Today;
            var isWeekend = i >= 5;

            var dayLabel = new Label
            {
                Text = $"{dayNames[i]}\n{dayDate:dd.MM.yyyy}",
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = isToday ? Color.Gold : (isWeekend ? Color.LightPink : Color.LightGray),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
            weekPanel.Controls.Add(dayLabel, i, 0);

            // События дня
            var eventsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                AutoScroll = true,
                Padding = new Padding(5)
            };

            var eventsListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.None,
                DrawMode = DrawMode.OwnerDrawVariable
            };

            eventsListBox.DrawItem += (s, e) =>
            {
                e.DrawBackground();
                if (e.Index >= 0 && e.Index < eventsListBox.Items.Count)
                {
                    var itemText = eventsListBox.Items[e.Index]?.ToString() ?? "";
                    using var brush = new SolidBrush(e.ForeColor);
                    e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds);
                }
            };

            eventsListBox.MeasureItem += (s, e) =>
            {
                e.ItemHeight = 60;
            };

            try
            {
                if (weekEvents.TryGetValue(dayDate.Date, out var dayEvents) && dayEvents != null && dayEvents.Count > 0)
                {
                    foreach (var evt in dayEvents.OrderBy(e => e.StartTime))
                    {
                        if (evt == null) continue;

                        var time = evt.IsAllDay ? "Весь день" : $"{evt.StartTime:HH:mm}";
                        var description = string.IsNullOrEmpty(evt.Description) ? "" : $"\n{evt.Description}";
                        var eventText = $"{GetCategoryIcon(evt.Category)} {evt.Title}\n⏰ {time}{description}";
                        eventsListBox.Items.Add(eventText);
                    }
                }
                else
                {
                    eventsListBox.Items.Add("Нет событий");
                    eventsListBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                eventsListBox.Items.Add("Ошибка загрузки событий");
                eventsListBox.Enabled = false;
            }

            eventsPanel.Controls.Add(eventsListBox);
            weekPanel.Controls.Add(eventsPanel, i, 1);
        }

        mainPanel.Controls.Add(weekPanel);
        Controls.Add(mainPanel);
    }

    private static string GetCategoryIcon(EventCategory category) => category switch
    {
        EventCategory.Work => "💼",
        EventCategory.Personal => "👤",
        EventCategory.Holiday => "🎉",
        EventCategory.Birthday => "🎂",
        EventCategory.Meeting => "🤝",
        EventCategory.Appointment => "📅",
        EventCategory.Other => "📌",
        _ => "📌"
    };
}