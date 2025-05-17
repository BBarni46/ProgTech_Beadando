using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando1.BELÉPÉS
{
    public static class UserSession
    {
        public static int Id { get; set; }
        public static string Username { get; set; } = string.Empty;
        public static decimal Balance { get; set; }

        public static bool IsLoggedIn => !string.IsNullOrEmpty(Username);

        public static void Clear()
        {
            Id = 0;
            Username = string.Empty;
            Balance = 0;
        }
    }
}
