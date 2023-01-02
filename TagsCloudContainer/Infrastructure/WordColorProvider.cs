﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.Infrastructure
{
    public class WordColorProvider : IWordColorProvider
    {
        public Color GetColor(string word)
        {
            return Color.BlueViolet;
        }
    }
}