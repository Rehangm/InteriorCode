using InteriorDesignStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace InteriorDesignStudio.Data;

/// <summary>
/// Inserts realistic sample content the first time the application runs against an empty database.
/// Existing data is never modified or duplicated.
/// </summary>
public static class DbSeeder
{
    private static string U(string id, int w = 1600) =>
        $"https://images.unsplash.com/{id}?auto=format&fit=crop&w={w}&q=80";

    public static async Task SeedAsync(AppDbContext db, ILogger logger)
    {
        if (!await db.Services.AnyAsync())
        {
            db.Services.AddRange(GetServices());
            await db.SaveChangesAsync();
            logger.LogInformation("Seeded services.");
        }

        if (!await db.Projects.AnyAsync())
        {
            db.Projects.AddRange(GetProjects());
            await db.SaveChangesAsync();
            logger.LogInformation("Seeded projects and gallery images.");
        }

        if (!await db.Testimonials.AnyAsync())
        {
            db.Testimonials.AddRange(GetTestimonials());
            await db.SaveChangesAsync();
            logger.LogInformation("Seeded testimonials.");
        }
    }

    private static IEnumerable<Service> GetServices() => new[]
    {
        new Service
        {
            DisplayOrder = 1, Icon = "fa-house-chimney", Slug = "residential-interior-design",
            ServiceName = "Residential Interior Design",
            ImagePath = U("photo-1600210492486-724fe5c67fb0"),
            ShortDescription = "Complete, turnkey interiors for apartments, villas and penthouses – shaped around the way you live.",
            Description = "Your home should feel like a quiet expression of who you are. Our residential practice takes a home from bare shell to finished, styled residence – planning, materials, bespoke joinery, lighting, furniture, art and soft furnishings – all coordinated by a single design team. We work closely with families to understand daily rituals, storage needs and the feeling they want to come home to, and translate that into calm, enduring interiors.",
            KeyBenefits = "One accountable team from concept to handover\nBespoke furniture and joinery made for your space\nTransparent budgets with itemised material schedules\nDetailed 3D visualisation before anything is built\nWeekly site updates and a dedicated project manager",
            DesignApproach = "We begin with a lifestyle brief and a measured survey, then present two distinct concept directions with mood boards and material palettes. Once a direction is chosen we develop photo-realistic 3D views, working drawings and a fixed-scope estimate before supervising execution through to final styling."
        },
        new Service
        {
            DisplayOrder = 2, Icon = "fa-building", Slug = "commercial-interior-design",
            ServiceName = "Commercial Interior Design",
            ImagePath = U("photo-1497366811353-6870744d04b2"),
            ShortDescription = "Retail, hospitality and workplace interiors that express your brand and perform every day.",
            Description = "Commercial interiors must work hard: they carry a brand, guide customers, support staff and withstand heavy use. We design boutiques, showrooms, restaurants, lounges and hospitality spaces that feel distinctive and are built to last – balancing atmosphere with circulation, compliance, maintenance and return on investment.",
            KeyBenefits = "Brand-led spatial storytelling\nDurable, commercial-grade material specifications\nCompliance with fire, accessibility and local codes\nPhased execution to minimise business downtime\nLighting design that sells product and sets mood",
            DesignApproach = "We start with your brand, customer journey and operational requirements, then build a spatial strategy and concept. Detailed design is coordinated with MEP consultants and contractors, and we manage the programme closely so you open on time."
        },
        new Service
        {
            DisplayOrder = 3, Icon = "fa-couch", Slug = "luxury-living-room-design",
            ServiceName = "Luxury Living Room Design",
            ImagePath = U("photo-1618221195710-dd6b41faaea6"),
            ShortDescription = "Elegant, layered living spaces for entertaining, conversation and quiet evenings.",
            Description = "The living room sets the tone for the entire home. We compose layered spaces with considered furniture layouts, sculptural lighting, natural stone, fine textiles and curated art – rooms that feel generous when hosting and intimate when it is just the family.",
            KeyBenefits = "Furniture layouts planned for conversation and flow\nCurated art, accessories and styling\nLayered ambient, task and accent lighting\nConcealed media and acoustic solutions\nAccess to premium furniture and fabric houses",
            DesignApproach = "We study light, views and how the room is used through the day, then design a focal point and layered layout. Every piece – from the sofa profile to the rug weave – is selected or custom-made to work as one composition."
        },
        new Service
        {
            DisplayOrder = 4, Icon = "fa-kitchen-set", Slug = "modular-kitchen-design",
            ServiceName = "Modular Kitchen Design",
            ImagePath = U("photo-1556909114-f6e7ad7d3136"),
            ShortDescription = "Ergonomic, beautifully detailed kitchens with premium hardware and intelligent storage.",
            Description = "A great kitchen is a precise piece of design. We plan efficient work zones, generous storage and seamless appliance integration, finished in materials chosen for both beauty and durability – from fluted veneers and lacquer to quartz, granite and natural marble.",
            KeyBenefits = "Ergonomic work-triangle and zoning\nSoft-close premium European hardware\nIntegrated appliances and hidden storage\nMoisture- and heat-resistant material choices\nUp to 10-year warranty on modules",
            DesignApproach = "We map how you cook, store and entertain, then design the layout around those habits. Detailed elevations, material samples and hardware demonstrations ensure every drawer and handle is right before production begins."
        },
        new Service
        {
            DisplayOrder = 5, Icon = "fa-bed", Slug = "bedroom-interior-design",
            ServiceName = "Bedroom Interior Design",
            ImagePath = U("photo-1616594039964-ae9021a400a0"),
            ShortDescription = "Restful, hotel-inspired bedrooms and suites with tailored wardrobes and soft lighting.",
            Description = "Bedrooms should be sanctuaries. We design calm, tactile spaces with upholstered headboards, walk-in wardrobes, dressing areas and lighting scenes for every hour – drawing on the comfort and detail of the world’s finest hotel suites.",
            KeyBenefits = "Custom wardrobes and walk-in closets\nAcoustic and blackout solutions for better sleep\nSoft, dimmable lighting scenes\nTactile, natural material palettes\nThoughtful children’s and guest rooms",
            DesignApproach = "We focus on proportion, comfort and calm: a restrained palette, carefully placed light sources and storage that keeps surfaces clear. Each room is detailed with you down to bedside power points and reading-light positions."
        },
        new Service
        {
            DisplayOrder = 6, Icon = "fa-briefcase", Slug = "office-interior-design",
            ServiceName = "Office Interior Design",
            ImagePath = U("photo-1497366216548-37526070297c"),
            ShortDescription = "Workplaces that attract talent, encourage collaboration and reflect your culture.",
            Description = "Today’s office must earn the commute. We design corporate headquarters, studios and executive suites that support focused work and collaboration alike – with biophilic elements, acoustic comfort, flexible meeting spaces and a strong sense of identity.",
            KeyBenefits = "Workplace strategy and headcount planning\nAcoustic zoning for focus and collaboration\nErgonomic, flexible furniture systems\nBiophilic design and wellness-led features\nIntegrated AV and smart-building readiness",
            DesignApproach = "We interview leadership and teams, analyse how space is used, and develop a workplace strategy before design begins. The result is an office planned around real behaviour rather than assumptions."
        },
        new Service
        {
            DisplayOrder = 7, Icon = "fa-ruler-combined", Slug = "space-planning",
            ServiceName = "Space Planning",
            ImagePath = U("photo-1503387762-592deb58ef4e"),
            ShortDescription = "Intelligent layouts that unlock the full potential of every square foot.",
            Description = "Good design starts with a good plan. Our space-planning service studies circulation, light, views and function to create layouts that feel larger, work better and future-proof your investment – ideal before buying, renovating or fitting out a new property.",
            KeyBenefits = "Measured surveys and accurate CAD drawings\nMultiple layout options with pros and cons\nFurniture planning to scale\nImproved light, flow and storage\nClear foundation for contractors and budgets",
            DesignApproach = "We survey the space, capture your requirements and test several layout options. Each option is presented with furniture plans and 3D massing so you can make confident decisions early."
        },
        new Service
        {
            DisplayOrder = 8, Icon = "fa-hammer", Slug = "renovation-and-remodeling",
            ServiceName = "Renovation & Remodeling",
            ImagePath = U("photo-1581858726788-75bc0f6a952d"),
            ShortDescription = "Thoughtful transformations of existing homes and workspaces, managed end to end.",
            Description = "Whether you are updating a single room or reimagining an entire property, we manage renovations with care – from structural changes and services upgrades to finishes and furnishing – while respecting the character of the original building.",
            KeyBenefits = "Structural and services assessment\nCoordination of approvals and society permissions\nVetted contractors and quality checks\nClear schedules and cost control\nMinimal disruption with phased works",
            DesignApproach = "We assess what to keep, what to improve and what to replace, then design a renovation plan that balances ambition with budget. Our site team oversees every trade so quality stays consistent from demolition to handover."
        }
    };

