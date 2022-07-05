﻿using AutoMapper;
using Notes.Application.Notes.Queries.GetNoteDetails;
using Notes.Persistence;
using Notes.Tests.Common;
using Notes.Tests.Notes.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Tests.Notes.Queries;

[Collection("QueryCollection")]
public class GetNoteDetailsQueryHandlerTests
{
    private readonly NotesDbContext Context;
    private readonly IMapper Mapper;

    public GetNoteDetailsQueryHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    [Fact]
    public async Task GetNoteDetailsQueryHandler_Success()
    {
        //Arrange
        var handler = new GetNoteDetailsQueryHandler(Context, Mapper);

        //Act
        var result = await handler.Handle(
            new GetNoteDetailsQuery
            {
                UserId = NotesContextFactory.UserBId,
                Id = Guid.Parse("{23f17b6e-8754-4570-a737-43aa6bd4a177}")
            },
            CancellationToken.None);

        // Assert
        result.ShouldBeOfType<NoteDetailsVm>();
        result.Title.ShouldBe("Title2");
        result.CreationDate.ShouldBe(DateTime.Today);
    }
}
