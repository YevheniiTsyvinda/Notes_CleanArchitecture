using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Notes.Commands.DeleteNote;

public class DeleteNotCommandValidator :AbstractValidator<DeleteNoteCommand>
{
    public DeleteNotCommandValidator()
    {
        RuleFor(deleteNoteCommand => deleteNoteCommand.Id).NotEqual(Guid.Empty);
        RuleFor(deleteNoteCommand => deleteNoteCommand.UserId).NotEqual(Guid.Empty);
    }
}
