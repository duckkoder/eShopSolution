using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObj)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
        }

        public ApiSuccessResult<T> CreateMessage(string message)
        {
            IsSuccessed = true;
            Message = message;
            return this;
        }

        public ApiSuccessResult()
        {
            IsSuccessed = true;
        }
    }
}