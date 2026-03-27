using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using NievoEasyfin.Application.Interfaces.Enum;

namespace NievoEasyfin.Application.Interfaces.Response
{
    public class ResponseApiError
    {
        public bool Error { get; set; } = true;

        public List<string> Messages { get; set; }

        public int Errors { get; set; }

        public ResponseApiError(List<string> errors)
        {
            Messages = errors;
            Errors = errors.Count;
        }
    }
}