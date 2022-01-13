using System.Collections.Generic;
using System.Linq;
using hookup_backend.Data;
using hookup_backend.Data.Interfaces;
using hookup_backend.Data.Repositories;
using hookup_backend.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Moq;
using Xunit;
using FluentAssertions;
using System;
using hookup_backend.Dtos;
using BCryptNet = BCrypt.Net.BCrypt;

namespace hookup_backend_unit_tests.RepoTests
{
    public abstract class UserRepoTests
    {
        protected UserRepoTests(DbContextOptions<HookupContext> contextOptions)
        {
            ContextOptions = contextOptions;
            Seed();

        }
        protected DbContextOptions<HookupContext> ContextOptions { get; }
        private readonly Mock<IUser> repositoryStub = new();
        private readonly Mock<IMapper> mapperStub = new();
        private readonly Mock<IJwtUtils> JwtStub = new();

        private void Seed()
        {
            using (var context = new HookupContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var userOne = new User()
                {
                    Name = "Sam",
                    Email = "sam@boers.family",
                    PasswordHash = "MaxVerstappen1"
                };


                context.Add(userOne);
                context.SaveChanges();

            }
        }

        [Fact]
        public void GetAllUsers_ReturnsUsers()
        {
            // Arrange
            using (var context = new HookupContext(ContextOptions))
            {
                repositoryStub.Setup(repo => repo.GetAllUsers())
                    .Returns(context.Users);


                var repo = new UserRepo(context, mapperStub.Object, JwtStub.Object);
                // Act 
                var result = repo.GetAllUsers();


                // Assert
                result.Should().Equal(context.Users);
            }
        }

        [Fact]
        public void GetUserById_ReturnsUser()
        {
            // Arrange
            using (var context = new HookupContext(ContextOptions))
            {
                repositoryStub.Setup(repo => repo.GetUserById(1))
                    .Returns(context.Users.FirstOrDefault(u => u.UserId == 1));

                var repo = new UserRepo(context, mapperStub.Object, JwtStub.Object);
                // Act
                var result = repo.GetUserById(1);

                // Assert
                Assert.Equal("Sam", result.Name);
            }
        }

        [Fact]
        public void GetUserByEmail_ReturnsUser()
        {
            // Arrange
            using (var context = new HookupContext(ContextOptions))
            {
                repositoryStub.Setup(repo => repo.GetUserByEmail("sam@boers.family"))
                    .Returns(context.Users.FirstOrDefault(u => u.Email == "sam@boers.family"));

                var repo = new UserRepo(context, mapperStub.Object, JwtStub.Object);
                // Act
                var result = repo.GetUserByEmail("sam@boers.family");

                // Assert
                Assert.Equal("Sam", result.Name);
            }
        }

        [Fact]
        public void Register_ReturnsUserId()
        {
            UserCreateDto userCreateDto = new UserCreateDto
            {
                Email = "marcel@boers.family",
                Password = "MaxVerstappen33"
            };
            using (var context = new HookupContext(ContextOptions))
            {
                var repo = new UserRepo(context, mapperStub.Object, JwtStub.Object);

                var result = repo.Register(userCreateDto);

                Assert.Equal(result, true);
            }
        }
    }
}