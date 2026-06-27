using LMS.Helpers;
using library_management_system.Data;
using LMS.Models;
using LMS.Models.DTO;
using LMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;


public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var username = HttpContext.Session.GetString("Username");

        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Account");
        }

        var result = await GetHomeData();

        return View(result);
    }

    public async Task<HomeDataDto> GetHomeData()
    {
        var result = new HomeDataDto();

        using (var connection = _context.Database.GetDbConnection())
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "sp_GetHomeData";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    // TABLE 1 - CATEGORY
                    var categories = new List<CATEGORY>();
                    while (await reader.ReadAsync())
                    {
                        categories.Add(new CATEGORY
                        {
                            CATEGORY_ID = reader.GetInt32(0),
                            CATEGORY_NAME = reader.GetString(1),
                            ICON = reader.GetString(2),
                            DESCRIPTION = reader.IsDBNull(3) ? null : reader.GetString(3),
                            IS_ACTIVE = reader.GetInt32(4)
                        });
                    }

                    result.Categories = categories;

                    // TABLE 2 - FOOD
                    await reader.NextResultAsync();

                    var foods = new List<FOOD>();
                    while (await reader.ReadAsync())
                    {
                        foods.Add(new FOOD
                        {
                            FOOD_ID = reader.GetInt32(0),
                            FOOD_NAME = reader.GetString(1),
                            DESCRIPTION = reader.IsDBNull(2) ? null : reader.GetString(2),
                            IMAGE_URL = reader.GetString(3),
                            CATEGORY_ID = reader.GetInt32(4),
                            COOK_TIME = reader.GetInt32(5),
                            IS_ACTIVE = reader.GetInt32(6)
                        });
                    }

                    result.Foods = foods;

                    // TABLE 3 - WEEKLY MENU
                    await reader.NextResultAsync();

                    var weeklyMenus = new List<WEEKLY_MENU>();

                    while (await reader.ReadAsync())
                    {
                        weeklyMenus.Add(new WEEKLY_MENU
                        {
                            ID = reader.GetInt32(0),
                            DAY_NAME = reader.GetString(1),
                            BREAKFAST = reader.IsDBNull(2) ? null : reader.GetString(2),
                            LUNCH = reader.IsDBNull(3) ? null : reader.GetString(3),
                            DINNER = reader.IsDBNull(4) ? null : reader.GetString(4)
                        });
                    }

                    result.WeeklyMenus = weeklyMenus;
                }
            }
        }

        return result;
    }

}