﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaSimple.CustomControlItem
{
    public class ComboBoxItem
    {

        public string Text { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
