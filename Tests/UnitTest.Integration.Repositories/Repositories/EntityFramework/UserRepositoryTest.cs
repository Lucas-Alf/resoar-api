using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Domain.EFCore;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using UnitTest.Integration.Repositories.DBConfiguration.EFCore;
using UnitTest.Integration.Repositories.Repositories.DataBuilder;

namespace UnitTest.Integration.Repositories.Repositories.EntityFramework
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private ApplicationContext? dbContext;
        private IDbContextTransaction? transaction;

        private IUserRepository? userEntityFramework;
        private UserBuilder? builder;

        [OneTimeSetUp]
        public void GlobalPrepare()
        {
            dbContext = new EntityFrameworkConnection().DataBaseConfiguration();
        }

        [SetUp]
        public void SetUp()
        {
            userEntityFramework = new UserRepository(dbContext!);
            builder = new UserBuilder();
            transaction = dbContext!.Database.BeginTransaction();
        }

        [TearDown]
        public void TearDown()
        {
            transaction?.Rollback();
        }

        [Test]
        public void Add()
        {
            var result = userEntityFramework!.Add(builder!.CreateUser());
            Assert.Greater(result.Id, 0);
        }

        [Test]
        public void AddRange()
        {
            var result = userEntityFramework!.AddRange(builder!.CreateUserList(3));
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Remove()
        {
            var inserted = userEntityFramework!.Add(builder!.CreateUser());
            var result = userEntityFramework.Delete(inserted.Id);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void RemoveObj()
        {
            var inserted = userEntityFramework!.Add(builder!.CreateUser());
            var result = userEntityFramework.Delete(inserted);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void RemoveRange()
        {
            var inserted1 = userEntityFramework!.Add(builder!.CreateUser());
            var inserted2 = userEntityFramework.Add(builder.CreateUser());
            var usersRange = new List<User>()
            {
                inserted1, inserted2
            };
            var result = userEntityFramework.DeleteRange(usersRange);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void Update()
        {
            var inserted = userEntityFramework!.Add(builder!.CreateUser());
            inserted.Name = "Update";
            var result = userEntityFramework.Update(inserted);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void UpdateRange()
        {
            var inserted1 = userEntityFramework!.Add(builder!.CreateUser());
            var inserted2 = userEntityFramework!.Add(builder!.CreateUser());
            inserted1.Name = "Update1";
            inserted2.Name = "Update2";
            var usersRange = new List<User>()
            {
                inserted1, inserted2
            };
            var result = userEntityFramework.UpdateRange(usersRange);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetById()
        {
            var user = userEntityFramework!.Add(builder!.CreateUser());
            var result = userEntityFramework.GetById(user.Id);
            Assert.AreEqual(result?.Id, user.Id);
        }

        [Test]
        public void GetAll()
        {
            var user1 = userEntityFramework!.Add(builder!.CreateUser());
            var user2 = userEntityFramework!.Add(builder!.CreateUser());
            var result = userEntityFramework.GetAll();
            Assert.AreEqual(result?.OrderBy(u => u.Id)?.FirstOrDefault()?.Id, user1.Id);
            Assert.AreEqual(result?.OrderBy(u => u.Id)?.LastOrDefault()?.Id, user2.Id);
        }
    }
}