using System;
using Xunit;
using MeilisearchDotnet;

namespace IndexTests
{
    public class DocTest
    {
        public string id { get; set; }
        public DateTime date { get; set; }
        public double n_double { get; set; }
        public int n_int { get; set; }
        public SubDocTest subDocTest { get; set; }
    }

    public struct SubDocTest
    {
        public string str { get; set; }
    }

    [Collection("Sequential")]
    public class GlobalTests
    {
        Meilisearch ms;
        MeilisearchDotnet.Index index { get; set; }

        string Uid = "a";

        string PrimaryKey = "id";

        public GlobalTests()
        {
            ms = new Meilisearch("http://localhost:7700", "masterKey");
            index = ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest {
                uid = Uid,
                primaryKey = PrimaryKey
            }).Result;
        }

        [Fact]
        public async void Show()
        {
            MeilisearchDotnet.Types.IndexResponse res = await index.Show();
            Assert.Equal(Uid, res.Uid);
            Assert.Equal(Uid, res.Name);
            Assert.Equal(PrimaryKey, res.PrimaryKey);
        }
    }
}
