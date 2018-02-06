using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exemploasp.Models
{
    //interface utilizada pelo UserAccount e por Exposicao para gerir os temas de ambos os modelos de forma mais simples. 
	public interface ITabelas
	{
		ICollection<Tema> Temas { get; set; }
	}
}
