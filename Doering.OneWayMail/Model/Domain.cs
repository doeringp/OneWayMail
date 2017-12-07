using System.ComponentModel.DataAnnotations;

namespace Doering.OneWayMail.Model
{
    public class Domain
    {
        [Key]
        public string Name { get; set; }

        public Domain()
        {
        }

        public Domain(string name)
        {
            Name = name;
        }
    }
}
