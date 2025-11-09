using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NT106_BattleshipClient
{
    public static class Session
    {
        public static string Username = null;
        public static bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Username); }
        }
        public static void Logout()
        {
            Username = null;
        }
    }
}
