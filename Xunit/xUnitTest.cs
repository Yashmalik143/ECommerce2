﻿using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using ECommerce.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Xunit
{
    public class xUnitTest
    {
        private readonly UsersController controller;
        private readonly RolesController rolesController;
        private readonly Mock<IRoles> rolesRepository = new Mock<IRoles>();
        private readonly Mock<IUser> userRepository = new Mock<IUser>();
        public xUnitTest()
        {
            controller = new UsersController(userRepository.Object);
        }
        
        //public void LogInTest()
        //{
        //    //User u1 = new User()
        //    //{
        //    //    ID = 1,
        //    //    Name = "Test",
        //    //    RoleId = 1,
        //    //    IsActive = true,

        //    //};

        //    //Arrange
        //    string u2 = "hello";
        //    userRepository.Setup(x => x.Login(It.IsAny<int>())).Returns(u2);
        //    //Act
        //    var getUserList = controller.Login(1);
        //    //Assert
        //    Assert.NotEmpty(getUserList);
        //    //Assert.IsNotNull(getUserList);

        //}

        [Fact]
        public void AllUserTest()
        {
            //User u1 = new User()
            //{
            //    ID = 1,
            //    Name = "Test",
            //    RoleId = 1,
            //    IsActive = true,

            //};


            //Arrange
            List<UserResponseDTO> list = new List<UserResponseDTO>();
            userRepository.Setup(x => x.AllUsers()).ReturnsAsync(list);
            //Act
            var getUserList = controller.GetAll().Status.ToString();
            //Assert
          
            Assert.Equal("200",getUserList);
            

        }

        //[TestMethod]
        //public void AddUserTest()
        //{
        //    //Arrange
        //    UserDTO userDTO = new UserDTO()
        //    {
        //        ID = 3,
        //        Name = "Yash"

        //    };
        //    userRepository.Setup(x => x.AddUserasync(It.IsAny<UserDTO>(), It.IsAny<int>())).ReturnsAsync(userDTO);

        //    //Act
        //    var Adduser = controller.AddCustomer(userDTO);

        //    //Assert
        //    Assert.IsNotNull(Adduser);
        //    Assert.AreSame(Adduser, userDTO);
        //    Adduser.Should().NotBeNull();


        //}
        //[TestMethod]
        //public void viewRoleTest()
        //{
        //    //Action
        //    IEnumerable<RoleDTO> list = new List<RoleDTO>();
        //    rolesRepository.Setup(x => x.ViewRolesAsync()).ReturnsAsync(list);
        //    //Act
        //    var callfunc = rolesController.ViewRoles();
        //    //Assert
        //    Assert.IsNotNull(callfunc);
        //    Assert.AreSame(list, callfunc);



        //}
    }
}
