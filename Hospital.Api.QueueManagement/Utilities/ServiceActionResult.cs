using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Inventory.Api.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceActionResult<T> : IActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HasError;
        /// <summary>
        /// 
        /// </summary>
        public string Message;
        /// <summary>
        /// result
        /// </summary>
        public T Result { get; set; }

        private readonly HttpStatusCode httpStatusCode;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="successMessage"></param>
        public ServiceActionResult(T result, string successMessage = "DONE")
        {
            HasError = false;
            Message = successMessage;
            Result = result;
            httpStatusCode = HttpStatusCode.OK;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="httpStatusCode"></param>
        public ServiceActionResult(string errorMessage = "Exception Happens on runtime!", HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
        {
            this.httpStatusCode = httpStatusCode;
            HasError = true;
            Message = errorMessage;
            T t = default;
            Result = t;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)httpStatusCode;

            await new ObjectResult(new { Result, HasError, Message }).ExecuteResultAsync(context);
        }
    }

}
