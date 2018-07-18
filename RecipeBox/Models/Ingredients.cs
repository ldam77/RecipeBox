using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
  public class Ingredient
  {
    private int id;
    private string name;

    public Ingredient(string newName, int newId = 0)
    {
      id = newId;
      name = newName;
    }
    public int GetId()
    {
      return id;
    }
    public string GetName()
    {
      return name;
    }

    public override bool Equals(System.Object otherIngredient)
    {
      if(!(otherIngredient is Ingredient))
      {
        return false;
      }
      else
      {
        Ingredient newIngredient = (Ingredient) otherIngredient;
        bool idEquality = (this.GetId() == newIngredient.GetId());
        bool nameEquality = (this.GetName() == newIngredient.GetName());
        return (idEquality && nameEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO ingredients (name) VALUES (@inputName);";
      MySqlParameter newName = new MySqlParameter();
      newName.ParameterName = "@inputName";
      newName.Value = this.name;
      cmd.Parameters.Add(newName);
      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Ingredient> GetAll()
    {
      List<Ingredient> allIngredients = new List<Ingredient> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM ingredients;";
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

    public static Ingredient FindIngredientById(int searchId)
    {
      int id = 0;
      string name = "";
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM ingredients WHERE id = @MatchId;";
      MySqlParameter newMatchId = new MySqlParameter();
      newMatchId.ParameterName = "@MatchId";
      newMatchId.Value = searchId;
      cmd.Parameters.Add(newMatchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
      Ingredient foundIngredient = new Ingredient(name, id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundIngredient;
    }

    public static Ingredient FindIngredientByName(string searchName)
    {
      int id = 0;
      string name = "";
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM ingredients Where name = @MatchName;";
      MySqlParameter newMatchName = new MySqlParameter();
      newMatchName.ParameterName = "@MatchName";
      newMatchName.Value = searchName;
      cmd.Parameters.Add(newMatchName);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
      }
      Ingredient foundIngredient = new Ingredient(name, id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundIngredient;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM ingredients;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
