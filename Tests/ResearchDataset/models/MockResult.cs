using Domain.Models;

namespace ResearchDataset.Models
{
    public class MockResult
    {
        public int Success { get; set; }
        public int Errors
        {
            get
            {
                return ErrorMessages.Count;
            }
        }

        public int Total
        {
            get
            {
                return Success + Errors;
            }
        }

        public IList<ResponseMessageModel> ErrorMessages { get; set; } = new List<ResponseMessageModel>();
    }
}