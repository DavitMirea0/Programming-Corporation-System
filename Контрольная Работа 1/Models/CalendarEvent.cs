using System;
using System.Drawing;

namespace CalendarApp.Models
{
    public class CalendarEvent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public EventCategory Category { get; set; }
        public string Color { get; set; }
        public ReminderType Reminder { get; set; }
    }

    public enum EventCategory
    {
        Personal,
        Work,
        Holiday,
        Birthday,
        Meeting,
        Appointment,
        Other
    }
}