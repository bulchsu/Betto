﻿using Betto.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class HelpersRegistry
    {
        internal static IServiceCollection RegisterHelpers(this IServiceCollection services)
        {
            return services.AddSingleton<ILogger, Logger>()
                .AddScoped<IPasswordHasher, PasswordHasher>()
                .AddScoped<ITokenGenerator, TokenGenerator>()
                .AddScoped<IObjectValidator, ObjectValidator>()
                .AddScoped<IRelationCreator, RelationCreator>();
        }
    }
}