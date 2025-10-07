using CalendarApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace CalendarApp.Services
{
    public class CalendarService
    {
        private List<CalendarEvent> _events;
        private readonly string _dataFilePath = "calendar_data.json";
        private readonly string _backupFilePath = "calendar_backup.json";

        public CalendarService()
        {
            _events = new List<CalendarEvent>();
            LoadData(); // Автоматическая загрузка при создании
        }

        // Основные методы работы с событиями (должны быть уже в вашем классе)
        public void AddEvent(CalendarEvent calendarEvent)
        {
            _events.Add(calendarEvent);
            SaveData(); // Автосохранение при добавлении
        }

        public List<CalendarEvent> GetEventsForDate(DateTime date)
        {
            return _events.Where(e => e.Date.Date == date.Date).ToList();
        }

        public Dictionary<DateTime, List<CalendarEvent>> GetEventsForWeek(DateTime startDate)
        {
            var endDate = startDate.AddDays(6);
            return _events
                .Where(e => e.Date >= startDate && e.Date <= endDate)
                .GroupBy(e => e.Date.Date)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<CalendarEvent> SearchEvents(string searchText)
        {
            return _events.Where(e =>
                e.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                e.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        public List<CalendarEvent> GetAllEvents()
        {
            return _events.OrderBy(e => e.Date).ThenBy(e => e.StartTime).ToList();
        }
        public List<CalendarEvent> GetUpcomingEvents(DateTime fromTime, TimeSpan within)
        {
            var toTime = fromTime.Add(within);

            return _events.Where(e =>
            {
                var eventTime = e.IsAllDay ? e.Date.Date.AddHours(9) : e.StartTime;
                return eventTime >= fromTime && eventTime <= toTime;
            }).ToList();
        }
        // Методы сохранения/загрузки
        public void SaveData()
        {
            try
            {
                // Создаем backup перед сохранением
                if (File.Exists(_dataFilePath))
                {
                    File.Copy(_dataFilePath, _backupFilePath, true);
                }

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_events, options);
                File.WriteAllText(_dataFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения данных: {ex.Message}");
            }
        }

        public void LoadData()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    var json = File.ReadAllText(_dataFilePath);
                    _events = JsonSerializer.Deserialize<List<CalendarEvent>>(json) ?? new List<CalendarEvent>();
                }
            }
            catch (Exception ex)
            {
                // Пробуем загрузить из backup
                if (File.Exists(_backupFilePath))
                {
                    try
                    {
                        var json = File.ReadAllText(_backupFilePath);
                        _events = JsonSerializer.Deserialize<List<CalendarEvent>>(json) ?? new List<CalendarEvent>();
                        // Восстанавливаем основной файл из backup
                        File.Copy(_backupFilePath, _dataFilePath, true);
                    }
                    catch
                    {
                        _events = new List<CalendarEvent>();
                        throw new Exception("Ошибка загрузки данных. Файл поврежден.");
                    }
                }
                else
                {
                    _events = new List<CalendarEvent>();
                }
            }
        }

        // Импорт/экспорт
        public void ExportToJson(string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_events, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка экспорта в JSON: {ex.Message}");
            }
        }

        public void ImportFromJson(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var importedEvents = JsonSerializer.Deserialize<List<CalendarEvent>>(json);

                if (importedEvents != null)
                {
                    _events.AddRange(importedEvents);
                    SaveData(); // Сохраняем объединенные данные
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка импорта из JSON: {ex.Message}");
            }
        }

        public void ExportToTxt(string filePath)
        {
            try
            {
                using var writer = new StreamWriter(filePath);
                writer.WriteLine($"Экспорт событий календаря - {DateTime.Now:dd.MM.yyyy HH:mm}");
                writer.WriteLine("==========================================");
                writer.WriteLine();

                var groupedEvents = _events.OrderBy(e => e.Date).GroupBy(e => e.Date);

                foreach (var dayGroup in groupedEvents)
                {
                    writer.WriteLine($"📅 {dayGroup.Key:dd.MM.yyyy (dddd)}");
                    writer.WriteLine(new string('-', 40));

                    foreach (var evt in dayGroup.OrderBy(e => e.StartTime))
                    {
                        var time = evt.IsAllDay ? "Весь день" : $"{evt.StartTime:HH:mm} - {evt.EndTime:HH:mm}";
                        writer.WriteLine($"  • {evt.Title}");
                        writer.WriteLine($"    ⏰ {time} | 🏷️ {evt.Category}");

                        if (!string.IsNullOrEmpty(evt.Description))
                            writer.WriteLine($"    📝 {evt.Description}");

                        writer.WriteLine();
                    }
                    writer.WriteLine();
                }

                writer.WriteLine($"Всего событий: {_events.Count}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка экспорта в TXT: {ex.Message}");
            }
        }

        public void ClearAllData()
        {
            _events.Clear();

            // Удаляем файлы данных
            if (File.Exists(_dataFilePath))
                File.Delete(_dataFilePath);

            if (File.Exists(_backupFilePath))
                File.Delete(_backupFilePath);
        }

        public int GetEventsCount() => _events.Count;
    }
}