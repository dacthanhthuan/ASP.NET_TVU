using library_management_system.Data;
using LMS.Models;
using LMS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public AdminController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // =========================
    // GET VIEW
    // =========================
    public IActionResult Food_Management()
    {
        var model = new FoodCreateViewModel
        {
            Food = new FOOD(),
            Categories = _context.M_CATEGORY.ToList(),
            Foods = _context.M_FOOD.ToList()
        };

        return View(model);
    }

    // =========================
    // ADD FOOD (AJAX)
    // =========================
    [HttpPost]
    public async Task<IActionResult> Food_Management(FoodCreateViewModel model, IFormFile? ImageFile)
    {
        if (model.Food == null)
            return BadRequest();

        if (ImageFile != null && ImageFile.Length > 0)
        {
            string folder = Path.Combine(_environment.WebRootPath, "images");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            model.Food.IMAGE_URL = "/images/" + fileName;
        }

        model.Food.IS_ACTIVE = 1;

        _context.M_FOOD.Add(model.Food);
        await _context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            food = new
            {
                food_ID = model.Food.FOOD_ID,
                food_NAME = model.Food.FOOD_NAME,
                description = model.Food.DESCRIPTION,
                cook_TIME = model.Food.COOK_TIME,
                image_URL = model.Food.IMAGE_URL,
                category_ID = model.Food.CATEGORY_ID
            }
        });
    }

    // =========================
    // DELETE FOOD (AJAX)
    // =========================
    [HttpPost]
    public IActionResult DeleteFood(int id)
    {
        var food = _context.M_FOOD.Find(id);

        if (food == null)
            return Json(new { success = false });

        _context.M_FOOD.Remove(food);
        _context.SaveChanges();

        return Json(new { success = true, id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFood(FoodCreateViewModel model, IFormFile? ImageFile)
    {
        var food = await _context.M_FOOD.FindAsync(model.Food.FOOD_ID);

        if (food == null)
            return Json(new { success = false });

        // update fields
        food.FOOD_NAME = model.Food.FOOD_NAME;
        food.CATEGORY_ID = model.Food.CATEGORY_ID;
        food.DESCRIPTION = model.Food.DESCRIPTION;
        food.COOK_TIME = model.Food.COOK_TIME;

        // update image nếu có
        if (ImageFile != null && ImageFile.Length > 0)
        {
            string folder = Path.Combine(_environment.WebRootPath, "images");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            string path = Path.Combine(folder, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            food.IMAGE_URL = "/images/" + fileName;
        }

        await _context.SaveChangesAsync();

        return Json(new
        {
            success = true,
            food = new
            {
                food_ID = food.FOOD_ID,
                food_NAME = food.FOOD_NAME,
                description = food.DESCRIPTION,
                cook_TIME = food.COOK_TIME,
                image_URL = food.IMAGE_URL
            }
        });
    }
}