using System;


namespace MtdKey.Storage
{
    public interface IRequestResult
    {        
        public bool Success { get;}
        public Exception Exception { get; }
        
        public void SetResultInfo(bool success, Exception exception = null);              

    }
}
