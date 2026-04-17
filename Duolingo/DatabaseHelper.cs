using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Duolingo
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Experience { get; set; }
        public int StreakDays { get; set; }
        public int Level { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int CompletedLessons { get; set; }
        public int WordsLearned { get; set; }
        public int TotalTimeMinutes { get; set; }
        public DateTime LastPracticeDate { get; set; }
        public List<int> CompletedLessonIds { get; set; } = new List<int>();
        public Dictionary<int, int> WordPracticeCount { get; set; } = new Dictionary<int, int>();
        public string PasswordHash { get; set; } // Для демо оставляем пустым
    }

    public class UserStatistic
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
    }

    public class Achievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool IsUnlocked { get; set; }
        public DateTime UnlockedDate { get; set; }
        public int RequiredValue { get; set; }
    }

    public class Word
    {
        public int Id { get; set; }
        public string English { get; set; }
        public string Russian { get; set; }
        public string Category { get; set; }
        public int TimesPracticed { get; set; }
        public DateTime LastPracticed { get; set; }
        public int MasteryLevel { get; set; }
        public int LessonId { get; set; }
    }

    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int Score { get; set; }
        public int OrderIndex { get; set; }
        public int RequiredWords { get; set; }
        public int ExperienceReward { get; set; }
    }

    public class LessonProgress
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public bool IsCompleted { get; set; }
        public int Score { get; set; }
        public DateTime CompletedDate { get; set; }
        public int Attempts { get; set; }
    }

    public class DatabaseHelper
    {
        private List<User> users;
        private List<Word> words;
        private List<Lesson> lessons;
        private List<Achievement> achievements;
        private List<LessonProgress> lessonProgress;

        private string dataPath;
        private Random random = new Random();

        public DatabaseHelper()
        {
            dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);

            InitializeData();
        }

        private void InitializeData()
        {
            // Инициализируем тестовые данные
            users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "Иван Петров",
                    Email = "ivan@example.com",
                    Experience = 1250,
                    StreakDays = 7,
                    Level = 3,
                    RegistrationDate = DateTime.Now.AddDays(-30),
                    CompletedLessons = 3,
                    WordsLearned = 45,
                    TotalTimeMinutes = 320,
                    LastPracticeDate = DateTime.Now.AddDays(-1),
                    CompletedLessonIds = new List<int> { 1, 2, 3 },
                    WordPracticeCount = new Dictionary<int, int>
                    {
                        {1, 15}, {2, 12}, {3, 10}, {4, 8}, {5, 8}, {6, 6}, {7, 6}, {8, 10}, {9, 9}, {10, 5}
                    },
                    PasswordHash = "demo" // Для демо
                },
                new User
                {
                    Id = 2,
                    Username = "Анна Смирнова",
                    Email = "anna@example.com",
                    Experience = 800,
                    StreakDays = 3,
                    Level = 2,
                    RegistrationDate = DateTime.Now.AddDays(-20),
                    CompletedLessons = 2,
                    WordsLearned = 32,
                    TotalTimeMinutes = 200,
                    LastPracticeDate = DateTime.Now,
                    CompletedLessonIds = new List<int> { 1, 2 },
                    WordPracticeCount = new Dictionary<int, int>
                    {
                        {1, 8}, {2, 6}, {3, 5}, {4, 4}, {5, 7}, {6, 5}, {7, 3}, {8, 6}, {9, 5}, {10, 2}
                    },
                    PasswordHash = "demo" // Для демо
                }
            };

            words = new List<Word>
            {
                new Word { Id = 1, English = "Hello", Russian = "Привет", Category = "Основы", TimesPracticed = 15, LastPracticed = DateTime.Now.AddDays(-1), MasteryLevel = 3, LessonId = 1 },
                new Word { Id = 2, English = "Goodbye", Russian = "До свидания", Category = "Основы", TimesPracticed = 12, LastPracticed = DateTime.Now.AddDays(-2), MasteryLevel = 3, LessonId = 1 },
                new Word { Id = 3, English = "Thank you", Russian = "Спасибо", Category = "Основы", TimesPracticed = 10, LastPracticed = DateTime.Now.AddDays(-3), MasteryLevel = 2, LessonId = 1 },
                new Word { Id = 4, English = "Please", Russian = "Пожалуйста", Category = "Основы", TimesPracticed = 8, LastPracticed = DateTime.Now.AddDays(-4), MasteryLevel = 2, LessonId = 1 },
                new Word { Id = 5, English = "Apple", Russian = "Яблоко", Category = "Еда", TimesPracticed = 8, LastPracticed = DateTime.Now.AddDays(-5), MasteryLevel = 2, LessonId = 2 },
                new Word { Id = 6, English = "Bread", Russian = "Хлеб", Category = "Еда", TimesPracticed = 6, LastPracticed = DateTime.Now.AddDays(-6), MasteryLevel = 2, LessonId = 2 },
                new Word { Id = 7, English = "Water", Russian = "Вода", Category = "Еда", TimesPracticed = 6, LastPracticed = DateTime.Now.AddDays(-7), MasteryLevel = 1, LessonId = 2 },
                new Word { Id = 8, English = "Dog", Russian = "Собака", Category = "Животные", TimesPracticed = 10, LastPracticed = DateTime.Now.AddDays(-1), MasteryLevel = 3, LessonId = 3 },
                new Word { Id = 9, English = "Cat", Russian = "Кот", Category = "Животные", TimesPracticed = 9, LastPracticed = DateTime.Now.AddDays(-2), MasteryLevel = 2, LessonId = 3 },
                new Word { Id = 10, English = "Bird", Russian = "Птица", Category = "Животные", TimesPracticed = 5, LastPracticed = DateTime.Now.AddDays(-3), MasteryLevel = 1, LessonId = 3 },
                new Word { Id = 11, English = "Family", Russian = "Семья", Category = "Семья", TimesPracticed = 3, LastPracticed = DateTime.Now.AddDays(-8), MasteryLevel = 1, LessonId = 4 },
                new Word { Id = 12, English = "Mother", Russian = "Мама", Category = "Семья", TimesPracticed = 2, LastPracticed = DateTime.Now.AddDays(-9), MasteryLevel = 1, LessonId = 4 },
                new Word { Id = 13, English = "Father", Russian = "Папа", Category = "Семья", TimesPracticed = 2, LastPracticed = DateTime.Now.AddDays(-10), MasteryLevel = 1, LessonId = 4 },
                new Word { Id = 14, English = "Work", Russian = "Работа", Category = "Работа", TimesPracticed = 1, LastPracticed = DateTime.Now.AddDays(-15), MasteryLevel = 1, LessonId = 5 },
                new Word { Id = 15, English = "Teacher", Russian = "Учитель", Category = "Работа", TimesPracticed = 1, LastPracticed = DateTime.Now.AddDays(-16), MasteryLevel = 1, LessonId = 5 }
            };

            lessons = new List<Lesson>
            {
                new Lesson { Id = 1, Title = "Приветствия", Description = "Основные фразы для приветствия", IsCompleted = true,
                           CompletedDate = DateTime.Now.AddDays(-15), Score = 95, OrderIndex = 1, RequiredWords = 4, ExperienceReward = 100 },
                new Lesson { Id = 2, Title = "Еда и напитки", Description = "Названия продуктов и блюд", IsCompleted = true,
                           CompletedDate = DateTime.Now.AddDays(-18), Score = 88, OrderIndex = 2, RequiredWords = 3, ExperienceReward = 150 },
                new Lesson { Id = 3, Title = "Животные", Description = "Названия животных", IsCompleted = true,
                           CompletedDate = DateTime.Now.AddDays(-22), Score = 92, OrderIndex = 3, RequiredWords = 3, ExperienceReward = 150 },
                new Lesson { Id = 4, Title = "Семья", Description = "Члены семьи и родственники", IsCompleted = false,
                           CompletedDate = null, Score = 0, OrderIndex = 4, RequiredWords = 3, ExperienceReward = 200 },
                new Lesson { Id = 5, Title = "Работа", Description = "Профессии и рабочее место", IsCompleted = false,
                           CompletedDate = null, Score = 0, OrderIndex = 5, RequiredWords = 2, ExperienceReward = 250 },
                new Lesson { Id = 6, Title = "Дом", Description = "Комнаты и предметы интерьера", IsCompleted = false,
                           CompletedDate = null, Score = 0, OrderIndex = 6, RequiredWords = 4, ExperienceReward = 300 },
                new Lesson { Id = 7, Title = "Город", Description = "Городские объекты и транспорт", IsCompleted = false,
                           CompletedDate = null, Score = 0, OrderIndex = 7, RequiredWords = 5, ExperienceReward = 350 }
            };

            achievements = new List<Achievement>
            {
                new Achievement { Id = 1, UserId = 1, Name = "Первые шаги", Description = "Завершить первый урок",
                                Icon = "🎯", IsUnlocked = true, UnlockedDate = DateTime.Now.AddDays(-15), RequiredValue = 1 },
                new Achievement { Id = 2, UserId = 1, Name = "Словарный запас", Description = "Выучить 10 слов",
                                Icon = "📚", IsUnlocked = true, UnlockedDate = DateTime.Now.AddDays(-20), RequiredValue = 10 },
                new Achievement { Id = 3, UserId = 1, Name = "Неделя практики", Description = "Заниматься 7 дней подряд",
                                Icon = "🔥", IsUnlocked = true, UnlockedDate = DateTime.Now.AddDays(-25), RequiredValue = 7 },
                new Achievement { Id = 4, UserId = 1, Name = "Быстрый ученик", Description = "Завершить урок за рекордное время",
                                Icon = "⚡", IsUnlocked = false, UnlockedDate = DateTime.MinValue, RequiredValue = 1 },
                new Achievement { Id = 5, UserId = 1, Name = "Эксперт перевода", Description = "Правильно перевести 50 слов",
                                Icon = "🏆", IsUnlocked = false, UnlockedDate = DateTime.MinValue, RequiredValue = 50 },
                new Achievement { Id = 6, UserId = 1, Name = "Перфекционист", Description = "Получить 100% в 5 уроках",
                                Icon = "💯", IsUnlocked = false, UnlockedDate = DateTime.MinValue, RequiredValue = 5 },
                new Achievement { Id = 7, UserId = 1, Name = "Полиглот", Description = "Выучить 100 слов",
                                Icon = "🌍", IsUnlocked = false, UnlockedDate = DateTime.MinValue, RequiredValue = 100 },
                new Achievement { Id = 8, UserId = 1, Name = "Мастер серий", Description = "30 дней практики подряд",
                                Icon = "🏅", IsUnlocked = false, UnlockedDate = DateTime.MinValue, RequiredValue = 30 },
                new Achievement { Id = 9, UserId = 2, Name = "Первые шаги", Description = "Завершить первый урок",
                                Icon = "🎯", IsUnlocked = true, UnlockedDate = DateTime.Now.AddDays(-12), RequiredValue = 1 },
                new Achievement { Id = 10, UserId = 2, Name = "Словарный запас", Description = "Выучить 10 слов",
                                Icon = "📚", IsUnlocked = false, UnlockedDate = DateTime.MinValue, RequiredValue = 10 }
            };

            lessonProgress = new List<LessonProgress>
            {
                new LessonProgress { UserId = 1, LessonId = 1, IsCompleted = true, Score = 95, CompletedDate = DateTime.Now.AddDays(-15), Attempts = 1 },
                new LessonProgress { UserId = 1, LessonId = 2, IsCompleted = true, Score = 88, CompletedDate = DateTime.Now.AddDays(-18), Attempts = 2 },
                new LessonProgress { UserId = 1, LessonId = 3, IsCompleted = true, Score = 92, CompletedDate = DateTime.Now.AddDays(-22), Attempts = 1 },
                new LessonProgress { UserId = 2, LessonId = 1, IsCompleted = true, Score = 90, CompletedDate = DateTime.Now.AddDays(-12), Attempts = 1 },
                new LessonProgress { UserId = 2, LessonId = 2, IsCompleted = true, Score = 85, CompletedDate = DateTime.Now.AddDays(-10), Attempts = 1 }
            };
        }

        // ========== МЕТОДЫ ДЛЯ РАБОТЫ С ПОЛЬЗОВАТЕЛЯМИ ==========

        public User GetUser(string username)
        {
            return users.FirstOrDefault(u => u.Username == username);
        }

        public User GetUserById(int userId)
        {
            return users.FirstOrDefault(u => u.Id == userId);
        }

        public User CreateUser(string username, string email)
        {
            try
            {
                // Проверяем, не существует ли уже пользователь
                if (users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    return null;
                }

                // Генерируем новый ID
                int newId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

                var newUser = new User
                {
                    Id = newId,
                    Username = username,
                    Email = email,
                    Experience = 0,
                    Level = 1,
                    StreakDays = 0,
                    RegistrationDate = DateTime.Now,
                    CompletedLessons = 0,
                    WordsLearned = 0,
                    TotalTimeMinutes = 0,
                    LastPracticeDate = DateTime.Now,
                    CompletedLessonIds = new List<int>(),
                    WordPracticeCount = new Dictionary<int, int>(),
                    PasswordHash = "demo" // В демо-версии фиксированный пароль
                };

                users.Add(newUser);

                // Создаем достижения для нового пользователя
                CreateDefaultAchievements(newUser.Id);

                // Создаем гостевого пользователя, если его нет
                if (!users.Any(u => u.Username == "Гость"))
                {
                    var guestUser = new User
                    {
                        Id = newId + 1,
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
                        CompletedLessonIds = new List<int>(),
                        WordPracticeCount = new Dictionary<int, int>(),
                        PasswordHash = ""
                    };
                    users.Add(guestUser);
                }

                return newUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateDefaultAchievements(int userId)
        {
            var defaultAchievements = new List<Achievement>
            {
                new Achievement { Id = achievements.Count + 1, UserId = userId, Name = "Первые шаги",
                                Description = "Завершить первый урок", Icon = "🎯", IsUnlocked = false,
                                UnlockedDate = DateTime.MinValue, RequiredValue = 1 },
                new Achievement { Id = achievements.Count + 2, UserId = userId, Name = "Словарный запас",
                                Description = "Выучить 10 слов", Icon = "📚", IsUnlocked = false,
                                UnlockedDate = DateTime.MinValue, RequiredValue = 10 },
                new Achievement { Id = achievements.Count + 3, UserId = userId, Name = "Неделя практики",
                                Description = "Заниматься 7 дней подряд", Icon = "🔥", IsUnlocked = false,
                                UnlockedDate = DateTime.MinValue, RequiredValue = 7 },
                new Achievement { Id = achievements.Count + 4, UserId = userId, Name = "Быстрый ученик",
                                Description = "Завершить урок за рекордное время", Icon = "⚡",
                                IsUnlocked = false, UnlockedDate = DateTime.MinValue, RequiredValue = 1 },
                new Achievement { Id = achievements.Count + 5, UserId = userId, Name = "Эксперт перевода",
                                Description = "Правильно перевести 50 слов", Icon = "🏆", IsUnlocked = false,
                                UnlockedDate = DateTime.MinValue, RequiredValue = 50 }
            };

            achievements.AddRange(defaultAchievements);
        }

        public List<User> GetAllUsers()
        {
            return users.Where(u => u.Username != "Гость").OrderByDescending(u => u.Experience).ToList();
        }

        public bool DeleteUser(int userId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null && user.Username != "Гость")
            {
                users.Remove(user);

                // Удаляем связанные данные
                achievements.RemoveAll(a => a.UserId == userId);
                lessonProgress.RemoveAll(lp => lp.UserId == userId);

                return true;
            }
            return false;
        }

        public void UpdateUserProfile(int userId, string username, string email)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                user.Username = username;
                user.Email = email;
            }
        }

        public bool VerifyPassword(string username, string password)
        {
            var user = GetUser(username);
            if (user == null) return false;

            // В демо-версии пароль всегда "demo"
            return password == "demo";
        }

        // ========== ОСТАЛЬНЫЕ МЕТОДЫ ==========

        public List<UserStatistic> GetUserStatistics(int userId)
        {
            var user = GetUserById(userId);
            if (user == null)
                return new List<UserStatistic>();

            // Расчет статистики
            int totalLessonsCompleted = lessonProgress.Count(lp => lp.UserId == userId && lp.IsCompleted);
            int averageScore = 0;
            if (totalLessonsCompleted > 0)
            {
                averageScore = lessonProgress.Where(lp => lp.UserId == userId && lp.IsCompleted)
                                            .Sum(lp => lp.Score) / totalLessonsCompleted;
            }

            int masteredWords = words.Count(w =>
                user.WordPracticeCount.ContainsKey(w.Id) &&
                user.WordPracticeCount[w.Id] >= 10);

            int practicedWords = words.Count(w =>
                user.WordPracticeCount.ContainsKey(w.Id) &&
                user.WordPracticeCount[w.Id] > 0);

            int totalPractice = user.WordPracticeCount.Values.Sum();

            return new List<UserStatistic>
            {
                new UserStatistic { Title = "Пройдено уроков", Value = totalLessonsCompleted.ToString(), Icon = "📚" },
                new UserStatistic { Title = "Средний балл", Value = $"{averageScore}%", Icon = "📊" },
                new UserStatistic { Title = "Освоено слов", Value = masteredWords.ToString(), Icon = "✅" },
                new UserStatistic { Title = "Практиковано слов", Value = practicedWords.ToString(), Icon = "🔄" },
                new UserStatistic { Title = "Текущая серия", Value = user.StreakDays.ToString() + " дней", Icon = "🔥" },
                new UserStatistic { Title = "Всего повторений", Value = totalPractice.ToString(), Icon = "🔁" },
                new UserStatistic { Title = "Всего опыта", Value = user.Experience.ToString() + " XP", Icon = "⭐" },
                new UserStatistic { Title = "Пройдено времени", Value = user.TotalTimeMinutes.ToString() + " мин", Icon = "⏱️" }
            };
        }

        public List<Achievement> GetUserAchievements(int userId)
        {
            var userAchievements = achievements.Where(a => a.UserId == userId).ToList();
            UpdateAchievements(userId);
            return userAchievements;
        }

        private void UpdateAchievements(int userId)
        {
            var user = GetUserById(userId);
            if (user == null) return;

            var userProgress = lessonProgress.Where(lp => lp.UserId == userId).ToList();
            int totalWordsLearned = user.WordPracticeCount.Values.Sum();
            int perfectLessons = userProgress.Count(lp => lp.Score == 100);

            // Обновляем достижения
            foreach (var achievement in achievements.Where(a => a.UserId == userId && !a.IsUnlocked))
            {
                bool shouldUnlock = false;

                switch (achievement.Name)
                {
                    case "Первые шаги":
                        shouldUnlock = user.CompletedLessonIds.Count >= achievement.RequiredValue;
                        break;
                    case "Словарный запас":
                        shouldUnlock = totalWordsLearned >= achievement.RequiredValue;
                        break;
                    case "Неделя практики":
                        shouldUnlock = user.StreakDays >= achievement.RequiredValue;
                        break;
                    case "Быстрый ученик":
                        // Для демо - разблокируем после 5 уроков
                        shouldUnlock = user.CompletedLessonIds.Count >= 5;
                        break;
                    case "Эксперт перевода":
                        shouldUnlock = totalWordsLearned >= achievement.RequiredValue;
                        break;
                    case "Перфекционист":
                        shouldUnlock = perfectLessons >= achievement.RequiredValue;
                        break;
                    case "Полиглот":
                        shouldUnlock = totalWordsLearned >= achievement.RequiredValue;
                        break;
                    case "Мастер серий":
                        shouldUnlock = user.StreakDays >= achievement.RequiredValue;
                        break;
                }

                if (shouldUnlock)
                {
                    achievement.IsUnlocked = true;
                    achievement.UnlockedDate = DateTime.Now;
                }
            }
        }

        public List<string> GetRecentlyLearnedWords(int userId, int count = 5)
        {
            var user = GetUserById(userId);
            if (user == null) return new List<string>();

            return words
                .Where(w => user.WordPracticeCount.ContainsKey(w.Id) && user.WordPracticeCount[w.Id] > 0)
                .OrderByDescending(w => w.LastPracticed)
                .ThenByDescending(w => user.WordPracticeCount[w.Id])
                .Take(count)
                .Select(w => $"{w.English} - {w.Russian}")
                .ToList();
        }

        public List<Word> GetWordsForLesson(int lessonId)
        {
            if (lessonId == 0)
                return words.ToList();

            return words.Where(w => w.LessonId == lessonId).ToList();
        }

        public List<Lesson> GetAvailableLessons()
        {
            return lessons.OrderBy(l => l.OrderIndex).ToList();
        }

        public void UpdateWordPractice(int wordId, int userId)
        {
            var word = words.FirstOrDefault(w => w.Id == wordId);
            var user = GetUserById(userId);

            if (word != null && user != null)
            {
                word.TimesPracticed++;
                word.LastPracticed = DateTime.Now;

                // Обновляем уровень мастерства
                if (word.TimesPracticed >= 20)
                    word.MasteryLevel = 3;
                else if (word.TimesPracticed >= 10)
                    word.MasteryLevel = 2;
                else
                    word.MasteryLevel = 1;

                // Обновляем статистику пользователя
                if (!user.WordPracticeCount.ContainsKey(wordId))
                {
                    user.WordPracticeCount[wordId] = 1;
                    user.WordsLearned++;
                }
                else
                {
                    user.WordPracticeCount[wordId]++;
                }

                // Проверяем достижения
                UpdateAchievements(userId);
            }
        }

        public void UpdateUserProgress(int userId, int xpEarned, int wordsLearned = 0, int minutesSpent = 0)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                user.Experience += xpEarned;
                user.WordsLearned += wordsLearned;
                user.TotalTimeMinutes += minutesSpent;
                user.LastPracticeDate = DateTime.Now;

                // Обновляем уровень (каждые 500 XP)
                int newLevel = user.Experience / 500 + 1;
                if (newLevel > user.Level)
                {
                    user.Level = newLevel;
                }

                // Обновляем серию дней
                UpdateStreak(userId);
            }
        }

        public void UpdateStreak(int userId)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                TimeSpan timeSinceLastPractice = DateTime.Now - user.LastPracticeDate;

                if (timeSinceLastPractice.TotalDays <= 2) // Если занимался вчера или сегодня
                {
                    if (timeSinceLastPractice.TotalDays >= 1)
                    {
                        user.StreakDays++; // Увеличиваем серию
                    }
                }
                else
                {
                    user.StreakDays = 1; // Сбрасываем серию
                }
            }
        }

        public bool SaveLessonResult(int userId, int lessonId, int score, bool isCompleted, int minutesSpent = 0)
        {
            var lesson = lessons.FirstOrDefault(l => l.Id == lessonId);
            var user = GetUserById(userId);

            if (lesson != null && user != null)
            {
                // Обновляем прогресс урока
                var progress = lessonProgress.FirstOrDefault(lp => lp.UserId == userId && lp.LessonId == lessonId);
                if (progress == null)
                {
                    progress = new LessonProgress
                    {
                        UserId = userId,
                        LessonId = lessonId,
                        Attempts = 1
                    };
                    lessonProgress.Add(progress);
                }
                else
                {
                    progress.Attempts++;
                }

                if (isCompleted)
                {
                    progress.IsCompleted = true;
                    progress.Score = Math.Max(progress.Score, score);
                    progress.CompletedDate = DateTime.Now;

                    // Обновляем урок для этого пользователя
                    if (!user.CompletedLessonIds.Contains(lessonId))
                    {
                        user.CompletedLessonIds.Add(lessonId);
                        user.CompletedLessons++;
                    }

                    // Начисляем опыт
                    int baseXP = lesson.ExperienceReward;
                    int bonusXP = (int)(baseXP * (score / 100.0));
                    int totalXP = baseXP + bonusXP;

                    UpdateUserProgress(userId, totalXP, lesson.RequiredWords, minutesSpent);
                    UpdateAchievements(userId);

                    return true;
                }
            }
            return false;
        }

        public int GetUserTotalWordsPracticed(int userId)
        {
            var user = GetUserById(userId);
            return user?.WordPracticeCount.Values.Sum() ?? 0;
        }

        public int GetUserMasteredWordsCount(int userId)
        {
            var user = GetUserById(userId);
            if (user == null) return 0;

            return words.Count(w =>
                user.WordPracticeCount.ContainsKey(w.Id) &&
                user.WordPracticeCount[w.Id] >= 10);
        }

        public double GetUserAverageScore(int userId)
        {
            var userScores = lessonProgress
                .Where(lp => lp.UserId == userId && lp.IsCompleted)
                .Select(lp => lp.Score)
                .ToList();

            return userScores.Count > 0 ? userScores.Average() : 0;
        }

        public List<User> SearchUsers(string searchText)
        {
            // ИСПРАВЛЕНИЕ: используем IndexOf вместо Contains с параметрами
            return users
                .Where(u => u.Username.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           u.Email.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderByDescending(u => u.Experience)
                .ToList();
        }

        public bool ChangePassword(int userId, string newPassword)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                // В реальном приложении здесь должно быть хеширование пароля
                user.PasswordHash = newPassword;
                return true;
            }
            return false;
        }
    }
}