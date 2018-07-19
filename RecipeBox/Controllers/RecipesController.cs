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
      List<Recipe> newRecipes = Recipe.GetAll();
      return View(newRecipes);
    }
    [HttpGet("/Recipes/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/Recipes/new")]
    public ActionResult Create(string recipeName, string recipeRating)
    {
      Recipe newRecipe = new Recipe(recipeName, int.Parse(recipeRating));
      newRecipe.Save();
      return RedirectToAction("Index");
    }



    [HttpGet("Recipes/Search")]
    public ActionResult SearchForm()
    {
      return View();
    }
  }
}
