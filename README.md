<h1 align="center">MeiliSearch .NET</h1>

<h4 align="center">
  <a href="https://github.com/meilisearch/MeiliSearch">MeiliSearch</a> |
  <a href="https://www.meilisearch.com">Website</a> |
  <a href="https://blog.meilisearch.com">Blog</a> |
  <a href="https://twitter.com/meilisearch">Twitter</a> |
  <a href="https://docs.meilisearch.com">Documentation</a> |
  <a href="https://docs.meilisearch.com/faq">FAQ</a>
</h4>

<p align="center">
  <a href='https://github.com/ndudnicz/meilisearch-dotnet/actions?query=workflow%3A"default"'><img src="https://img.shields.io/github/workflow/status/ndudnicz/meilisearch-dotnet/default?style=for-the-badge"></a>
  <a href='https://github.com/ndudnicz/meilisearch-dotnet/actions?query=workflow%3A"nuget-push"'><img src="https://img.shields.io/github/workflow/status/ndudnicz/meilisearch-dotnet/nuget-push?style=for-the-badge&label=NuGet%20push"></a>
  <a href="https://www.nuget.org/packages/MeilisearchDotnet"><img src="https://img.shields.io/nuget/v/MeilisearchDotnet?style=for-the-badge" alt="NuGet version"></a>
  <a href="https://github.com/ndudnicz/meilisearch-dotnet/blob/master/LICENSE"><img src="https://img.shields.io/badge/license-MIT-informational?style=for-the-badge" alt="License"></a>
</p>

<p align="center">⚡ Lightning Fast, Ultra Relevant, and Typo-Tolerant Search Engine MeiliSearch client written in C#</p>

**MeiliSearchDotnet** is a client for **MeiliSearch** written in .NET standard 2.0. **MeiliSearch** is a powerful, fast, open-source, easy to use and deploy search engine. Both searching and indexing are highly customizable. Features such as typo-tolerance, filters, and synonyms are provided out-of-the-box.

## Table of Contents

- [🔧 Installation](#-installation)
- [🚀 Getting started](#-getting-started)
- [⚙️ Development workflow](#%EF%B8%8F-development-workflow)
- [🤖 Compatibility with MeiliSearch](#-compatibility-with-meilisearch)

## 🔧 Installation
It's available as NuGet package :
```bash
dotnet add package MeilisearchDotnet --version 0.0.4
```
[https://www.nuget.org/packages/MeilisearchDotnet](https://www.nuget.org/packages/MeilisearchDotnet)

### 🏃‍♀️ Run MeiliSearch

There are many easy ways to [download and run a MeiliSearch instance](https://docs.meilisearch.com/guides/advanced_guides/installation.html#download-and-launch).

For example, if you use Docker:

```bash
docker run -it --rm -p 7700:7700 getmeili/meilisearch:latest ./meilisearch --master-key=masterKey
```

NB: you can also download MeiliSearch from **Homebrew** or **APT**.

## 🎬 Getting started

Here is a quickstart how to add / update documents

```cs
using System.Collections.Generic;
using System.Threading.Tasks;

using MeilisearchDotnet;

namespace console
{
    public class Doc
    {
        public int key1 { get; set; }
        public string value { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            Meilisearch ms = new Meilisearch("http://localhost:7700", "masterKey");
            MeilisearchDotnet.Index index = await ms.CreateIndex(new MeilisearchDotnet.Types.IndexRequest
            {
                uid = "kero",
                primaryKey = "key1"
            });

            MeilisearchDotnet.Types.EnqueuedUpdate ret = await index.AddDocuments<Doc>(new List<Doc>() {
                new Doc { key1 = 222, value = "aaa" },
                new Doc { key1 = 333, value = "bbb" }
            });

            await index.WaitForPendingUpdate(ret.UpdateId);

            Doc doc = await index.GetDocument<Doc>("222");

            // doc => { key1 = 222, value = "aaa" }

            ret = await index.AddDocuments<Doc>(new List<Doc>() {
                new Doc { key1 = 444, value = "aaa" },
                new Doc { key1 = 555, value = "bbb" }
            }, new MeilisearchDotnet.Types.AddDocumentParams
            {
                primaryKey = "key1"
            });

            await index.WaitForPendingUpdate(ret.UpdateId);

            ret = await index.UpdateDocuments(new List<Doc>() {
                new Doc { key1 = 222, value = "tpayet" },
                new Doc { key1 = 444, value = "tutu" }
            });

            await index.WaitForPendingUpdate(ret.UpdateId);

            doc = await index.GetDocument<Doc>("222");

            // doc => { key1 = 222, value = "tpayet" }
        }
    }
}
```

#### Search in index

```csharp
// MeiliSearch is typo-tolerant:
MeilisearchDotnet.Types.SearchResponse<Doc> result = await index.Search<Doc>("tpyaet");
// result => {
//   "Hits": [{"key1": 222,"value": "tpayet"}],
//   "Offset": 0,
//   "Limit": 20,
//   "ProcessingTimeMs": 1,
//   "Query": "tpyaet"
// }
```

## ⚙️ Development Workflow

If you want to contribute, this sections describes the steps to follow.

### Tests

```bash
# Tests
docker run -d -p 7700:7700 getmeili/meilisearch:latest ./meilisearch --master-key=masterKey --no-analytics
dotnet restore
dotnet test
```

### Release

MeiliSearch tools follow the [Semantic Versioning Convention](https://semver.org/).

You must do a PR modifying the file `meilisearch-dotnet.csproj` with the right version.<br>

```xml
<Version>x.x.x</Version>
```

## 🤖 Compatibility with MeiliSearch

This package works for MeiliSearch `>=0.10.x`.
