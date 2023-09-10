using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgGameApi.Models
{
    // Service işlem sonuç çıktılarını yönetebilmek için oluşturduğumuz Generic bir ServiceResponse Class
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success {get; set; } = true;
        public string Message {get;set;} = string.Empty;
    }
}