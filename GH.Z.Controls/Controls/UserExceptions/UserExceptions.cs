using System;

namespace GH.UserExceptions
{

    public class UserWantExit : Exception
    {
        public UserWantExit() : base("Пользователь отказался от входа в программу") { }
    }


}
