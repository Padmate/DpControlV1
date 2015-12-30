﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpControl.Domain.Entities
{
    public class OperatorLocation
    {
        public int OperatorId { get; set; }
        public int LocationId { get; set; }
        public Operator Operator { get; set; }
        public Location Location { get; set; }
    }
}
