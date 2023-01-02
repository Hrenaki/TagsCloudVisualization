﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudContainer.Infrastructure
{
    public interface ITagsCloudGenerator
    {
        public WordPlate[] GeneratePlates(IEnumerable<string> words, string fontName, PointF center);
    }
}