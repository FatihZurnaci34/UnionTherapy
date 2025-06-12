using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionTherapy.Application.Utilities
{
    public static class ResponseHelper
    {
        public static Response<T> Success<T>(T data, string messageKey = "OperationSuccessful")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = messageKey,
                Data = data
            };
        }

        public static Response<object> Success(string messageKey = "OperationSuccessful")
        {
            return new Response<object>
            {
                IsSuccess = true,
                Message = messageKey,
                Data = null
            };
        }

        public static Response<T> Fail<T>(string messageKey, T? data = default)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = messageKey,
                Data = data
            };
        }

        public static Response<object> Fail(string messageKey)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = messageKey,
                Data = null
            };
        }

        public static Response<T> FailWithParameters<T>(string messageKey, object[] parameters, T? data = default)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = messageKey,
                Data = data
            };
        }

        public static Response<object> FailWithParameters(string messageKey, object[] parameters)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = messageKey,
                Data = null
            };
        }
    }

    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
} 