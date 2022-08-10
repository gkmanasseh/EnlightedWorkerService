using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnlightedWorkService.Data
{
    [Table(name:"floors")]
    public class Floor
    {
        [Key,Required]
		public int Id { get; set; }
        [Required]
		public int Name { get; set; }
		public int Building { get; set; }
		public int Campus { get; set; }
		public int Company { get; set; }
		public string? Description { get; set; }
		public string? FloorPlanUrl { get; set; }
		public string? ParentFloorId { get; set; }

		
		public virtual ICollection<Fixture> Fixtures { get; set; }

        public Floor()
        {
			Fixtures = new HashSet<Fixture>();
        }
	}
}
