﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exemploasp.Patterns
{
	public class CandidaturaAceite : State
	{
        //neste estado nada acontece. FIM
		public CandidaturaAceite(DecisorCandidatura adecisorCandidatura) : base(adecisorCandidatura)
		{
		}
	}
}