Put your own photography in this folder (e.g. hero.jpg, projects/marine-drive/01.jpg)
and reference it with a root-relative path such as /images/hero.jpg in:
  - Helpers/SiteImages.cs           (hero, about, section images)
  - the Services / Projects / ProjectImages tables (ImagePath, CoverImage columns)
Any image that fails to load is replaced automatically with placeholder.svg.
