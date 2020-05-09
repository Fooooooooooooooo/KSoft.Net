﻿using KSoftNet.Responses;
using RestSharp;
using System;

namespace KSoftNet.KSoft {
    public class KSoftAPI {
        const string BaseUrl = "https://api.ksoft.si/";
        readonly IRestClient _client;

        /// <summary>
        /// KSoftApi
        /// </summary>
        /// <param name="accountToken">KSoft token located on your dashboard</param>
        public KSoftAPI(string accountToken) => _client = new RestClient(BaseUrl) {
            Authenticator = new KSoftAuthenticator(accountToken)
        };

        public T Execute<T>(RestRequest request) where T : new() {
            IRestResponse<T> response = _client.Execute<T>(request);

            if (response.ErrorException != null) {
                const string errorMessage = "Error retrieving response. Check inner details for more info.";
                throw new ApplicationException(errorMessage, response.ErrorException);
            }
            return response.Data;
        }

        #region Images API

        /// <summary>
        /// Gets random image from the specified tag.
        /// </summary>
        /// <param name="tag">Default: false, if to show nsfw content</param>
        /// <param name="nsfw">Name of the tag</param>  
        /// <returns>KSoftImage</returns>
        public KSoftImage RandomImage(string tag, bool nsfw = false) {
            RestRequest request = new RestRequest("images/random-image");

            request.AddQueryParameter("tag", tag);
            request.AddQueryParameter("nsfw", nsfw.ToString());

            return Execute<KSoftImage>(request);
        }

        /// <summary>
        /// Retrieve the list of all available tags.
        /// </summary>
        /// <returns>KSoftTags</returns>
        public KSoftTags Tags() {
            RestRequest request = new RestRequest("images/tags");

            return Execute<KSoftTags>(request);
        }

        /// <summary>
        /// Search for tags.
        /// </summary>
        /// <param name="query">Search query</param>
        /// <returns>KSoftTags</returns>
        public KSoftTags TagSearch(string query) {
            RestRequest request = new RestRequest($"images/tags/{query}");

            return Execute<KSoftTags>(request);
        }

        /// <summary>
        /// Retrieve image data.
        /// </summary>
        /// <param name="snowflake">Image snowflake (unique ID)</param>
        /// <returns>KSoftImage</returns>
        public KSoftImage ImageFromSnowflake(string snowflake) {
            RestRequest request = new RestRequest($"images/image/{snowflake}");

            return Execute<KSoftImage>(request);
        }

        /// <summary>
        /// Retrieves a random meme from the cache. Source: reddit
        /// </summary>
        /// <returns>KSoftRedditPost</returns>
        public KSoftRedditPost RandomMeme() {
            RestRequest request = new RestRequest("images/random-meme");

            return Execute<KSoftRedditPost>(request);
        }

        /// <summary>
        /// Retrieves weird images from WikiHow
        /// </summary>
        /// <param name="nsfw">Default: false, if to display nsfw content.</param>
        /// <returns>KSoftWikiHowPost</returns>
        public KSoftWikiHowPost RandomWikiHow(bool nsfw = false) {
            RestRequest request = new RestRequest("images/random-wikihow");

            request.AddQueryParameter("nsfw", nsfw.ToString());

            return Execute<KSoftWikiHowPost>(request);
        }

        /// <summary>
        /// Get random cute pictures, mostly animals.
        /// </summary>
        /// <returns>KSoftRedditPost</returns>
        public KSoftRedditPost RandomAww() {
            RestRequest request = new RestRequest("images/random-aww");

            return Execute<KSoftRedditPost>(request);
        }

        /// <summary>
        /// Retrieves random NSFW pics. (real life stuff)
        /// </summary>
        /// <param name="gifs">Default: false, if to retrieve gifs instead of images</param>
        /// <returns>KSoftRedditPost</returns>
        public KSoftRedditPost RandomNsfw(bool gifs) {
            RestRequest request = new RestRequest("images/random-nsfw");

            request.AddQueryParameter("gifs", gifs.ToString());

            return Execute<KSoftRedditPost>(request);
        }

        /// <summary>
        /// Retrieve images from the specified subreddit.
        /// </summary>
        /// <param name="subreddit">Specified subreddit</param>
        /// <param name="removeNsfw">Default: false, if set to true, endpoint will filter out nsfw posts.</param>
        /// <param name="span">Default: "day", select range from which to get the images. Can be one of the following: "hour", "day", "week", "month", "year", "all"</param>
        /// <returns>KSoftRedditPost</returns>
        public KSoftRedditPost RandomReddit(string subreddit, bool removeNsfw, string span) {
            RestRequest request = new RestRequest($"images/rand-reddit/{subreddit}");

            request.AddQueryParameter("remove-nsfw", removeNsfw.ToString());
            request.AddQueryParameter("span", span);

            return Execute<KSoftRedditPost>(request);
        }

        #endregion


        #region Bans API

