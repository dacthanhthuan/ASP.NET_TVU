using library_management_system.Data;
using LMS.Helpers;
using LMS.Models;
using LMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;


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
        // kiểm tra rỗng trước
        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            TempData["Message"] = "Vui lòng nhập tài khoản và mật khẩu";
            TempData["Type"] = "error";
            return View();
        }

        // hash mật khẩu
        string hashedPassword = SecurityHelper.HashSHA256(password);

        // kết nối tới stored procedure
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

        TempData["Message"] = "Sai tài khoản hoặc mật khẩu";
        TempData["Type"] = "error";
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        // 1. kiểm tra rỗng
        if (model == null ||
            string.IsNullOrWhiteSpace(model.Username) ||
            string.IsNullOrWhiteSpace(model.Fullname) ||
            string.IsNullOrWhiteSpace(model.Password) ||
            string.IsNullOrWhiteSpace(model.Phone))
        {
            TempData["Message"] = "Các trường không được để trống";
            TempData["Type"] = "error";
            return View(model);
        }

        // kiểm tra password
        var passwordRegex = new Regex(
            @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$"
        );

        if (!passwordRegex.IsMatch(model.Password))
        {
            TempData["Message"] =
                "Mật khẩu phải có chữ, số và ký tự đặc biệt, tối thiểu 6 ký tự";

            TempData["Type"] = "error";

            return RedirectToAction("Register");
        }

        // 2. kiểm tra username đã tồn tại
        var exists = _context.M_USER
            .Any(x => x.Username == model.Username);

        if (exists)
        {
            TempData["Message"] = "Tên đăng nhập đã tồn tại";
            TempData["Type"] = "error";
            return View(model);
        }

        try
        {
            // 3. hash password
            string hashedPassword = SecurityHelper.HashSHA256(model.Password);

            // 4. tạo user
            var user = new USER
            {
                Username = model.Username.Trim(),
                Password = hashedPassword,
                Fullname = model.Fullname.Trim(),
                Phone = model.Phone.Trim()
            };

            // 5. lưu DB
            _context.M_USER.Add(user);
            _context.SaveChanges();

            TempData["Message"] = "Đăng ký thành công!";
            TempData["Type"] = "success";
            // 6. chuyển trang
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            // log nếu cần
            TempData["Message"] = "Có lỗi khi đăng ký, vui lòng thử lại";
            TempData["Type"] = "error";
            return View(model);
        }
    }


}