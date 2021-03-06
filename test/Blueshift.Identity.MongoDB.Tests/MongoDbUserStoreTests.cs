using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Blueshift.Identity.MongoDB.Tests
{
    public class MongoDbUserStoreTests : MongoDbIdentityStoreTestBase
    {
        private readonly IUserStore<MongoDbIdentityUser> _mongoDbUserStore;

        public MongoDbUserStoreTests(MongoDbIdentityFixture mongoDbIdentityFixture)
            : base(mongoDbIdentityFixture)
        {
            _mongoDbUserStore = mongoDbIdentityFixture.GetService<IUserStore<MongoDbIdentityUser>>();
        }

        [Fact]
        public async Task Can_create_user_async()
        {
            Assert.NotNull(await CreateUserInDatabase());
        }

        [Fact]
        public async Task Can_delete_user_async()
        {
            var user = await CreateUserInDatabase();
            Assert.Equal(IdentityResult.Success, await _mongoDbUserStore.DeleteAsync(user, new CancellationToken()));
        }

        [Fact]
        public async Task Can_find_by_id_async()
        {
            var user = await CreateUserInDatabase();
            Assert.Equal(user, await _mongoDbUserStore.FindByIdAsync(user.Id.ToString(), new CancellationToken()), new MongoDbIdentityUserComparer());
        }

        [Fact]
        public async Task Can_find_by_name_async()
        {
            var user = await CreateUserInDatabase();
            Assert.Equal(user, await _mongoDbUserStore.FindByNameAsync(user.NormalizedUserName, new CancellationToken()), new MongoDbIdentityUserComparer());
        }

        [Fact]
        public async Task Can_get_normalized_user_name_async()
        {
            var user = CreateUser();
            Assert.Equal(user.NormalizedUserName, await _mongoDbUserStore.GetNormalizedUserNameAsync(user, new CancellationToken()), StringComparer.Ordinal);
        }

        [Fact]
        public async Task Can_get_user_id_async()
        {
            var user = await CreateUserInDatabase();
            Assert.Equal(user.Id.ToString(), await _mongoDbUserStore.GetUserIdAsync(user, new CancellationToken()), StringComparer.Ordinal);
        }

        [Fact]
        public async Task Can_get_user_name_async()
        {
            var user = CreateUser();
            Assert.Equal(user.UserName, await _mongoDbUserStore.GetUserNameAsync(user, new CancellationToken()), StringComparer.Ordinal);
        }

        [Fact]
        public async Task Can_set_normalized_user_name_async()
        {
            var user = await CreateUserInDatabase();
            var newNormalizedUserName = "NORMALIZED.TEST.USER";
            await _mongoDbUserStore.SetNormalizedUserNameAsync(user, newNormalizedUserName, new CancellationToken());
            Assert.Equal(newNormalizedUserName, user.NormalizedUserName, StringComparer.Ordinal);
        }

        [Fact]
        public async Task Can_set_user_name_async()
        {
            var user = await CreateUserInDatabase();
            var newUserName = "another.user@different.com";
            await _mongoDbUserStore.SetUserNameAsync(user, newUserName, new CancellationToken());
            Assert.Equal(newUserName, user.UserName, StringComparer.Ordinal);
        }

        [Fact]
        public async Task Can_update_user_async()
        {
            var user = await CreateUserInDatabase();
            var newUserName = "another.user@different.com";
            var newNormalizedUserName = "NORMALIZED.TEST.USER";
            user.UserName = newUserName;
            user.NormalizedUserName = newNormalizedUserName;
            Assert.Equal(IdentityResult.Success, await _mongoDbUserStore.UpdateAsync(user, new CancellationToken()));
            Assert.Equal(user, await _mongoDbUserStore.FindByNameAsync(newNormalizedUserName, new CancellationToken()), new MongoDbIdentityUserComparer());
        }
    }
}