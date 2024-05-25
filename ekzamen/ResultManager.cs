using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ekzamen
{
    internal class Result
    {
        public string UserName { get; set; }
        public string TopicName { get; set; }
        public int Claimed { get; set; }
        public int All { get; set; }
        public TimeSpan CompletionTime { get; set; }
        public DateTime DoneDate { get; set; }

        public Result()
        {

        }
        public Result(string topicName, int claimed, int all, TimeSpan completionTime, DateTime doneDate)
        {
            TopicName = topicName;
            Claimed = claimed;
            All = all;
            CompletionTime = completionTime;
            DoneDate = doneDate;
        }
        public double GetResPercent()
        {
            return (double)Claimed / (double)All;
        }
    }

    internal class ResultElement : IConsoleElement
    {
        public bool IsActive { get; protected set; } = true;
        public Point Position { get; private set; }
        public ConsoleColor Color { get; private set; }

        private Result result;


        public ResultElement(Point position, ConsoleColor color, Result result)
        {
            Position = position;
            Color = color;
            this.result = result;
        }

        public void Print()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write($"|{result.UserName}");
            Console.SetCursorPosition(Position.X + 21, Position.Y);
            Console.Write($"|{result.TopicName}");
            Console.SetCursorPosition(Position.X + 42, Position.Y);
            Console.Write($"|{result.Claimed}/{result.All}");
            Console.SetCursorPosition(Position.X + 53, Position.Y);
            Console.Write($"|{result.CompletionTime}");
            Console.SetCursorPosition(Position.X + 62, Position.Y);
            Console.Write($"       |{result.DoneDate}|");
        }
        public void SetResult(Result result)
        {
            this.result = result;
        }
        public void SetActive(bool state)
        {
            IsActive = state;
        }
    }

    internal sealed class ResultManager
    {
        #region Singleton
        public static ResultManager Instance { get; } = new ResultManager();
        private ResultManager()
        {

        }
        static ResultManager()
        {

        }
        #endregion

        public List<Result> Results;
        private string path;

        public void Setup(string path = "")
        {
            if (path == "")
                this.path = Directory.GetCurrentDirectory() + "\\ResultsDataBase.txt";
            else
                this.path = path;
            Results = JsonConvert.DeserializeObject<List<Result>>(File.ReadAllText(this.path));
            if (Results == null) Results = new List<Result>();
            Sort();
        }

        public void AddResult(Result result)
        {
            Results.Add(result);
            Sort();
            SaveResultsAsync();
        }
        public void Sort()
        {
            Results = Results.OrderBy(r => r.CompletionTime).ToList();
            Results = Results.OrderByDescending(r => r.GetResPercent()).ToList();
        }
        private async void SaveResultsAsync()
        {
            await Task.Run(() => { File.WriteAllText(path, JsonConvert.SerializeObject(Results, Formatting.Indented)); });
        }
    }
}