using System;
using System.Collections.Generic;
using System.Text;
using PublicDomain.Feeder.Rss;
using System.Globalization;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRssFeed : IFeed
    {
        /// <summary>
        /// Required.
        /// </summary>
        /// <value>The title.</value>
        string Title { get; set; }

        /// <summary>
        /// Required.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the managing editor.
        /// </summary>
        /// <value>The managing editor.</value>
        string ManagingEditor { get; set; }

        /// <summary>
        /// Gets or sets the web master.
        /// </summary>
        /// <value>The web master.</value>
        string WebMaster { get; set; }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        string Generator { get; set; }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        TzDateTime PublicationDate { get; set; }

        /// <summary>
        /// Gets or sets the last changed.
        /// </summary>
        /// <value>The last changed.</value>
        TzDateTime LastChanged { get; set; }

        /// <summary>
        /// Gets or sets the doc.
        /// </summary>
        /// <value>The doc.</value>
        Uri Doc { get; set; }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        int? TimeToLive { get; set; }

        // TODO PICS rating
        /// <summary>
        /// Gets or sets the cloud.
        /// </summary>
        /// <value>The cloud.</value>
        IRssCloud Cloud { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        IRssCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the text input.
        /// </summary>
        /// <value>The text input.</value>
        IRssTextInput TextInput { get; set; }

        /// <summary>
        /// Gets or sets the skip hours.
        /// </summary>
        /// <value>The skip hours.</value>
        IList<uint> SkipHours { get; set; }

        /// <summary>
        /// Gets or sets the skip days.
        /// </summary>
        /// <value>The skip days.</value>
        IList<DayOfWeek> SkipDays { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        IRssImage Image { get; set; }
    }
}
