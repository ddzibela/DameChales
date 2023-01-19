using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DameChales.API.BL.Facades;
using DameChales.API.DAL.Common.Repositories;
using DameChales.Common.Enums;
using DameChales.Common.Models;
using Moq;
using Xunit;

namespace DameChales.API.BL.UnitTests;
public class FoodFacadeTests
{
    [Fact]
    public void Delete_Calls_Correct_Method_On_Repository()
    {
        //arrange
        var repositoryMock = new Mock<IFoodRepository>(MockBehavior.Strict);
        repositoryMock.Setup(foodRepository => foodRepository.Remove(It.IsAny<Guid>()));

        var repository = repositoryMock.Object;
        var mapper = new Mock<IMapper>(MockBehavior.Strict).Object;
        var facade = new FoodFacade(repository, mapper);

        var itemId = Guid.NewGuid();
        //act
        facade.Delete(itemId);

        //assert
        repositoryMock.Verify(foodRepository => foodRepository.Remove(itemId));
    }
}
