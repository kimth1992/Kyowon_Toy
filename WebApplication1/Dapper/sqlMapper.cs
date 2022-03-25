using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using WebApplication1.Controllers;
using WebApplication1.Models.Login;

namespace Dapper
{
    internal class sqlMapper
    {
        internal static IActionResult Execute(MySqlConnection conn, object sql, LoginController loginController)
        {
            throw new NotImplementedException();
        }

        internal static int Execute(MySqlConnection conn, string sql, UserModel userModel)
        {
            throw new NotImplementedException();
        }
    }
}