        /// <summary>
        /// This endpoint allows you to add bans to the list. If you don't have BAN_MANAGER permission your ban will be automatically converted to a ban report and we will take a look and act accordingly.
        /// </summary>
        /// <param name="userID">Users Discord ID that you are banning/reporting</param>
        /// <param name="reason">Reason why user should be globally banned</param>
        /// <param name="proof">URL of the image showing the act</param>
        /// <param name="mod">Users Discord ID who posted/reported the ban</param>
        /// <param name="userName">Users Discord username</param>
        /// <param name="userDiscriminator">Users Discord discriminator</param>
        /// <param name="appealPossible">If appeal should be disabled for that user.</param>
        /// <returns>KSoftBan</returns>
        public KSoftBan AddBan(long userID, string reason, string proof, long mod = 0, string userName = "0", string userDiscriminator = "0", bool appealPossible = false) {
            RestRequest request = new RestRequest("bans/add", Method.POST);

            request.AddQueryParameter("user", userID.ToString());
            request.AddQueryParameter("reason", reason);
            request.AddQueryParameter("proof", proof);
            request.AddQueryParameter("mod", mod.ToString());
            request.AddQueryParameter("user_discriminator", userDiscriminator);
            request.AddQueryParameter("appeal_possible", appealPossible.ToString());

            return Execute<KSoftBan>(request);
        }

        /// <summary>
        /// Get more information about a ban
        /// </summary>
        /// <param name="userID">Users Discord ID who's ban you'd like to check</param>
        /// <returns>KSoftBanInfo</returns>
        public KSoftBanInfo BanInfo(long userID) {
            RestRequest request = new RestRequest("bans/info");

            request.AddQueryParameter("user", userID.ToString());

            return Execute<KSoftBanInfo>(request);
        }

        /// <summary>
        /// Simple way to check if the user is banned
        /// </summary>
        /// <param name="userID">Users Discord ID that you'd like to check</param>
        /// <returns>KSoftBanCheck</returns>
        public KSoftBanCheck BanCheck(long userID) {
            RestRequest request = new RestRequest("bans/check");

            request.AddQueryParameter("user", userID.ToString());

            return Execute<KSoftBanCheck>(request);
        }

        /// <summary>
        /// Delete a ban, only users with BAN_MANAGER permission can use this.
        /// </summary>
        /// <param name="userID">Users Discord ID</param>
        /// <param name="force">Default: false, if true it deletes the entry from the database instead of deactivating</param>
        /// <returns>KSoftBanDeletion</returns>
        public KSoftBanDeletion DeleteBan(long userID, bool force = false) {
            RestRequest request = new RestRequest("bans/delete", Method.DELETE);

            request.AddQueryParameter("user", userID.ToString());
            request.AddQueryParameter("force", force.ToString());

            return Execute<KSoftBanDeletion>(request);
        }

        /// <summary>
        /// Pagination of bans, you can request up to 1000 records per page, default is 20.
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="perPage">Number of bans per page (default: 20)</param>
        /// <returns>KSoftBanList</returns>
        public KSoftBanList BanList(int page, int perPage) {
            RestRequest request = new RestRequest("bans/list");

            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("per_page", perPage.ToString());

            return Execute<KSoftBanList>(request);
        }

        /// <summary>
        /// Gets updates from the previous update
        /// </summary>
        /// <param name="timestamp">Timestamp in seconds from 1.1.1970 (epoch time)</param>
        /// <returns>KSoftBanUpdates</returns>
        public KSoftBanUpdates BanUpdates(DateTimeOffset timestamp) {
            RestRequest request = new RestRequest("bans/updates");

            request.AddQueryParameter("timestamp", timestamp.ToUnixTimeSeconds().ToString());

            return Execute<KSoftBanUpdates>(request);
        }

        #endregion

        #region Kumo API

        /// <summary>
        /// You can get coordinates and more information about the searched location, if needed image of the area is generated.
        /// </summary>
        /// <param name="query">Your location query</param>
        /// <param name="fast">Default: fast, return location data faster, but with less information</param>
        /// <param name="more">Default: false, return more than one location</param>
        /// <param name="mapZoom">Default: 12, set your own zoom level, if fast is not set or false, then this setting will be ignored because map zoom is calculated</param>
        /// <param name="includeMap">Default: false, if to include an image of the area</param>
        /// <returns>KSoftLocation</returns>
        public KSoftLocation SearchLocation(string query, bool fast = false, bool more = false, int mapZoom = 12, bool includeMap = false) {
            RestRequest request = new RestRequest("kumo/gis");

            request.AddQueryParameter("q", query);
            request.AddQueryParameter("fast", fast.ToString());
            request.AddQueryParameter("more", more.ToString());
            request.AddQueryParameter("map_zoom", mapZoom.ToString());
            request.AddQueryParameter("include_map", includeMap.ToString());

            return Execute<KSoftLocation>(request);
        }

