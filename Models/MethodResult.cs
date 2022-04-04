using System;

namespace Mvc_Apteka.Models
{
    public class MethodResult
    {
        public object Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public static MethodResult FromResult(object result)
            => new MethodResult()
            {
                Message = "Операция выполнена успешно",
                Success = true,
                Result = result
            };
        public static MethodResult FromException(Exception ex)
            => new MethodResult()
            {
                Message = "Операция выполнена завершена с ошибкой: " + ex.Message,
                Success = false,
                Result = null
            };
    }
}
