using Auth.Application.Common.Behaviors;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application
{
    public static class DependencyInjeciton
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjeciton).Assembly);
            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehaviors<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}