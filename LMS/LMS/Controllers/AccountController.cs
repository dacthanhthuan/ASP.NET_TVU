using library_management_system.Data;
using LMS.Helpers;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        string hashedPassword = SecurityHelper.HashSHA256(password);

        string connStr = _context.Database.GetDbConnection().ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();

            SqlCommand cmd = new SqlCommand("dbo.sp_Login", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
        return View();
    }

    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        // 1. hash password
        string hashedPassword = SecurityHelper.HashSHA256(password);

        // 2. tạo user
        var user = new USER
        {
            Username = username,
            Password = hashedPassword
        };

        // 3. lưu DB
        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }


}