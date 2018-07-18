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

    public static Recipe FindRecipeByName(string searchName)
    {
      int id = 0;
      string name = "";
      int rating = 0;
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes Where name = @MatchName;";
      MySqlParameter newMatchName = new MySqlParameter();
      newMatchName.ParameterName = "@MatchName";
      newMatchName.Value = searchName;
      cmd.Parameters.Add(newMatchName);
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
