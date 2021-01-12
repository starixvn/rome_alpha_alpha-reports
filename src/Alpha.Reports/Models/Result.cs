namespace Alpha.Reports.Models
{
    public class Result
    {
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }

        public string Message { get; set; }

        public bool IsOk => Code == 0;
    }
}
