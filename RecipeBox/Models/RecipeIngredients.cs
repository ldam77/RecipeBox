using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
  public class RecipeIngredients

  {
    private int recipeID;
    private int ingredientID;
    private int id;

    public RecipeIngredients
    (int newRecipeID, int newIngredientID, int newID = 0)
    {
      recipeID = newRecipeID;
      ingredientID = newIngredientID;
      id = newID;
    }
    public int GetRecipeID()
    {
      return recipeID;
    }
    public int GetIngredientID()
    {
      return ingredientID;
    }
    public int GetId()
    {
      return id;
    }
    public override bool Equals(System.Object otherRecipeIngredients)
    {
      if(!(otherRecipeIngredients is RecipeIngredients))
      {
        return false;
      }
      else
      {
        RecipeIngredients newRecipeIngredients = (RecipeIngredients) otherRecipeIngredients;
        bool recipeIDEquality = (this.GetRecipeID() == newRecipeIngredients.GetRecipeID());
        bool ingredientIDEquality = (this.GetIngredientID() == newRecipeIngredients.GetIngredientID());
        bool idEquality = (this.GetId() == newRecipeIngredients.GetId());
        return (recipeIDEquality && ingredientIDEquality && idEquality);
      }
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO recipe_ingredients (recipe_id, ingredient_id) VALUES (@inputRecipeID, @inputIngredientID);";
      MySqlParameter newRecipeID = new MySqlParameter();
      newRecipeID.ParameterName = "@inputRecipeID";
      newRecipeID.Value = this.recipeID;
      cmd.Parameters.Add(newRecipeID);
      MySqlParameter newIngredientID = new MySqlParameter();
      newIngredientID.ParameterName = "@inputIngredientID";
      newIngredientID.Value = this.ingredientID;
      cmd.Parameters.Add(newIngredientID);
      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<RecipeIngredients> GetAll()
    {
      List<RecipeIngredients> allRecipeIngredients = new List<RecipeIngredients> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipe_ingredients;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int recipeID = rdr.GetInt32(1);
        int ingredientID = rdr.GetInt32(2);
        RecipeIngredients newRecipeIngredient = new RecipeIngredients(recipeID, ingredientID, id);
        allRecipeIngredients.Add(newRecipeIngredient);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allRecipeIngredients;
    }
    public static RecipeIngredients FindRecipeingredientsById(int searchId)
    {
      int id = 0;
      int recipeID = 0;
      int ingredientID = 0;
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipe_ingredients WHERE id = @MatchId;";
      MySqlParameter newMatchId = new MySqlParameter();
      newMatchId.ParameterName = "@MatchId";
      newMatchId.Value = searchId;
      cmd.Parameters.Add(newMatchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        recipeID = rdr.GetInt32(1);
        ingredientID = rdr.GetInt32(2);
      }
      RecipeIngredients foundRecipeingredients = new RecipeIngredients(recipeID, ingredientID, id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundRecipeingredients;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM recipe_ingredients;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
