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
            IsSuccess = false;
        }
    }
}
