﻿using MediatR;
using Notes.Application.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{

    ICurrentUserService _currentUserService;

    public LoggingBehavior(ICurrentUserService currentUserService) =>
        _currentUserService = currentUserService;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId;

        Log.Information($"Notes Request: {requestName} {userId} {request}");

        var response = await next();
        return response;
    }
}
