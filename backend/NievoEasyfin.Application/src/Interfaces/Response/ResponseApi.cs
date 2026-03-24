using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

namespace NievoEasyfin.Application.Interfaces.Response
{
    public class ResponseApi
    {
        public object? Data { get; set; }
        public bool? Success { get; set; }
        public List<string>? Errors { get; set; }

        public ResponseApi()
        {

        }

        public ResponseApi(object data, bool success, List<string> errors)
        {
            Data = data;
            Success = success;
            Errors = errors;
        }

        public void SuccessResponse(object data)
        {
            Data = data;
            Success = true;
            Errors = new List<string>();
        }

        public void ErrorResponse(List<string> errors)
        {
            Data = new { };
            Success = false;
            Errors = errors;
        }
    }
}