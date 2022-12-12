using System.Text.Json.Serialization;

namespace EFCoreRelationships.Data.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }

        [JsonIgnore]
        public Character? Character { get; set; }
        public int CharacterId { get; set; }

    }
}
