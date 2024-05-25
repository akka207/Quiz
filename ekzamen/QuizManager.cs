using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ekzamen
{
    internal enum QuizType
    {
        OneChoice,
        MultiChoice
    }

    internal class Quiz
    {
        public QuizType QuizType { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Question { get; set; }
        public List<Tuple<string, bool>> Answers { get; set; }

        public Quiz(QuizType quizType, string title, string author, string question, List<Tuple<string, bool>> answers)
        {
            QuizType = quizType;
            Title = title;
            Author = author;
            Question = question;
            Answers = answers;
        }
        public Quiz()
        {
            
        }
    }

    internal class QuizTopic
    {
        public string Name { get; set; }
        public List<Quiz> quizzes = new List<Quiz>();

        public QuizTopic(string name)
        {
            Name = name;
        }

        public void AddQuiz(Quiz quiz)
        {
            if (!quizzes.Any(q => q.Title == quiz.Title)) quizzes.Add(quiz);
        }
        public void RemoveQuiz(Quiz quiz)
        {
            quizzes.Remove(quiz);
        }
    }

    internal class QuizElement : IConsoleElement
    {
        public bool IsActive { get; protected set; } = true;
        public Point Position { get; private set; }
        public ConsoleColor Color { get; private set; }

        private Quiz quiz;


        public QuizElement(Point position, ConsoleColor color, Quiz quiz)
        {
            Position = position;
            Color = color;
            this.quiz = quiz;
        }

        public void Print()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(quiz.Title);
            Console.SetCursorPosition(Position.X, Position.Y + 1);
            Console.Write("by " + quiz.Author);
            Console.SetCursorPosition(Position.X, Position.Y + 2);
            if (quiz.Question.Length > 50)
                Console.Write(quiz.Question.Insert(50, "\n\t\t"));
            else
                Console.Write(quiz.Question); 
            for (int i = 0; i < quiz.Answers.Count; i++)
            {
                Console.SetCursorPosition(Position.X + 4, Position.Y + 4 + i);
                Console.Write(quiz.Answers[i].Item1);
            }
            if (quiz.QuizType == QuizType.MultiChoice)
            {
                Console.SetCursorPosition(Position.X + 3, Position.Y + 5 + quiz.Answers.Count);
                Console.Write("Enter");
            }
        }
        public void SetQuiz(Quiz quiz)
        {
            this.quiz = quiz;
        }
        public void SetActive(bool state)
        {
            IsActive = state;
        }
    }

    internal sealed class QuizManager
    {
        #region Singleton
        public static QuizManager Instance { get; } = new QuizManager();
        private QuizManager()
        {

        }
        static QuizManager()
        {

        }
        #endregion

        public List<QuizTopic> Topics = new List<QuizTopic>();
        private string path;

        public void Setup(string path = "")
        {
            if (path == "")
                this.path = Directory.GetCurrentDirectory() + "\\QizzesDataBase.txt";
            else
                this.path = path;
            Topics = JsonConvert.DeserializeObject<List<QuizTopic>>(File.ReadAllText(this.path));
            if (Topics == null) Topics = new List<QuizTopic>();
            SetupTopics();
        }

        public void AddTopic(QuizTopic topic)
        {
            QuizTopic top = Topics.Find(t => t.Name == topic.Name);
            if (top == null)
            {
                Topics.Add(topic);
            }
            else
            {
                foreach (var quiz in topic.quizzes)
                {
                    top.AddQuiz(quiz);
                }
            }
            Topics.RemoveAll(t=>t.Name == "Mixed");
            SaveTopics();
        }
        public void RemoveTopic(QuizTopic topic)
        {
            Topics.Remove(topic);
        }
        public void SetupTopics()
        {
            QuizTopic qt = new QuizTopic("Mixed");
            foreach (var topic in Topics)
            {
                Shuffle(topic.quizzes);
                qt.quizzes.AddRange(topic.quizzes);
            }
            Shuffle(qt.quizzes);
            if (qt.quizzes.Count > 20)
                qt.quizzes.RemoveRange(20, qt.quizzes.Count - 20);
            AddTopic(qt);
        }

        private void Shuffle(List<Quiz> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = new Random().Next(n + 1);
                Quiz value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        private void SaveTopics()
        {
            List<QuizTopic> toSave = new List<QuizTopic>();
            foreach (var item in Topics)
            {
                if (item.Name != "Mixed")
                {
                    toSave.Add(item);
                }
            }
            File.WriteAllText(path, JsonConvert.SerializeObject(toSave, Formatting.Indented));
        }
    }
}