using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ResearchKeyWord : IIdentityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Publicação é obrigatório")]
        public int? ResearchId { get; set; }

        [Required(ErrorMessage = "O Campo Área do Conhecimento é obrigatório")]
        public int? KeyWordId { get; set; }

        #region references
        public virtual Research? Research { get; set; }
        public virtual KeyWord? KeyWord { get; set; }

        #endregion references
    }
}
