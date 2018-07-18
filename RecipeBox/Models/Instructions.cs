using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
  public class Instruction
  {
    private int id;
    private string instruction;
    private int recipeId;

    public Instruction(string newInstruction, int newRecipeId, int newId = 0)
    {
      id = newId;
      instruction = newInstruction;
      recipeId = newRecipeId;
    }
    public int GetId()
    {
      return id;
    }
    public string GetInstruction()
    {
      return instruction;
    }
    public int GetRecipeId()
    {
      return recipeId;
    }

    public override bool Equals(System.Object otherInstruction)
    {
      if(!(otherInstruction is Instruction))
      {
        return false;
      }
      else
      {
        Instruction newInstruction = (Instruction) otherInstruction;
        bool idEquality = (this.GetId() == newInstruction.GetId());
        bool instructionEquality = (this.GetInstruction() == newInstruction.GetInstruction());
        bool recipeIdEquality = (this.GetRecipeId() == newInstruction.GetRecipeId());
        return (idEquality && instructionEquality && recipeIdEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO instructions (instruction, recipe_id) VALUES (@inputInstruction, @inputRecipeId);";
      MySqlParameter newInstruction = new MySqlParameter();
      newInstruction.ParameterName = "@inputInstruction";
      newInstruction.Value = this.instruction;
      cmd.Parameters.Add(newInstruction);
      MySqlParameter newRecipeId = new MySqlParameter();
      newRecipeId.ParameterName = "@inputRecipeId";
      newRecipeId.Value = this.recipeId;
      cmd.Parameters.Add(newRecipeId);
      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Instruction> GetAll()
    {
      List<Instruction> allInstructions = new List<Instruction> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM instructions;";
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

    public static Instruction FindInstructionById(int searchId)
    {
      int id = 0;
      string instruction = "";
      int recipeId = 0;
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM instructions WHERE id = @MatchId;";
      MySqlParameter newMatchId = new MySqlParameter();
      newMatchId.ParameterName = "@MatchId";
      newMatchId.Value = searchId;
      cmd.Parameters.Add(newMatchId);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        instruction = rdr.GetString(1);
        recipeId = rdr.GetInt32(2);
      }
      Instruction foundInstruction = new Instruction(instruction, recipeId, id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundInstruction;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM instructions;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
