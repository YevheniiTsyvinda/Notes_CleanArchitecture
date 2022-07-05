﻿using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistence;
using Notes.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Tests.Notes.Common;

public class QueryTestFixture : IDisposable
{
    public NotesDbContext Context;
    public IMapper Mapper;

    public QueryTestFixture()
    {
        Context = NotesContextFactory.Create();
        var configurationProvider = new MapperConfiguration(cfg =>
          {
              cfg.AddProfile(new AssemblyMappingProfile(
                  typeof(INotesDbContext).Assembly));
  
          });

        Mapper = configurationProvider.CreateMapper();
    }
    public void Dispose()
    {
        NotesContextFactory.Destroy(Context);
    }
}

[CollectionDefinition("QueryCollection")]
public class QueryCollection: ICollectionFixture<QueryTestFixture> { }