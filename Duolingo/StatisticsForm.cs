using System;
using System.Drawing;
using System.Windows.Forms;

namespace Duolingo
{
    public class StatisticsForm : Form
    {
        private DatabaseHelper dbHelper;
        private User currentUser;

        public StatisticsForm(DatabaseHelper dbHelper, User user)
        {
            this.dbHelper = dbHelper;
            this.currentUser = user;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Детальная статистика";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            var titleLabel = new Label
            {
                Text = "📊 Детальная статистика",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            var closeButton = new Button
            {
                Text = "Закрыть",
                Location = new Point(250, 400),
                Size = new Size(100, 35)
            };
            closeButton.Click += (s, e) => this.Close();

            this.Controls.Add(titleLabel);
            this.Controls.Add(closeButton);
        }
    }
}