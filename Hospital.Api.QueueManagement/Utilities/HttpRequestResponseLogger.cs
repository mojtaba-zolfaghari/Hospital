using ElmahCore;
using Hospital.Application.Interfaces;
using Hospital.Domain.LoggerEntity;

namespace Hospital.Api.QueueManagement.Utilities
{
    /// <summary>
    /// HttpRequestResponseLogger
    /// </summary>
    public class HttpRequestResponseLogger
    {
        readonly RequestDelegate next;
        /// <summary>
        /// HttpRequestResponseLogger
        /// </summary>
        /// <param name="next"></param>
        public HttpRequestResponseLogger(RequestDelegate next)
        {
            this.next = next;
        }
        //can not inject as a constructor parameter in Middleware because only Singleton services can be resolved
        //by constructor injection in Middleware. Moved the dependency to the Invoke method
        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var _unitOfWork = context.RequestServices.GetRequiredService<IHospitalUnitOfWork>();
                Log logEntry = new();
                await RequestLogger(context, logEntry);

                await next.Invoke(context);

                await ResponseLogger(context, logEntry);

                //store log to database repository
                _unitOfWork.LoggerRepository.Create(logEntry);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await context.RaiseError(ex);
                throw;
            }
        }

        // Handle web request values
        /// <summary>
        /// RequestLogger
        /// </summary>
        /// <param name="context"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task RequestLogger(HttpContext context, Log log)
        {
            string requestHeaders = string.Empty;

            log.Method = context.Request.Method;
            log.Host = context.Request.Host.Host + ":" + context.Request.Host.Port.Value.ToString();
            log.Path = context.Request.Path;
            log.QueryString = context.Request.QueryString.ToString();
            log.ContentType = context.Request.ContentType;

            foreach (var headerDictionary in context.Request.Headers)
            {
                //ignore secrets and unnecessary header values
                if (headerDictionary.Key != "Authorization" && headerDictionary.Key != "Connection" &&
                    headerDictionary.Key != "User-Agent" && headerDictionary.Key != "Postman-Token" &&
                    headerDictionary.Key != "Accept-Encoding")
                {
                    requestHeaders += headerDictionary.Key + "=" + headerDictionary.Value + ", ";
                }
            }

            if (requestHeaders != string.Empty)
                log.Headers = requestHeaders;

            //Request handling. Check if the Request is a POST call 
            if (context.Request.Method == "POST" || context.Request.Method == "PUT")
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;
                log.Payload = body;
            }
        }

        //handle response values
        /// <summary>
        /// ResponseLogger
        /// </summary>
        /// <param name="context"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task ResponseLogger(HttpContext context, Log log)
        {
            using Stream originalRequest = context.Response.Body;
            try
            {
                using var memStream = new MemoryStream();
                context.Response.Body = memStream;
                // All the Request processing as described above 
                // happens from here.
                // Response handling starts from here
                // set the pointer to the beginning of the 
                // memory stream to read
                memStream.Position = 0;
                // read the memory stream till the end
                var response = await new StreamReader(memStream).ReadToEndAsync();
                // write the response to the log object
                log.Response = response;
                log.ResponseCode = context.Response.StatusCode.ToString();
                log.IsSuccessStatusCode = context.Response.StatusCode is 200 or 201;
                log.RespondedOn = DateTime.UtcNow;

                // since we have read till the end of the stream, 
                // reset it onto the first position
                memStream.Position = 0;

                // now copy the content of the temporary memory 
                // stream we have passed to the actual response body 
                // which will carry the response out.
                await memStream.CopyToAsync(originalRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                // assign the response body to the actual context
                context.Response.Body = originalRequest;
            }
        }
    }
}
