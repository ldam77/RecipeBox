using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System;
using System.Collections.Generic;

namespace RecipeBox.Tests
{
  [TestClass]
  public class IngredientTests : IDisposable
  {
    public void Dispose()
    {
      Ingredient.DeleteAll();
    }
    public IngredientTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }
    [TestMethod]
    public void GetTest_ReturnDataField()
    {
      // Arrange
      string testName = "tomato";
      Ingredient testIngredient = new Ingredient(testName);

      // act
      string resultName = testIngredient.GetName();

      // assert
      Assert.AreEqual(testName, resultName);
    }
    [TestMethod]
    public void Equals_ReturnsTrueIfAllAreTheSame_Ingredient()
    {
      // Arrange, Act
      Ingredient firstIngredient = new Ingredient("testName");
      Ingredient secondIngredient = new Ingredient("testName");

      // Assert
      Assert.AreEqual(firstIngredient, secondIngredient);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("testName");

      //Act
      testIngredient.Save();
      Ingredient savedIngredient = Ingredient.GetAll()[0];

      int result = savedIngredient.GetId();
      int testId = testIngredient.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void SaveAndGetAll_SavesToDatabaseAndReturnAll_Ingredient()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("testName");

      //Act
      testIngredient.Save();
      List<Ingredient> result = Ingredient.GetAll();
      List<Ingredient> testList = new List<Ingredient>{testIngredient};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Find_FindIngredientInDatabase_Ingredient()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("testName");
      testIngredient.Save();

      //Act
      Ingredient resultById = Ingredient.FindIngredientById(testIngredient.GetId());
      Ingredient resultByName = Ingredient.FindIngredientByName(testIngredient.GetName());

      //Assert
      Assert.AreEqual(testIngredient, resultById);
      Assert.AreEqual(testIngredient, resultByName);
    }
  }
}
