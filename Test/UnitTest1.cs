using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using ECommerce.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Moq;
using Newtonsoft.Json.Linq;

namespace Test
{
    public class UnitTest1
    {
        private readonly UsersController controller;
        private readonly Mock<IUser> userRepository = new Mock<IUser>();

        public UnitTest1()
        {
            controller = new UsersController(userRepository.Object);
        }
        [Fact]
        public void Test1()
        {
            int x = 0;
            int y = 0;
            Assert.Equal(x, y);
        }
        [Fact]
        public void Test2()
        {
            var userList = GetProductsData();
          //  List<UserResponseDTO> list = new List<UserResponseDTO>();
            userRepository.Setup(x => x.AllUsers()).ReturnsAsync(userList);
            
            //Act
            var getUserList = controller.GetAll().Result;

            Assert.NotNull(getUserList);
            Assert.Equal(GetProductsData().Count(), userList.Count());
            Assert.Equal(GetProductsData().ToString(), getUserList.ToString());
            Assert.True(userList.Equals(getUserList));

        }
        [Fact]
        public void Test3()
        {
            UserDTO userDTO = new UserDTO()
            {
                ID = 4,
                Name = "Yash"

            };
            userRepository.Setup(x => x.AddUserasync(userDTO, 2)).ReturnsAsync(userDTO);
            var Adduser = controller.AddCustomer(userDTO);

            Assert.NotNull(Adduser);
            Assert.Equal(userDTO.ToString(), Adduser.Result.ToString());
          //  Assert.True(userDTO.Equals(Adduser));
            //Assert
            // IEnumerable<UserResponseDTO> result = new List<UserResponseDTO>(getUserList);

            //Assert.NotEmpty(model);
            //   Assert.NotNull(Adduser);

        }


        private List<UserResponseDTO> GetProductsData()
        {
            List<UserResponseDTO> productsData = new List<UserResponseDTO>
        {
            new UserResponseDTO
            {
               ID= 1,
               Name ="Admin",
               RoleName="Admin"
            },
            new UserResponseDTO
            {
                ID =  2,
                Name =  "cust-1",
                RoleName =  "Customer"
              },
            new UserResponseDTO
              {
                ID =  3,
                Name =  "cust-2",
                RoleName = "Customer"
              }

           
        };
            return productsData;
        }
    }
}