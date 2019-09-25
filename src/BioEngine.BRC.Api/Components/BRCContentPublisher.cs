using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.BRC.Domain.Entities;
using BioEngine.Core.Entities;
using BioEngine.Core.Properties;
using BioEngine.Core.Repository;
using BioEngine.Extra.Facebook;
using BioEngine.Extra.IPB.Properties;
using BioEngine.Extra.IPB.Publishing;
using BioEngine.Extra.Twitter;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace BioEngine.BRC.Api.Components
{
    [UsedImplicitly]
    public class BRCContentPublisher
    {
        private readonly IPBContentPublisher _ipbContentPublisher;
        private readonly TwitterContentPublisher _twitterContentPublisher;
        private readonly FacebookContentPublisher _facebookContentPublisher;
        private readonly SitesRepository _sitesRepository;
        private readonly SectionsRepository _sectionsRepository;
        private readonly PropertiesProvider _propertiesProvider;
        private readonly BrcApiOptions _options;

        public BRCContentPublisher(IPBContentPublisher ipbContentPublisher,
            TwitterContentPublisher twitterContentPublisher,
            FacebookContentPublisher facebookContentPublisher, SitesRepository sitesRepository,
            SectionsRepository sectionsRepository,
            PropertiesProvider propertiesProvider, IOptions<BrcApiOptions> options)
        {
            _ipbContentPublisher = ipbContentPublisher;
            _twitterContentPublisher = twitterContentPublisher;
            _facebookContentPublisher = facebookContentPublisher;
            _sitesRepository = sitesRepository;
            _sectionsRepository = sectionsRepository;
            _propertiesProvider = propertiesProvider;
            _options = options.Value;
        }

        private Guid GetMainSiteId(ContentItem contentItem)
        {
            return contentItem.SiteIds.Contains(_options.DefaultMainSiteId)
                ? _options.DefaultMainSiteId
                : contentItem.SiteIds.First();
        }


        public async Task PublishOrDeleteAsync(ContentItem contentItem,
            PropertyChange[] changes)
        {
            var sites = await _sitesRepository.GetByIdsAsync(contentItem.SiteIds);
            foreach (var site in sites)
            {
                if (site.Id == GetMainSiteId(contentItem))
                {
                    var ipbSettings = await _propertiesProvider.GetAsync<IPBSitePropertiesSet>(site);
                    if (ipbSettings != null && ipbSettings.IsEnabled && ipbSettings.ForumId > 0)
                    {
                        await _ipbContentPublisher.PublishAsync(contentItem,
                            new IPBPublishConfig(ipbSettings.ForumId), true);
                    }
                }

                var twitterSettings = await _propertiesProvider.GetAsync<TwitterSitePropertiesSet>(site);
                if (twitterSettings != null && twitterSettings.IsEnabled)
                {
                    var hasChanges = changes != null && changes.Any(c =>
                                         c.Name == nameof(contentItem.Title) || c.Name == nameof(contentItem.Url));

                    var sections = await _sectionsRepository.GetByIdsAsync(contentItem.SectionIds);
                    var tags = new List<string>();
                    foreach (var section in sections)
                    {
                        if (section is Developer developer && !string.IsNullOrEmpty(developer.Data.Hashtag))
                        {
                            tags.Add($"#{developer.Data.Hashtag.Replace("#", "")}");
                        }

                        if (section is Game game && !string.IsNullOrEmpty(game.Data.Hashtag))
                        {
                            tags.Add($"#{game.Data.Hashtag.Replace("#", "")}");
                        }

                        if (section is Topic topic && !string.IsNullOrEmpty(topic.Data.Hashtag))
                        {
                            tags.Add($"#{topic.Data.Hashtag.Replace("#", "")}");
                        }
                    }


                    var twitterConfig = new TwitterPublishConfig(
                        new TwitterConfig(twitterSettings.ConsumerKey, twitterSettings.ConsumerSecret,
                            twitterSettings.AccessToken, twitterSettings.AccessTokenSecret), tags);

                    if (hasChanges || !contentItem.IsPublished)
                    {
                        await _twitterContentPublisher.DeleteAsync(contentItem, twitterConfig, site);
                    }

                    if (contentItem.IsPublished)
                    {
                        await _twitterContentPublisher.PublishAsync(contentItem, twitterConfig, hasChanges, site);
                    }
                }

                var facebookSettings = await _propertiesProvider.GetAsync<FacebookSitePropertiesSet>(site);
                if (facebookSettings != null && facebookSettings.IsEnabled)
                {
                    var hasChanges = changes != null && changes.Any(c => c.Name == nameof(contentItem.Url));

                    var facebookConfig = new FacebookConfig(new Uri(facebookSettings.ApiUrl), facebookSettings.PageId,
                        facebookSettings.AccessToken);

                    if (hasChanges || !contentItem.IsPublished)
                    {
                        await _facebookContentPublisher.DeleteAsync(contentItem, facebookConfig, site);
                    }

                    if (contentItem.IsPublished)
                    {
                        await _facebookContentPublisher.PublishAsync(contentItem, facebookConfig, hasChanges, site);
                    }
                }
            }
        }

        public async Task DeleteAsync(ContentItem contentItem)
        {
            var sites = await _sitesRepository.GetByIdsAsync(contentItem.SiteIds);
            foreach (var site in sites)
            {
                if (site.Id == GetMainSiteId(contentItem))
                {
                    var ipbSettings = await _propertiesProvider.GetAsync<IPBSitePropertiesSet>(site);
                    if (ipbSettings != null && ipbSettings.IsEnabled && ipbSettings.ForumId > 0)
                    {
                        await _ipbContentPublisher.DeleteAsync(contentItem,
                            new IPBPublishConfig(ipbSettings.ForumId), site);
                    }
                }

                var twitterSettings = await _propertiesProvider.GetAsync<TwitterSitePropertiesSet>(site);
                if (twitterSettings != null && twitterSettings.IsEnabled)
                {
                    await _twitterContentPublisher.DeleteAsync(contentItem,
                        new TwitterPublishConfig(
                            new TwitterConfig(twitterSettings.ConsumerKey, twitterSettings.ConsumerSecret,
                                twitterSettings.AccessToken, twitterSettings.AccessTokenSecret), new List<string>()),
                        site);
                }

                var facebookSettings = await _propertiesProvider.GetAsync<FacebookSitePropertiesSet>(site);
                if (facebookSettings != null && facebookSettings.IsEnabled)
                {
                    await _facebookContentPublisher.DeleteAsync(contentItem,
                        new FacebookConfig(new Uri(facebookSettings.ApiUrl), facebookSettings.PageId,
                            facebookSettings.AccessToken), site);
                }
            }
        }
    }
}
