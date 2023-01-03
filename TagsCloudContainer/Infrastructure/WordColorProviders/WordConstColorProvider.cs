﻿using FluentResults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.Infrastructure.Settings;

namespace TagsCloudContainer.Infrastructure.WordColorProviders
{
    public class WordConstColorProvider : IWordColorProvider
    {
        public Result<Color> GetColor(string word)
        {
            return Result.Ok(Color.BlueViolet);
        }
    }
}