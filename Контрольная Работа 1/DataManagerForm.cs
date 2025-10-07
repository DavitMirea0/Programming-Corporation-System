using CalendarApp.Models;
using CalendarApp.Services;
using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace CalendarApp
{
    public partial class DataManagerForm : Form
    {
        private readonly CalendarService _calendarService;

        public DataManagerForm(CalendarService calendarService)
        {
            _calendarService = calendarService;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "💾 Управление данными";
            Size = new Size(500, 400);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Padding = new Padding(20);

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Заголовок
            var headerLabel = new Label
            {
                Text = "Управление данными календаря",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 130, 180),
                Size = new Size(400, 40),
                Location = new Point(0, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Кнопка сохранения
            var saveButton = new Button
            {
                Text = "💾 Сохранить данные",
                Location = new Point(50, 70),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(34, 139, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            saveButton.Click += SaveButton_Click;

            // Кнопка загрузки
            var loadButton = new Button
            {
                Text = "📂 Загрузить данные",
                Location = new Point(250, 70),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(30, 144, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            loadButton.Click += LoadButton_Click;

            // Кнопка экспорта в JSON
            var exportJsonButton = new Button
            {
                Text = "📄 Экспорт в JSON",
                Location = new Point(50, 130),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(106, 90, 205),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            exportJsonButton.Click += ExportJsonButton_Click;

            // Кнопка импорта из JSON
            var importJsonButton = new Button
            {
                Text = "📥 Импорт из JSON",
                Location = new Point(250, 130),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(205, 92, 92),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            importJsonButton.Click += ImportJsonButton_Click;

            // Кнопка экспорта в TXT
            var exportTxtButton = new Button
            {
                Text = "📝 Экспорт в TXT",
                Location = new Point(50, 190),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(139, 69, 19),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            exportTxtButton.Click += ExportTxtButton_Click;

            // Кнопка очистки данных
            var clearButton = new Button
            {
                Text = "🗑️ Очистить все данные",
                Location = new Point(250, 190),
                Size = new Size(180, 45),
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            clearButton.Click += ClearButton_Click;

            // Статусная строка
            var statusLabel = new Label
            {
                Text = "Выберите действие для управления данными",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Size = new Size(400, 25),
                Location = new Point(50, 250),
                TextAlign = ContentAlignment.MiddleCenter
            };

            mainPanel.Controls.AddRange(new Control[]
            {
                headerLabel,
                saveButton,
                loadButton,
                exportJsonButton,
                importJsonButton,
                exportTxtButton,
                clearButton,
                statusLabel
            });

            Controls.Add(mainPanel);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                _calendarService.SaveData();
                MessageBox.Show("Данные успешно сохранены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                _calendarService.LoadData();
                MessageBox.Show("Данные успешно загружены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportJsonButton_Click(object sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Экспорт данных в JSON",
                FileName = $"calendar_export_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _calendarService.ExportToJson(saveDialog.FileName);
                    MessageBox.Show($"Данные успешно экспортированы в {saveDialog.FileName}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportJsonButton_Click(object sender, EventArgs e)
        {
            using var openDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Импорт данных из JSON"
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _calendarService.ImportFromJson(openDialog.FileName);
                    MessageBox.Show($"Данные успешно импортированы из {openDialog.FileName}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при импорте: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportTxtButton_Click(object sender, EventArgs e)
        {
            using var saveDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "Экспорт данных в TXT",
                FileName = $"calendar_events_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _calendarService.ExportToTxt(saveDialog.FileName);
                    MessageBox.Show($"Данные успешно экспортированы в {saveDialog.FileName}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите удалить все данные? Это действие нельзя отменить.",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _calendarService.ClearAllData();
                    MessageBox.Show("Все данные успешно удалены!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении данных: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}