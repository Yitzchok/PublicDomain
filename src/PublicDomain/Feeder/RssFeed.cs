using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using PublicDomain.Feeder.Rss;

namespace PublicDomain.Feeder
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class RssFeed : Feed, IRssFeed
    {
        #region IRssFeed Members

        /// <summary>
        /// Required.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return Getter("Title");
            }
            set
            {
                Setter("Title", value);
            }
        }

        /// <summary>
        /// Required.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return Getter("Description");
            }
            set
            {
                Setter("Description", value);
            }
        }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture
        {
            get
            {
                return Getter<CultureInfo>("Culture", CachedPropertiesProvider.ConvertToCultureInfo);
            }
            set
            {
                Setter("Culture", value);
            }
        }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public string Copyright
        {
            get
            {
                return Getter("Copyright");
            }
            set
            {
                Setter("Copyright", value);
            }
        }

        /// <summary>
        /// Gets or sets the managing editor.
        /// </summary>
        /// <value>The managing editor.</value>
        public string ManagingEditor
        {
            get
            {
                return Getter("ManagingEditor");
            }
            set
            {
                Setter("ManagingEditor", value);
            }
        }

        /// <summary>
        /// Gets or sets the web master.
        /// </summary>
        /// <value>The web master.</value>
        public string WebMaster
        {
            get
            {
                return Getter("WebMaster");
            }
            set
            {
                Setter("WebMaster", value);
            }
        }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        public string Generator
        {
            get
            {
                return Getter("Generator");
            }
            set
            {
                Setter("Generator", value);
            }
        }

        /// <summary>
        /// Gets or sets the publication date.
        /// </summary>
        /// <value>The publication date.</value>
        public TzDateTime PublicationDate
        {
            get
            {
                return Getter<TzDateTime>("PublicationDate", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("PublicationDate", value);
            }
        }

        /// <summary>
        /// Gets or sets the last changed.
        /// </summary>
        /// <value>The last changed.</value>
        public TzDateTime LastChanged
        {
            get
            {
                return Getter<TzDateTime>("LastChanged", CachedPropertiesProvider.ConvertToTzDateTime);
            }
            set
            {
                Setter("LastChanged", value);
            }
        }

        /// <summary>
        /// Gets or sets the doc.
        /// </summary>
        /// <value>The doc.</value>
        public Uri Doc
        {
            get
            {
                return Getter<Uri>("Doc", CachedPropertiesProvider.ConvertToUri);
            }
            set
            {
                Setter("Doc", value);
            }
        }

        /// <summary>
        /// Gets or sets the time to live.
        /// </summary>
        /// <value>The time to live.</value>
        public int? TimeToLive
        {
            get
            {
                return Getter<int?>("TimeToLive", CachedPropertiesProvider.ConvertToIntNullable);
            }
            set
            {
                Setter("TimeToLive", value);
            }
        }

        /// <summary>
        /// Gets or sets the cloud.
        /// </summary>
        /// <value>The cloud.</value>
        public Feeder.Rss.IRssCloud Cloud
        {
            get
            {
                return Getter<IRssCloud>("Cloud", RssFeedParser.ConvertToIRssCloud);
            }
            set
            {
                Setter("Cloud", value);
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public Feeder.Rss.IRssCategory Category
        {
            get
            {
                return Getter<IRssCategory>("Category", RssFeedParser.ConvertToIRssCategory);
            }
            set
            {
                Setter("Category", value);
            }
        }

        /// <summary>
        /// Gets or sets the text input.
        /// </summary>
        /// <value>The text input.</value>
        public Feeder.Rss.IRssTextInput TextInput
        {
            get
            {
                return Getter<IRssTextInput>("TextInput", RssFeedParser.ConvertToIRssTextInput);
            }
            set
            {
                Setter("TextInput", value);
            }
        }

        /// <summary>
        /// Gets or sets the skip hours.
        /// </summary>
        /// <value>The skip hours.</value>
        public IList<uint> SkipHours
        {
            get
            {
                return Getter<IList<uint>>("SkipHours", RssFeedParser.ConvertToSkipHourList);
            }
            set
            {
                Setter("SkipHours", value);
            }
        }

        /// <summary>
        /// Gets or sets the skip days.
        /// </summary>
        /// <value>The skip days.</value>
        public IList<DayOfWeek> SkipDays
        {
            get
            {
                return Getter<IList<DayOfWeek>>("SkipDays", RssFeedParser.ConvertToDayOfWeekList);
            }
            set
            {
                Setter("SkipDays", value);
            }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Feeder.Rss.IRssImage Image
        {
            get
            {
                return Getter<IRssImage>("Image", RssFeedParser.ConvertToIRssImage);
            }
            set
            {
                Setter("Image", value);
            }
        }

        #endregion

        /// <summary>
        /// Distills this instance.
        /// </summary>
        /// <returns></returns>
        public override IDistilledFeed Distill()
        {
            IDistilledFeed distilled = new DistilledFeed();
            distilled.Properties = Properties;
            if (Category != null)
            {
                distilled.Category = Category.CategoryName;
            }
            distilled.Copyright = Copyright;
            distilled.Culture = Culture;
            distilled.Description = Description;
            distilled.FeedUri = FeedUri;
            distilled.Generator = Generator;
            distilled.LastChanged = LastChanged;
            distilled.PublicationDate = PublicationDate;
            distilled.TimeToLive = TimeToLive;
            distilled.Title = Title;
            distilled.Items.Clear();

            foreach (IFeedItem feedItem in Items)
            {
                distilled.Items.Add(feedItem.Distill());
            }
            return distilled;
        }
    }
}
