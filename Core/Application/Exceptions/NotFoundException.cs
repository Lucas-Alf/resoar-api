namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Registro não encontrado") { }
    }
}