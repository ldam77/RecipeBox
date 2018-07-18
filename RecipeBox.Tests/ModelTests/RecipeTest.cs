using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System;
using System.Collections.Generic;

namespace RecipeBox.Tests
{
  [TestClass]
  public class RecipeTests : IDisposable
  {
    public void Dispose()
    {
      Recipe.DeleteAll();
    }
    public RecipeTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }
    [TestMethod]
    public void GetTest_ReturnDataField()
    {
      // Arrange
      string testName = "cookie";
      int testRating = 1;
      Recipe testRecipe = new Recipe(testName, testRating);

      // act
      string resultName = testRecipe.GetName();
      int resultRating = testRecipe.GetRating();

      // assert
      Assert.AreEqual(testName, resultName);
      Assert.AreEqual(testRating, resultRating);
    }
    [TestMethod]
    public void Equals_ReturnsTrueIfAllAreTheSame_Recipe()
    {
      // Arrange, Act
      Recipe firstRecipe = new Recipe("testName", 1);
      Recipe secondRecipe = new Recipe("testName", 1);

      // Assert
      Assert.AreEqual(firstRecipe, secondRecipe);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Recipe testRecipe = new Recipe("testName", 1);

      //Act
      testRecipe.Save();
      Recipe savedRecipe = Recipe.GetAll()[0];

      int result = savedRecipe.GetId();
      int testId = testRecipe.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void SaveAndGetAll_SavesToDatabaseAndReturnAll_Recipe()
    {
      //Arrange
      Recipe testRecipe = new Recipe("testName", 1);

      //Act
      testRecipe.Save();
      List<Recipe> result = Recipe.GetAll();
      List<Recipe> testList = new List<Recipe>{testRecipe};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Find_FindRecipeInDatabase_Recipe()
    {
      //Arrange
      Recipe testRecipe = new Recipe("testName", 1);
      testRecipe.Save();

      //Act
      Recipe resultById = Recipe.FindRecipeById(testRecipe.GetId());
      Recipe resultByName = Recipe.FindRecipeByName(testRecipe.GetName());

      //Assert
      Assert.AreEqual(testRecipe, resultById);
      Assert.AreEqual(testRecipe, resultByName);
    }
  }
}
