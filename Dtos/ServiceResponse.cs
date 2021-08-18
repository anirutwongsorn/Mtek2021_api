public class ServiceResponse<T>
{
   public T data { get; set; }
   public string responseMsg { get; set; } = "OK";
   public bool IsOk { get; set; } = true;
}