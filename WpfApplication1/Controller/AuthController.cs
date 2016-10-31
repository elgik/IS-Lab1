using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;
using WpfApplication1.Domain;

namespace WpfApplication1.Controller
{
    public class AuthController
    {
        private static Context db = new Context();       

        public static User CurrentUser { get; set; }

        public bool firstLogin { get; set; }

        public AuthController()
        {
            db.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
            firstLogin = LoadByLogin("Admin") == null;
        }

        public static void SaveDto(User entity)
        {
            
            db.Users.Add(entity);
            db.SaveChanges();
        }

        public static void UpdateDto(User entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
        
        public static string Autorization(string login, string pass)
        {
            string validation = null;
            var user = LoadByLogin(login);
            if (user != null)
            {
                if (user.Password != pass)
                    return validation = "Неправильный пароль";
                else if (user.isBlocked)
                    return validation = "Пользователь заблокирован";
            }
            else return validation = "Пользователь не найден";
            CurrentUser = user;            
            return validation;
        }

        public static void DeleteDto(User entity)
        {
            db.Users.Remove(entity);
            db.SaveChanges();
        }

        public static User LoadByLogin(string login)
        {
            return db.Users
                .Where(u => u.Login == login)
                .SingleOrDefault();
        }

        public static IEnumerable<User> GetAllUsers()
        {
            return db.Users.Select(u => u).ToList();
        }
    }
}
