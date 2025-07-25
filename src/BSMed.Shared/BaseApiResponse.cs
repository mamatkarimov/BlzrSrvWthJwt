using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMed.Shared
{
    public class BaseApiResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public BaseApiResponse() { }

        public BaseApiResponse(bool success)
        {
            Success = success;
        }

        public BaseApiResponse(string error)
        {
            Success = false;
            Error = error;
        }

        public BaseApiResponse(IEnumerable<string> errors)
        {
            Success = false;
            Errors = errors.ToList();
        }

        public void AddModelErrors(ModelStateDictionary modelState)
        {
            Success = false;
            Errors = modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
        }
    }



    public class BaseApiResponse<T> : BaseApiResponse
    {
        public T Data { get; set; }

        public BaseApiResponse() { }

        public BaseApiResponse(T data)
        {
            Success = true;
            Data = data;
        }

        public BaseApiResponse(T data, bool success)
        {
            Success = success;
            Data = data;
        }

        public BaseApiResponse(T data, string error) : base(error)
        {
            Data = data;
        }

        public BaseApiResponse(T data, IEnumerable<string> errors) : base(errors)
        {
            Data = data;
        }
    }
}
