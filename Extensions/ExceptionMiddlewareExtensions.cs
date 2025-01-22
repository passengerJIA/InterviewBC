using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using InterviewBC.Error;
using NLog;
public static class ExeptionMiddlewareExtensions{
    public static void ConfigureExceptionHandler(this WebApplication app, NLog.ILogger logger ){
        app.UseExceptionHandler(appError => {
            appError.Run(async context =>{
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeatrure = context.Features.Get<IExceptionHandlerFeature>();
                if(contextFeatrure != null){
                    logger.Error($"There is an error: {contextFeatrure.Error}");
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = string.Concat("Internal Server Error: " , contextFeatrure.Error)
                    }.ToString());
                }
            });
        });
    }
}