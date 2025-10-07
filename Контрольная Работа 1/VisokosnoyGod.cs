using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalendarApp
{
    public partial class LeapYearForm: Form
    {
        private NumericUpDown _yearInput;
        private Button _checkButton;
        private Label _resultLabel;
        private Panel _infoPanel;
        private Label _daysLabel;
        private Label _februaryLabel;

        public LeapYearForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "🗓️ Проверка високосного года";
            Size = new Size(500, 450);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Padding = new Padding(20);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Заголовок
            var headerLabel = new Label
            {
                Text = "Проверка високосного года",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 130, 180),
                Size = new Size(400, 40),
                Location = new Point(0, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Описание
            var descLabel = new Label
            {
                Text = "Високосный год - это год, который делится на 4,\n" +
                       "но не делится на 100, за исключением тех,\n" +
                       "которые делятся на 400.",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Size = new Size(400, 60),
                Location = new Point(0, 60),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Поле ввода года
            var yearLabel = new Label
            {
                Text = "Введите год:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(150, 25),
                Location = new Point(50, 130)
            };

            _yearInput = new NumericUpDown
            {
                Location = new Point(50, 160),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 12),
                Minimum = 1,
                Maximum = 9999,
                Value = DateTime.Now.Year,
                TextAlign = HorizontalAlignment.Center
            };

            // Кнопка проверки
            _checkButton = new Button
            {
                Text = "🔍 Проверить",
                Location = new Point(220, 160),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            _checkButton.Click += CheckButton_Click;

            // Результат
            _resultLabel = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Size = new Size(400, 50),
                Location = new Point(0, 220),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };

            // Панель с дополнительной информацией
            _infoPanel = new Panel
            {
                Location = new Point(50, 280),
                Size = new Size(380, 100),
                BackColor = Color.FromArgb(240, 248, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            _daysLabel = new Label
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(360, 25),
                Location = new Point(10, 15),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _februaryLabel = new Label
            {
                Font = new Font("Segoe UI", 10),
                Size = new Size(360, 25),
                Location = new Point(10, 45),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _infoPanel.Controls.AddRange(new Control[] { _daysLabel, _februaryLabel });

            // Кнопка закрытия
            var closeButton = new Button
            {
                Text = "❌ Закрыть",
                Location = new Point(175, 390),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(220, 20, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            closeButton.Click += (s, e) => Close();

            mainPanel.Controls.AddRange(new Control[]
            {
                headerLabel,
                descLabel,
                yearLabel,
                _yearInput,
                _checkButton,
                _resultLabel,
                _infoPanel,
                closeButton
            });

            Controls.Add(mainPanel);

            // Проверяем текущий год при открытии
            CheckYear((int)_yearInput.Value);
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            CheckYear((int)_yearInput.Value);
        }

        private void CheckYear(int year)
        {
            bool isLeapYear = IsLeapYear(year);

            if (isLeapYear)
            {
                _resultLabel.Text = $"✅ {year} год - ВИСОКОСНЫЙ";
                _resultLabel.ForeColor = Color.FromArgb(34, 139, 34);
                _daysLabel.Text = $"📅 Количество дней в году: 366";
                _februaryLabel.Text = $"📆 Дней в феврале: 29";
            }
            else
            {
                _resultLabel.Text = $"❌ {year} год - НЕ високосный";
                _resultLabel.ForeColor = Color.FromArgb(220, 20, 60);
                _daysLabel.Text = $"📅 Количество дней в году: 365";
                _februaryLabel.Text = $"📆 Дней в феврале: 28";
            }

            _infoPanel.Visible = true;

            // Добавляем информацию о правиле
            var explanation = GetLeapYearExplanation(year);
            var tooltipLabel = new Label
            {
                Text = $"ℹ️ {explanation}",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                Size = new Size(360, 30),
                Location = new Point(10, 70),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Удаляем старую подсказку если есть
            foreach (Control control in _infoPanel.Controls)
            {
                if (control is Label label && label.Text.StartsWith("ℹ️"))
                {
                    _infoPanel.Controls.Remove(control);
                    break;
                }
            }

            _infoPanel.Controls.Add(tooltipLabel);
        }

        private bool IsLeapYear(int year)
        {
            // Правило определения високосного года:
            // 1. Год делится на 4
            // 2. Если год делится на 100, то он должен делиться и на 400

            if (year % 4 != 0)
            {
                return false; // Не делится на 4 - не високосный
            }
            else if (year % 100 != 0)
            {
                return true; // Делится на 4, но не на 100 - високосный
            }
            else if (year % 400 == 0)
            {
                return true; // Делится на 400 - високосный
            }
            else
            {
                return false; // Делится на 100, но не на 400 - не високосный
            }
        }

        private string GetLeapYearExplanation(int year)
        {
            if (year % 400 == 0)
            {
                return $"Делится на 400 ({year} ÷ 400 = {year / 400})";
            }
            else if (year % 100 == 0)
            {
                return $"Делится на 100, но не на 400";
            }
            else if (year % 4 == 0)
            {
                return $"Делится на 4 ({year} ÷ 4 = {year / 4})";
            }
            else
            {
                return $"Не делится на 4";
            }
        }
    }
}