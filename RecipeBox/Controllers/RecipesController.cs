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
    public ActionResult Create(string recipeName, int recipeRating, string categoryName)
    {
      Recipe newRecipe = new Recipe(recipeName, recipeRating);
      newRecipe.Save();
      RecipeCategory newPair = new RecipeCategory(newRecipe.GetId(), Category.FindCategoryByName(categoryName).GetId());
      newPair.Save();
      return RedirectToAction("Index");
    }
    [HttpGet("/Recipes/{id}")]
    public ActionResult Detail(int id)
    {
      return View(Recipe.FindRecipeById(id));
    }
    [HttpPost("/Recipes/{recipeId}/AddIngredient")]
    public ActionResult AddIngredient(int recipeId, string ingredient)
    {
      if(Ingredient.FindIngredientByName(ingredient).GetId() == 0)
      {
        Ingredient newIngredient = new Ingredient(ingredient);
        newIngredient.Save();
      }
      RecipeIngredients newPair = new RecipeIngredients(recipeId, Ingredient.FindIngredientByName(ingredient).GetId());
      newPair.Save();
      return RedirectToAction("Detail", new { id = recipeId});
    }
    [HttpPost("/Recipes/{recipeId}/AddInstruction")]
    public ActionResult AddInstruction(int recipeId, string instruction)
    {
      Instruction newInstruction = new Instruction(instruction, recipeId);
      newInstruction.Save();
      return RedirectToAction("Detail", new { id = recipeId});
    }
    [HttpGet("/Recipes/Search")]
    public ActionResult SearchForm()
    {
      return View();
    }
    [HttpPost("/Recipes/Search")]
    public ActionResult Search(string searchFx, string searchTerm)
    {
      if (searchFx.Equals("byName"))
      {
        return View("Index", Recipe.FindRecipeByName(searchTerm));
      }
      else
      {
        return View("Index", Recipe.FindRecipeByIngredient(searchTerm));
      }
    }
  }
}
