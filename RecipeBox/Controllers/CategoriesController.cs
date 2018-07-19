using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RecipeBox.Models;
using MySql.Data.MySqlClient;
using System;

namespace RecipeBox.Controllers
{
  public class CategoriesController : Controller
  {
    [HttpGet("/Categories")]
    public ActionResult Index()
    {
      List<Category> newCategories = Category.GetAll();
      return View(newCategories);
    }
    [HttpGet("/Categories/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/Categories/New")]
    public ActionResult Create(string categoryName)
    {
      Category newCategory = new Category(categoryName);
      newCategory.Save();
      return RedirectToAction("Index");
    }
  }
}
