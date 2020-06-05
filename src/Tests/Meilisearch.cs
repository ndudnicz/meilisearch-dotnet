using System.Linq;
using System.Collections.Generic;
using Xunit;
using MeilisearchDotnet;

namespace MeilisearchTests
{
    [Collection("Sequential")]
    public class GetListTests
    {
        Meilisearch ms { get; set; }

        public GetListTests()
        {
            ms = new Meilisearch("http://localhost:7700", "masterKey");
        }

        [Fact]
        public async void GetIndexesList()
        {
            int n = 3;
            int i = 0;
            for (; i < n; i++)
            {
                MeilisearchDotnet.Index index = await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest { Uid = i.ToString() });
                Assert.NotNull(index);
                Assert.Equal(index.Uid, i.ToString());
            }
            IEnumerable<MeilisearchDotnet.Types.IndexResponse> responses = await ms.ListIndexes();
            Assert.Equal(n, ms.Indexes.Keys.Count());
            Assert.Equal(n, responses.Count());
            i = 0;
            for (; i < n; i++)
            {
                await ms.DeleteIndex(i.ToString());
            }
        }
    }

    [Collection("Sequential")]
    public class GlobalTests
    {
        Meilisearch ms { get; set; }

        public GlobalTests()
        {
            ms = new Meilisearch("http://localhost:7700", "masterKey");
        }

        [Fact]
        public async void IndexNotFound()
        {
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.NotFoundException>(async () => await ms.GetIndex("I don't exists"));
        }

        [Fact]
        public async void GetIndex_UidNull()
        {
            await Assert.ThrowsAsync<System.ArgumentNullException>(async () => await ms.GetIndex(null));
        }

        [Fact]
        public async void CreateIndex_UidNull()
        {
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.BadRequestException>(async () => await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
            {
                Uid = null
            }));
        }

        [Fact]
        public async void CreateIndex()
        {
            string uid = "kero";
            MeilisearchDotnet.Index index = await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
            {
                Uid = uid
            });
            Assert.Equal(uid, index.Uid);
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.BadRequestException>(async () => await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
            {
                Uid = uid
            }));
            await ms.DeleteIndex(uid);
        }

        [Fact]
        public async void GetOrCreateIndex()
        {
            string uid = "lalalalalalalalalala";
            MeilisearchDotnet.Index index1 = await ms.GetOrCreateIndex(new MeilisearchDotnet.Types.IndexRequest
            {
                Uid = uid
            });
            MeilisearchDotnet.Index index2 = await ms.GetOrCreateIndex(new MeilisearchDotnet.Types.IndexRequest
            {
                Uid = uid
            });
            Assert.Equal(uid, index1.Uid);
            Assert.Equal(uid, index2.Uid);
            Assert.Same(index1, index2);
            await ms.DeleteIndex(uid);
        }

        [Fact]
        public async void DeleteIndex()
        {
            string uid = "orek";
            await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest { Uid = uid });
            await ms.DeleteIndex(uid);
            await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest { Uid = uid });
            await ms.DeleteIndex(uid);
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.NotFoundException>(async () => await ms.GetIndex(uid));
        }

        [Fact]
        public async void UpdateIndex()
        {
            string uid = "orek";
            string primaryKey = "key1";
            await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest { Uid = uid });
            MeilisearchDotnet.Types.IndexResponse res = await ms.UpdateIndex(uid, new MeilisearchDotnet.Types.UpdateIndexRequest
            {
                PrimaryKey = primaryKey
            });
            Assert.Equal(primaryKey, res.PrimaryKey);
            Assert.Equal(uid, res.Name);
            Assert.Equal(uid, res.Uid);
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.BadRequestException>(async () => await ms.UpdateIndex(uid, new MeilisearchDotnet.Types.UpdateIndexRequest
            {
                PrimaryKey = primaryKey
            }));
            await ms.DeleteIndex(uid);
        }

        [Fact]
        public async void UpdateIndex_IndexAlreadyHasAPrimaryKey()
        {
            string uid = "orek";
            await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest { Uid = uid, PrimaryKey = "aaa" });
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.BadRequestException>(async () => await ms.UpdateIndex(uid, new MeilisearchDotnet.Types.UpdateIndexRequest
            {
                PrimaryKey = "key1"
            }));
            await ms.DeleteIndex(uid);
        }

        [Fact]
        public async void GetKeys()
        {
            MeilisearchDotnet.Types.Keys keys = await ms.GetKeys();
            Assert.NotNull(keys.Private);
            Assert.NotNull(keys.Public);
        }

        [Fact]
        public async void Healthyness()
        {
            await ms.ChangeHealthTo(true);
            bool health = await ms.IsHealthy();
            Assert.True(health);
            await ms.SetUnhealthy();
            health = await ms.IsHealthy();
            Assert.False(health);
            await ms.SetHealthy();
            health = await ms.IsHealthy();
            Assert.True(health);
            await ms.ChangeHealthTo(false);
            health = await ms.IsHealthy();
            Assert.False(health);
        }

    }
}
