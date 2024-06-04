
public class ServiceResult<T>
{
    public bool IsSuccess { set; get; } = false;
    public T Data { set; get; }
    public string Message { set; get; } = string.Empty;

    public int errorCode = 0;

    // -1 = server error
    // 0 = ok
    // 1 = bad request
    // 2 = db error
    // 3 bad auth or authorization



}
