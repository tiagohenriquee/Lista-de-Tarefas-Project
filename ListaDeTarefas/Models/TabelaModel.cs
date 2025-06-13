using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ListaDeTarefas.Models
{
    public class TabelaModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Preencha a descrição!")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Preencha a data de vencimento!")]
        public DateTime? DataVencimento { get; set; }
        [Required(ErrorMessage = "Selecione uma categoria!")]
        public string CategoriaId { get; set; }
        [ValidateNever]
        public CategoriaModel Categoria { get; set; }
        [Required(ErrorMessage ="Selecione um status!")]
        public string StatusId { get; set; }
        [ValidateNever]
        public StatusModel Status { get; set; }
        public bool Atrasado => StatusId == "Aberto" && DataVencimento < DateTime.Today;
    }
}
