using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using NievoEasyfin.Application.Interfaces.Enum;

namespace NievoEasyfin.Application.Interfaces.Response
{
    public class ResponseApiSucess
    {
        public bool Success { get; set; } = true;

        public object? Data { get; set; }

        public ResponseApiSucess()
        {

        }

        public ResponseApiSucess(object data)
        {
            Data = data;
        }
    }
}