using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System;
using System.Collections.Generic;

namespace RecipeBox.Tests
{
  [TestClass]
  public class InstructionTests : IDisposable
  {
    public void Dispose()
    {
      Instruction.DeleteAll();
    }
    public InstructionTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }
    [TestMethod]
    public void GetTest_ReturnDataField()
    {
      // Arrange
      string testString = "bake";
      int testRecipeId = 1;
      Instruction testInstruction = new Instruction(testString, testRecipeId);

      // act
      string resultInstruction = testInstruction.GetInstruction();
      int resultRecipeId = testInstruction.GetRecipeId();

      // assert
      Assert.AreEqual(testString, resultInstruction);
      Assert.AreEqual(testRecipeId, resultRecipeId);
    }
    [TestMethod]
    public void Equals_ReturnsTrueIfAllAreTheSame_Instruction()
    {
      // Arrange, Act
      Instruction firstInstruction = new Instruction("testInstruction", 1);
      Instruction secondInstruction = new Instruction("testInstruction", 1);

      // Assert
      Assert.AreEqual(firstInstruction, secondInstruction);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Instruction testInstruction = new Instruction("testInstruction", 1);

      //Act
      testInstruction.Save();
      Instruction savedInstruction = Instruction.GetAll()[0];

      int result = savedInstruction.GetId();
      int testId = testInstruction.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void SaveAndGetAll_SavesToDatabaseAndReturnAll_Instruction()
    {
      //Arrange
      Instruction testInstruction = new Instruction("testInstruction", 1);

      //Act
      testInstruction.Save();
      List<Instruction> result = Instruction.GetAll();
      List<Instruction> testList = new List<Instruction>{testInstruction};
      
      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Find_FindInstructionInDatabase_Instruction()
    {
      //Arrange
      Instruction testInstruction = new Instruction("testInstruction", 1);
      testInstruction.Save();

      //Act
      Instruction resultById = Instruction.FindInstructionById(testInstruction.GetId());

      //Assert
      Assert.AreEqual(testInstruction, resultById);
    }
  }
}
