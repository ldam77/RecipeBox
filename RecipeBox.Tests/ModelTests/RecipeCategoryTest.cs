using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System;
using System.Collections.Generic;

namespace RecipeBox.Tests
{
  [TestClass]
  public class RecipeCategoryTests : IDisposable
  {
    public void Dispose()
    {
      RecipeCategory.DeleteAll();
    }
    public RecipeCategoryTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }
    [TestMethod]
    public void GetTest_ReturnDataField()
    {
      // Arrange
      int testRecipeId = 1;
      int testCategoryId = 1;
      RecipeCategory testRecipeCategory = new RecipeCategory(testRecipeId, testCategoryId);

      // act
      int resultRecipeId = testRecipeCategory.GetRecipeID();
      int resultCategoryId = testRecipeCategory.GetCategoryID();

      // assert
      Assert.AreEqual(testRecipeId, resultRecipeId);
      Assert.AreEqual(testCategoryId, resultCategoryId);
    }
    [TestMethod]
    public void Equals_ReturnsTrueIfAllAreTheSame_RecipeCategory()
    {
      // Arrange, Act
      RecipeCategory firstRecipeCategory = new RecipeCategory(1, 1);
      RecipeCategory secondRecipeCategory = new RecipeCategory(1, 1);

      // Assert
      Assert.AreEqual(firstRecipeCategory, secondRecipeCategory);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      RecipeCategory testRecipeCategory = new RecipeCategory(1, 1);

      //Act
      testRecipeCategory.Save();
      RecipeCategory savedRecipeCategory = RecipeCategory.GetAll()[0];

      int result = savedRecipeCategory.GetId();
      int testId = testRecipeCategory.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void SaveAndGetAll_SavesToDatabaseAndReturnAll_RecipeCategory()
    {
      //Arrange
      RecipeCategory testRecipeCategory = new RecipeCategory(1, 1);

      //Act
      testRecipeCategory.Save();
      List<RecipeCategory> result = RecipeCategory.GetAll();
      List<RecipeCategory> testList = new List<RecipeCategory>{testRecipeCategory};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Find_FindRecipeCategoryInDatabase_RecipeCategory()
    {
      //Arrange
      RecipeCategory testRecipeCategory = new RecipeCategory(1, 1);
      testRecipeCategory.Save();

      //Act
      RecipeCategory resultById = RecipeCategory.FindRecipeCategoryById(testRecipeCategory.GetId());

      //Assert
      Assert.AreEqual(testRecipeCategory, resultById);
    
    }
  }
}
