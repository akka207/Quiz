using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ekzamen
{
    internal enum CursorType
    {
        Dot,
        Left,
        Right,
        Up,
        Down
    }
    internal enum EntryType
    {
        Standart,
        StarsBlur,
        Invisible
    }

    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public static Point LeftVector() { return new Point(-1, 0); }
        public static Point RightVector() { return new Point(1, 0); }
        public static Point UpVector() { return new Point(0, -1); }
        public static Point DownVector() { return new Point(0, 1); }
    }

    internal interface IConsoleElement
    {
        bool IsActive { get; }
        Point Position { get; }
        ConsoleColor Color { get; }
        void Print();
    }

    internal class Cursor : IConsoleElement
    {
        public bool IsActive { get; private set; } = true;
        public Point Position { get; private set; }
        public ConsoleColor Color { get; private set; }


        private Point upleftBorder;
        private Point downrightBorder;
        public Point UpleftBorder { get { return upleftBorder; } set { Position = new Point(value.X, value.Y); upleftBorder = value; } }
        public Point DownrightBorder { get { return downrightBorder; } set { Position = new Point(upleftBorder.X, upleftBorder.Y); downrightBorder = value; } }


        private CursorType cursorType;
        private bool isEnable = true;


        public Cursor(ConsoleColor Color, CursorType cursorType, Point upleftBorder, Point downrightBorder)
        {
            this.Position = new Point(upleftBorder.X, upleftBorder.Y);
            this.Color = Color;
            this.cursorType = cursorType;
            UpleftBorder = upleftBorder;
            DownrightBorder = downrightBorder;
        }
        public void MoveCursor(Point vector, Action rule = null)
        {
            if (isEnable)
            {
                bool flag = false;

                if (Position.X + vector.X >= UpleftBorder.X && Position.X + vector.X < DownrightBorder.X)
                { Position.X += vector.X; flag = true; }
                if (Position.Y + vector.Y >= UpleftBorder.Y && Position.Y + vector.Y < DownrightBorder.Y)
                { Position.Y += vector.Y; flag = true; }
                if (flag)
                {
                    ClearCursorArea();
                    Print();
                }

                rule?.Invoke();
            }
        }
        public void Print()
        {
            Console.ForegroundColor = Color;
            switch (cursorType)
            {
                case CursorType.Dot:
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write("■");
                    break;
                case CursorType.Left:
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write("<-");
                    break;
                case CursorType.Right:
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write("->");
                    break;
                case CursorType.Up:
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write("^");
                    break;
                case CursorType.Down:
                    Console.SetCursorPosition(Position.X, Position.Y);
                    Console.Write("V");
                    break;
            }
        }
        public int GetXState()
        {
            return Position.X - UpleftBorder.X;
        }
        public int GetYState()
        {
            return Position.Y - UpleftBorder.Y;
        }
        public void ChangeCursorType(CursorType type)
        {
            cursorType = type;
        }
        public void ClearCursorArea()
        {
            for (int i = UpleftBorder.Y; i < DownrightBorder.Y; i++)
            {
                for (int j = UpleftBorder.X; j <= DownrightBorder.X + (cursorType == CursorType.Left || cursorType == CursorType.Right ? 1 : 0); j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(' ');
                }
            }
        }
        public void SetActive(bool state)
        {
            IsActive = state;
        }
        public void SetEnable(bool state)
        {
            isEnable = state;
        }
    }
    internal class Text : IConsoleElement
    {
        public bool IsActive { get; protected set; } = true;
        public Point Position { get; private set; }
        public ConsoleColor Color { get; private set; }
        private string text;

        public Text(Point position, ConsoleColor color, string text)
        {
            Position = position;
            Color = color;
            this.text = text;
        }

        public void Print()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(text);
        }
        public void SetActive(bool state)
        {
            IsActive = state;
        }
        public void SetText(string text)
        {
            this.text = text;
        }
    }
    internal class Entry : IConsoleElement
    {
        public bool IsActive { get; private set; } = true;
        public Point Position { get; private set; }
        public ConsoleColor Color { get; private set; }
        public string DefaultText { get; set; } = "";

        private int length;
        private string text = "";
        private int pointer = 0;
        private bool isEntringActive = false;
        private EntryType entryType;
        private Predicate<char> rule;


        public Entry(Point position, ConsoleColor color, int length, EntryType entryType = 0, Predicate<char> rule = null)
        {
            Position = position;
            Color = color;
            this.length = length;
            this.entryType = entryType;
            this.rule = rule;
        }

        public void Print()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write("|");
            for (int i = Position.X; i < Position.X + length; i++)
            {
                if (i < Position.X + pointer)
                {
                    switch (entryType)
                    {
                        case EntryType.Standart:
                            Console.Write(text[i - Position.X]);
                            break;
                        case EntryType.StarsBlur:
                            Console.Write('*');
                            break;
                        case EntryType.Invisible:
                            Console.Write(' ');
                            break;
                    }
                }
                else
                {
                    if (DefaultText == "")
                        Console.Write('_');
                    else if (DefaultText.Length > i - Position.X)
                        Console.Write(DefaultText[i - Position.X]);
                }
            }
            Console.Write("|");
        }
        public void EnterLetter(char letter)
        {
            if (rule != null)
            {
                if (rule(letter))
                {
                    if (pointer < length)
                    {
                        text += letter;
                        pointer++;
                        Print();
                    }
                }
            }
            else if (Char.IsLetterOrDigit(letter) || Char.IsSymbol(letter) || Char.IsSeparator(letter))
            {
                if (pointer < length)
                {
                    text += letter;
                    pointer++;
                    Print();
                }
            }
        }
        public void EraseLatestLetter()
        {
            if (pointer > 0)
            {
                text = text.Remove(text.Length - 1, 1);
                pointer--;
            }
            Print();
        }
        public string GetText()
        {
            return text;
        }
        public void SetText(string text)
        {
            this.text = text;
            pointer = text.Length;
        }
        public void SetActive(bool state)
        {
            IsActive = state;
        }
        public void SetEntringActive(bool state)
        {
            isEntringActive = state;
        }
        public bool IsEntringActive()
        {
            return isEntringActive;
        }
    }
    internal class Message : Text
    {
        public static Message activeMessage = null;
        public Message(ConsoleColor color, string text) : base(new Point(0, 29), color, text)
        {

        }
        public new void SetActive(bool state)
        {
            if (activeMessage != null) activeMessage.IsActive = false;
            activeMessage = this;
            IsActive = state;
        }
    }
    internal class Flag : IConsoleElement
    {
        public bool IsActive { get; protected set; } = true;
        public Point Position { get; private set; }
        public ConsoleColor Color { get; private set; }

        public bool State = false;


        public Flag(Point position, ConsoleColor color)
        {
            Position = position;
            Color = color;
        }

        public void Print()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Position.X, Position.Y);
            Console.Write(State ? '█' : '░');
        }
        public void SetActive(bool state)
        {
            IsActive = state;
        }
    }

    internal sealed class ConsoleManager
    {
        #region Singleton
        public static ConsoleManager Instance { get; } = new ConsoleManager();
        private ConsoleManager()
        {

        }
        static ConsoleManager()
        {

        }
        #endregion


        private List<IConsoleElement> elements = new List<IConsoleElement>();


        public void UpdateConsole()
        {
            Console.Clear();
            foreach (IConsoleElement element in elements)
            {
                if (element.IsActive)
                    element.Print();
            }
        }
        public void AddElements(List<IConsoleElement> elements)
        {
            foreach (var item in elements)
                this.elements.Add(item);
            UpdateConsole();
        }
        public void AddElements(IConsoleElement element)
        {
            this.elements.Add(element);
            UpdateConsole();
        }
        public void DeleteElement(IConsoleElement element)
        {
            elements.Remove(element);
            UpdateConsole();
        }
        public void DeleteElement(List<IConsoleElement> elements)
        {
            if (elements != null)
            {
                foreach (var item in elements)
                {
                    this.elements.Remove(item);
                }
                UpdateConsole();
            }
        }
        public void ClearConsole()
        {
            elements.Clear();
        }
        public void EnterChar(char character)
        {
            foreach (var element in elements)
            {
                if (element is Entry)
                {
                    Entry curEntry = element as Entry;
                    if (curEntry.IsEntringActive())
                    {
                        curEntry.EnterLetter(character);
                    }
                }
            }
        }
        public void RemoveChar()
        {
            foreach (var element in elements)
            {
                if (element is Entry)
                {
                    Entry curEntry = element as Entry;
                    if (curEntry.IsEntringActive())
                    {
                        curEntry.EraseLatestLetter();
                    }
                }
            }
        }
    }
}