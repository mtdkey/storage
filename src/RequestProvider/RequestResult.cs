using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public class RequestResult<T> : IRequestResult
    {
        public bool Success { get; private set; }
        public Exception Exception { get; private set; }

        public List<T> DataSet { get; private set; }

        public RequestResult() { }
        
        public RequestResult(bool success, Exception exception = null)
        {
            Success = success;
            Exception = exception;            
        }

        public void FillDataSet(List<T> dataSet) {
            DataSet = dataSet;
        }

        public void SetResultInfo(bool success, Exception exception = null)
        {
            Success = success;
            Exception = exception;
        }

        public static implicit operator RequestResult<T>(Func<Task<RequestResult<BunchPattern>>> v)
        {
            throw new NotImplementedException();
        }
    }
}
