using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System;
using System.Collections.Generic;

namespace RecipeBox.Tests
{
  [TestClass]
  public class CategoryTests : IDisposable
  {
    public void Dispose()
    {
      Category.DeleteAll();
    }
    public CategoryTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }
    [TestMethod]
    public void GetTest_ReturnDataField()
    {
      // Arrange
      string testName = "mexican";
      Category testCategory = new Category(testName);

      // act
      string resultName = testCategory.GetName();

      // assert
      Assert.AreEqual(testName, resultName);
    }
    [TestMethod]
    public void Equals_ReturnsTrueIfAllAreTheSame_Category()
    {
      // Arrange, Act
      Category firstCategory = new Category("testName");
      Category secondCategory = new Category("testName");

      // Assert
      Assert.AreEqual(firstCategory, secondCategory);
    }
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Category testCategory = new Category("testName");

      //Act
      testCategory.Save();
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void SaveAndGetAll_SavesToDatabaseAndReturnAll_Category()
    {
      //Arrange
      Category testCategory = new Category("testName");

      //Act
      testCategory.Save();
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Find_FindCategoryInDatabase_Category()
    {
      //Arrange
      Category testCategory = new Category("testName");
      testCategory.Save();

      //Act
      Category resultById = Category.FindCategoryById(testCategory.GetId());

      //Assert
      Assert.AreEqual(testCategory, resultById);
    }
  }
}
