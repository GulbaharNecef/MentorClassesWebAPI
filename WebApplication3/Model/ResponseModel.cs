namespace WebApplication3.Model
{
    public class ResponseModel<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; }
    }
}
