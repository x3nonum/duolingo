using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Duolingo
{
    public class AuthForm : Form
    {
        private DatabaseHelper dbHelper;
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Button btnGuest;
        private Label lblStatus;
        private Panel loginPanel;
        private Panel registerPanel;
        private User currentUser;

        public AuthForm(DatabaseHelper dbHelper)
        {
            this.dbHelper = dbHelper;
            InitializeForm();
            ShowLoginPanel();
        }

        public User GetAuthenticatedUser()
        {
            return currentUser;
        }

        private void InitializeForm()
        {
            this.Text = "Duolingo - Вход";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Заголовок
            var titleLabel = new Label
            {
                Text = "Duolingo",
                Font = new Font("Arial", 32, FontStyle.Bold),
                Location = new Point(150, 30),
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 206, 138)
            };

            var subtitleLabel = new Label
            {
                Text = "Изучай языки бесплатно",
                Font = new Font("Arial", 14),
                Location = new Point(140, 80),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            // Статус
            lblStatus = new Label
            {
                Location = new Point(50, 120),
                Size = new Size(400, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 10)
            };

            // Панель входа
            loginPanel = new Panel
            {
                Location = new Point(50, 160),
                Size = new Size(400, 250),
                Visible = false
            };

            CreateLoginPanel();

            // Панель регистрации
            registerPanel = new Panel
            {
                Location = new Point(50, 160),
                Size = new Size(400, 320),
                Visible = false
            };

            CreateRegisterPanel();

            // Кнопка гостя
            btnGuest = new Button
            {
                Text = "Продолжить как гость",
                Location = new Point(150, 450),
                Size = new Size(200, 40),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(88, 206, 138),
                BackColor = Color.Transparent
            };
            btnGuest.FlatAppearance.BorderSize = 0;
            btnGuest.Click += (s, e) => LoginAsGuest();

            this.Controls.Add(titleLabel);
            this.Controls.Add(subtitleLabel);
            this.Controls.Add(lblStatus);
            this.Controls.Add(loginPanel);
            this.Controls.Add(registerPanel);
            this.Controls.Add(btnGuest);
        }

        private void CreateLoginPanel()
        {
            loginPanel.Controls.Clear();

            var lblLoginTitle = new Label
            {
                Text = "Вход в аккаунт",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Location = new Point(100, 10),
                AutoSize = true
            };

            var lblUsername = new Label
            {
                Text = "Имя пользователя:",
                Location = new Point(0, 60),
                AutoSize = true
            };

            txtUsername = new TextBox
            {
                Location = new Point(0, 85),
                Size = new Size(400, 30),
                Font = new Font("Arial", 12)
            };

            var lblPassword = new Label
            {
                Text = "Пароль:",
                Location = new Point(0, 125),
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Location = new Point(0, 150),
                Size = new Size(400, 30),
                Font = new Font("Arial", 12),
                PasswordChar = '*'
            };

            btnLogin = new Button
            {
                Text = "Войти",
                Location = new Point(100, 200),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(88, 206, 138),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += (s, e) => Login();

            var switchToRegister = new LinkLabel
            {
                Text = "Нет аккаунта? Зарегистрироваться",
                Location = new Point(80, 250),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };
            switchToRegister.LinkClicked += (s, e) => ShowRegisterPanel();

            loginPanel.Controls.Add(lblLoginTitle);
            loginPanel.Controls.Add(lblUsername);
            loginPanel.Controls.Add(txtUsername);
            loginPanel.Controls.Add(lblPassword);
            loginPanel.Controls.Add(txtPassword);
            loginPanel.Controls.Add(btnLogin);
            loginPanel.Controls.Add(switchToRegister);
        }

        private void CreateRegisterPanel()
        {
            registerPanel.Controls.Clear();

            var lblRegisterTitle = new Label
            {
                Text = "Регистрация",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Location = new Point(120, 10),
                AutoSize = true
            };

            var lblRegUsername = new Label
            {
                Text = "Имя пользователя:",
                Location = new Point(0, 60),
                AutoSize = true
            };

            var txtRegUsername = new TextBox
            {
                Location = new Point(0, 85),
                Size = new Size(400, 30),
                Font = new Font("Arial", 12)
            };

            var lblRegEmail = new Label
            {
                Text = "Email:",
                Location = new Point(0, 125),
                AutoSize = true
            };

            txtEmail = new TextBox
            {
                Location = new Point(0, 150),
                Size = new Size(400, 30),
                Font = new Font("Arial", 12)
            };

            var lblRegPassword = new Label
            {
                Text = "Пароль:",
                Location = new Point(0, 190),
                AutoSize = true
            };

            var txtRegPassword = new TextBox
            {
                Location = new Point(0, 215),
                Size = new Size(400, 30),
                Font = new Font("Arial", 12),
                PasswordChar = '*'
            };

            var lblRegConfirmPassword = new Label
            {
                Text = "Подтвердите пароль:",
                Location = new Point(0, 255),
                AutoSize = true
            };

            var txtRegConfirmPassword = new TextBox
            {
                Location = new Point(0, 280),
                Size = new Size(400, 30),
                Font = new Font("Arial", 12),
                PasswordChar = '*'
            };

            btnRegister = new Button
            {
                Text = "Зарегистрироваться",
                Location = new Point(100, 320),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += (s, e) => Register(txtRegUsername.Text, txtEmail.Text,
                txtRegPassword.Text, txtRegConfirmPassword.Text);

            var switchToLogin = new LinkLabel
            {
                Text = "Уже есть аккаунт? Войти",
                Location = new Point(100, 370),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };
            switchToLogin.LinkClicked += (s, e) => ShowLoginPanel();

            registerPanel.Controls.Add(lblRegisterTitle);
            registerPanel.Controls.Add(lblRegUsername);
            registerPanel.Controls.Add(txtRegUsername);
            registerPanel.Controls.Add(lblRegEmail);
            registerPanel.Controls.Add(txtEmail);
            registerPanel.Controls.Add(lblRegPassword);
            registerPanel.Controls.Add(txtRegPassword);
            registerPanel.Controls.Add(lblRegConfirmPassword);
            registerPanel.Controls.Add(txtRegConfirmPassword);
            registerPanel.Controls.Add(btnRegister);
            registerPanel.Controls.Add(switchToLogin);
        }

        private void ShowLoginPanel()
        {
            loginPanel.Visible = true;
            registerPanel.Visible = false;
            lblStatus.Text = "";
            txtUsername?.Focus();
        }

        private void ShowRegisterPanel()
        {
            loginPanel.Visible = false;
            registerPanel.Visible = true;
            lblStatus.Text = "";
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Введите имя пользователя");
                return;
            }

            var user = dbHelper.GetUser(txtUsername.Text);
            if (user == null)
            {
                ShowError("Пользователь не найден");
                return;
            }

            // В демо-версии проверка пароля упрощена
            if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text != "demo")
            {
                ShowError("Неверный пароль. Для демо используйте 'demo'");
                return;
            }

            currentUser = user;
            ShowSuccess($"Добро пожаловать, {user.Username}!");

            // Закрываем форму через 1 секунду
            var timer = new Timer { Interval = 1000 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            timer.Start();
        }

        private void Register(string username, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Введите имя пользователя");
                return;
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                ShowError("Введите корректный email");
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 3)
            {
                ShowError("Пароль должен содержать минимум 3 символа");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Пароли не совпадают");
                return;
            }

            // Проверяем, не существует ли уже пользователь
            if (dbHelper.GetUser(username) != null)
            {
                ShowError("Пользователь с таким именем уже существует");
                return;
            }

            // Создаем нового пользователя
            var newUser = dbHelper.CreateUser(username, email);
            if (newUser != null)
            {
                ShowSuccess($"Аккаунт {username} успешно создан!");
                currentUser = newUser;

                // Переключаемся на вход через 1.5 секунды
                var timer = new Timer { Interval = 1500 };
                timer.Tick += (s, e) =>
                {
                    timer.Stop();
                    ShowLoginPanel();
                    txtUsername.Text = username;
                    txtPassword.Text = "";
                };
                timer.Start();
            }
            else
            {
                ShowError("Ошибка при создании аккаунта");
            }
        }

        private void LoginAsGuest()
        {
            // Создаем гостевого пользователя или используем существующего
            var guestUser = dbHelper.GetUser("Гость");
            if (guestUser == null)
            {
                guestUser = new User
                {
                    Id = 999,
                    Username = "Гость",
                    Email = "guest@example.com",
                    Experience = 0,
                    Level = 1,
                    StreakDays = 0,
                    RegistrationDate = DateTime.Now,
                    CompletedLessons = 0,
                    WordsLearned = 0,
                    TotalTimeMinutes = 0,
                    LastPracticeDate = DateTime.Now,
                    CompletedLessonIds = new System.Collections.Generic.List<int>(),
                    WordPracticeCount = new System.Collections.Generic.Dictionary<int, int>()
                };
            }

            currentUser = guestUser;
            ShowSuccess("Вы вошли как гость");

            var timer = new Timer { Interval = 1000 };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            timer.Start();
        }

        private void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.Red;
        }

        private void ShowSuccess(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.Green;
        }
    }
}