        /// <summary>
        /// Gets weather by location.
        /// </summary>
        /// <param name="reportType">Select weather report type. Can be one of: "currently", "minutely", "hourly", "daily"  </param>
        /// <param name="query">Location query</param>
        /// <param name="units">Default: auto, select units, you can choose from: "si", "us", "uk2", "ca", "auto"</param>
        /// <param name="language">Default: en, select language, all available languages: 'ar', 'az', 'be', 'bg', 'bs', 'ca', 'cs', 'da', 'de', 'el', 'en', 'es', 'et', 'fi', 'fr', 'he', 'hr', 'hu', 'id', 'is', 'it', 'ja', 'ka', 'ko', 'kw', 'nb', 'nl', 'no', 'pl', 'pt', 'ro', 'ru', 'sk', 'sl', 'sr', 'sv', 'tet', 'tr', 'uk', 'x-pig-latin', 'zh', 'zh-tw'</param>
        /// <param name="icons">Default: original, select icon pack</param>
        /// <returns>KSoftWeather</returns>
        public KSoftWeather BasicWeather(string reportType, string query, string units = "auto", string language = "en", string icons = "original") {
            RestRequest request = new RestRequest($"kumo/weather/{reportType}");

            request.AddQueryParameter("q", query);
            request.AddQueryParameter("units", units);
            request.AddQueryParameter("lang", language);
            request.AddQueryParameter("icons", icons);

            return Execute<KSoftWeather>(request);
        }

        /// <summary>
        /// Gets weather by coordinates, this endpoint is faster than weather - easy, because it doesn't need to lookup the location.
        /// </summary>
        /// <param name="latitude">Latitude coordinate</param>
        /// <param name="longitude">Longitude coordinate</param>
        /// <param name="reportType">Select weather type. Can be one of: "currently", "minutely", "hourly", "daily"</param>
        /// <param name="units">Default: auto, select units, you can choose from: "si", "us", "uk2", "ca", "auto"</param>
        /// <param name="language">Default: en, select language, all available languages: 'ar', 'az', 'be', 'bg', 'bs', 'ca', 'cs', 'da', 'de', 'el', 'en', 'es', 'et', 'fi', 'fr', 'he', 'hr', 'hu', 'id', 'is', 'it', 'ja', 'ka', 'ko', 'kw', 'nb', 'nl', 'no', 'pl', 'pt', 'ro', 'ru', 'sk', 'sl', 'sr', 'sv', 'tet', 'tr', 'uk', 'x-pig-latin', 'zh', 'zh-tw'</param>
        /// <param name="icons">Default: original, select icon pack</param>
        /// <returns>KSoftWeather</returns>
        public KSoftWeather AdvancedWeather(float latitude, float longitude, string reportType, string units = "auto", string language = "en", string icons = "original") {
            RestRequest request = new RestRequest($"kumo/weather/{latitude},{longitude}/{reportType}");

            request.AddQueryParameter("units", units);
            request.AddQueryParameter("lang", language);
            request.AddQueryParameter("icons", icons);

            return Execute<KSoftWeather>(request);
        }

        /// <summary>
        /// Gets location data from the IP address.
        /// </summary>
        /// <param name="ip">IP address</param>
        /// <returns>KSoftGeoIP</returns>
        public KSoftGeoIP GeoIP(string ip) {
            RestRequest request = new RestRequest($"kumo/geoip");

            request.AddQueryParameter("ip", ip);

            return Execute<KSoftGeoIP>(request);
        }

        /// <summary>
        /// Currency conversion.
        /// </summary>
        /// <param name="from">ISO Standard for 3 letter currency naming: https://en.wikipedia.org/wiki/ISO_4217#Active_codes</param>
        /// <param name="to">ISO Standard for 3 letter currency naming: https://en.wikipedia.org/wiki/ISO_4217#Active_codes</param>
        /// <param name="value">Float or Integer you want to convert</param>
        /// <returns>KSoftCurrency</returns>
        public KSoftCurrency CurrencyConversion(string from, string to, float value) {
            RestRequest request = new RestRequest($"kumo/currency");

            request.AddQueryParameter("from", from);
            request.AddQueryParameter("to", to);
            request.AddQueryParameter("value", value.ToString());

            return Execute<KSoftCurrency>(request);
        }

        #endregion

        #region Lyrics & Music API

        public KSoftLyrics SearchLyrics(string query, bool textOnly, int limit) {
            RestRequest request = new RestRequest($"lyrics/search");

            request.AddQueryParameter("q", query);
            request.AddQueryParameter("text_only", textOnly.ToString());
            request.AddQueryParameter("limit", limit.ToString());

            return Execute<KSoftLyrics>(request);
        }

        /* Error
        public KSoftRecommendations MusicRecommendations(Array tracks, string provider) {
            var request = new RestRequest($"music/recommendations", Method.POST);

            request.AddJsonBody(tracks);
            request.AddBody(provider);

            return Execute<KSoftRecommendations>(request);
        }
        */

        public KSoftArtistInfo ArtistByID(int id) {
            RestRequest request = new RestRequest($"lyrics/artist/{id}");

            return Execute<KSoftArtistInfo>(request);
        }


        public KSoftAlbumInfo AlbumByID(int id) {
            RestRequest request = new RestRequest($"lyrics/album/{id}");

            return Execute<KSoftAlbumInfo>(request);
        }


        public KSoftTrackInfo TrackByID(int id) {
            RestRequest request = new RestRequest($"lyrics/track/{id}");

            return Execute<KSoftTrackInfo>(request);
        }
        #endregion
    }
}
