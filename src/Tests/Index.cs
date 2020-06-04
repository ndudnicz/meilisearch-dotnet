using System;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using MeilisearchDotnet;
using System.Text.Json;

namespace IndexTests
{
    public class DocTest
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public double Ndouble { get; set; }
        public int Nint { get; set; }
        public IEnumerable<SubDocTest> SubDocTest { get; set; }
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

        string Uid = "index_global_test";

        string PrimaryKey = "Id";

        public GlobalTests()
        {
            ms = new Meilisearch("http://localhost:7700", "masterKey");
            index = ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
            {
                Uid = Uid,
                PrimaryKey = PrimaryKey
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

    [Collection("Sequential")]
    public class SearchTests
    {
        Meilisearch ms;
        MeilisearchDotnet.Index index { get; set; }

        List<DocTest> docs = new List<DocTest> {
            new DocTest { Id = "0", Date = DateTime.MinValue, Ndouble = 0.3, Nint = 1, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" },  new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "1", Date = DateTime.MinValue, Ndouble = 0.4, Nint = 42, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "dcba" }
            }},
            new DocTest { Id = "2", Date = DateTime.MinValue, Ndouble = 0.55555, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "deadbeef" }, new SubDocTest { str = "badcoffee" }, new SubDocTest { str = "a" }
            }},
            new DocTest { Id = "3", Date = DateTime.MinValue, Ndouble = 0.42, Nint = 7, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "coucou" }
            }},
            new DocTest { Id = "4", Date = DateTime.MinValue, Ndouble = 0.001, Nint = 9, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "tpayet" }, new SubDocTest { str = "kero" }
            }},
            new DocTest { Id = "5", Date = DateTime.MinValue, Ndouble = 44444444.8, Nint = 2, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "6", Date = DateTime.MinValue, Ndouble = 1.0, Nint = 3333333, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "7", Date = DateTime.MinValue, Ndouble = 4.8, Nint = 0, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "e=e&e %" }
            }},
            new DocTest { Id = "8", Date = new DateTime(2020, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "9a", Date = new DateTime(2021, 1, 1), Ndouble = 2020.1, Nint = 4, SubDocTest = null }
        };

        public SearchTests()
        {
            if (index == null)
            {
                ms = new Meilisearch("http://localhost:7700", "masterKey");
                index = ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
                {
                    Uid = "search_tests",
                    PrimaryKey = "Id"
                }).Result;
                MeilisearchDotnet.Types.EnqueuedUpdate e = index.AddDocuments(docs).Result;
                MeilisearchDotnet.Types.Update u = index.WaitForPendingUpdate(e.UpdateId).Result;
            }
        }

        [Fact]
        public async Task Search()
        {
            List<DocTest> _docs = new List<DocTest> {
                new DocTest { Id = "8", Date = new DateTime(2020, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
                }},
                new DocTest { Id = "9a", Date = new DateTime(2021, 1, 1), Ndouble = 2020.1, Nint = 4, SubDocTest = null }
            };

            MeilisearchDotnet.Types.SearchResponse<DocTest> result = await index.Search<DocTest>("2020");
            Assert.Equal(JsonSerializer.Serialize(_docs), JsonSerializer.Serialize(result.Hits));
        }
    }

    [Collection("Sequential")]
    public class SearchWithParamTests
    {
        Meilisearch ms;
        MeilisearchDotnet.Index index { get; set; }

        List<DocTest> docs = new List<DocTest> {
            new DocTest { Id = "0", Date = DateTime.MinValue, Ndouble = 0.3, Nint = 1, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" },  new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "1", Date = DateTime.MinValue, Ndouble = 0.4, Nint = 42, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "dcba" }
            }},
            new DocTest { Id = "2", Date = DateTime.MinValue, Ndouble = 0.55555, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "deadbeef" }, new SubDocTest { str = "badcoffee" }, new SubDocTest { str = "a" }
            }},
            new DocTest { Id = "3", Date = DateTime.MinValue, Ndouble = 0.42, Nint = 7, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "coucou" }
            }},
            new DocTest { Id = "4", Date = DateTime.MinValue, Ndouble = 0.001, Nint = 9, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "tpayet" }, new SubDocTest { str = "kero" }
            }},
            new DocTest { Id = "5", Date = DateTime.MinValue, Ndouble = 44444444.8, Nint = 2, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "6", Date = DateTime.MinValue, Ndouble = 1.0, Nint = 3333333, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "7", Date = DateTime.MinValue, Ndouble = 4.8, Nint = 0, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "e=e&e %" }
            }},
            new DocTest { Id = "8", Date = new DateTime(2020, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "9a", Date = new DateTime(2021, 1, 1), Ndouble = 2020.1, Nint = 4, SubDocTest = null }
        };

        public SearchWithParamTests()
        {
            ms = new Meilisearch("http://localhost:7700", "masterKey");
            index = ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
            {
                Uid = "search_with_param_tests",
                PrimaryKey = "Id"
            }).Result;
            MeilisearchDotnet.Types.EnqueuedUpdate e = index.AddDocuments(docs).Result;
            MeilisearchDotnet.Types.Update u = index.WaitForPendingUpdate(e.UpdateId).Result;
        }

        [Fact]
        public async Task SearchWithParam()
        {
            List<DocTest> _docs_limit_1 = new List<DocTest> {
                new DocTest { Id = "8", Date = new DateTime(2020, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
                }}
            };
            List<DocTest> _docs_limit_1_bis = new List<DocTest> {
                new DocTest { Id = "7", Date = DateTime.MinValue, Ndouble = 4.8, Nint = 0, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "e=e&e %" }
                }},
            };
            List<DocTest> _docs_limit_1_offset_1 = new List<DocTest> {
                new DocTest { Id = "9a", Date = new DateTime(2021, 1, 1), Ndouble = 2020.1, Nint = 4, SubDocTest = null }
            };

            List<DocTest> _docs_AttributesToRetrieve_id = new List<DocTest> {
                new DocTest { Id = "8", Date = DateTime.MinValue, Ndouble = 0.0, Nint = 0, SubDocTest = null
                }
            };

            MeilisearchDotnet.Types.SearchRequest options = new MeilisearchDotnet.Types.SearchRequest
            {
                Offset = 0,
                Limit = 1
            };
            MeilisearchDotnet.Types.SearchResponse<DocTest> result = await index.Search<DocTest>("2020", options);
            Assert.Equal(JsonSerializer.Serialize(_docs_limit_1), JsonSerializer.Serialize(result.Hits));
            Assert.Equal(0, result.Offset);
            Assert.Equal(1, result.Limit);

            result = await index.Search<DocTest>("e=e&e %", options);
            Assert.Equal(JsonSerializer.Serialize(_docs_limit_1_bis), JsonSerializer.Serialize(result.Hits));

            options.Offset = 1;
            result = await index.Search<DocTest>("2020", options);
            Assert.Equal(JsonSerializer.Serialize(_docs_limit_1_offset_1), JsonSerializer.Serialize(result.Hits));
            Assert.Equal(1, result.Offset);
            Assert.Equal(1, result.Limit);

            result = await index.Search<DocTest>("e=e&e %", options);
            Assert.Equal(JsonSerializer.Serialize(new List<DocTest> { }), JsonSerializer.Serialize(result.Hits));

            options.Offset = 0;
            options.AttributesToRetrieve = new string[] { "Id" };
            result = await index.Search<DocTest>("2020", options);
            Assert.Equal(JsonSerializer.Serialize(_docs_AttributesToRetrieve_id), JsonSerializer.Serialize(result.Hits));
            Assert.Equal(0, result.Offset);
            Assert.Equal(1, result.Limit);
        }

    }

    [Collection("Sequential")]
    public class AddDocumentsTests
    {
        Meilisearch ms;
        MeilisearchDotnet.Index index { get; set; }

        List<DocTest> docs = new List<DocTest> {
            new DocTest { Id = "0", Date = DateTime.MinValue, Ndouble = 0.3, Nint = 1, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" },  new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "1", Date = DateTime.MinValue, Ndouble = 0.4, Nint = 42, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "dcba" }
            }},
            new DocTest { Id = "2", Date = DateTime.MinValue, Ndouble = 0.55555, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "deadbeef" }, new SubDocTest { str = "badcoffee" }, new SubDocTest { str = "a" }
            }},
            new DocTest { Id = "3", Date = DateTime.MinValue, Ndouble = 0.42, Nint = 7, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "coucou" }
            }},
            new DocTest { Id = "4", Date = DateTime.MinValue, Ndouble = 0.001, Nint = 9, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "tpayet" }, new SubDocTest { str = "kero" }
            }},
            new DocTest { Id = "5", Date = DateTime.MinValue, Ndouble = 44444444.8, Nint = 2, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "6", Date = DateTime.MinValue, Ndouble = 1.0, Nint = 3333333, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "7", Date = DateTime.MinValue, Ndouble = 4.8, Nint = 0, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "e=e&e %" }
            }},
            new DocTest { Id = "8", Date = new DateTime(2020, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "9a", Date = new DateTime(2021, 1, 1), Ndouble = 2020.1, Nint = 4, SubDocTest = null }
        };

        public AddDocumentsTests()
        {
            if (index == null)
            {
                ms = new Meilisearch("http://localhost:7700", "masterKey");
                index = ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
                {
                    Uid = "add_document_tests",
                    PrimaryKey = "Id"
                }).Result;
                MeilisearchDotnet.Types.EnqueuedUpdate e = index.AddDocuments(docs).Result;
                MeilisearchDotnet.Types.Update u = index.WaitForPendingUpdate(e.UpdateId).Result;
            }
        }

        [Fact]
        public async Task AddDocuments()
        {
            MeilisearchDotnet.Types.EnqueuedUpdate q = await index.AddDocuments<DocTest>(docs);
            await index.WaitForPendingUpdate(q.UpdateId);
            List<DocTest> result = await index.GetDocuments<List<DocTest>>();
            result.Sort(delegate (DocTest a, DocTest b)
            {
                return string.Compare(a.Id, b.Id);
            });
            Assert.Equal(JsonSerializer.Serialize(docs), JsonSerializer.Serialize(result));
            DocTest newDoc = new DocTest
            {
                Id = "10",
                Date = new DateTime(2020, 1, 1),
                Ndouble = 3.4,
                Nint = 4,
                SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }
            };
            docs.Add(newDoc);
            docs.Sort(delegate (DocTest a, DocTest b)
            {
                return string.Compare(a.Id, b.Id);
            });
            q = await index.AddDocuments<DocTest>(new List<DocTest> { newDoc });
            await index.WaitForPendingUpdate(q.UpdateId);
            result = await index.GetDocuments<List<DocTest>>();
            result.Sort(delegate (DocTest a, DocTest b)
            {
                return string.Compare(a.Id, b.Id);
            });
            Assert.Equal(JsonSerializer.Serialize(docs), JsonSerializer.Serialize(result));
        }
    }

    [Collection("Sequential")]
    public class UpdateDocumentsTests
    {
        Meilisearch ms;
        MeilisearchDotnet.Index index { get; set; }

        List<DocTest> docs = new List<DocTest> {
            new DocTest { Id = "0", Date = DateTime.MinValue, Ndouble = 0.3, Nint = 1, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" },  new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "1", Date = DateTime.MinValue, Ndouble = 0.4, Nint = 42, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "dcba" }
            }},
            new DocTest { Id = "2", Date = DateTime.MinValue, Ndouble = 0.55555, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "deadbeef" }, new SubDocTest { str = "badcoffee" }, new SubDocTest { str = "a" }
            }},
            new DocTest { Id = "3", Date = DateTime.MinValue, Ndouble = 0.42, Nint = 7, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "coucou" }
            }},
            new DocTest { Id = "4", Date = DateTime.MinValue, Ndouble = 0.001, Nint = 9, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "tpayet" }, new SubDocTest { str = "kero" }
            }},
            new DocTest { Id = "5", Date = DateTime.MinValue, Ndouble = 44444444.8, Nint = 2, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "6", Date = DateTime.MinValue, Ndouble = 1.0, Nint = 3333333, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "7", Date = DateTime.MinValue, Ndouble = 4.8, Nint = 0, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "e=e&e %" }
            }},
            new DocTest { Id = "8", Date = new DateTime(2020, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "9a", Date = new DateTime(2021, 1, 1), Ndouble = 2020.1, Nint = 4, SubDocTest = null }
        };

        public UpdateDocumentsTests()
        {
            if (index == null)
            {
                ms = new Meilisearch("http://localhost:7700", "masterKey");
                index = ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
                {
                    Uid = "update_document_tests",
                    PrimaryKey = "Id"
                }).Result;
                MeilisearchDotnet.Types.EnqueuedUpdate e = index.AddDocuments(docs).Result;
                MeilisearchDotnet.Types.Update u = index.WaitForPendingUpdate(e.UpdateId).Result;
            }
        }

        [Fact]
        public async Task UpdateDocuments()
        {
            List<DocTest> newDocs = new List<DocTest> {
                new DocTest { Id = "0", Date = DateTime.MinValue, Ndouble = 5, Nint = 3, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" },  new SubDocTest { str = "efgh" }
                }},
                new DocTest { Id = "1", Date = DateTime.MinValue, Ndouble = 0.4, Nint = 42, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "dcba" }
                }},
                new DocTest { Id = "2", Date = DateTime.MinValue, Ndouble = 0.55555, Nint = 4, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "deadbeef" }, new SubDocTest { str = "coucou" }, new SubDocTest { str = "a" }
                }},
                new DocTest { Id = "3", Date = DateTime.MinValue, Ndouble = 0.42, Nint = 7, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "coucou" }
                }},
                new DocTest { Id = "4", Date = DateTime.MinValue, Ndouble = 0.001, Nint = 9, SubDocTest = new SubDocTest[] {
                        new SubDocTest { str = "kekekeke" }, new SubDocTest { str = "kero" }
                }},
                new DocTest { Id = "5", Date = DateTime.MinValue, Ndouble = 3.8, Nint = 2, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
                }},
                new DocTest { Id = "6", Date = DateTime.MinValue, Ndouble = 1.0, Nint = 3333333, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
                }},
                new DocTest { Id = "7", Date = DateTime.MinValue, Ndouble = 4.8, Nint = 0, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "e=e&e %" }
                }},
                new DocTest { Id = "8", Date = new DateTime(2077, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "covfefe" }, new SubDocTest { str = "efgh" }
                }}
            };
            DocTest oneDoc = new DocTest { Id = "9a", Date = new DateTime(2021, 1, 1), Ndouble = 2020.1, Nint = 4, SubDocTest = null };

            MeilisearchDotnet.Types.EnqueuedUpdate q = await index.AddDocuments<DocTest>(docs);
            await index.WaitForPendingUpdate(q.UpdateId);
            q = await index.UpdateDocuments(newDocs);
            await index.WaitForPendingUpdate(q.UpdateId);
            newDocs.Add(oneDoc);
            newDocs.Sort(delegate (DocTest a, DocTest b)
            {
                return string.Compare(a.Id, b.Id);
            });
            List<DocTest> result = await index.GetDocuments<List<DocTest>>();
            result.Sort(delegate (DocTest a, DocTest b)
            {
                return string.Compare(a.Id, b.Id);
            });
            Assert.Equal(JsonSerializer.Serialize(newDocs), JsonSerializer.Serialize(result));
        }
    }

    [Collection("Sequential")]
    public class DeleteDocumentsTests
    {
        Meilisearch ms;
        MeilisearchDotnet.Index index { get; set; }

        List<DocTest> docs = new List<DocTest> {
            new DocTest { Id = "0", Date = DateTime.MinValue, Ndouble = 5, Nint = 3, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" },  new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "1", Date = DateTime.MinValue, Ndouble = 0.4, Nint = 42, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "dcba" }
            }},
            new DocTest { Id = "2", Date = DateTime.MinValue, Ndouble = 0.55555, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "deadbeef" }, new SubDocTest { str = "coucou" }, new SubDocTest { str = "a" }
            }},
            new DocTest { Id = "3", Date = DateTime.MinValue, Ndouble = 0.42, Nint = 7, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "coucou" }
            }},
            new DocTest { Id = "4", Date = DateTime.MinValue, Ndouble = 0.001, Nint = 9, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "kekekeke" }, new SubDocTest { str = "kero" }
            }},
            new DocTest { Id = "5", Date = DateTime.MinValue, Ndouble = 3.8, Nint = 2, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "6", Date = DateTime.MinValue, Ndouble = 1.0, Nint = 3333333, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "7", Date = DateTime.MinValue, Ndouble = 4.8, Nint = 0, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "abcd" }, new SubDocTest { str = "e=e&e %" }
            }},
            new DocTest { Id = "8", Date = new DateTime(2077, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                new SubDocTest { str = "covfefe" }, new SubDocTest { str = "efgh" }
            }},
            new DocTest { Id = "9a", Date = new DateTime(2021, 1, 1), Ndouble = 2020.1, Nint = 4, SubDocTest = null }
        };

        public DeleteDocumentsTests()
        {
            if (index == null)
            {
                ms = new Meilisearch("http://localhost:7700", "masterKey");
                index = ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
                {
                    Uid = "delete_document_tests",
                    PrimaryKey = "Id"
                }).Result;
                MeilisearchDotnet.Types.EnqueuedUpdate e = index.AddDocuments(docs).Result;
                MeilisearchDotnet.Types.Update u = index.WaitForPendingUpdate(e.UpdateId).Result;
            }
        }

        [Fact]
        public async Task DeleteDocuments()
        {
            List<DocTest> newDocs_1 = new List<DocTest> {
                new DocTest { Id = "0", Date = DateTime.MinValue, Ndouble = 5, Nint = 3, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" },  new SubDocTest { str = "efgh" }
                }},
                new DocTest { Id = "1", Date = DateTime.MinValue, Ndouble = 0.4, Nint = 42, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "dcba" }
                }},
                new DocTest { Id = "2", Date = DateTime.MinValue, Ndouble = 0.55555, Nint = 4, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "deadbeef" }, new SubDocTest { str = "coucou" }, new SubDocTest { str = "a" }
                }},
                new DocTest { Id = "3", Date = DateTime.MinValue, Ndouble = 0.42, Nint = 7, SubDocTest = new SubDocTest[] {
                  new SubDocTest { str = "coucou" }
                }},
                new DocTest { Id = "4", Date = DateTime.MinValue, Ndouble = 0.001, Nint = 9, SubDocTest = new SubDocTest[] {
                        new SubDocTest { str = "kekekeke" }, new SubDocTest { str = "kero" }
                }},
                new DocTest { Id = "5", Date = DateTime.MinValue, Ndouble = 3.8, Nint = 2, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
                }},
                new DocTest { Id = "6", Date = DateTime.MinValue, Ndouble = 1.0, Nint = 3333333, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
                }},
                new DocTest { Id = "7", Date = DateTime.MinValue, Ndouble = 4.8, Nint = 0, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "e=e&e %" }
                }},
                new DocTest { Id = "8", Date = new DateTime(2077, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "covfefe" }, new SubDocTest { str = "efgh" }
                }}
            };

            List<DocTest> newDocs_2 = new List<DocTest> {
                new DocTest { Id = "5", Date = DateTime.MinValue, Ndouble = 3.8, Nint = 2, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "abcd" }, new SubDocTest { str = "efgh" }
                }},
                new DocTest { Id = "8", Date = new DateTime(2077, 1, 1), Ndouble = 3.4, Nint = 4, SubDocTest = new SubDocTest[] {
                    new SubDocTest { str = "covfefe" }, new SubDocTest { str = "efgh" }
                }}
            };


            MeilisearchDotnet.Types.EnqueuedUpdate q = await index.AddDocuments<DocTest>(docs);
            await index.WaitForPendingUpdate(q.UpdateId);
            q = await index.DeleteDocument("9a");
            await index.WaitForPendingUpdate(q.UpdateId);
            q = await index.DeleteDocument("9a");
            q = await index.DeleteDocument("9a");
            q = await index.DeleteDocument("9a");
            q = await index.DeleteDocument("9a");
            q = await index.DeleteDocument("9a");
            q = await index.DeleteDocument("9a");
            q = await index.DeleteDocument("9a");
            List<DocTest> result = await index.GetDocuments<List<DocTest>>();
            result.Sort(delegate (DocTest a, DocTest b)
            {
                return string.Compare(a.Id, b.Id);
            });
            Assert.Equal(JsonSerializer.Serialize(newDocs_1), JsonSerializer.Serialize(result));

            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            await index.WaitForPendingUpdate(q.UpdateId);
            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            q = await index.DeleteDocuments(new string[] { "0", "1", "2", "3", "4", "6", "7" });
            result = await index.GetDocuments<List<DocTest>>();
            result.Sort(delegate (DocTest a, DocTest b)
            {
                return string.Compare(a.Id, b.Id);
            });
            Assert.Equal(JsonSerializer.Serialize(newDocs_2), JsonSerializer.Serialize(result));
            q = await index.DeleteDocument("toto");
        }
    }
}
