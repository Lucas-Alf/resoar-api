using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ResearchAdvisorApproval : IIdentityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Publicação é obrigatório")]
        public int? ResearchId { get; set; }

        [Required(ErrorMessage = "O Campo Usuário é obrigatório")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "O Campo Aprovação é obrigatório")]
        public bool? Approved { get; set; }

        #region references
        public virtual Research? Research { get; set; }
        public virtual User? User { get; set; }

        #endregion references
    }
}
