using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System;
using System.Collections.Generic;

namespace RecipeBox.Tests
{
  [TestClass]
  public class RecipeIngredientsTests : IDisposable
  {
    public void Dispose()
    {
      RecipeIngredients.DeleteAll();
    }
    public RecipeIngredientsTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }
    [TestMethod]
    public void GetTest_ReturnDataField()
    {
      // Arrange
      int testRecipeId = 1;
      int testIngredientId = 1;
      RecipeIngredients testRecipeIngredients = new RecipeIngredients(testRecipeId, testIngredientId);

      // act
      int resultRecipeId = testRecipeIngredients.GetRecipeID();
      int resultIngredientId = testRecipeIngredients.GetIngredientID();

      // assert
      Assert.AreEqual(testRecipeId, resultRecipeId);
      Assert.AreEqual(testIngredientId, resultIngredientId);
    }
    [TestMethod]
    public void Equals_ReturnsTrueIfAllAreTheSame_RecipeIngredients()
    {
      // Arrange, Act
      RecipeIngredients firstRecipeIngredients = new RecipeIngredients(1, 1);
      RecipeIngredients secondRecipeIngredients = new RecipeIngredients(1, 1);

      // Assert
      Assert.AreEqual(firstRecipeIngredients, secondRecipeIngredients);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      RecipeIngredients testRecipeIngredients = new RecipeIngredients(1, 1);

      //Act
      testRecipeIngredients.Save();
      RecipeIngredients savedRecipeIngredients = RecipeIngredients.GetAll()[0];

      int result = savedRecipeIngredients.GetId();
      int testId = testRecipeIngredients.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void SaveAndGetAll_SavesToDatabaseAndReturnAll_RecipeIngredients()
    {
      //Arrange
      RecipeIngredients testRecipeIngredients = new RecipeIngredients(1, 1);

      //Act
      testRecipeIngredients.Save();
      List<RecipeIngredients> result = RecipeIngredients.GetAll();
      List<RecipeIngredients> testList = new List<RecipeIngredients>{testRecipeIngredients};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Find_FindRecipeIngredientsInDatabase_RecipeIngredients()
    {
      //Arrange
      RecipeIngredients testRecipeIngredients = new RecipeIngredients(1, 1);
      testRecipeIngredients.Save();

      //Act
      RecipeIngredients resultById = RecipeIngredients.FindRecipeIngredientsById(testRecipeIngredients.GetId());

      //Assert
      Assert.AreEqual(testRecipeIngredients, resultById);

    }
  }
}
