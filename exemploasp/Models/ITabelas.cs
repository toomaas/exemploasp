using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exemploasp.Models
{
	public interface ITabelas
	{
		ICollection<Tema> Temas { get; set; }



	}
}