    private static ProjectImage[] Gallery(params string[] ids) =>
        ids.Select((id, i) => new ProjectImage { ImagePath = U(id), DisplayOrder = i + 1 }).ToArray();

    private static IEnumerable<Project> GetProjects()
    {
        var baseDate = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        return new[]
        {
            new Project
            {
                ProjectName = "The Marine Drive Penthouse", Slug = "marine-drive-penthouse",
                Category = ProjectCategories.Residential, Location = "Mumbai, Maharashtra",
                Area = "5,400 sq ft", CompletionYear = 2025, IsFeatured = true, CreatedDate = baseDate.AddDays(-10),
                CoverImage = U("photo-1600607687939-ce8a6c25118c"),
                Description = "A duplex penthouse overlooking the Arabian Sea, reimagined for a family of five who entertain often. We opened up the plan to frame uninterrupted sea views, introduced a floating oak staircase and created a series of intimate retreats on the upper level.",
                DesignConcept = "‘Quiet luxury by the sea.’ A restrained palette of travertine, bleached oak and linen lets the ocean take centre stage, while brushed brass details catch the evening light. Every room is oriented towards the water, and joinery conceals storage so surfaces stay serene.",
                Images = Gallery("photo-1600566753190-17f0baa2a6c3", "photo-1600585154526-990dced4db0d", "photo-1600573472550-8090b5e0745e", "photo-1600210491892-03d54c0aaf87")
            },
            new Project
            {
                ProjectName = "Aurelia Corporate Headquarters", Slug = "aurelia-corporate-headquarters",
                Category = ProjectCategories.Office, Location = "Bengaluru, Karnataka",
                Area = "32,000 sq ft", CompletionYear = 2025, IsFeatured = true, CreatedDate = baseDate.AddDays(-20),
                CoverImage = U("photo-1497366754035-f200968a6e72"),
                Description = "A two-floor headquarters for a fast-growing technology firm, designed for 280 people working in a hybrid model. The brief called for a workplace that feels premium yet relaxed, with abundant collaboration space and quiet zones for focused work.",
                DesignConcept = "Inspired by a hospitality lobby, the reception flows into a café and town-hall stair. Warm timber slats, planted courtyards and acoustic felt create a calm, biophilic environment, while neighbourhoods of desks are wrapped around flexible meeting rooms.",
                Images = Gallery("photo-1497215728101-856f4ea42174", "photo-1524758631624-e2822e304c36", "photo-1604328698692-f76ea9498e76", "photo-1497366412874-3415097a27e7")
            },
            new Project
            {
                ProjectName = "Sandstone Villa Living Room", Slug = "sandstone-villa-living-room",
                Category = ProjectCategories.LivingRoom, Location = "Jaipur, Rajasthan",
                Area = "1,100 sq ft", CompletionYear = 2024, IsFeatured = true, CreatedDate = baseDate.AddDays(-30),
                CoverImage = U("photo-1586023492125-27b2c045efd7"),
                Description = "A double-height living room in a contemporary villa, designed to host large family gatherings while still feeling intimate on quiet evenings.",
                DesignConcept = "We drew on Jaipur’s sandstone heritage with hand-finished lime plaster walls, a carved stone fireplace and jali-inspired screens. Low, sculptural seating and a monumental pendant bring the scale of the room down to a human level.",
                Images = Gallery("photo-1567016432779-094069958ea5", "photo-1493663284031-b7e3aefcae8e", "photo-1615529182904-14819c35db37", "photo-1618221195710-dd6b41faaea6")
            },
            new Project
            {
                ProjectName = "Monochrome Chef’s Kitchen", Slug = "monochrome-chefs-kitchen",
                Category = ProjectCategories.Kitchen, Location = "Gurugram, Haryana",
                Area = "480 sq ft", CompletionYear = 2025, IsFeatured = true, CreatedDate = baseDate.AddDays(-40),
                CoverImage = U("photo-1556912173-3bb406ef7e77"),
                Description = "A show kitchen and separate working kitchen for a passionate home cook, with a twelve-seat island that doubles as a dining table.",
                DesignConcept = "A study in contrast: matte charcoal cabinetry, honed Nero Marquina marble and warm walnut accents. Appliances disappear behind flush panels, and a full-height pantry wall keeps everything within reach yet out of sight.",
                Images = Gallery("photo-1484154218962-a197022b5858", "photo-1565538810643-b5bdb714032a", "photo-1556911220-bff31c812dba", "photo-1556909114-f6e7ad7d3136")
            },
            new Project
            {
                ProjectName = "Serene Master Suite", Slug = "serene-master-suite",
                Category = ProjectCategories.Bedroom, Location = "Pune, Maharashtra",
                Area = "750 sq ft", CompletionYear = 2024, IsFeatured = true, CreatedDate = baseDate.AddDays(-50),
                CoverImage = U("photo-1505693416388-ac5ce068fe85"),
                Description = "A master suite with bedroom, lounge, walk-in wardrobe and spa-style bathroom, created as a private retreat for a couple with demanding careers.",
                DesignConcept = "Soft, hotel-inspired calm. A wall-to-wall upholstered headboard, sheer linen drapery and indirect cove lighting create a gentle, enveloping atmosphere. Smoked oak wardrobes with leather pulls lead into a backlit onyx dressing area.",
                Images = Gallery("photo-1540518614846-7eded433c457", "photo-1522771739844-6a9f6d5f14af", "photo-1617325247661-675ab4b64ae2", "photo-1616594039964-ae9021a400a0")
            },
            new Project
            {
                ProjectName = "Maison Lumière Boutique", Slug = "maison-lumiere-boutique",
                Category = ProjectCategories.Commercial, Location = "Hyderabad, Telangana",
                Area = "2,600 sq ft", CompletionYear = 2025, IsFeatured = true, CreatedDate = baseDate.AddDays(-60),
                CoverImage = U("photo-1441986300917-64674bd600d8"),
                Description = "A flagship fashion boutique for a couture label, designed to slow customers down and make every garment feel precious.",
                DesignConcept = "A gallery for fashion. Curved plaster alcoves, a central travertine plinth and theatrical lighting turn each collection into an exhibit. Fitting rooms are designed as private salons with soft seating and flattering light.",
                Images = Gallery("photo-1567401893414-76b7b1e5a7a5", "photo-1604014237800-1c9102c219da", "photo-1555529669-e69e7aa0ba9a", "photo-1441984904996-e0b6ba687e04")
            },
            new Project
            {
                ProjectName = "Coastal Retreat Villa", Slug = "coastal-retreat-villa",
                Category = ProjectCategories.Residential, Location = "Assagao, Goa",
                Area = "6,800 sq ft", CompletionYear = 2024, IsFeatured = false, CreatedDate = baseDate.AddDays(-70),
                CoverImage = U("photo-1613490493576-7fde63acd811"),
                Description = "A holiday villa set among palms, designed to blur the line between indoors and out and to welcome friends and family for long stays.",
                DesignConcept = "Relaxed tropical modernism: lime-washed walls, cane and teak furniture, terrazzo floors and wide verandas. Pocket doors disappear into the walls so every living space opens onto the pool garden.",
                Images = Gallery("photo-1600596542815-ffad4c1539a9", "photo-1600047509807-ba8f99d2cdde", "photo-1600566752355-35792bedcfea", "photo-1600210492493-0946911123ea")
            },
            new Project
            {
                ProjectName = "The Oakwood Lounge", Slug = "the-oakwood-lounge",
                Category = ProjectCategories.Commercial, Location = "Chennai, Tamil Nadu",
                Area = "4,100 sq ft", CompletionYear = 2023, IsFeatured = false, CreatedDate = baseDate.AddDays(-80),
                CoverImage = U("photo-1517248135467-4c7edcad34c4"),
                Description = "A fine-dining restaurant and cocktail lounge with seating for 120 guests, private dining and a chef’s table.",
                DesignConcept = "Warm, low-lit and intimate. Fluted oak, deep green velvet banquettes and aged brass create a timeless supper-club feel, while acoustic ceilings keep conversation easy even when the room is full.",
                Images = Gallery("photo-1414235077428-338989a2e8c0", "photo-1552566626-52f8b828add9", "photo-1559339352-11d035aa65de", "photo-1514933651103-005eec06c04b")
            },
            new Project
            {
                ProjectName = "Skyline Executive Suite", Slug = "skyline-executive-suite",
                Category = ProjectCategories.Office, Location = "BKC, Mumbai",
                Area = "6,500 sq ft", CompletionYear = 2024, IsFeatured = false, CreatedDate = baseDate.AddDays(-90),
                CoverImage = U("photo-1462826303086-329426d1aef5"),
                Description = "An executive floor for a private equity firm, including partner cabins, a boardroom and a client lounge with skyline views.",
                DesignConcept = "Discreet, tailored and confident – like a well-cut suit. Walnut panelling, bronze glass and wool carpets create an atmosphere of trust, with a boardroom table hand-made from a single book-matched slab.",
                Images = Gallery("photo-1556761175-b413da4baf72", "photo-1531973576160-7125cd663d86", "photo-1497366811353-6870744d04b2", "photo-1497366216548-37526070297c")
            },
            new Project
            {
                ProjectName = "Teakwood Family Lounge", Slug = "teakwood-family-lounge",
                Category = ProjectCategories.LivingRoom, Location = "Kochi, Kerala",
                Area = "900 sq ft", CompletionYear = 2023, IsFeatured = false, CreatedDate = baseDate.AddDays(-100),
                CoverImage = U("photo-1600121848594-d8644e57abab"),
                Description = "A family lounge in a restored heritage home, combining the original teak ceiling with contemporary comfort.",
                DesignConcept = "Old and new in conversation: the restored teak ceiling and red-oxide floors are paired with crisp contemporary seating, woven textiles from local artisans and a library wall for the family’s collection of books.",
                Images = Gallery("photo-1631679706909-1844bbd07221", "photo-1616137466211-f939a420be84", "photo-1600210492486-724fe5c67fb0", "photo-1586023492125-27b2c045efd7")
            }
        };
    }

