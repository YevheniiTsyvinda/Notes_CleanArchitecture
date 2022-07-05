﻿using Microsoft.EntityFrameworkCore;
using Notes.Domain;
using Notes.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Tests.Common;

public class NotesContextFactory
{
    public static Guid UserAId = Guid.NewGuid();
    public static Guid UserBId = Guid.NewGuid();

    public static Guid NoteIdForDelete = Guid.NewGuid();
    public static Guid NoteIdForUpdate = Guid.NewGuid();

    public static NotesDbContext Create()
    {
        var options = new DbContextOptionsBuilder<NotesDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new NotesDbContext(options);
        context.Database.EnsureCreated();
        context.Notes.AddRange(new Note
        {
            CreationDate = DateTime.Today,
            Details = "Details1",
            EditDate = null,
            Id = Guid.Parse("{de79bc7e-c8b6-4dd2-b4b2-4d718387ea86}"),
            Title = "Title1",
            UserId = UserAId
        },
       new Note
       {
           CreationDate = DateTime.Today,
           Details = "Details2",
           EditDate = null,
           Id = Guid.Parse("{23f17b6e-8754-4570-a737-43aa6bd4a177}"),
           Title = "Title2",
           UserId = UserBId
       },new Note
       {
           CreationDate = DateTime.Today,
           Details = "Details3",
           EditDate = null,
           Id = NoteIdForDelete,
           Title = "Title3",
           UserId = UserAId
       }, new Note
       {
           CreationDate = DateTime.Today,
           Details = "Details4",
           EditDate = null,
           Id = NoteIdForUpdate,
           Title = "Title4",
           UserId = UserBId
       });
        context.SaveChanges();
        return context;
    }

    public static void Destroy(NotesDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
