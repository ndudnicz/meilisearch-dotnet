using System.Linq;
using System.Collections.Generic;
using Xunit;
using MeilisearchDotnet;

namespace Tests
{
    public class MeilisearchTest
    {
        Meilisearch ms;

        public MeilisearchTest()
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
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.BadRequestException>(async () => await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {
                uid = null
            }));
        }

        [Fact]
        public async void CreateIndex()
        {
            string uid = "kero";
            MeilisearchDotnet.Index index = await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {
                uid = uid
            });
            Assert.Equal(index.Uid, uid);
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.BadRequestException>(async () => await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {
                uid = "kero"
            }));
        }

        [Fact]
        public async void DeleteIndex()
        {
            string uid = "orek";
            await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {uid = uid});
            await ms.DeleteIndex(uid);
            await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {uid = uid});
            await ms.DeleteIndex(uid);
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.NotFoundException>(async () => await ms.GetIndex(uid));
        }

        [Fact]
        public async void GetIndexesList()
        {
            int i = 1;
            for (; i < 42; i++)
            {
                await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {uid = i.ToString()});
            }
            IEnumerable<MeilisearchDotnet.Types.IndexResponse> responses = await ms.ListIndexes();
            Assert.Equal(responses.Count(), 42);
            Assert.Equal(ms.Indexes.Keys.Count(), 42);
            i = 1;
            for (; i < 42; i++)
            {
                await ms.DeleteIndex(i.ToString());
            }
        }

        [Fact]
        public async void UpdateIndex()
        {
            string uid = "orek";
            await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {uid = uid});
            await ms.UpdateIndex(uid, new MeilisearchDotnet.Types.UpdateIndexRequest {
                primaryKey = "key1"
            });
            await ms.DeleteIndex(uid);
        }

        [Fact]
        public async void UpdateIndex_IndexAlreadyHasAPrimaryKey()
        {
            string uid = "orek";
            await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {uid = uid, primaryKey = "aaa"});
            await Assert.ThrowsAsync<MeilisearchDotnet.Exceptions.BadRequestException>(async () => await ms.UpdateIndex(uid, new MeilisearchDotnet.Types.UpdateIndexRequest {
                primaryKey = "key1"
            }));
            await ms.DeleteIndex(uid);
        }

    }
}
