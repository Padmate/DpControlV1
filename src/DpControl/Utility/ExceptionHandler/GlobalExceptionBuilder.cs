﻿using Microsoft.AspNet.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Diagnostics;
using System.Net;
using Microsoft.AspNet.Http;
using DpControl.Domain.Models;
using System.Text;
using DpControl.Models;
using DpControl.Utility;
using Microsoft.Extensions.Logging;
using DpControl.Domain.Execptions;

namespace DpControl.Utility.ExceptionHandler
{
    public class GlobalExceptionBuilder 
    {
        private static ILogger _logger;
       
        public GlobalExceptionBuilder(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GlobalExceptionBuilder>();

        }


        /// <summary>
        /// 判断不同的异常并将不同类型的异常转换为HttpStatusCode
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public IApplicationBuilder ExceptionBuilder(IApplicationBuilder builder)
        {
            builder.Run(async context =>
            {

                var error = context.Features.Get<IExceptionHandlerFeature>();
                if (error != null)
                {
                    
                    var exceptionType = error.Error.GetType();
                    var exceptionMessage = error.Error.Message;

                    int httpStatusCode = (int)HttpStatusCode.BadRequest;
                    if (exceptionType != typeof(ExpectException))
                    {
                        httpStatusCode = (int)HttpStatusCode.InternalServerError;
                        //系统异常
                        exceptionMessage = "System is abnormal !";
                        //记录异常日志
                        _logger.LogError(exceptionMessage+" Request finished.");
                            
                    }
                    string errMessage = ResponseHandler.ReturnError(httpStatusCode, new List<string>() { exceptionMessage });
                    context.Response.StatusCode = httpStatusCode;
                    await context.Response.WriteAsync(errMessage, Encoding.UTF8);
                    
                }
                //await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
            });
            

            return builder;

        }
        
    }
}
