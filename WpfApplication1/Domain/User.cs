using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WpfApplication1
{
    public class User
    {
        public int Id { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Признак блокировки
        /// </summary>
        public bool isBlocked { get; set; }
        /// <summary>
        /// Признак ограничения пользователя
        /// </summary>
        public bool isRestricted { get; set; } 
    }
}
