using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Compendium.Models
{
    [Table("Characters")]
    public class Character
    {
        [Key]
        public Guid Id { get; set; }
        public enum CharacterType { Townsfolk, Outsider, Minion, Demon, Traveler, Fabled }
        public enum GameEdition { TroubleBrewing, BadMoonRising, SectsAndViolets, Experimental }
        public string Name { get; set; }
        public CharacterType Type { get; set; }
        public GameEdition Edition { get; set; }
        public string Ability { get; set; }
    }
}
