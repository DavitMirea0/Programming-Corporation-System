using CalendarApp.Models;
using CalendarApp.Services;
using System;
using System.Linq;

namespace CalendarApp.Services
{
    public class ReminderService
    {
        private readonly CalendarService _calendarService;
        private readonly System.Timers.Timer _timer;

        public ReminderService(CalendarService calendarService)
        {
            _calendarService = calendarService;
            _timer = new System.Timers.Timer(30000); // Проверка каждые 30 секунд
            _timer.Elapsed += CheckReminders;
            _timer.AutoReset = true;
            _timer.Start();
        }

        private void CheckReminders(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var now = DateTime.Now;
                var upcomingEvents = _calendarService.GetUpcomingEvents(now, TimeSpan.FromMinutes(30));

                foreach (var evt in upcomingEvents)
                {
                    if (ShouldShowReminder(evt, now))
                    {
                        ShowReminder(evt);
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку, но не прерываем работу таймера
                System.Diagnostics.Debug.WriteLine($"Ошибка в ReminderService: {ex.Message}");
            }
        }

        private bool ShouldShowReminder(CalendarEvent evt, DateTime now)
        {
            if (evt.Reminder == ReminderType.None)
                return false;

            var reminderTime = GetReminderTime(evt);
            return now >= reminderTime && now < reminderTime.AddMinutes(1);
        }

        private DateTime GetReminderTime(CalendarEvent evt)
        {
            var eventTime = evt.IsAllDay ? evt.Date.Date.AddHours(9) : evt.StartTime; // Для целодневных событий - 9:00

            return evt.Reminder switch
            {
                ReminderType.FiveMinutes => eventTime.AddMinutes(-5),
                ReminderType.FifteenMinutes => eventTime.AddMinutes(-15),
                ReminderType.ThirtyMinutes => eventTime.AddMinutes(-30),
                ReminderType.OneHour => eventTime.AddHours(-1),
                ReminderType.OneDay => eventTime.AddDays(-1),
                _ => eventTime
            };
        }

        private void ShowReminder(CalendarEvent evt)
        {
            // Показываем напоминание в основном потоке UI
            System.Windows.Forms.Form mainForm = System.Windows.Forms.Application.OpenForms["MainCalendarForm"];

            if (mainForm != null && !mainForm.IsDisposed)
            {
                mainForm.Invoke(new Action(() =>
                {
                    var timeText = evt.IsAllDay ? "Весь день" : $"{evt.StartTime:HH:mm} - {evt.EndTime:HH:mm}";
                    var message = $"🔔 Напоминание!\n\n" +
                                 $"Событие: {evt.Title}\n" +
                                 $"Время: {timeText}\n" +
                                 $"Дата: {evt.Date:dd.MM.yyyy}";

                    if (!string.IsNullOrEmpty(evt.Description))
                        message += $"\n\nОписание: {evt.Description}";

                    System.Windows.Forms.MessageBox.Show(message, "Напоминание о событии",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information);
                }));
            }
        }

        public void Stop()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}