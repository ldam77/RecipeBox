using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
  public class RecipeCategory
  {
    private int id;
    private int recipeID;
    private int categoryID;

    public RecipeCategory(int newRecipeID, int newCategoryID, int newID = 0)
    {
      recipeID = newRecipeID;
      categoryID = newCategoryID;
      id = newID;
    }
    public int GetRecipeID()
    {
      return recipeID;
    }
    public int GetCategoryID()
    {
      return categoryID;
    }
    public int GetId()
    {
      return id;
    }
    public override bool Equals(System.Object otherRecipeCategory)
    {
      if(!(otherRecipeCategory is RecipeCategory))
      {
        return false;
      }
      else
      {
        RecipeCategory newRecipeCategory = (RecipeCategory) otherRecipeCategory;
        bool recipeIDEquality = (this.GetRecipeID() == newRecipeCategory.GetRecipeID());
        bool categoryIDEquality = (this.GetCategoryID() == newRecipeCategory.GetCategoryID());
        bool idEquality = (this.GetId() == newRecipeCategory.GetId());
        return (recipeIDEquality && categoryIDEquality && idEquality);
      }
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO recipe_category (recipeID, categoryID) VALUES (@inputRecipeID, @inputCategoryID);";
      MySqlParameter newRecipeID = new MySqlParameter();
      newRecipeID.ParameterRecipeID = "@inputRecipeID";
      newRecipeID.Value = this.recipeID;
      cmd.Parameters.Add(newRecipeID);
      MySqlParameter newCategoryID = new MySqlParameter();
      newCategoryID.ParameterCategoryID = "@inputCategoryID";
      newCategoryID.Value = this.categoryID;
      cmd.Parameters.Add(newCategoryID);
      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<RecipeCategory> GetAll()
    {
      List<RecipeCategory> allRecipeCategories = new List<RecipeCategory> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipe_category;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int recipeID = rdr.GetInt32(1);
        int categoryID = rdr.GetInt32(2);
        RecipeCategory newRecipeCategory = new RecipeCategory(recipeID, categoryID, id);
        allRecipeCategories.Add(newRecipeCategory);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allRecipeCategories;
    }
    public static Category FindRecipeCategoryById(int searchId)
    {
      int id = 0;
      int recipeID = "";
      int categoryID = "";
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipe_category WHERE id = @MatchId;";
      MySqlParameter newMatchId = new MySqlParameter();
      newMatchId.ParameterName = "@MatchId";
      newMatchId.Value = searchId;
      cmd.Parameters.Add(newMatchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        recipeID = rdr.GetInt32(1);
        categoryID = rdr.GetInt32(2);
      }
      RecipeCategory foundRecipeCategory = new RecipeCategory(recipeID, categoryID, id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundRecipeCategory;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM recipe_category;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
