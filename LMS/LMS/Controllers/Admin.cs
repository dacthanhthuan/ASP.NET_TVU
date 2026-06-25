using library_management_system.Data;
using LMS.Helpers;
using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;


public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Food_Management()
    {
        var foods = _context.M_CATEGORY.ToList();
        return View(foods);
    }
}