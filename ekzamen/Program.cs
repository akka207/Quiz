using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace ekzamen
{
    public enum MenuType
    {
        LogIn,
        SignIn,
        MainMenu,
        Quiz,
        EditQuiz,
        UserQuizResults,
        Top20Results,
        Settings,
        Exit
    }

    internal class Program
    {
        public static MenuType Menu = MenuType.LogIn;
        public static User currentUser = null;
        public static QuizTopic QuizTopic = null;
        static void Main(string[] args)
        {
            UserManager.Instance.Setup();
            QuizManager.Instance.Setup();
            ResultManager.Instance.Setup();


            while (true)
            {
                Console.CursorVisible = false;
                ConsoleManager.Instance.ClearConsole();
                switch (Menu)
                {
                    case MenuType.LogIn:
                        LogIn();
                        break;
                    case MenuType.SignIn:
                        SignUp();
                        break;
                    case MenuType.MainMenu:
                        MainMenu();
                        break;
                    case MenuType.Quiz:
                        Quiz();
                        break;
                    case MenuType.EditQuiz:
                        EditQuiz();
                        break;
                    case MenuType.UserQuizResults:
                        CurrentUserResults();
                        break;
                    case MenuType.Top20Results:
                        Top20Results();
                        break;
                    case MenuType.Settings:
                        Settings();
                        break;
                    case MenuType.Exit:
                        return;
                }
            }
        }
        static void LogIn()
        {
            Cursor cursor = new Cursor(ConsoleColor.White, CursorType.Right, new Point(12, 4), new Point(12, 11));
            Text text1 = new Text(new Point(12, 3), ConsoleColor.White, "Log In");
            Text text2 = new Text(new Point(14, 4), ConsoleColor.White, "Login:");
            Text text3 = new Text(new Point(14, 5), ConsoleColor.White, "Password:");
            Text text4 = new Text(new Point(14, 6), ConsoleColor.White, "Enter");
            Text text5 = new Text(new Point(14, 8), ConsoleColor.White, "Sign In");
            Text text6 = new Text(new Point(14, 10), ConsoleColor.White, "Exit");
            Message ErrorText = new Message(ConsoleColor.Red, "ErrorText"); ErrorText.SetActive(false);
            Entry entry1 = new Entry(new Point(24, 4), ConsoleColor.White, 20);
            Entry entry2 = new Entry(new Point(24, 5), ConsoleColor.White, 20, EntryType.StarsBlur);
            ConsoleManager.Instance.AddElements(new List<IConsoleElement>() { cursor, ErrorText, text1, text2, text3, text4, text5, text6, entry1, entry2 });

            Action rule = () =>
            {
                switch (cursor.GetYState())
                {
                    case 0:
                        entry1.SetEntringActive(true);
                        entry2.SetEntringActive(false);
                        break;
                    case 1:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(true);
                        break;
                    default:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(false);
                        break;
                }
            };
            rule();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        cursor.MoveCursor(Point.UpVector(), rule);
                        break;
                    case ConsoleKey.DownArrow:
                        cursor.MoveCursor(Point.DownVector(), rule);
                        break;
                    case ConsoleKey.Backspace:
                        ConsoleManager.Instance.RemoveChar();
                        break;
                    case ConsoleKey.Enter:
                        switch (cursor.GetYState())
                        {
                            case 2:
                                User user = UserManager.Instance.FindUser(entry1.GetText(), entry2.GetText());
                                if (user == null)
                                {
                                    ErrorText.SetText("Incorect Login or Password");
                                    ErrorText.SetActive(true);
                                    ConsoleManager.Instance.UpdateConsole();
                                }
                                else
                                {
                                    currentUser = user;
                                    Menu = MenuType.MainMenu;
                                    return;
                                }
                                break;
                            case 4:
                                Menu = MenuType.SignIn;
                                return;
                            case 6:
                                Menu = MenuType.Exit;
                                return;
                        }
                        break;
                    default:
                        ConsoleManager.Instance.EnterChar(key.KeyChar);
                        break;
                }
            }
        }
        static void SignUp()
        {
            Cursor cursor = new Cursor(ConsoleColor.White, CursorType.Right, new Point(12, 4), new Point(12, 13));
            Text text1 = new Text(new Point(12, 3), ConsoleColor.White, "Sigh In");
            Text text2 = new Text(new Point(14, 4), ConsoleColor.White, "NickName:");
            Text text3 = new Text(new Point(14, 5), ConsoleColor.White, "Login:");
            Text text4 = new Text(new Point(14, 6), ConsoleColor.White, "Password:");
            Text text5 = new Text(new Point(14, 7), ConsoleColor.White, "BirthDay:");
            Text text6 = new Text(new Point(14, 8), ConsoleColor.White, "Enter");
            Text text7 = new Text(new Point(14, 10), ConsoleColor.White, "Log In");
            Text text8 = new Text(new Point(14, 12), ConsoleColor.White, "Exit");
            Message ErrorText = new Message(ConsoleColor.Red, "ErrorText"); ErrorText.SetActive(false);
            Message SuccesText = new Message(ConsoleColor.Green, "SuccesText"); SuccesText.SetActive(false);
            Entry entry1 = new Entry(new Point(24, 4), ConsoleColor.White, 20);
            Entry entry2 = new Entry(new Point(24, 5), ConsoleColor.White, 20, EntryType.Standart, c => Char.IsLetterOrDigit(c));
            Entry entry3 = new Entry(new Point(24, 6), ConsoleColor.White, 20);
            Entry entry4 = new Entry(new Point(24, 7), ConsoleColor.White, 10, EntryType.Standart, c => Char.IsDigit(c) || Char.IsSeparator(c));
            entry4.DefaultText = "dd mm yyyy";
            ConsoleManager.Instance.AddElements(new List<IConsoleElement>() { cursor, ErrorText, SuccesText, text1, text2, text3, text4, text5, text6, text7, text8, entry1, entry2, entry3, entry4 });

            Action rule = () =>
            {
                switch (cursor.GetYState())
                {
                    case 0:
                        entry1.SetEntringActive(true);
                        entry2.SetEntringActive(false);
                        entry3.SetEntringActive(false);
                        entry4.SetEntringActive(false);
                        break;
                    case 1:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(true);
                        entry3.SetEntringActive(false);
                        entry4.SetEntringActive(false);
                        break;
                    case 2:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(false);
                        entry3.SetEntringActive(true);
                        entry4.SetEntringActive(false);
                        break;
                    case 3:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(false);
                        entry3.SetEntringActive(false);
                        entry4.SetEntringActive(true);
                        break;
                    default:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(false);
                        entry3.SetEntringActive(false);
                        entry4.SetEntringActive(false);
                        break;
                }
            };
            rule();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        cursor.MoveCursor(Point.UpVector(), rule);
                        break;
                    case ConsoleKey.DownArrow:
                        cursor.MoveCursor(Point.DownVector(), rule);
                        break;
                    case ConsoleKey.Backspace:
                        ConsoleManager.Instance.RemoveChar();
                        break;
                    case ConsoleKey.Enter:
                        switch (cursor.GetYState())
                        {
                            case 4:
                                try
                                {
                                    UserManager.Instance.AddUser(new User(entry1.GetText(), entry2.GetText(), entry3.GetText(), DateTime.Parse(entry4.GetText())));

                                    SuccesText.SetText("Succesfully added!");
                                    SuccesText.SetActive(true);
                                }
                                catch (Exception ex)
                                {
                                    ErrorText.SetText(ex.Message);
                                    ErrorText.SetActive(true);
                                }
                                finally
                                {
                                    ConsoleManager.Instance.UpdateConsole();
                                }
                                break;
                            case 6:
                                Menu = MenuType.LogIn;
                                return;
                            case 8:
                                Menu = MenuType.Exit;
                                return;
                        }
                        break;
                    default:
                        ConsoleManager.Instance.EnterChar(key.KeyChar);
                        break;
                }
            }
        }
        static void MainMenu()
        {
            QuizManager.Instance.SetupTopics();
            Cursor cursor = new Cursor(ConsoleColor.White, CursorType.Right, new Point(20, 4), new Point(20, 11));
            Text text1 = new Text(new Point(20, 2), ConsoleColor.DarkGray, $"User: {currentUser.NickName}");
            Text text2 = new Text(new Point(20, 3), ConsoleColor.White, "Main Menu");
            Text text3 = new Text(new Point(22, 4), ConsoleColor.White, "Start Quiz");
            Text text4 = new Text(new Point(22, 5), ConsoleColor.White, "My Results");
            Text text5 = new Text(new Point(22, 6), ConsoleColor.White, "Top 20");
            Text text6 = new Text(new Point(22, 7), ConsoleColor.White, "Settings");
            Text text7 = new Text(new Point(22, 8), ConsoleColor.White, "Quiz Editing");
            Text text8 = new Text(new Point(22, 9), ConsoleColor.White, "Logout");
            Text text9 = new Text(new Point(22, 10), ConsoleColor.White, "Logout end Exit");

            ConsoleManager.Instance.AddElements(new List<IConsoleElement>() { text1, text2, text3, text4, text5, text6, text7, text8, text9, cursor });

            while (true)
            {
                if (QuizTopic != null)
                {
                    Menu = MenuType.Quiz;
                    return;
                }
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        cursor.MoveCursor(Point.UpVector());
                        break;
                    case ConsoleKey.DownArrow:
                        cursor.MoveCursor(Point.DownVector());
                        break;
                    case ConsoleKey.Enter:
                        switch (cursor.GetYState())
                        {
                            case 0: // Start Quiz
                                cursor.SetEnable(false);
                                int i = 0;
                                QuizTopic topic = QuizManager.Instance.Topics[i];
                                bool isEnteringTopic = true;
                                Text topicText = new Text(new Point(40, 4), ConsoleColor.White, $"<  {topic.Name}  >");
                                ConsoleManager.Instance.AddElements(topicText);
                                while (isEnteringTopic)
                                {
                                    key = Console.ReadKey(true);
                                    switch (key.Key)
                                    {
                                        case ConsoleKey.LeftArrow:
                                            if (i > 0)
                                                topicText.SetText($"<  {QuizManager.Instance.Topics[--i].Name}  >");
                                            else
                                            {
                                                isEnteringTopic = false;
                                                ConsoleManager.Instance.DeleteElement(topicText);
                                            }
                                            break;
                                        case ConsoleKey.RightArrow:
                                            i = i < QuizManager.Instance.Topics.Count - 1 ? i + 1 : QuizManager.Instance.Topics.Count - 1;
                                            topicText.SetText($"<  {QuizManager.Instance.Topics[i].Name}  >");
                                            break;
                                        case ConsoleKey.Enter:
                                            QuizTopic = QuizManager.Instance.Topics[i];
                                            isEnteringTopic = false;
                                            break;
                                    }
                                    ConsoleManager.Instance.UpdateConsole();
                                    topicText.SetText($"<  {QuizManager.Instance.Topics[i].Name}  >");
                                }
                                cursor.SetEnable(true);
                                break;
                            case 1:
                                Menu = MenuType.UserQuizResults;
                                return;
                            case 2:
                                Menu = MenuType.Top20Results;
                                return;
                            case 3:
                                Menu = MenuType.Settings;
                                return;
                            case 4:
                                Menu = MenuType.EditQuiz;
                                return;
                            case 5:
                                currentUser = null;
                                Menu = MenuType.LogIn;
                                return;
                            case 6:
                                currentUser = null;
                                Menu = MenuType.Exit;
                                return;
                        }
                        break;
                }
            }
        }
        static void Settings()
        {
            Cursor cursor = new Cursor(ConsoleColor.White, CursorType.Right, new Point(20, 3), new Point(20, 9));
            Message ErrorMessage = new Message(ConsoleColor.Red, "ErrorMessage"); ErrorMessage.SetActive(false);
            Message SuccesMessage = new Message(ConsoleColor.Green, "SuccesMessage"); SuccesMessage.SetActive(false);
            Text text1 = new Text(new Point(20, 2), ConsoleColor.DarkGray, $"Login: {currentUser.NickName}");
            Text text2 = new Text(new Point(22, 3), ConsoleColor.White, "NickName: ");
            Text text3 = new Text(new Point(22, 4), ConsoleColor.White, "Password: ");
            Text text4 = new Text(new Point(22, 5), ConsoleColor.White, "Birthday: ");
            Text text5 = new Text(new Point(22, 7), ConsoleColor.White, "Save");
            Text text6 = new Text(new Point(22, 8), ConsoleColor.White, "Main Menu");
            Entry entry1 = new Entry(new Point(35, 3), ConsoleColor.White, 20); entry1.SetText(currentUser.NickName);
            Entry entry2 = new Entry(new Point(35, 4), ConsoleColor.White, 20); entry2.SetText(currentUser.Password);
            Entry entry3 = new Entry(new Point(35, 5), ConsoleColor.White, 10, EntryType.Standart, c => Char.IsDigit(c) || Char.IsSeparator(c)); entry3.SetText($"{currentUser.Birthday.Day} {currentUser.Birthday.Month} {currentUser.Birthday.Year}");

            ConsoleManager.Instance.AddElements(new List<IConsoleElement>() { ErrorMessage, SuccesMessage, text1, text2, text3, text4, text5, text6, entry1, entry2, entry3, cursor });
            Action rule = () =>
            {
                switch (cursor.GetYState())
                {
                    case 0:
                        entry1.SetEntringActive(true);
                        entry2.SetEntringActive(false);
                        entry3.SetEntringActive(false);
                        break;
                    case 1:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(true);
                        entry3.SetEntringActive(false);
                        break;
                    case 2:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(false);
                        entry3.SetEntringActive(true);
                        break;
                    default:
                        entry1.SetEntringActive(false);
                        entry2.SetEntringActive(false);
                        entry3.SetEntringActive(false);
                        break;
                }
            };
            rule();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        cursor.MoveCursor(Point.UpVector(), rule);
                        break;
                    case ConsoleKey.DownArrow:
                        cursor.MoveCursor(Point.DownVector(), rule);
                        break;
                    case ConsoleKey.Enter:
                        switch (cursor.GetYState())
                        {
                            case 4:
                                try
                                {
                                    UserManager.Instance.CheckUser(new User(entry1.GetText(), currentUser.Login, entry2.GetText(), DateTime.Parse(entry3.GetText())), false, true);

                                    UserManager.Instance.RemoveUser(currentUser);
                                    currentUser.NickName = entry1.GetText();
                                    currentUser.Password = entry2.GetText();
                                    currentUser.Birthday = DateTime.Parse(entry3.GetText());
                                    UserManager.Instance.AddUser(currentUser);
                                    SuccesMessage.SetText("Saved Succesfull");
                                    SuccesMessage.SetActive(true);
                                }
                                catch (Exception ex)
                                {
                                    ErrorMessage.SetText(ex.Message);
                                    ErrorMessage.SetActive(true);
                                }
                                finally
                                {
                                    ConsoleManager.Instance.UpdateConsole();
                                }
                                break;
                            case 5:
                                Menu = MenuType.MainMenu;
                                return;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        ConsoleManager.Instance.RemoveChar();
                        break;
                    default:
                        ConsoleManager.Instance.EnterChar(key.KeyChar);
                        break;
                }
            }
        }
        static void Quiz()
        {
            int result = 0;
            int all = QuizTopic.quizzes.Count;
            DateTime startTime = DateTime.Now;

            Result res = new Result();
            res.UserName = currentUser.NickName;
            res.TopicName = QuizTopic.Name + "";
            res.All = all;

            {       // QUIZ
                int curentQuiz = 0;
                bool isEnter = false;
                List<IConsoleElement> flags = null;

                Text text1 = new Text(new Point(20, 2), ConsoleColor.DarkGray, $"User: {currentUser.NickName}");
                Text text2 = new Text(new Point(20, 3), ConsoleColor.White, "Quiz");
                QuizElement quiz = new QuizElement(new Point(22, 4), ConsoleColor.White, QuizTopic.quizzes[curentQuiz]);
                Cursor cursor = new Cursor(ConsoleColor.White, CursorType.Right, new Point(22, 8), new Point(22, 8 + QuizTopic.quizzes[curentQuiz].Answers.Count));

                ConsoleManager.Instance.AddElements(new List<IConsoleElement>() { cursor, text1, text2, quiz });

                while (QuizTopic != null)
                {
                    if (isEnter)
                    {
                        if (curentQuiz == QuizTopic.quizzes.Count - 1)
                        {
                            QuizTopic = null;
                            break;
                        }
                        else
                        {
                            curentQuiz++;
                            quiz.SetQuiz(QuizTopic.quizzes[curentQuiz]);
                            cursor.DownrightBorder = new Point(cursor.DownrightBorder.X, 8 + QuizTopic.quizzes[curentQuiz].Answers.Count + (QuizTopic.quizzes[curentQuiz].QuizType == QuizType.MultiChoice ? 2 : 0));
                            ConsoleManager.Instance.UpdateConsole();
                        }
                        if (QuizTopic.quizzes[curentQuiz].QuizType == QuizType.MultiChoice)
                        {
                            flags = new List<IConsoleElement>();
                            for (int i = 0; i < QuizTopic.quizzes[curentQuiz].Answers.Count; i++)
                            {
                                flags.Add(new Flag(new Point(25, 8 + i), ConsoleColor.White));
                            }
                            ConsoleManager.Instance.AddElements(flags);
                        }
                        else
                        {
                            ConsoleManager.Instance.DeleteElement(flags);
                        }
                        isEnter = false;
                    }
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            cursor.MoveCursor(Point.UpVector());
                            break;
                        case ConsoleKey.DownArrow:
                            cursor.MoveCursor(Point.DownVector());
                            break;
                        case ConsoleKey.Enter:
                            if (QuizTopic.quizzes[curentQuiz].QuizType == QuizType.OneChoice)
                            {
                                if (QuizTopic.quizzes[curentQuiz].Answers[cursor.GetYState()].Item2 == true)
                                {
                                    result++;
                                }

                                isEnter = true;
                            }
                            else if (QuizTopic.quizzes[curentQuiz].QuizType == QuizType.MultiChoice)
                            {
                                if (cursor.GetYState() == QuizTopic.quizzes[curentQuiz].Answers.Count + 1)
                                {
                                    bool isCorrect = true;
                                    for (int i = 0; i < QuizTopic.quizzes[curentQuiz].Answers.Count; i++)
                                    {
                                        Flag flag = flags[i] as Flag;
                                        if (QuizTopic.quizzes[curentQuiz].Answers[i].Item2 != flag.State)
                                        {
                                            isCorrect = false;
                                            break;
                                        }
                                    }
                                    if (isCorrect)
                                    {
                                        result++;
                                    }
                                    isEnter = true;
                                    ConsoleManager.Instance.UpdateConsole();
                                }
                                else if (cursor.GetYState() < QuizTopic.quizzes[curentQuiz].Answers.Count)
                                {
                                    Flag flag = flags[cursor.GetYState()] as Flag;
                                    flag.State = !flag.State;
                                    flag.Print();
                                }
                            }
                            break;
                    }
                }
            }
            {       // RESULTS
                res.Claimed = result;
                res.CompletionTime = DateTime.Now - startTime;
                res.DoneDate = DateTime.Now;

                ResultManager.Instance.AddResult(res);
                currentUser.AddResult(res);

                ConsoleManager.Instance.ClearConsole();

                Text text1 = new Text(new Point(20, 2), ConsoleColor.DarkGray, $"User: {currentUser.NickName}");
                Text text2 = new Text(new Point(20, 3), ConsoleColor.White, "Quiz Results");
                Text text3 = new Text(new Point(22, 4), ConsoleColor.Green, $"Correct answers {result}/{all}");
                Text text4 = new Text(new Point(20, 5), ConsoleColor.White, "-> Back to Main Menu");

                ConsoleManager.Instance.AddElements(new List<IConsoleElement>() { text1, text2, text3, text4 });
                while (true)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        Menu = MenuType.MainMenu;
                        return;
                    }
                }
            }
        }
        static void EditQuiz()
        {
            QuizTopic enteredTopic = QuizTopic;
            Quiz enteredQuiz = null;

            {       //Enter Topic
                Cursor cursor = new Cursor(ConsoleColor.White, CursorType.Right, new Point(20, 5), new Point(20, 6 + QuizManager.Instance.Topics.Count));
                Text text1 = new Text(new Point(20, 2), ConsoleColor.DarkGray, $"User: {currentUser.NickName}");
                Text text2 = new Text(new Point(20, 3), ConsoleColor.White, "Quiz Editing");
                Text text3 = new Text(new Point(21, 4), ConsoleColor.White, "Choose topic");
                Text text4 = new Text(new Point(22, 5 + QuizManager.Instance.Topics.Count), ConsoleColor.White, "+ New topic");
                Text text5 = new Text(new Point(20, 6 + QuizManager.Instance.Topics.Count), ConsoleColor.White, "Press ESC to go to Main Menu");
                Entry entry = new Entry(new Point(40, 5 + QuizManager.Instance.Topics.Count), ConsoleColor.White, 20);
                Message message = new Message(ConsoleColor.Red, "Name is too short"); message.SetActive(false);

                List<IConsoleElement> texts = new List<IConsoleElement>();
                for (int i = 0; i < QuizManager.Instance.Topics.Count; i++)
                {
                    texts.Add(new Text(new Point(22, 5 + i), ConsoleColor.White, QuizManager.Instance.Topics[i].Name));
                }
                ConsoleManager.Instance.AddElements(texts);
                ConsoleManager.Instance.AddElements(new List<IConsoleElement>() { cursor, text1, text2, text3, text4, text5, entry, message });

                Action rule = () =>
                {
                    if (cursor.GetYState() == QuizManager.Instance.Topics.Count)
                        entry.SetEntringActive(true);
                    else
                        entry.SetEntringActive(false);
                };
                rule();

                while (enteredTopic == null)
                {

                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            Menu = MenuType.MainMenu;
                            QuizTopic = null;
                            return;
                        case ConsoleKey.UpArrow:
                            cursor.MoveCursor(Point.UpVector(), rule);
                            break;
                        case ConsoleKey.DownArrow:
                            cursor.MoveCursor(Point.DownVector(), rule);
                            break;
                        case ConsoleKey.Enter:
                            if (cursor.GetYState() < QuizManager.Instance.Topics.Count)
                                enteredTopic = QuizManager.Instance.Topics[cursor.GetYState()];
                            else if (entry.GetText().Length > 0)
                            {
                                QuizManager.Instance.AddTopic(new QuizTopic(entry.GetText()));
                                return;
                            }
                            else
                            {
                                message.SetActive(true);
                                ConsoleManager.Instance.UpdateConsole();
                            }
                            break;
                        case ConsoleKey.Backspace:
                            ConsoleManager.Instance.RemoveChar();
                            break;
                        default:
                            if (cursor.GetYState() == QuizManager.Instance.Topics.Count)
                            {
                                ConsoleManager.Instance.EnterChar(key.KeyChar);
                            }
                            break;
                    }
                }
            }
            ConsoleManager.Instance.ClearConsole();
            {       //Enter Quiz
                Cursor cursor = new Cursor(ConsoleColor.White, CursorType.Right, new Point(20, 5), new Point(20, 6 + enteredTopic.quizzes.Count));
                Message message = new Message(ConsoleColor.Red, "Access Denied"); message.SetActive(false);
                Text text1 = new Text(new Point(20, 2), ConsoleColor.DarkGray, $"User: {currentUser.NickName}");
                Text text2 = new Text(new Point(20, 3), ConsoleColor.White, "Quiz Editing");
                Text text3 = new Text(new Point(22, 4), ConsoleColor.White, "Choose Question");
                Text text4 = new Text(new Point(22, 5 + enteredTopic.quizzes.Count), ConsoleColor.White, "+ New Question");
                Text text5 = new Text(new Point(20, 6 + enteredTopic.quizzes.Count), ConsoleColor.White, "Press ESC to go to Choose Topic");
                Entry entry = new Entry(new Point(40, 5 + enteredTopic.quizzes.Count), ConsoleColor.White, 20);
                List<IConsoleElement> texts = new List<IConsoleElement>();
                for (int i = 0; i < enteredTopic.quizzes.Count; i++)
                {
                    texts.Add(new Text(new Point(22, 5 + i), ConsoleColor.White, enteredTopic.quizzes[i].Title));
                }
                ConsoleManager.Instance.AddElements(texts);
                ConsoleManager.Instance.AddElements(new List<IConsoleElement>() { message, cursor, text1, text2, text3, text4, text5, entry });

                Action rule = () =>
                {
                    if (cursor.GetYState() == enteredTopic.quizzes.Count)
                        entry.SetEntringActive(true);
                    else
                        entry.SetEntringActive(false);
                };
                rule();

                while (true)
                {

                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            return;
                        case ConsoleKey.UpArrow:
                            cursor.MoveCursor(Point.UpVector(), rule);
                            break;
                        case ConsoleKey.DownArrow:
                            cursor.MoveCursor(Point.DownVector(), rule);
                            break;
                        case ConsoleKey.Enter:
                            if(enteredTopic.quizzes != null && enteredTopic.quizzes.Count != 0 && enteredTopic.quizzes[cursor.GetYState()].Author != currentUser.NickName)
                            {
                                message.SetText("Access Denied");
                                message.SetActive(true);
                                ConsoleManager.Instance.UpdateConsole();
                                break;
                            }
                            if (cursor.GetYState() != enteredTopic.quizzes.Count)
                                enteredQuiz = enteredTopic.quizzes[cursor.GetYState()];
                            else if (entry.GetText() != "")
                            {
                                enteredQuiz = new Quiz();
                                enteredQuiz.Title = entry.GetText();
                                enteredTopic.AddQuiz(enteredQuiz);
                                QuizManager.Instance.SetupTopics();
                            }
                            else
                            {
                                message.SetText("Title is too short");
                                message.SetActive(true);
                                ConsoleManager.Instance.UpdateConsole();
                            }
                            return;
                        case ConsoleKey.Backspace:
                            ConsoleManager.Instance.RemoveChar();
                            break;
                        default:
                            if (cursor.GetYState() == enteredTopic.quizzes.Count)
                            {
                                ConsoleManager.Instance.EnterChar(key.KeyChar);
                            }
                            break;
                    }
                }
            }
            ConsoleManager.Instance.ClearConsole();
            {       // Editing Question
                Cursor cursor = new Cursor(ConsoleColor.White, CursorType.Right, new Point(20, 5), new Point(20, 5 + QuizManager.Instance.Topics.Count - 1));

                Text text1 = new Text(new Point(20, 2), ConsoleColor.White, "Question Editing");
                Text text2 = new Text(new Point(22, 3), ConsoleColor.White, "Title");
                Text text3 = new Text(new Point(22, 4), ConsoleColor.White, "Author");
                Text text4 = new Text(new Point(22, 5), ConsoleColor.White, "Question");
                Text text5 = new Text(new Point(22, 8), ConsoleColor.White, "Answers:");

                while(true)
                {

                }
            }
        }
        static void CurrentUserResults()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            Text text1 = new Text(new Point(0, 0), ConsoleColor.DarkGreen, "|User Name           |Quiz Name           |Result    |Completion Time|Passing Date       |");
            List<IConsoleElement> elements = new List<IConsoleElement>();

            for (int i = 0; i < currentUser.Results.Count && i < 20; i++)
            {
                elements.Add(new ResultElement(new Point(0, i + 1), ConsoleColor.White, currentUser.Results[i]));
            }

            Text text2 = new Text(new Point(0, elements.Count + 1), ConsoleColor.White, "-> Exit to Main Menu");

            elements.Add(text1);
            elements.Add(text2);
            ConsoleManager.Instance.AddElements(elements);

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Menu = MenuType.MainMenu;
                    return;
                }
            }
        }
        static void Top20Results()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            Text text1 = new Text(new Point(0, 0), ConsoleColor.DarkGreen, "|User Name           |Quiz Name           |Result    |Completion Time|Passing Date       |");
            List<IConsoleElement> elements = new List<IConsoleElement>();

            for (int i = 0; i < ResultManager.Instance.Results.Count; i++)
            {
                elements.Add(new ResultElement(new Point(0, i + 1), ConsoleColor.White, ResultManager.Instance.Results[i]));
            }

            Text text2 = new Text(new Point(0, elements.Count + 1), ConsoleColor.White, "-> Exit to Main Menu");

            elements.Add(text1);
            elements.Add(text2);
            ConsoleManager.Instance.AddElements(elements);

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Menu = MenuType.MainMenu;
                    return;
                }
            }
        }
    }
}