﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.Infrastructure
{
    public class WordConstFontSizeProvider : IWordFontSizeProvider
    {
        public float GetFontSize(string word)
        {
            return 14F;
        }
    }
}