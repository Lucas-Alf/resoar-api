using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    [Keyless]
    public class GenericValueModel<T>
    {
        public T? Value { get; set; }
    }
}