    private static IEnumerable<Testimonial> GetTestimonials() => new[]
    {
        new Testimonial
        {
            ClientName = "Ananya & Vikram Mehta", Designation = "Homeowners, Marine Drive Penthouse", Rating = 5,
            Review = "They understood exactly how we wanted to live. Every detail – from the joinery to the lighting scenes – feels considered, and the project was delivered on schedule. Our home finally feels like us."
        },
        new Testimonial
        {
            ClientName = "Rahul Iyer", Designation = "COO, Aurelia Technologies", Rating = 5,
            Review = "Our new headquarters has transformed how our teams work together. The studio handled a complex brief with calm professionalism and created a space our people genuinely enjoy coming to."
        },
        new Testimonial
        {
            ClientName = "Sophie D’Souza", Designation = "Founder, Maison Lumière", Rating = 5,
            Review = "The boutique is more beautiful than we imagined. Customers stay longer, and the space tells our brand story better than any campaign we have run. A true creative partnership."
        },
        new Testimonial
        {
            ClientName = "Karan Malhotra", Designation = "Homeowner, Gurugram", Rating = 5,
            Review = "From the first sketch to the final styling, the experience was seamless. The kitchen is a joy to cook in, and the quality of craftsmanship is exceptional."
        },
        new Testimonial
        {
            ClientName = "Dr. Meera Nair", Designation = "Homeowner, Kochi", Rating = 4,
            Review = "They respected the heritage of our home while making it comfortable for modern family life. Transparent budgeting and regular updates made the whole process stress-free."
        }
    };
}
