﻿using System.Collections.Generic;
using System.Linq;
using TagsCloudVisualization.Configurations;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization
{
    public class TagCloudCreator
    {
        public static void Create(IEnumerable<string> words, 
            string imageSavePath, 
            CloudConfiguration cloudConfiguration,
            DistributionConfiguration distributionConfiguration,
            int amountClouds = 1)
        {
            var tags = TagCloudHelper.CreateTagsFromWords(words);
            var sizes = TagCloudHelper.GenerateRectangleSizes(tags);
            
            for (var i = 1; i <= amountClouds; i++)
            {
                var savePath = imageSavePath + $"cloud_{i}.png";
                
                var rectangles = new CircularCloudLayouter(cloudConfiguration.Center, distributionConfiguration)
                    .GenerateCloud(sizes);
            
                var tagsByRectangles = tags.Zip(rectangles, (k, v) => new { k, v })
                    .ToDictionary(x => x.k, x => x.v);

                DrawingHelper.DrawTagCloud(tagsByRectangles, cloudConfiguration).Save(savePath);

                TagCloudHelper.ShuffleTags(tags, sizes);
            }
        }
    }
}