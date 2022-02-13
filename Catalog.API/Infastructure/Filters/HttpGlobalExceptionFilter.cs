namespace Catalog.API.Infastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {

        private readonly ILogger<HttpGlobalExceptionFilter> _logger;
        private readonly IWebHostEnvironment _env;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger, IWebHostEnvironment ev)
        {
            _logger = logger;
            _env = ev;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            if (context.Exception.GetType() == typeof(CatalogDomainException))
            {
                var problem = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Please refer to the errors property for additional details."
                };

                problem.Errors.Add("DomainValidations", new string[] { context.Exception.Message.ToString() });

                context.Result = new BadRequestObjectResult(problem);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                var json = new JsonErrorResponse()
                {
                    Messages = new[] { "An error ocurred." }
                };

                if (_env.IsDevelopment())
                    json.DeveloperMessage = context.Exception;

                context.Result = new InternalServerErrorObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            context.ExceptionHandled = true;
        }



        private class JsonErrorResponse
        {
            public string[] Messages { get; set; }
            public object DeveloperMessage { get; set; }
        }
    }
}
