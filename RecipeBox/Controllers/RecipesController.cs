using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RecipeBox.Models;
using MySql.Data.MySqlClient;
using System;

namespace RecipeBox.Controllers
{
  public class RecipesController : Controller
  {
    [HttpGet("/Recipes")]
    public ActionResult Index()
    {
      List<Recipe> allRecipes = Recipe.GetAll();
      return View(allRecipes);
    }
    [HttpGet("/Recipes/new")]
    public ActionResult CreateForm()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }
    [HttpPost("/Recipes/new")]
    public ActionResult Create(string recipeName, string recipeRating, string categoryName)
    {
      Recipe newRecipe = new Recipe(recipeName, int.Parse(recipeRating));
      newRecipe.Save();
      RecipeCategory newPair = new RecipeCategory(newRecipe.GetId(), Category.FindCategoryByName(categoryName).GetId());
      newPair.Save();
      return RedirectToAction("Index");
    }



    [HttpGet("Recipes/Search")]
    public ActionResult SearchForm()
    {
      return View();
    }
  }
}
