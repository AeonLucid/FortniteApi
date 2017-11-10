namespace FortniteApi
{
    internal static class Fortnite
    {
        public const string UserAgent = "game=UELauncher, engine=UE4, build=6.8.0-3742144+++Portal+Release-Live";

        public const string UserAgentFortnite = "game=FortniteGame, engine=UE4, version=4.16.0-3741772+++Fortnite+Release-Cert";

        public const string UrlAuthenticate = "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token";

        public const string UrlAuthExchange = "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/exchange";

        public const string UrlAccountLookup = "https://persona-public-service-prod06.ol.epicgames.com/persona/api/public/account/lookup";

        public const string UrlFortniteStats = "https://fortnite-public-service-prod11.ol.epicgames.com/fortnite/api/stats/accountId/{0}/bulk/window/alltime";
    }
}