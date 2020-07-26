using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.BRC.Common.Entities;
using BioEngine.BRC.Common.Entities.Abstractions;
using BioEngine.BRC.Common.Facebook;
using BioEngine.BRC.Common.IPB.Properties;
using BioEngine.BRC.Common.IPB.Publishing;
using BioEngine.BRC.Common.Properties;
using BioEngine.BRC.Common.Repository;
using BioEngine.BRC.Common.Twitter;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Sitko.Core.Repository;

namespace BioEngine.BRC.Api.Components
{
    [UsedImplicitly]
    public class BRCPostsPublisher
    {
        private readonly IPBContentPublisher _ipbContentPublisher;
        private readonly TwitterContentPublisher _twitterContentPublisher;
        private readonly FacebookContentPublisher _facebookContentPublisher;
        private readonly SitesRepository _sitesRepository;
        private readonly SectionsRepository _sectionsRepository;
        private readonly PropertiesProvider _propertiesProvider;
        private readonly BrcApiOptions _options;

        public BRCPostsPublisher(IPBContentPublisher ipbContentPublisher,
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

        private Guid GetMainSiteId(IContentItem contentItem)
        {
            return contentItem.SiteIds.Contains(_options.DefaultMainSiteId)
                ? _options.DefaultMainSiteId
                : contentItem.SiteIds[0];
        }


        public async Task PublishOrDeleteAsync(Post post,
            PropertyChange[] changes)
        {
            var sites = await _sitesRepository.GetByIdsAsync(post.SiteIds);
            foreach (var site in sites)
            {
                if (site.Id == GetMainSiteId(post))
                {
                    var ipbSettings = await _propertiesProvider.GetAsync<IPBSitePropertiesSet>(site);
                    if (ipbSettings != null && ipbSettings.IsEnabled && ipbSettings.ForumId > 0)
                    {
                        await _ipbContentPublisher.PublishAsync(post,
                            new IPBPublishConfig(ipbSettings.ForumId, post.AuthorId), true, site,
                            true);
                    }
                }

                var twitterSettings = await _propertiesProvider.GetAsync<TwitterSitePropertiesSet>(site);
                if (twitterSettings != null && twitterSettings.IsEnabled)
                {
                    var hasChanges = changes != null && changes.Any(c =>
                                         c.Name == nameof(post.Title) || c.Name == nameof(post.Url));

                    var sections = await _sectionsRepository.GetByIdsAsync(post.SectionIds);
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

                    if (hasChanges || !post.IsPublished)
                    {
                        await _twitterContentPublisher.DeleteAsync(post, twitterConfig, site);
                    }

                    if (post.IsPublished)
                    {
                        await _twitterContentPublisher.PublishAsync(post, twitterConfig, hasChanges, site);
                    }
                }

                var facebookSettings = await _propertiesProvider.GetAsync<FacebookSitePropertiesSet>(site);
                if (facebookSettings != null && facebookSettings.IsEnabled)
                {
                    var hasChanges = changes != null && changes.Any(c => c.Name == nameof(post.Url));

                    var facebookConfig = new FacebookConfig(new Uri(facebookSettings.ApiUrl), facebookSettings.PageId,
                        facebookSettings.AccessToken);

                    if (hasChanges || !post.IsPublished)
                    {
                        await _facebookContentPublisher.DeleteAsync(post, facebookConfig, site);
                    }

                    if (post.IsPublished)
                    {
                        await _facebookContentPublisher.PublishAsync(post, facebookConfig, hasChanges, site);
                    }
                }
            }
        }

        public async Task DeleteAsync(Post post)
        {
            var sites = await _sitesRepository.GetByIdsAsync(post.SiteIds);
            foreach (var site in sites)
            {
                if (site.Id == GetMainSiteId(post))
                {
                    var ipbSettings = await _propertiesProvider.GetAsync<IPBSitePropertiesSet>(site);
                    if (ipbSettings != null && ipbSettings.IsEnabled && ipbSettings.ForumId > 0)
                    {
                        await _ipbContentPublisher.DeleteAsync(post,
                            new IPBPublishConfig(ipbSettings.ForumId, post.AuthorId), site);
                    }
                }

                var twitterSettings = await _propertiesProvider.GetAsync<TwitterSitePropertiesSet>(site);
                if (twitterSettings != null && twitterSettings.IsEnabled)
                {
                    await _twitterContentPublisher.DeleteAsync(post,
                        new TwitterPublishConfig(
                            new TwitterConfig(twitterSettings.ConsumerKey, twitterSettings.ConsumerSecret,
                                twitterSettings.AccessToken, twitterSettings.AccessTokenSecret), new List<string>()),
                        site);
                }

                var facebookSettings = await _propertiesProvider.GetAsync<FacebookSitePropertiesSet>(site);
                if (facebookSettings != null && facebookSettings.IsEnabled)
                {
                    await _facebookContentPublisher.DeleteAsync(post,
                        new FacebookConfig(new Uri(facebookSettings.ApiUrl), facebookSettings.PageId,
                            facebookSettings.AccessToken), site);
                }
            }
        }
    }
}
