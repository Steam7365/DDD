namespace DDD.Infrastructure.Common
{
    public class HeaderResult<T>
    {

        public virtual bool IsSuccess { get; set; }

        public virtual string Mseeage { get; set; }

        public virtual string StatusCode { get; set; }

        public virtual T Result { get; set; }

        public virtual int Total { get; set; }

        public HeaderResult()
        {
            IsSuccess = true;
        }
        public HeaderResult(bool IsSuccess, string Mseeage, T Result)
        {
            this.IsSuccess= IsSuccess;
            this.Mseeage= Mseeage;
            this.Result= Result;
        }
        public HeaderResult(T Result)
        {
            this.IsSuccess = true;
            this.Mseeage = "";
            this.Result = Result;
        }
        public HeaderResult(bool IsSuccess, string Mseeage)
        {
            this.IsSuccess = IsSuccess;
            this.Mseeage = Mseeage;
        }
        public HeaderResult(bool IsSuccess, string Mseeage, string StatusCode)
        {
            this.IsSuccess = IsSuccess;
            this.Mseeage = Mseeage;
            this.StatusCode= StatusCode;
        }
        public HeaderResult(bool IsSuccess, string Mseeage, T Result,int Total)
        {
            this.IsSuccess = IsSuccess;
            this.Mseeage = Mseeage;
            this.Result = Result;
            this.Total= Total;
        }

        public HeaderResult(bool IsSuccess, string Mseeage, T Result, string StatusCode)
        {
            this.IsSuccess = IsSuccess;
            this.Mseeage = Mseeage;
            this.Result = Result;
            this.StatusCode = StatusCode;
        }

    }
}
