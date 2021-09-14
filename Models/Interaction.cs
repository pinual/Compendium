using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Compendium.Models
{
    [Table("Interactions")]
    public class Interaction
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AffectedCharacter1 { get; set; }
        public Guid AffectedCharacter2 { get; set; }
        public string Explanation { get; set; }
        public Guid Source { get; set; }
    }
}
