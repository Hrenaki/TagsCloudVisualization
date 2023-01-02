﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.Infrastructure
{
    public interface IWordPreparer
    {
        public string[] Prepare(IEnumerable<string> words);
    }
}