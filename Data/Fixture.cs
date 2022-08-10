using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnlightedWorkService.Data
{
    [Table(name:"fixtures")]
    public class Fixture
    {
        [Key,Required]
		public int Id { get; set; }
        [Required]
		public string Name { get; set; }
		public int Xaxis { get; set; }
		public int Yaxis { get; set; }
		public int GroupId { get; set; }
		public string? MacAddress { get; set; }
		public string? Class { get; set; }
        public int FloorId { get; set; }

        [ForeignKey(nameof(FloorId))]
        public virtual Floor Floor { get; set; }
    }
}
