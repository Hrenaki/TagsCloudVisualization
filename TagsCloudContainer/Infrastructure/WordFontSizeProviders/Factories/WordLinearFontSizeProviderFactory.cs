﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.Infrastructure
{
    public class WordLinearFontSizeProviderFactory : IWordFontSizeProviderFactory
    {
        public IWordFontSizeProvider CreateDefault(WordFontSizeSettings settings) => new WordLinearFontSizeProvider(settings);
    }
}