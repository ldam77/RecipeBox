using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
  public class Recipe
  {
    private int id;
    private string name;
    private int rating;

    public Recipe(string newName, int newRating = 0, int newId = 0)
    {
      id = newId;
      name = newName;
      rating = newRating;
    }
    public int GetId()
    {
      return id;
    }
    public string GetName()
    {
      return name;
    }
    public int GetRating()
    {
      return rating;
    }

    public override bool Equals(System.Object otherRecipe)
    {
      if(!(otherRecipe is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) otherRecipe;
        bool idEquality = (this.GetId() == newRecipe.GetId());
        bool nameEquality = (this.GetName() == newRecipe.GetName());
        bool ratingEquality = (this.GetRating() == newRecipe.GetRating());
        return (idEquality && nameEquality && ratingEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO recipes (name, rating) VALUES (@inputName, @inputRating);";
      MySqlParameter newName = new MySqlParameter();
      newName.ParameterName = "@inputName";
      newName.Value = this.name;
      cmd.Parameters.Add(newName);
      MySqlParameter newRating = new MySqlParameter();
      newRating.ParameterName= "@inputRating";
      newRating.Value = this.rating;
      cmd.Parameters.Add(newRating);
      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Recipe> GetAll()
    {
      List<Recipe> allRecipes = new List<Recipe> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int rating = rdr.GetInt32(2);
        Recipe newRecipe = new Recipe(name, rating, id);
        allRecipes.Add(newRecipe);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allRecipes;
    }

    public static Recipe FindRecipeById(int searchId)
    {
      int id = 0;
      string name = "";
      int rating = 0;
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes WHERE id = @MatchId;";
      MySqlParameter newMatchId = new MySqlParameter();
      newMatchId.ParameterName = "@MatchId";
      newMatchId.Value = searchId;
      cmd.Parameters.Add(newMatchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
        rating = rdr.GetInt32(2);
      }
      Recipe foundRecipe = new Recipe(name, rating, id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundRecipe;
    }

    public static List<Recipe> FindRecipeByName(string searchName)
    {
      List<Recipe> foundRecipes = new List<Recipe> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes WHERE name LIKE @MatchName;";
      MySqlParameter newMatchName = new MySqlParameter();
      newMatchName.ParameterName = "@MatchName";
      newMatchName.Value = searchName + "%";
      cmd.Parameters.Add(newMatchName);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int rating = rdr.GetInt32(2);

        Recipe newRecipe = new Recipe(name, rating, id);
        foundRecipes.Add(newRecipe);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundRecipes;
    }

    public static List<Recipe> FindRecipeByIngredient(string searchName)
    {
      List<Recipe> foundRecipes = new List<Recipe> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT recipes.* FROM recipes JOIN recipe_ingredients ON (recipes.id = recipe_ingredients.id) JOIN ingredients ON (recipe_ingredients.id = ingredients.id) WHERE ingredients.name LIKE @MatchName;";
      MySqlParameter newMatchName = new MySqlParameter();
      newMatchName.ParameterName = "@MatchName";
      newMatchName.Value = searchName + "%";
      cmd.Parameters.Add(newMatchName);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int rating = rdr.GetInt32(2);

        Recipe newRecipe = new Recipe(name, rating, id);
        foundRecipes.Add(newRecipe);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundRecipes;
    }

    public Category GetCategory()
    {
      int id = 0;
      string name = "";
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT categories.* FROM categories JOIN recipe_category ON (categories.id = recipe_category.category_id) JOIN recipes ON (recipe_category.recipe_id = recipes.id) WHERE recipes.id = @MatchId;";
      MySqlParameter newMatchId = new MySqlParameter();
      newMatchId.ParameterName = "@MatchId";
      newMatchId.Value = this.id;
      cmd.Parameters.Add(newMatchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
      Category foundCategory = new Category(name, id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundCategory;
    }

    public List<Instruction> GetInstructions()
    {
      List<Instruction> allInstructions = new List<Instruction> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM instructions WHERE recipe_id = @MatchId;";
      MySqlParameter newMatchId = new MySqlParameter();
      newMatchId.ParameterName = "@MatchId";
      newMatchId.Value = this.id;
      cmd.Parameters.Add(newMatchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string instruction = rdr.GetString(1);
        int recipeId = rdr.GetInt32(2);
        Instruction newInstruction = new Instruction(instruction, recipeId, id);
        allInstructions.Add(newInstruction);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allInstructions;
    }

    public List<Ingredient> GetIngredients()
    {
      List<Ingredient> allIngredients = new List<Ingredient> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT ingredients.* FROM ingredients JOIN recipe_ingredients ON (ingredients.id = recipe_ingredients.ingredient_id) JOIN recipes ON (recipe_ingredients.recipe_id = recipes.id) WHERE recipes.id = @MatchId;";
      MySqlParameter newMatchId = new MySqlParameter();
      newMatchId.ParameterName = "@MatchId";
      newMatchId.Value = this.id;
      cmd.Parameters.Add(newMatchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        Ingredient newIngredient = new Ingredient(name, id);
        allIngredients.Add(newIngredient);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allIngredients;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM recipes;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
