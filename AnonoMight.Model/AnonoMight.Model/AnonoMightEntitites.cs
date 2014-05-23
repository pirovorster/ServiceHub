using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonoMight.Model
{
	public partial class AnonoMightEntities
	{
		
		public AnonoMightEntities(DbCompiledModel compiledModel)
			: base("name=AnonoMightConnection", compiledModel)
		{
		}
    
	}
}
