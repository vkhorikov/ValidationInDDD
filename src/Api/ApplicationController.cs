using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        protected new IActionResult Ok(object result = null)
        {
            return new EnvelopeResult(Envelope.Ok(result), HttpStatusCode.OK);
        }

        protected IActionResult NotFound(Error error, string invalidField = null)
        {
            return new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.NotFound);
        }

        protected IActionResult Error(Error error, string invalidField = null)
        {
            return new EnvelopeResult(Envelope.Error(error, invalidField), HttpStatusCode.BadRequest);
        }

        protected IActionResult FromResult<T>(Result<T, Error> result)
        {
            if (result.IsSuccess)
                return Ok();

            return Error(result.Error);
        }
    }

    public sealed class EnvelopeResult : IActionResult
    {
        private readonly Envelope _envelope;
        private readonly int _statusCode;

        public EnvelopeResult(Envelope envelope, HttpStatusCode statusCode)
        {
            _statusCode = (int)statusCode;
            _envelope = envelope;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_envelope)
            {
                StatusCode = _statusCode
            };

            return objectResult.ExecuteResultAsync(context);
        }
    }
}
