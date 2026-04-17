using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Duolingo
{
    public class LessonForm : Form
    {
        private DatabaseHelper dbHelper;
        private User currentUser;
        private int lessonId;
        private List<Word> words;
        private int currentQuestionIndex = 0;
        private int score = 0;
        private int totalQuestions;
        private DateTime startTime;
        private int correctAnswers = 0;
        private int wordsLearnedInLesson = 0;

        private Label questionLabel;
        private TextBox answerTextBox;
        private Button submitButton;
        private Button nextButton;
        private ProgressBar progressBar;
        private Label scoreLabel;
        private Label timerLabel;
        private Label hintLabel;
        private Panel resultPanel;

        public LessonForm(DatabaseHelper dbHelper, User user, int lessonId)
        {
            this.dbHelper = dbHelper;
            this.currentUser = user;
            this.lessonId = lessonId;
            InitializeForm();
            LoadLesson();
        }

        private void InitializeForm()
        {
            this.Text = "Урок - Duolingo";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            startTime = DateTime.Now;

            // Заголовок
            var titleLabel = new Label
            {
                Text = "🎯 Урок",
                Font = new Font("Arial", 24, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 206, 138)
            };

            // Панель вопроса
            var questionPanel = new Panel
            {
                Location = new Point(30, 80),
                Size = new Size(740, 200),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            questionLabel = new Label
            {
                Font = new Font("Arial", 36, FontStyle.Bold),
                Location = new Point(50, 50),
                Size = new Size(640, 100),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(33, 33, 33)
            };

            questionPanel.Controls.Add(questionLabel);

            // Панель ответа
            var answerPanel = new Panel
            {
                Location = new Point(30, 300),
                Size = new Size(740, 100),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            answerTextBox = new TextBox
            {
                Font = new Font("Arial", 20),
                Location = new Point(20, 30),
                Size = new Size(600, 40),
                BorderStyle = BorderStyle.FixedSingle
            };

            submitButton = new Button
            {
                Text = "Проверить",
                Location = new Point(630, 25),
                Size = new Size(90, 40),
                BackColor = Color.FromArgb(88, 206, 138),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            submitButton.FlatAppearance.BorderSize = 0;
            submitButton.Click += SubmitButton_Click;

            answerPanel.Controls.Add(answerTextBox);
            answerPanel.Controls.Add(submitButton);

            // Панель управления
            var controlPanel = new Panel
            {
                Location = new Point(30, 420),
                Size = new Size(740, 100)
            };

            progressBar = new ProgressBar
            {
                Location = new Point(20, 10),
                Size = new Size(700, 20),
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };

            scoreLabel = new Label
            {
                Text = "Счет: 0",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 40),
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 206, 138)
            };

            timerLabel = new Label
            {
                Text = "Время: 00:00",
                Font = new Font("Arial", 12),
                Location = new Point(600, 40),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            nextButton = new Button
            {
                Text = "Следующий вопрос →",
                Location = new Point(300, 40),
                Size = new Size(200, 35),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };
            nextButton.FlatAppearance.BorderSize = 0;
            nextButton.Click += NextButton_Click;

            controlPanel.Controls.Add(progressBar);
            controlPanel.Controls.Add(scoreLabel);
            controlPanel.Controls.Add(timerLabel);
            controlPanel.Controls.Add(nextButton);

            // Подсказка
            hintLabel = new Label
            {
                Text = "Введите перевод слова и нажмите 'Проверить'",
                Font = new Font("Arial", 11),
                Location = new Point(30, 530),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            // Кнопка выхода
            var exitButton = new Button
            {
                Text = "✕ Выход",
                Location = new Point(670, 20),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10),
                ForeColor = Color.Gray,
                BackColor = Color.Transparent
            };
            exitButton.FlatAppearance.BorderSize = 0;
            exitButton.Click += (s, e) => this.Close();

            // Таймер обновления времени
            var timer = new Timer { Interval = 1000 };
            timer.Tick += (s, e) =>
            {
                var elapsed = DateTime.Now - startTime;
                timerLabel.Text = $"Время: {elapsed.Minutes:00}:{elapsed.Seconds:00}";
            };
            timer.Start();

            // Добавляем элементы на форму
            this.Controls.Add(titleLabel);
            this.Controls.Add(exitButton);
            this.Controls.Add(questionPanel);
            this.Controls.Add(answerPanel);
            this.Controls.Add(controlPanel);
            this.Controls.Add(hintLabel);

            // Назначаем обработчики клавиш
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter && submitButton.Visible)
                    SubmitButton_Click(s, e);
                else if (e.KeyCode == Keys.Space && nextButton.Visible)
                    NextButton_Click(s, e);
            };
        }

        private void LoadLesson()
        {
            words = dbHelper.GetWordsForLesson(lessonId);
            totalQuestions = Math.Min(words.Count, 10);

            if (words.Count == 0)
            {
                MessageBox.Show("В этом уроке нет слов для изучения.", "Информация",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            // Перемешиваем слова
            var rnd = new Random();
            words = words.OrderBy(x => rnd.Next()).ToList();

            ShowNextQuestion();
        }

        private void ShowNextQuestion()
        {
            if (currentQuestionIndex >= totalQuestions)
            {
                FinishLesson();
                return;
            }

            var currentWord = words[currentQuestionIndex];

            // Случайно выбираем направление перевода
            bool englishToRussian = new Random().Next(0, 2) == 0;

            if (englishToRussian)
            {
                questionLabel.Text = currentWord.English;
                hintLabel.Text = "Введите перевод на русский язык";
            }
            else
            {
                questionLabel.Text = currentWord.Russian;
                hintLabel.Text = "Введите перевод на английский язык";
            }

            answerTextBox.Text = "";
            answerTextBox.Tag = new
            {
                CorrectAnswer = englishToRussian ? currentWord.Russian.ToLower() : currentWord.English.ToLower(),
                WordId = currentWord.Id
            };
            answerTextBox.Focus();

            submitButton.Visible = true;
            nextButton.Visible = false;

            progressBar.Value = (int)((double)currentQuestionIndex / totalQuestions * 100);

            currentQuestionIndex++;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            var tag = answerTextBox.Tag as dynamic;
            if (tag == null) return;

            string userAnswer = answerTextBox.Text.Trim().ToLower();
            string correctAnswer = tag.CorrectAnswer;
            int wordId = tag.WordId;

            // Проверяем ответ (более гибкая проверка)
            bool isCorrect = CheckAnswer(userAnswer, correctAnswer);

            if (isCorrect)
            {
                score += 10;
                correctAnswers++;
                questionLabel.ForeColor = Color.Green;
                hintLabel.Text = "✅ Правильно! +10 очков";

                // Обновляем статистику слова
                dbHelper.UpdateWordPractice(wordId, currentUser.Id);
                wordsLearnedInLesson++;
            }
            else
            {
                questionLabel.ForeColor = Color.Red;
                hintLabel.Text = $"❌ Неправильно. Правильный ответ: {correctAnswer}";
            }

            scoreLabel.Text = $"Счет: {score}";
            submitButton.Visible = false;
            nextButton.Visible = true;

            // Возвращаем цвет через 1 секунду
            var timer = new Timer { Interval = 1000 };
            timer.Tick += (s, ev) =>
            {
                questionLabel.ForeColor = Color.FromArgb(33, 33, 33);
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        private bool CheckAnswer(string userAnswer, string correctAnswer)
        {
            // Убираем лишние пробелы и приводим к нижнему регистру
            userAnswer = userAnswer.Trim().ToLower();
            correctAnswer = correctAnswer.Trim().ToLower();

            // Разрешаем несколько вариантов ответа (разделенных запятыми)
            string[] correctVariants = correctAnswer.Split(',');

            foreach (string variant in correctVariants)
            {
                if (userAnswer == variant.Trim().ToLower())
                    return true;
            }

            return false;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            ShowNextQuestion();
        }

        private void FinishLesson()
        {
            // Скрываем элементы вопроса
            questionLabel.Visible = false;
            answerTextBox.Visible = false;
            submitButton.Visible = false;
            nextButton.Visible = false;
            hintLabel.Visible = false;

            // Показываем результаты
            resultPanel = new Panel
            {
                Location = new Point(100, 100),
                Size = new Size(600, 400),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            var elapsed = DateTime.Now - startTime;
            bool isCompleted = score >= totalQuestions * 7; // 70% для завершения

            // Рассчитываем итоговый балл
            int finalScore = (int)((double)score / (totalQuestions * 10) * 100);

            // Сохраняем результат
            dbHelper.SaveLessonResult(currentUser.Id, lessonId, finalScore, isCompleted, (int)elapsed.TotalMinutes);

            // Начисляем опыт
            int xpEarned = finalScore * 2;
            dbHelper.UpdateUserProgress(currentUser.Id, xpEarned, wordsLearnedInLesson, (int)elapsed.TotalMinutes);

            // Обновляем серию
            dbHelper.UpdateStreak(currentUser.Id);

            // Текст результата
            var resultTitle = new Label
            {
                Text = isCompleted ? "🎉 Урок завершен!" : "📝 Урок пройден",
                Font = new Font("Arial", 28, FontStyle.Bold),
                Location = new Point(150, 50),
                AutoSize = true,
                ForeColor = isCompleted ? Color.FromArgb(88, 206, 138) : Color.FromArgb(255, 193, 7)
            };

            var resultText = new Label
            {
                Text = $"Очки: {score} из {totalQuestions * 10}\n" +
                       $"Правильных ответов: {correctAnswers} из {totalQuestions}\n" +
                       $"Итоговый балл: {finalScore}%\n" +
                       $"Время: {elapsed.Minutes:00}:{elapsed.Seconds:00}\n" +
                       $"Получено опыта: {xpEarned} XP\n" +
                       $"Выучено слов: {wordsLearnedInLesson}\n" +
                       $"Статус: {(isCompleted ? "Завершено ✅" : "Пройдено")}",
                Font = new Font("Arial", 16),
                Location = new Point(150, 120),
                AutoSize = true
            };

            // Прогресс бар результата
            var resultProgress = new ProgressBar
            {
                Location = new Point(150, 220),
                Size = new Size(300, 30),
                Value = finalScore,
                Style = ProgressBarStyle.Continuous
            };

            var progressLabel = new Label
            {
                Text = $"{finalScore}%",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(460, 220),
                AutoSize = true,
                ForeColor = Color.FromArgb(88, 206, 138)
            };

            // Кнопки
            var restartButton = new Button
            {
                Text = "🔄 Повторить урок",
                Location = new Point(150, 280),
                Size = new Size(180, 40),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                FlatStyle = FlatStyle.Flat
            };
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.Click += (s, e) =>
            {
                // Сброс и перезапуск
                currentQuestionIndex = 0;
                score = 0;
                correctAnswers = 0;
                wordsLearnedInLesson = 0;
                startTime = DateTime.Now;
                resultPanel.Visible = false;
                questionLabel.Visible = true;
                answerTextBox.Visible = true;
                submitButton.Visible = true;
                hintLabel.Visible = true;
                ShowNextQuestion();
            };

            var closeButton = new Button
            {
                Text = "🏠 В главное меню",
                Location = new Point(350, 280),
                Size = new Size(180, 40),
                BackColor = Color.FromArgb(88, 206, 138),
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                FlatStyle = FlatStyle.Flat
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();

            resultPanel.Controls.Add(resultTitle);
            resultPanel.Controls.Add(resultText);
            resultPanel.Controls.Add(resultProgress);
            resultPanel.Controls.Add(progressLabel);
            resultPanel.Controls.Add(restartButton);
            resultPanel.Controls.Add(closeButton);

            this.Controls.Add(resultPanel);

            // Показываем сообщение
            if (isCompleted)
            {
                MessageBox.Show($"Поздравляем! Вы завершили урок с результатом {finalScore}%!\n" +
                               $"Получено {xpEarned} XP.\n" +
                               $"Выучено {wordsLearnedInLesson} новых слов!",
                               "Урок завершен",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}