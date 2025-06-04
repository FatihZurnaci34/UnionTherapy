using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionTherapy.Application.Utilities
{
    public static class ResponseHelper
    {
        public static Response<T> Success<T>(T data, string message = "Operation successful")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static Response<object> Success(string message = "Operation successful")
        {
            return new Response<object>
            {
                IsSuccess = true,
                Message = message,
                Data = null
            };
        }

        public static Response<T> Fail<T>(string message, T data = default)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Data = data
            };
        }

        public static Response<object> Fail(string message)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = message,
                Data = null
            };
        }
    }

    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
} 