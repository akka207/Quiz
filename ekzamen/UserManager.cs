using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace ekzamen
{
    internal class User
    {
        public string NickName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public List<Result> Results { get; set; } = new List<Result>();
        public User(string nickName, string login, string password, DateTime birthday)
        {
            NickName = nickName;
            Login = login;
            Password = password;
            Birthday = birthday;
        }
        public void AddResult(Result result)
        {
            Results.Add(result);
            UserManager.Instance.ManualSave();
        }
    }

    internal sealed class UserManager
    {
        #region Singleton
        public static UserManager Instance { get; } = new UserManager();
        private UserManager()
        {

        }
        static UserManager()
        {

        }
        #endregion


        private List<User> users;
        private string path;

        public void Setup(string path = "")
        {
            if (path == "")
                this.path = Directory.GetCurrentDirectory() + "\\UsersDataBase.txt";
            else
                this.path = path;
            users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(this.path));
            if (users == null) users = new List<User>();
        }

        public void AddUser(User user)
        {
            CheckUser(user, true, true);
            users.Add(user);
            SaveUsersAsync();
        }
        public void RemoveUser(User user)
        {
            users.Remove(user);
        }
        public User FindUser(string login, string password)
        {
            return users?.Find(u => u.Login == login && u.Password == password);
        }
        public bool CheckUser(User user, bool checkLogin, bool checkPassword)
        {
            if(user.NickName.Length == 0) throw new Exception("Enter Nick Name");
            if(user.Login.Length == 0) throw new Exception("Enter Login");
            if (checkLogin && users.Any(u => u.Login == user.Login)) throw new Exception("Login already exists");
            if (checkPassword && user.Password.Length < 8) throw new Exception("Password is too short (less than 8 symbols)");
            return true;
        }
        public void ManualSave()
        {
            SaveUsersAsync();
        }
        private async void SaveUsersAsync()
        {
            await Task.Run(() => { File.WriteAllText(path, JsonConvert.SerializeObject(users, Formatting.Indented)); });
        }
    }
}