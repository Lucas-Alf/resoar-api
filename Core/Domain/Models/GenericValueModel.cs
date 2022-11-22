using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    [Keyless]
    // [NotMapped]
    public class GenericValueModel<T>
    {
        public T? Value { get; set; }
    }
}
