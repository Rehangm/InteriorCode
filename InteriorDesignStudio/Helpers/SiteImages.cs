namespace InteriorDesignStudio.Helpers;

/// <summary>
/// Static (non-database) imagery used across page sections.
/// These are royalty-free Unsplash placeholders – replace them with your own photography
/// by dropping files into wwwroot/images and changing the paths below
/// (e.g. "/images/hero.jpg"). Any image that fails to load falls back to
/// /images/placeholder.svg automatically.
/// </summary>
public static class SiteImages
{
    private static string U(string id, int w = 1600) =>
        $"https://images.unsplash.com/{id}?auto=format&fit=crop&w={w}&q=80";

    public static readonly string Hero = U("photo-1616486338812-3dadae4b4ace", 2000);
    public static readonly string Intro = U("photo-1615874959474-d609969a20ed");
    public static readonly string IntroSecondary = U("photo-1616137466211-f939a420be84");
    public static readonly string Philosophy = U("photo-1618220179428-22790b461013");
    public static readonly string CallToAction = U("photo-1600607687920-4e2a09cf159d", 2000);

    public static readonly string AboutHeader = U("photo-1600210492493-0946911123ea", 2000);
    public static readonly string AboutStory = U("photo-1600880292203-757bb62b4baf");
    public static readonly string AboutStudio = U("photo-1618219908412-a29a1bb7b86e");
    public static readonly string AboutExpertise = U("photo-1600566753086-00f18fb6b3ea");

    public static readonly string ServicesHeader = U("photo-1600585154340-be6161a56a0c", 2000);
    public static readonly string ProjectsHeader = U("photo-1600607687939-ce8a6c25118c", 2000);
    public static readonly string ContactHeader = U("photo-1600566752355-35792bedcfea", 2000);
}
