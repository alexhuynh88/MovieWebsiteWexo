using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public static class SessionExtensions
{
    // Gem en objekt i sessionen
    public static void SetObject(this ISession session, string key, object value)
    {
        var json = JsonConvert.SerializeObject(value);
        session.SetString(key, json);
    }

    // Hent en objekt fra sessionen
    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        if (string.IsNullOrEmpty(value))
        {
            return default(T);
        }

        return JsonConvert.DeserializeObject<T>(value);
    }

    // Tilføj film til ønskelisten
    public static void AddToWishlist(this ISession session, int movieId)
    {
        var wishlist = session.GetObject<List<int>>("Wishlist") ?? new List<int>();

        if (!wishlist.Contains(movieId))
        {
            wishlist.Add(movieId);
        }

        // Gem ønskelisten i sessionen
        session.SetObject("Wishlist", wishlist);
    }

    // Hent ønskelisten fra sessionen
    public static List<int> GetWishlist(this ISession session)
    {
        var wishlist = session.GetObject<List<int>>("Wishlist");
        return wishlist ?? new List<int>(); // Hvis sessionen er tom, returner en tom liste
    }

    // Gem ønskelisten i sessionen
    public static void SetWishlist(this ISession session, List<int> wishlist)
    {
        session.SetObject("Wishlist", wishlist);
    }

    // Fjern film fra ønskelisten
    public static void RemoveFromWishlist(this ISession session, int movieId)
    {
        var wishlist = session.GetObject<List<int>>("Wishlist") ?? new List<int>();

        wishlist.Remove(movieId);

        // Gem opdateret ønskeliste i sessionen
        session.SetObject("Wishlist", wishlist);
    }
}